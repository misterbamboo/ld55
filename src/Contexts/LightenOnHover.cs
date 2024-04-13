using Godot;

public partial class LightenOnHover : ColorRect
{
    [Export] private float lightenAmount = 1.2f;
    [Export] private double fadeTimeInSeconds = 0.2f;

    private Color originalColor;
    private Color lightenColor;

    private bool lighten = false;
    private double t = 0;

    public override void _Ready()
    {
        originalColor = Color;
        lightenColor = originalColor * lightenAmount;
    }

    public override async void _Process(double delta)
    {
        if(lighten && Color != lightenColor)
        {
            LerpColor(originalColor, lightenColor, t/fadeTimeInSeconds);
            t += (float)delta;
            if(t > fadeTimeInSeconds)
            {
                Color = lightenColor;
            }
        }
        else if(!lighten && Color != originalColor)
        {
            LerpColor(lightenColor, originalColor, t/fadeTimeInSeconds);
            t += (float)delta;
            if(t > fadeTimeInSeconds)
            {
                Color = originalColor;
            }
        }
    }

    public void OnPointerEnter()
    {
        lighten = true;
        t = 0;
    }

    public void OnPointerExit()
    {
        lighten = false;
        t = 0;
    }

    public void LerpColor(Color color1, Color color2, double t)
    {
        Color = color1.Lerp(color2, (float)t);
    }
}
