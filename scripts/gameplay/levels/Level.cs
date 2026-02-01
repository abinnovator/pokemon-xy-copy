using Godot;
using System;
using Game.Core;
using Logger = Game.Core.Logger;

namespace Game.Gameplay
{
	public partial class Level : Node2D
	{
		[ExportCategory("Level Basics")]
		[Export]
		public LevelName LevelName;

		[Export(PropertyHint.Range, "0,100")]
		public int encounterRate;
		[ExportCategory("Camera Limits")]
		[Export]
		public int top;
		[Export]
		public int bottom;
		[Export]
		public int left;
		[Export]
		public int right;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Logger.Info($"Level {LevelName} loaded");
		}
	}

}
