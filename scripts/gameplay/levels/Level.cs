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
		public AStarGrid2D Grid;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Logger.Info($"Level {LevelName} loaded");
		}
		public override void _Process(double delta)
		{
			if (Grid == null && GameManager.GetPlayer() != null){
				SetupGrid();
			}

			if (Grid != null ){
				QueueRedraw();
			}
		}

		public void SetupGrid(){
			Grid = new(){
				Region = new Rect2I(0,0, right, bottom),
				CellSize = new Vector2(Globals.GridSize, Globals.GridSize),
				DefaultComputeHeuristic = AStarGrid2D.Heuristic.Manhattan,
				DefaultEstimateHeuristic = AStarGrid2D.Heuristic.Manhattan,
				DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never
				
			};

			Grid.Update();

			var mapHeight = bottom / Globals.GridSize;
			var mapWidth = right / Globals.GridSize;

			for (int y = 0; y < mapHeight; y++){
				for (int x = 0; x < mapWidth; x++){
					Vector2I cell = new(x,y);
					Vector2 worldPosition = new(x* Globals.GridSize, y * Globals.GridSize);

					var (_, collisions) = GameManager.GetPlayer().GetNode<CharacterMovement>("Movement").GetTargetColliders(worldPosition);
					foreach (var collision in collisions){
						var collider = (Node)(GodotObject)collision["collider"];
						var colliderType = collider.GetType().Name;
						if (colliderType == "TallGrass" || colliderType == "Player"){
							continue;
						}
						if (colliderType == "Npc"){
							switch (((Npc)collider).NpcInputConfig.NpcMovementType){
								case NpcMovementType.Patrol:
									continue;
								case NpcMovementType.Wander:
									continue;				
							}
						}
						Grid.SetPointSolid(cell,true);

						
					}
				}
			}

		}
		public override void _Draw(){
			if (Grid == null)
			{
				return;
			}
			var mapHeight = bottom / Globals.GridSize;
			var mapWidth = right / Globals.GridSize;

			for (int y = 0; y < mapHeight; y++){
				for (int x = 0; x < mapWidth; x++){
					Vector2I cell = new(x,y);
					Vector2 worldPosition = new(x* Globals.GridSize, y * Globals.GridSize);

					var color = Grid.IsPointSolid(cell) ? new Color(1,0,0,0.3f) : new Color(0,1,0,0.3f);
					DrawRect(new Rect2(worldPosition, Grid.CellSize), color, filled: true);
					
				}
			}
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
