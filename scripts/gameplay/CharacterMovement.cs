using Godot;
using System;
using Logger = Game.Core.Logger;
using Game.Core; // <--- ADD THIS LINE

namespace Game.Gameplay
{
	public partial class CharacterMovement : Node
	{
		[Signal] public delegate void AnimationEventHandler(string animationType);

		[ExportCategory("Nodes")]
		[Export] public Node2D Character;
		[Export] public CharecterInput CharacterInput; // Fixed typo: Charecter -> Character (ensure this matches your class name)

		[ExportCategory("Movement Settings")] 
		[Export] public Vector2 TargetPosition = Vector2.Down;
		[Export] public bool IsWalking = false;

		public override void _Ready()
		{
			CharacterInput.Walk += StartWalking;
			CharacterInput.Turn += StartTurn;
			Logger.Info("CharacterMovement ready");
		}

		public override void _Process(double delta)
		{
			// Usually you call Walk(delta) here if you want constant movement checking
			Walk(delta);
		}

		public bool IsMoving() // Capitalized for C# convention
		{
			return IsWalking;
		}

		public void StartWalking()
		{
			if (!IsMoving())
			{
				EmitSignal(SignalName.AnimationEventHandler, "walk");
				// Calculate next grid position
				TargetPosition = Character.Position + CharacterInput.Direction * Globals.Instance.GridSize;
				
				// Fixed String Interpolation: Uses { } inside $" " 
				Logger.Info($"Moving from {Character.Position} to {TargetPosition}");
				
				IsWalking = true;
				// TODO: Emit Walk Animation signal
			}
		}

		public void Walk(double delta)
		{
			if (IsWalking)
			{
				// Move toward target at a set speed (GridSize * 4 per second)
				float moveSpeed = (float)delta * Globals.Instance.GridSize * 4;
				Character.Position = Character.Position.MoveToward(TargetPosition, moveSpeed);

				// Check if we arrived (Distance is less than a small threshold)
				if (Character.Position.DistanceTo(TargetPosition) < 0.1f)
				{
					StopWalking();
				}
			}
			else
			{
				EmitSignal(SignalName.AnimationEventHandler, "idle");
			}
		}
		public void Turn()
		{
			EmitSignal(SignalName.AnimationEventHandler, "turn");
		}

		public void StopWalking()
		{
			IsWalking = false;
			SnapPositionToGrid();
			// TODO: Emit Idle Animation signal
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
