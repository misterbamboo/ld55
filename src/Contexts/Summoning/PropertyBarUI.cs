using Godot;

public partial class PropertyBarUI : Node
{
	private Slider primarySlider;
	private Slider secondarySlider;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		primarySlider = GetNode<Slider>("PrimarySlider");
		secondarySlider = GetNode<Slider>("SecondarySlider");
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
