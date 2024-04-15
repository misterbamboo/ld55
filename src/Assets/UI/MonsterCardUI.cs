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

	public SummoningSpecs SummoningSpecs { get; private set; } = new SummoningSpecs();

	private MonsterImageLoader MonsterImageLoader { get; set; }
	private SpecDefinition Emotion { get; set; } = SpecDefinition.Empty();
	private SpecDefinition Element { get; set; } = SpecDefinition.Empty();
	private SpecDefinition Species { get; set; } = SpecDefinition.Empty();

	private ControlDragHandler DragHandler { get; set; }
	public bool IsAnimating => shiftAnimRun || stickToAnimRun;

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

	public void Init(SummoningSpecs monsterSpecs, SpecDefinition emotion, SpecDefinition element, SpecDefinition species)
	{
		SummoningSpecs = monsterSpecs;
		Emotion = emotion;
		Element = element;
		Species = species;
	}

	public override void _Ready()
	{
		MonsterImageLoader = GetNode<MonsterImageLoader>(MonsterImageLoader.Path);
		DragHandler = new ControlDragHandler(this, allowDrag: true);
		DragHandler.OnStartDraggin += DragHandler_OnStartDraggin;
		DragHandler.OnEndDraggin += DragHandler_OnEndDraggin;
		RedrawMonster();
	}
	private void DragHandler_OnStartDraggin()
	{
		ForceReleaseStickToAnim();
		PlayerHand.Instance.PutInHand(this);
	}

	private void DragHandler_OnEndDraggin()
	{
		PlayerHand.Instance.RemoveFromHand(this);
	}

	public override void _Process(double delta)
	{
		if (stickToAnimRun)
		{
			shiftAnimRun = false;
			StickToAnim((float)delta);
		}
		else
		{
			ShiftAnim((float)delta);
		}
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
