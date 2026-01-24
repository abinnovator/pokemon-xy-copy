using Godot;
using System;


namespace Game.Utilities
{
	public abstract partial class State : Node
	{
		[Export] public Node StateOwner;

        public virtual void EnterState() {
            Logger.Info($"Entering {this.GetType().Name}");
        }
        public virtual void ExitState() {
            Logger.Info($"Exiting {this.GetType().Name}");
        }
}