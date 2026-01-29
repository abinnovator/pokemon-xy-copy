using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using Game.Gameplay;
using System.Collections.Generic;
using Godot.Collections;
using Game.Core;

namespace Game.Core
{
	public partial class SceneManager : Node
	{
		public static SceneManager Instance {get; private set;}
		public static bool isChanging {get; private set;}
		[ExportCategory("Scene Manager Vars")]
		[Export]

		public ColorRect FadeRect {get; private set;}
		[Export]
		public Level CurrentLevel;
		[Export]
		public Array<Level> AllLevels;
		public override void _Ready()
		{
			Instance = this;
			isChanging = false;
			
			if (AllLevels == null)
			{
				AllLevels = new Array<Level>();
			}

			// Fix: Create FadeRect programmatically if null (because autoload is a script, not a scene)
			if (FadeRect == null)
			{
				var canvasLayer = new CanvasLayer();
				canvasLayer.Layer = 100; // Ensure it's on top
				AddChild(canvasLayer);
				
				FadeRect = new ColorRect();
				FadeRect.Color = new Color(0, 0, 0, 0); // Transparent black
				FadeRect.SetAnchorsPreset(Control.LayoutPreset.FullRect);
				canvasLayer.AddChild(FadeRect);
			}

			Game.Core.Logger.Info("Scene Manager Ready");
		}

		public static async void ChangeLevel(LevelName levelName = LevelName.small_town, int trigger = 0, bool spawn = false)
		{
			Game.Core.Logger.Info($"ChangeLevel called for {levelName}. isChanging: {isChanging}");
			while (isChanging){
				await Instance.ToSignal(Instance.GetTree(), "process_frame");
			}
			isChanging = true;
			
			try
			{
				await Instance.GetLevel(levelName);
				if (spawn)
				{
					Instance.Spawn();
				}
				else
				{
					Instance.Switch(trigger);
				}
				await Instance.FadeIn();
			}
			catch (Exception e)
			{
				Game.Core.Logger.Error($"Error changing level: {e.Message}\n{e.StackTrace}");
			}
			finally
			{
				isChanging = false;
				Game.Core.Logger.Info("ChangeLevel finished, isChanging set to false");
			}
		}

		public async Task GetLevel(LevelName levelName)
		{
			Game.Core.Logger.Info($"Getting Level: {levelName}");
			if (AllLevels == null)
			{
				Game.Core.Logger.Warning("AllLevels list was null, initializing new list.");
				AllLevels = new Array<Level>();
			}

			if (CurrentLevel != null){
				await Instance.FadeOut();
				GameManager.GetGameViewPort().RemoveChild(CurrentLevel);
			}
			
			// Filter out nulls and find level
			CurrentLevel = AllLevels.Where(l => l != null).FirstOrDefault(level => level.LevelName == levelName);
			if (CurrentLevel !=null)
			{
				GameManager.GetGameViewPort().AddChild(CurrentLevel);
			}
			else {
				Game.Core.Logger.Error("Level not found, loading from resource");
				CurrentLevel = GD.Load<PackedScene>("res://scenes/levels/" + levelName.ToString() + ".tscn").Instantiate<Level>();
				AllLevels.Add(CurrentLevel);
				GameManager.GetGameViewPort().AddChild(CurrentLevel);
			}

		}
		public void Spawn ()
		{
			var spawnPoints = CurrentLevel.GetTree().GetNodesInGroup(LevelGroups.SPAWNPOINTS.ToString());
			if (spawnPoints.Count <= 0){
				throw new Exception("No spawn points found in level"); 
				
			}
			// Fix 2 & 3: Cast to Node2D (or SpawnPoint if that class exists) to access .Position
			var spawnPoint = (Node2D)spawnPoints[0];
			var player = GD.Load<PackedScene>("res://scenes/charecters/player.tscn").Instantiate<Player>();
 
			GameManager.AddPlayer(player);
			GameManager.GetPlayer().Position = spawnPoint.Position;
			
		}
 
		public void Switch(int trigger)
		{
			var sceneTriggers = CurrentLevel.GetTree().GetNodesInGroup(LevelGroups.SCENETRIGGERS.ToString());
			if (sceneTriggers.Count <= 0){
				throw new Exception("No spawn triggers found in level"); 
				
			}
			if (sceneTriggers.FirstOrDefault(st => ((SceneTrigger)st).CurrentLevelTrigger == trigger) is not SceneTrigger sceneTrigger)
			{
				throw new Exception("No scene trigger found in level"); 
 
			}   
			// Fix 4: Assuming GridSize is the correct property name on Globals
			GameManager.GetPlayer().Position = sceneTrigger.Position + sceneTrigger.EntryDirection * Globals.Instance.GridSize;
		}
 
 
		public async Task FadeOut()
		{
			if (FadeRect == null) { Game.Core.Logger.Error("FadeRect is null in FadeOut"); return; }
			Tween tween = CreateTween();
			tween.TweenProperty(FadeRect, "color:a", 1.0, 0.25);
			await ToSignal(tween, "finished");
		}
		public async Task FadeIn()
		{
			 if (FadeRect == null) { Game.Core.Logger.Error("FadeRect is null in FadeIn"); return; }
			Tween tween = CreateTween();
			tween.TweenProperty(FadeRect, "color:a", 0.0, 0.25);
			await ToSignal(tween, "finished");
		}


	}
}
