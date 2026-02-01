using Godot;
using System;

namespace Game.Core // Fixed capitalization to match Logger
{
	public partial class Globals : Node
	{
		public static Globals Instance { get; private set; }

		[ExportCategory("Gameplay")]
		[Export] public int GridSize = 16; // Standard C# naming is PascalCase
		[Export]
		public ulong Seed = 1337;

		private RandomNumberGenerator randomNumberGenerator;

		public override void _Ready()
		{
			Instance = this;

			randomNumberGenerator = new()
			{
				Seed = Seed
			};
			Logger.Info("Loading globals...");
		}
		public static RandomNumberGenerator GetRandomNumberGenerator()
		{
			return Instance.randomNumberGenerator;
		} 
	}
}
