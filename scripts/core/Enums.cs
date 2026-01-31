namespace Game.Core{
	public enum LevelLog{
		Debug,
		Info,
		Warning,
		Error,
	}
	#region Characters

	public enum ECharacterAnimationState{
		idle_down,
		idle_up,
		idle_left,
		idle_right,
		walk_down,
		walk_up,
		walk_left,
		walk_right,
		turn_down,
		turn_up,
		turn_left,
		turn_right,

	}
	public enum ECharacterMovement
	{
		WALKING,
		JUMPING
	}
	#endregion


	#region Levels
	public enum LevelName{
		small_town,
		small_town_greens_house,
		small_town_purples_house,
		small_town_lab,
		small_town_pokemart,
		small_town_pokecenter,
		small_town_route_1,
		small_town_blues_house,
		small_town_cave_entrance,
		small_town_cave,
		
	}
	public enum LevelGroups{
		SPAWNPOINTS,
		SCENETRIGGERS
	}
	#endregion


}