using Godot;
using System;

public partial class MainMenu : Control
{
	private double timer1 = 2;
	private double timer2 = 1;

	[Export] private Godot.Collections.Array<Texture2D> slimeBodies;
	[Export] private Godot.Collections.Array<Texture2D> slimeFaces;

	[Export] private Godot.Collections.Array<Texture2D> undeadBodies;
	[Export] private Godot.Collections.Array<Texture2D> undeadFaces;

    [Export] private Godot.Collections.Array<Texture2D> golemBodies;
    [Export] private Godot.Collections.Array<Texture2D> golemFaces;

    [Export] private Godot.Collections.Array<Texture2D> ghostBodies;
    [Export] private Godot.Collections.Array<Texture2D> ghostFaces;

    [Export] private Godot.Collections.Array<Texture2D> fairyBodies;
    [Export] private Godot.Collections.Array<Texture2D> fairyFaces;

    private Sprite2D monster1Body;
    private Sprite2D monster1Face;

    private Sprite2D monster2Body;
    private Sprite2D monster2Face;

    private RandomNumberGenerator rng = new RandomNumberGenerator();
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        monster1Body = GetNode<Sprite2D>("Monster1/Monster1Body");
        monster1Face = GetNode<Sprite2D>("Monster1/Monster1Face");
        monster2Body = GetNode<Sprite2D>("Monster2/Monster2Body");
        monster2Face = GetNode<Sprite2D>("Monster2/Monster2Face");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timer1 -= delta;
        timer2 -= delta;

        if(timer1 <= 0)
		{
            timer1 = 2;
            AnimateMonster1();
        }

        if(timer2 <= 0)
		{
            timer2 = 2;
            AnimateMonster2();
        }
	}

	public void AnimateMonster1()
	{
        switch(rng.RandiRange(0, 4))
        {
            case 0:
                monster1Body.Texture = slimeBodies[rng.RandiRange(0,4)];
                monster1Face.Texture = slimeFaces[rng.RandiRange(0, 4)];
                break;
            case 1:
                monster1Body.Texture = undeadBodies[rng.RandiRange(0, 4)];
                monster1Face.Texture = undeadFaces[rng.RandiRange(0, 4)];
                break;
            case 2:
                monster1Body.Texture = golemBodies[rng.RandiRange(0, 4)];
                monster1Face.Texture = golemFaces[rng.RandiRange(0, 4)];
                break;
            case 3:
                monster1Body.Texture = ghostBodies[rng.RandiRange(0, 4)];
                monster1Face.Texture = ghostFaces[rng.RandiRange(0, 4)];
                break;
            case 4:
                monster1Body.Texture = fairyBodies[rng.RandiRange(0, 4)];
                monster1Face.Texture = fairyFaces[rng.RandiRange(0, 4)];
                break;
        }
        
    }

    public void AnimateMonster2()
    {
        switch(rng.RandiRange(0, 4))
        {
            case 0:
                monster2Body.Texture = slimeBodies[rng.RandiRange(0, 4)];
                monster2Face.Texture = slimeFaces[rng.RandiRange(0, 4)];
                break;
            case 1:
                monster2Body.Texture = undeadBodies[rng.RandiRange(0, 4)];
                monster2Face.Texture = undeadFaces[rng.RandiRange(0, 4)];
                break;
            case 2:
                monster2Body.Texture = golemBodies[rng.RandiRange(0, 4)];
                monster2Face.Texture = golemFaces[rng.RandiRange(0, 4)];
                break;
            case 3:
                monster2Body.Texture = ghostBodies[rng.RandiRange(0, 4)];
                monster2Face.Texture = ghostFaces[rng.RandiRange(0, 4)];
                break;
            case 4:
                monster2Body.Texture = fairyBodies[rng.RandiRange(0, 4)];
                monster2Face.Texture = fairyFaces[rng.RandiRange(0, 4)];
                break;
        }
    }

    public void PressPlay()
    {
        GetTree().ChangeSceneToFile("res://Scenes/spood_scene_2.tscn");
    }

    public void PressQuit()
    {
        GetTree().Quit();
    }
}
