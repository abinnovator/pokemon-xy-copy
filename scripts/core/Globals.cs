using Godot;
using System;

namespace Game.Core // Fixed capitalization to match Logger
{
	public partial class Globals : Node
	{
		public static Globals Instance { get; private set; }

		[ExportCategory("Gameplay")]
		[Export] public int GridSize = 16; // Standard C# naming is PascalCase

		public override void _Ready()
		{
			Instance = this;
			Logger.Info("Loading globals...");
		}
	}
}
