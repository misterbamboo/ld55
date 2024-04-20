using Godot;
using System;

public partial class MonsterCardUI : Control
{
    [Export] public bool IsDraggable { get; set; } = true;
    [Export] private RichTextLabel NameLabel { get; set; }

    [Export] private TextureRect SpeciesImage { get; set; }
    [Export] private TextureRect EmotionImage { get; set; }

    [Export] private TextureRect EmotionIconImage { get; set; }
    [Export] private TextureRect ElementIconImage { get; set; }
    [Export] private TextureRect SpeciesIconImage { get; set; }

    [Export] private ProgressBar LifeDisplayProgressBar { get; set; }

    public SummoningSpecs SummoningSpecs { get; private set; } = new SummoningSpecs();

    private GameDataService gameDataService;
    private DeskManager deskManager;

    private MonsterImageLoader MonsterImageLoader { get; set; }

    private ControlDragHandler DragHandler { get; set; }
    public bool IsAnimating => shiftAnimRun || stickToAnimRun;

    private bool initPending = false;

    // ShiftAnim
    Vector2 shiftInitPos;
    Vector2 shiftTargetPos;
    float shiftInitRot;
    float shiftTargetRot;
    private double shiftT;
    private bool shiftAnimRun;

    // StickToAnim
    Action stickToAnimCallback;
    Vector2 stickToInitPos;
    Vector2 stickToTargetPos;
    float stickToInitRot;
    float stickToTargetRot;
    private double stickToT;
    private bool stickToAnimRun;
    private bool stickToAnimRequestStop;

    // AnimProgressBar
    private bool lifeAnim;
    private double lifeAnimT;
    private double lifeAnimTMax = 0.25;
    private double lifeAnimInitRatio;
    private double lifeAnimTargetRatio;

    public string tiNom = string.Empty;

    public void Init(SummoningSpecs summoningSpecs, string name = "")
    {
        SummoningSpecs = summoningSpecs.Clone();
        initPending = true;

        tiNom = name;
        GD.Print(tiNom + ": " + SummoningSpecs);
    }

    public override void _Ready()
    {
        gameDataService = GetNode<GameDataService>(GameDataService.Path);
        deskManager = GetNode<DeskManager>(DeskManager.Path);
        MonsterImageLoader = GetNode<MonsterImageLoader>(MonsterImageLoader.Path);
        DragHandler = new ControlDragHandler(this, allowDrag: true);
        DragHandler.OnStartDraggin += DragHandler_OnStartDraggin;
        DragHandler.OnEndDraggin += DragHandler_OnEndDraggin;
    }

    private void DragHandler_OnStartDraggin()
    {
        ForceReleaseStickToAnim();
        PlayerHand.Instance.PutInHand(this);
        deskManager.CardMoving();
    }

    private void DragHandler_OnEndDraggin()
    {
        PlayerHand.Instance.RemoveFromHand(this);
    }

    public override void _Process(double delta)
    {
        if (initPending)
        {
            RedrawMonster();
            initPending = false;
        }

        var deltaF = (float)delta;
        if (stickToAnimRun)
        {
            shiftAnimRun = false;
            StickToAnim(deltaF);
        }
        else
        {
            ShiftAnim(deltaF);
        }

        ChangeProgressAnim(deltaF);
    }

    public override void _Input(InputEvent @event)
    {
        if (IsDraggable)
        {
            DragHandler.HandleInput(@event);
        }
    }

    private void ChangeName(string name)
    {
        NameLabel.Text = name;
        NameLabel.QueueRedraw();
    }

    private void ChangeImageAndIcons(MonsterImageResult imageResult)
    {
        SpeciesImage.Texture = imageResult.SpeciesImage;
        SpeciesImage.QueueRedraw();

        EmotionImage.Texture = imageResult.EmotionImage;
        EmotionImage.QueueRedraw();

        EmotionIconImage.Texture = imageResult.EmotionIconImage;
        EmotionIconImage.QueueRedraw();

        ElementIconImage.Texture = imageResult.ElementIconImage;
        ElementIconImage.QueueRedraw();

        SpeciesIconImage.Texture = imageResult.SpeciesIconImage;
        SpeciesIconImage.QueueRedraw();
    }

