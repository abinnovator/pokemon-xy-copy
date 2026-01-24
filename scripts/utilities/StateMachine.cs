using Godot;
using System;

namespace Game.Utilities
{
	public partial class StateMachine: Node
	{
		[ExportCategory("State Machine vars")]
		[Export] public Node Customer;
		[Export] public State CurrentState;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			foreach (Node child in GetChildren())
			{
				if (child is State state)
				{
					state.StateOwner = Customer;
					state.setProcess(false);
				}
			}
		}

		public
	}
	
}
