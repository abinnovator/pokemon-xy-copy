#if TOOLS
using Game.Core;
using Godot;
using System;
using System.Threading.Tasks;
using Logger = Game.Core.Logger;


[Tool]
public partial class PokemonImporter : EditorPlugin
{
	private const string importMenuItemText = "Import Pokemon";
	private const string folderPath = "res://resources/pokemon/";
	private const string spriteFolderPath = "res://assets/pokemon/";
	public const string apiPath= "https://pokeapi.co/api/v2/pokemon/?offset=0&limit=200";
	public const string menuIconApiPath = "https://img.pokemondb.net/sprites/lets-go-pikachu-eevee/normal/";
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
		AddToolMenuItem(importMenuItemText, Callable.From(ImportPokemon));
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
		RemoveToolMenuItem(importMenuItemText);
	}
	public async void ImportPokemon(){
		Logger.Info("Attempting to import pokemon.");
		DirAccess.MakeDirRecursiveAbsolute(ProjectSettings.GlobalizePath(folderPath));
		DirAccess.MakeDirRecursiveAbsolute(ProjectSettings.GlobalizePath(spriteFolderPath));

		const int gcInterval = 10;
		for (int i=1 ; i<=Globals.POKEMON_NUMBERS ; i++){
			MoveApiResponse pokemonData = await Modules.FetchDataFromPokeApi<MoveApiResponse>($"{apiPath}{i}");
			if (i % gcInterval == 0){
				GC.Collect();
				GC.WaitForPendingFinalizers();
				Logger.Info($"Collected garbage after pokemon {i}");
			}
			await Task.Delay(100);
			
		}

	} 
}
#endif