    public void RedrawMonster()
    {
        var monsterName = gameDataService.GetMonsterName(SummoningSpecs);

        var fullName = $"[center]{monsterName}[/center]";
        GD.Print(tiNom + ": " + SummoningSpecs + " fullname: " + fullName);
        ChangeName(fullName);

        var imageResult = MonsterImageLoader.GetMonsterImage(SummoningSpecs);
        ChangeImageAndIcons(imageResult);
    }

    internal void TriggerShiftAnim(Vector2 targetPos)
    {
        shiftInitPos = Position;
        shiftTargetPos = targetPos;
        shiftInitRot = RotationDegrees;
        shiftTargetRot = 0;
        shiftT = 0;
        shiftAnimRun = true;
    }

    private void ShiftAnim(double delta)
    {
        if (shiftAnimRun)
        {
            shiftT += delta;
            shiftT = Math.Clamp(shiftT, 0, 1);
            float t = (float)EaseOutElastic(shiftT);
            Position = shiftInitPos.Lerp(shiftTargetPos, t);
            RotationDegrees = Mathf.Lerp(shiftInitRot, shiftTargetRot, t);
        }

        if (shiftT >= 1)
        {
            shiftAnimRun = false;
        }
    }

    internal void TriggerStickToAnim(Vector2 targetPos, Action callback)
    {
        stickToInitPos = Position;
        stickToTargetPos = targetPos;
        stickToInitRot = RotationDegrees;
        stickToTargetRot = 0;
        stickToT = 0;
        stickToAnimRun = true;
        stickToAnimCallback = callback;
    }

    private void StickToAnim(float delta)
    {
        stickToT += delta;
        stickToT = Math.Clamp(stickToT, 0, 1);
        float t = (float)EaseOutElastic(stickToT);
        Position = stickToInitPos.Lerp(stickToTargetPos, t);
        RotationDegrees = Mathf.Lerp(stickToInitRot, stickToTargetRot, t);

        if (stickToAnimRequestStop && stickToT >= 1)
        {
            ForceReleaseStickToAnim();
            stickToAnimCallback();
            stickToAnimCallback = null;
        }
        else if (stickToAnimCallback != null && stickToT >= 1)
        {
            stickToAnimCallback();
            stickToAnimCallback = null;
        }
    }

    public void TriggerChangeProgress(double ratio)
    {
        lifeAnim = true;
        lifeAnimT = 0;

        if (LifeDisplayProgressBar != null)
        {
            lifeAnimInitRatio = (LifeDisplayProgressBar.Value / LifeDisplayProgressBar.MaxValue);
        }
        else
        {
            lifeAnimInitRatio = 1f;
        }

        lifeAnimTargetRatio = ratio;
        GD.Print($"life ratio: {lifeAnimInitRatio} to {lifeAnimTargetRatio}");
    }

    private void ChangeProgressAnim(float delta)
    {
        if (lifeAnim)
        {
            if (LifeDisplayProgressBar == null)
            {
                lifeAnim = false;
                return;
            }

            lifeAnimT += delta;
            var t = lifeAnimT / lifeAnimTMax;
            t = Mathf.Clamp(t, 0, 1);
            var ratio = Mathf.Lerp(lifeAnimInitRatio, lifeAnimTargetRatio, t);
            LifeDisplayProgressBar.Value = LifeDisplayProgressBar.MaxValue * ratio;

            if (t >= 1)
            {
                lifeAnim = false;
            }
        }
    }

    internal void ReleaseStickToAnim()
    {
        stickToAnimRequestStop = true;
    }

    private void ForceReleaseStickToAnim()
    {
        stickToAnimRun = false;
        stickToAnimRequestStop = false;
    }

    private double EaseOutElastic(double x)
    {
        var c4 = (2 * Math.PI) / 3;

        return x <= 0 ? 0 :
               x >= 1 ? 1 :
               Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75) * c4) + 1;
    }


}
