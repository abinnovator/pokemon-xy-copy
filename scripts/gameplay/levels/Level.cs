using Godot;
using System;
using Game.Core;
using Logger = Game.Core.Logger;
using System.Collections.Generic;

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

		public readonly HashSet<Vector2> reservedTiles = [];

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Logger.Info($"Level {LevelName} loaded");
		}
		public bool ReservedTile(Vector2 position)
		{
			if (reservedTiles.Contains(position)){
				return false;
			}
			reservedTiles.Add(position);
			return true;
		}
		public bool IsTileFree(Vector2 position)
		{
			return !reservedTiles.Contains(position);
		}
		public void ReleaseTile(Vector2 position)
		{
			reservedTiles.Remove(position);
		}
	}

}
