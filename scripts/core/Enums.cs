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
		
		pallet_town,
		pallet_town_ashs_house,
		pallet_town_ashs_house_f1,
		pallet_town_rivals_house,
		pallet_town_lab,
		route1
		
	}
	public enum LevelGroups{
		SPAWNPOINTS,
		SCENETRIGGERS
	}
	public enum SignType{
		METAL,
		SNOWY_METAL,
		WOOD,
		SNOWY_WOOD,
		LARGE_WOOD,
		SNOWY_LARGE_WOOD,
		LARGE_METAL,
		SNOWY_LARGE_METAL,
	}
	#endregion
	#region Npcs
	public enum NpcAppearance{
		Gardener,
		Worker,
		BugCatcher
	}
	public enum NpcMovementType{
		Static,
		Wander,
		Patrol,
		LookAround
	}
	#endregion


}