using Game.Core;
using Godot;
using System;

namespace Game.Gameplay;

public partial class TallGrass : Area2D
{
	[Export]
	public AnimatedSprite2D AnimatedSprite;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AnimatedSprite ??= GetNode<AnimatedSprite2D>("AnimatedSprite2D");	
		BodyEntered += onBodyEntered;
		BodyExited += onBodyExited;
	}
	public void onBodyEntered(Node2D node2d){
		var className = node2d.GetType().Name;
		switch (className){
			case "Player":
				CalculateEncounterChance();
				break;
		}
		AnimatedSprite.Play("down");
	}
	public void onBodyExited(Node2D node2d){
		AnimatedSprite.Play("up");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void CalculateEncounterChance()
	{
		int rate = SceneManager.GetCurrentLevel().encounterRate;
		int chance = Globals.GetRandomNumberGenerator().RandiRange(0, 100);
		if (chance <= rate)
		{
			MessageManager.PlayText("You encountered a wild Pokemon!");
		}
		
	
	}
}
