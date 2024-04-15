using Godot;

public partial class ArcaneFocusUI : Control
{
    private DeskManager deskManager;

    private Slider hSlider;
    private Slider vSlider;

    private RandomNumberGenerator rng = new RandomNumberGenerator();

    private float hTarget = 0;
    private float vTarget = 0;

    private float hPower = 0;
    private float vPower = 0;
    public float TotalPower => (hPower + vPower) / 200;
    private Ingredient currentIngredient = null;

    public override void _Ready()
    {
        deskManager = GetNode<DeskManager>(DeskManager.Path);
        hSlider = GetNode<Slider>("HSlider");
        vSlider = GetNode<Slider>("VSlider");

        deskManager.OnArcaneFocusFilled += IngredientInserted;
        deskManager.OnArcaneFocusEmptied += IngredientRemoved;
    }

    public void DragStart()
    {
        deskManager.ArcaneFocusDragStart();
    }

    public void DragEnd(bool valueChanged)
    {
        deskManager.ArcaneFocusDragEnd();
    }

    public void OnHSliderValueChanged(float value)
    {
        if (currentIngredient == null) return;

        hPower = percentDistance(hTarget, value);
        currentIngredient.Processing = TotalPower;
        deskManager.AdjustArcaneFocus(currentIngredient);
    }

    public void OnVSliderValueChanged(float value)
    {
        if (currentIngredient == null) return;

        vPower = percentDistance(vTarget, value);
        currentIngredient.Processing = TotalPower;
        deskManager.AdjustArcaneFocus(currentIngredient);
    }

    public void IngredientInserted(Ingredient ingredient)
    {
        currentIngredient = ingredient;
        hTarget = rng.RandfRange(0, 100);
        vTarget = rng.RandfRange(0, 100);

        hPower = 0;
        vPower = 0;
    }

    public void IngredientRemoved()
    {
        currentIngredient = null;
        hTarget = 0;
        vTarget = 0;

        hPower = 0;
        vPower = 0;
    }

    public float percentDistance(float target, float value)
    {
        var distanceToTarget = Mathf.Abs(target - value);
        return 100 - distanceToTarget;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        deskManager.OnArcaneFocusFilled -= IngredientInserted;
        deskManager.OnArcaneFocusEmptied -= IngredientRemoved;
    }
}
