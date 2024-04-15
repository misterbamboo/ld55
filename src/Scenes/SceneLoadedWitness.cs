using Godot;
using System;

public partial class SceneLoadedWitness : Node
{
	private DeskManager deskManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		deskManager = GetNode<DeskManager>(DeskManager.Path);
		deskManager.SceneLoaded();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
