using Godot;
using System;

namespace Game.Core
{

	public partial class GameManager : Node
	{
		public static GameManager Instance {get;private set;}

		[ExportCategory("Nodes")]
		[Export]

		public SubViewport GameViewPort ;
		[ExportCategory("Vars")]
		[Export]
		public Player Player;


		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Instance = this;
			Logger.Info("GameManager ready");
			SceneManager.ChangeLevel(spawn: true);
		}

		public static SubViewport GetGameViewPort()
		{
			return Instance.GameViewPort;
		}
		public static Player AddPlayer(Player player)
		{
			Instance.GameViewPort.AddChild(player);
			Instance.Player = player;
			return Instance.Player;
		}
		public static Player GetPlayer()
		{
			return Instance.Player;
		}
	}
	

}
