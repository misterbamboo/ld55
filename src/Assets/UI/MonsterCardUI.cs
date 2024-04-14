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

    private MonsterImageLoader MonsterImageLoader { get; set; }
    public SummoningSpecs MonsterSpecs { get; set; } = new SummoningSpecs();

    private SpecDefinition Emotion { get; set; } = SpecDefinition.Empty();
    private SpecDefinition Element { get; set; } = SpecDefinition.Empty();
    private SpecDefinition Species { get; set; } = SpecDefinition.Empty();

    private ControlDragHandler DragHandler { get; set; }

    // ShiftAnim
    Vector2 shiftInitPos;
    Vector2 shiftTargetPos;
    float shiftInitRot;
    float shiftTargetRot;
    private double shiftT;
    private bool shiftAnimRun;

    public void Init(SummoningSpecs monsterSpecs, SpecDefinition emotion, SpecDefinition element, SpecDefinition species)
    {
        MonsterSpecs = monsterSpecs;
        Emotion = emotion;
        Element = element;
        Species = species;
    }

    public override void _Ready()
    {
        MonsterImageLoader = GetNode<MonsterImageLoader>(MonsterImageLoader.Path);
        DragHandler = new ControlDragHandler(this, allowDrag: true);
        RedrawMonster();
    }

    public override void _Process(double delta)
    {
        ShiftAnim((float)delta);
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
    }

    private void ChangeImageAndIcons(MonsterImageResult imageResult)
    {
        SpeciesImage.Texture = imageResult.SpeciesImage;
        EmotionImage.Texture = imageResult.EmotionImage;
        EmotionIconImage.Texture = imageResult.EmotionIconImage;
        ElementIconImage.Texture = imageResult.ElementIconImage;
        SpeciesIconImage.Texture = imageResult.SpeciesIconImage;
    }

    public void RedrawMonster()
    {
        ChangeName($"[center]{Emotion.MonsterNaming} {Element.MonsterNaming} {Species.MonsterNaming}[/center]");

        var imageResult = MonsterImageLoader.GetMonsterImage(MonsterSpecs);
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

    private double EaseOutElastic(double x)
    {
        var c4 = (2 * Math.PI) / 3;

        return x <= 0 ? 0 :
               x >= 1 ? 1 :
               Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75) * c4) + 1;
    }
}
