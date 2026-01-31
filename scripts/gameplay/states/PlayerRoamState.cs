using Game.Core;
using Game.Utilities;
using Godot;
using System;

namespace Game.Gameplay.States
{
	public partial class PlayerRoamState : State
	{
		[ExportCategory("State vars")]
		[Export] public PlayerInput PlayerInput;
		[Export] public CharacterMovement CharacterMovement;
		public override void _Ready ()
		{
			Signals.Instance.MessageBoxOpen += (value) => {
				if (value)
				{
					StateMachine.ChangeState("Message");
				}
			};
		}
		public override void _Process(double delta) {
			GetInputDirection();
			GetInput(delta);
		 }
		public void GetInputDirection() 
		{
			if ( Input.IsActionJustPressed("ui_up"))
			{
				PlayerInput.Direction = Vector2.Up;
				PlayerInput.TargetPosition = new Vector2(0, -Globals.Instance.GridSize);
			}
			else if ( Input.IsActionJustPressed("ui_down"))
			{
				PlayerInput.Direction = Vector2.Down;
				PlayerInput.TargetPosition = new Vector2(0, Globals.Instance.GridSize);
			}
			else if ( Input.IsActionJustPressed("ui_left"))
			{
				PlayerInput.Direction = Vector2.Left;
				PlayerInput.TargetPosition = new Vector2(-Globals.Instance.GridSize, 0);
			}
			else if ( Input.IsActionJustPressed("ui_right"))
			{
				PlayerInput.Direction = Vector2.Right;
				PlayerInput.TargetPosition = new Vector2(Globals.Instance.GridSize, 0);
			}
			else {}
		 }
		public void GetInput (double delta){

			if (CharacterMovement.IsMoving()){
				return;
			}
			if(Modules.IsActionJustReleased()){
				if (PlayerInput.HoldTime > PlayerInput.HoldThreshhold){
					PlayerInput.EmitSignal(CharecterInput.SignalName.Walk);
				}
				else{
					PlayerInput.EmitSignal(CharecterInput.SignalName.Turn);
				}
				PlayerInput.HoldTime = 0.0f;

			}
			if(Modules.IsActionPressed()){
				PlayerInput.HoldTime += delta;

				if (PlayerInput.HoldTime > PlayerInput.HoldThreshhold){
					PlayerInput.EmitSignal(CharecterInput.SignalName.Walk);
				}
			}
		}
	}
}
