using Godot;

public partial class PropertyBarUI : Node
{
	private Slider primarySlider;
	private Slider secondarySlider;

	[Export] private Godot.Collections.Array<Texture2D> Emotions;
	[Export] private Godot.Collections.Array<Texture2D> Species;
	[Export] private Godot.Collections.Array<Texture2D> Elements;
	[Export] private Texture2D deadzoneTexture;
	[Export] private HBoxContainer textureRects;

	private SpecTypes spectype;
	private int deadzone = 0;

    public override void _Ready()
	{
		primarySlider = GetNode<Slider>("PrimarySlider");
		secondarySlider = GetNode<Slider>("SecondarySlider");
	}

	public void Init(SpecTypes spectype, int deadzone)
	{
		this.spectype = spectype;
		var textures = spectype switch
		{
            SpecTypes.Emotion => Emotions,
            SpecTypes.Species => Species,
            SpecTypes.Element => Elements,
            _ => throw new System.NotImplementedException()
        };


		var children = textureRects.GetChildren();
		var actual_i = 0;
		for(var i = 0; i< children.Count;i++)
		{
            if(i == deadzone)
			{
                (children[i] as TextureRect).Texture = deadzoneTexture;
			}
			else
			{
                (children[i] as TextureRect).Texture = textures[actual_i];
                actual_i++;
            }
        }
	}

	public void SetValue(double value)
	{
        primarySlider.Value = value * 100;

        secondarySlider.Visible = false;
    }

	public void SetPreview(double value)
	{
        secondarySlider.Value = value * 100;
		
		if(primarySlider.Value != secondarySlider.Value)
		{
            secondarySlider.Visible = true;
        }
	}
}
