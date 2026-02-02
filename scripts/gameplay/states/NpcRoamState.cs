using Godot;
using System;
using Game.Utilities;
using Game.Core;

namespace Game.Gameplay;

public partial class NpcRoamState : State
{
	[ExportCategory("State Vars")]
	[Export] 
	public NpcInput NpcInput;

	[Export]
	public CharacterMovement CharacterMovement;
	
	private double timer = 2f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (CharacterMovement.IsMoving())
		{
			return;
		}
		switch (NpcInput.NpcInputConfig.NpcMovementType)
		{
			case NpcMovementType.Wander:
				HandleWander(delta, NpcInput.NpcInputConfig.WanderMoveInterval);
				break;
			case NpcMovementType.LookAround:
				HandleLookAround(delta, NpcInput.NpcInputConfig.LookAroundInterval);
				break;
		}
	}
	private void HandleWander(double delta, double interval)
	{
		timer -= delta;
		if (timer > 0)
		{
			return;
		}
		var (direction,targetPosition) = GetNewDirections();
		NpcInput.Direction = direction;
		NpcInput.TargetPosition = targetPosition;
		NpcInput.EmitSignal(CharecterInput.SignalName.Walk);
		timer = interval;

	}
	private void HandleLookAround(double delta, double interval)
	{
		timer -= delta;
		if (timer > 0)
		{
			return;
		}
		var (direction,targetPosition) = GetNewDirections();
		if (direction == NpcInput.Direction)
		{
			timer = interval;
			return;
		}
		NpcInput.Direction = direction;
		NpcInput.TargetPosition = targetPosition;
		NpcInput.EmitSignal(CharecterInput.SignalName.Turn);
		timer = interval;
	}
	private (Vector2, Vector2) GetNewDirections()
	{
		Vector2[] directions = [Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right];
		Vector2 chosenDirection;
		int tries = 0;
		do
		{
			chosenDirection = directions[Globals.GetRandomNumberGenerator().RandiRange(0,directions.Length-1)];
			Vector2 nextPosition = CharacterMovement.Character.Position + chosenDirection * Globals.GridSize;
			if (NpcInput.NpcInputConfig.NpcMovementType == NpcMovementType.Wander)
			{
				float distanceFromOrigin = nextPosition.DistanceTo(NpcInput.NpcInputConfig.WanderOrigin);
				if (distanceFromOrigin <= NpcInput.NpcInputConfig.WanderRadius)
				{
					break;
				}
			}else {
				break;
			}
			tries++;
		}while (tries<10);
		return (chosenDirection, chosenDirection * Globals.GridSize);
	}
}
