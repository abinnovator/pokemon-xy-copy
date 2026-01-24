using Godot;
using System;
using Logger = Game.Core.Logger;
using Game.Core;

namespace Game.Gameplay
{
	public partial class CharacterMovement : Node
	{
		// Signal definition - The "EventHandler" suffix is removed by Godot for the SignalName
		[Signal] public delegate void AnimationEventHandler(string animationType);

		[ExportCategory("Nodes")]
		[Export] public Node2D Character;

		[Export] public CharecterInput CharacterInput; 
		[Export] public CharacterCollisionRaycast CharacterCollisionRaycast;

		[ExportCategory("Movement Settings")] 
		[Export] public Vector2 TargetPosition = Vector2.Down;

		[Export] public bool IsWalking = false;
		[Export] public bool CollisionDetected = false;

		public override void _Ready()
		{
			// Connect to signals using the C# event syntax or method names
			CharacterInput.Walk += StartWalking;
			CharacterInput.Turn += Turn; // Fixed to match method name below

			CharacterCollisionRaycast.Collision += (value) =>CollisionDetected = value;

			Logger.Info("CharacterMovement ready");
		}

		public override void _Process(double delta)
		{
			Walk(delta);
		}

		public bool IsMoving() 
		{
			return IsWalking;
		}
		public bool IsColliding() 
		{
			return CollisionDetected;
		}

		public void StartWalking()
		{
			if (!IsMoving() && !IsColliding())
			{
				EmitSignal(SignalName.Animation, "walk");
				TargetPosition = Character.Position + CharacterInput.Direction * Globals.Instance.GridSize;
				
				Logger.Info($"Moving from {Character.Position} to {TargetPosition}");
				IsWalking = true;
			}
		}

		public void Walk(double delta)
		{
			if (IsWalking)
			{
				float moveSpeed = (float)delta * Globals.Instance.GridSize * 4;
				Character.Position = Character.Position.MoveToward(TargetPosition, moveSpeed);

				if (Character.Position.DistanceTo(TargetPosition) < 0.1f)
				{
					StopWalking();
				}
			}
			else
			{
				// Careful: Putting EmitSignal in _Process else will fire every frame.
				// It is better to emit "idle" inside StopWalking().
			}
		}

		public void Turn()
		{
			EmitSignal(SignalName.Animation, "turn");
		}

		public void StopWalking()
		{
			IsWalking = false;
			SnapPositionToGrid();
			EmitSignal(SignalName.Animation, "idle");
		}

		public void SnapPositionToGrid()
		{
			int gridSize = Globals.Instance.GridSize;
			float snappedX = Mathf.Round(Character.Position.X / gridSize) * gridSize;
			float snappedY = Mathf.Round(Character.Position.Y / gridSize) * gridSize;
			Character.Position = new Vector2(snappedX, snappedY);
		}
	}
}
