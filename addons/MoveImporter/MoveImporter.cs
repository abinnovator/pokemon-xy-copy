#if TOOLS
using Godot;
using System;
using Godot.Collections;
using Logger = Game.Core.Logger;
using Newtonsoft.Json;
using System.Collections.Generic;
using Game.Core;
using System.Threading.Tasks;
using Game.Gameplay;
using static Game.Core.MovesEnum;


public class ApiResource {
	[JsonProperty("name")]
	public string Name {get; set;}
	[JsonProperty("url")]
	public string Url {get; set;}
}
public class EffectEntry 
{
	[JsonProperty("effect")]
	public string Effect { get; set; }
	[JsonProperty("short_effect")]
	public string ShortEffect { get; set; }
	[JsonProperty("language")]
	public ApiResource Language { get; set; }
	[JsonProperty("version_group")]
	public ApiResource VersionGroup { get; set; }
}
public class MoveMeta
{
	[JsonProperty("ailment")]
	public ApiResource Ailment { get; set; }
	[JsonProperty("category")]
	public ApiResource Category { get; set; }
	[JsonProperty("crit_rate")]
	public int? CritRate { get; set; }
	[JsonProperty("drain")]
	public int? Drain { get; set; }
	[JsonProperty("flinch_chance")]
	public int? FlinchChance { get; set; }
	[JsonProperty("healing")]
	public int? Healing { get; set; }
	[JsonProperty("max_hits")]
	public int? MaxHits { get; set; }
	[JsonProperty("max_turns")]
	public int? MaxTurns { get; set; }
	[JsonProperty("min_hits")]
	public int? MinHits { get; set; }
	[JsonProperty("min_turns")]
	public int? MinTurns { get; set; }
	[JsonProperty("stat_chance")]
	public int? StatChance { get; set; }
	[JsonProperty("ailment_chance")]
	public int? AilmentChance { get; set; }
	
}

public class StatChangeEntry 
{
	[JsonProperty("stat")]
	public ApiResource Stat { get; set; }
	[JsonProperty("change")]
	public int Change { get; set; }
}

public class MoveApiResponse 
{
	[JsonProperty("id")]
	public int Id { get; set; }
	[JsonProperty("name")]
	public string Name { get; set; }
	[JsonProperty("accuracy")]
	public int? Accuracy { get; set; }
	[JsonProperty("effect_chance")]
	public int? EffectChance { get; set; }
	[JsonProperty("pp")]
	public int? Pp { get; set; }
	[JsonProperty("priority")]
	public int Priority { get; set; }
	[JsonProperty("power")]
	public int? Power { get; set; }

	[JsonProperty("target")]
	public ApiResource Target { get; set; }
	[JsonProperty("damage_class")]
	public ApiResource DamageClass { get; set; }
	[JsonProperty("generation")]
	public ApiResource Generation { get; set; }
	[JsonProperty("meta")]
	public MoveMeta Meta { get; set; }
	[JsonProperty("stat_changes")]
	public List<StatChangeEntry> StatChanges { get; set; }
	[JsonProperty("type")]
	public ApiResource Type { get; set; }
	[JsonProperty("effect_entries")]
	public List<EffectEntry> EffectEntries { get; set; }

}

[Tool]
public partial class MoveImporter : EditorPlugin
{
	private const string importMenuItemText = "Import Moves";
	private const string folderPath = "res://resources/moves/";
	public const string apiPath= "https://pokeapi.co/api/v2/move/";
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
		AddToolMenuItem(importMenuItemText, Callable.From(ImportMoves));
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
		RemoveToolMenuItem(importMenuItemText);

	}

	public async void ImportMoves(){
		Logger.Info("Attempting to fetch pokemon moves");
		DirAccess.MakeDirRecursiveAbsolute(ProjectSettings.GlobalizePath(folderPath));
		const int gcInterval = 10;
		for (int i=1 ; i <= Globals.MOVE_NUMBERS ; i++){
			Logger.Info($"Processing move with id: {i}");

			// MoveApiResponse data = await 
			MoveApiResponse data = await Modules.FetchDataFromPokeApi<MoveApiResponse>($"{apiPath}{i}");
			if (data == null){
				Logger.Error($"Failed to fetch data for move with id: {i}");
				continue;
			}
			var generation = data.Generation?.Name;
			var moveName = data.Name;
			if (string.IsNullOrEmpty(moveName)){
				Logger.Error($"Failed to fetch data for move with id: {i}");
				continue;
			}
			Logger.Info($"Creating Resource for move: {moveName}");
			// Create move resource here
			CreateMoveResource(moveName, data);

			if (i % gcInterval==0)
			{
				GC.Collect();
				GC.WaitForPendingFinalizers();
				Logger.Info($"Collected garbage after move {i}");
			}
			
			await Task.Delay(100);
		}
	}
	private void CreateMoveResource(string moveName, MoveApiResponse apiData){
		var move = new MoveResource(){
			Name = moveName,
			PokemonType = PokemonEnum.TypeMap.TryGetValue(apiData.Type?.Name ?? "", out var type) ? type : PokemonType.None,
			Category = MovesEnum.MovesMaps.CategoryMap.TryGetValue(apiData.DamageClass?.Name ?? "", out var category) ? category : MovesEnum.MoveCategory.Physical,
			Target = MovesEnum.MovesMaps.MoveTargetMap.TryGetValue(apiData.Target?.Name ?? "", out var target) ? target : MovesEnum.MoveTarget.SelectedPokemon,


			Accuracy = apiData.Accuracy ?? 0,
			Power = apiData.Power ?? 0,
			PP = apiData.Pp ?? 0,
			CritRate = apiData.Meta?.CritRate ?? 0,
			Drain = apiData.Meta?.Drain ?? 0,
			FlinchChance = apiData.Meta?.FlinchChance ?? 0,
			Healing = apiData.Meta?.Healing ?? 0,
			MaxHits = apiData.Meta?.MaxHits ?? -1,
			MinHits = apiData.Meta?.MinHits ?? -1,
			MaxTurns = apiData.Meta?.MaxTurns ?? -1,
			MinTurns = apiData.Meta?.MinTurns ?? -1,
			AilmentChance = apiData.Meta?.AilmentChance ?? 0,
			Ailment = PokemonEnum.AilmentMap.TryGetValue(apiData.Meta?.Ailment?.Name ?? "", out var ailment) ? ailment : PokemonAilment.None,
			StatChanges = []
		};
		if (apiData.StatChanges != null){
			foreach (var change in apiData.StatChanges){
				if (PokemonEnum.StatMap.TryGetValue(change.Stat?.Name ?? "", out var stat)){
					move.StatChanges[stat] = change.Change;
				}
			}
		}

		var savePath = $"{folderPath}{moveName.ToLower()}.tres";
		var result = ResourceSaver.Save(move, savePath);
		if (result != Error.Ok){
			Logger.Error($"Failed to save move resource for {moveName}: {result}");
		}
		else{
			Logger.Info($"Successfully saved move resource for {moveName} to {savePath}");
		}
	}
}
#endif
