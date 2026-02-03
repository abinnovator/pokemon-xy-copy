namespace Game.Core;

public static class MovesEnum {
	public enum MoveCategory{
		Physical,
		Special,
		Status
	}
	public enum MoveTarget {
		SpecificMove,
		UsersField,
		User,
		RandomOpponent,
		AllOtherPokemon,
		SelectedPokemon,
		AllOpponents,
		EntireField
	}
    public static class MovesEnum 
	{
        public static readonly Dictionary<string, MoveCategory> CategoryMap = new(){
            {"Physical", MoveCategory.Physical},
            {"Special", MoveCategory.Special},
            {"Status", MoveCategory.Status}
        };

        public static readonly Dictionary<string, MoveTarget> MoveTargetMap = new(){
            {"SpecificMove", MoveTarget.SpecificMove},
            {"UsersField", MoveTarget.UsersField},
            {"User", MoveTarget.User},
            {"RandomOpponent", MoveTarget.RandomOpponent},
            {"AllOtherPokemon", MoveTarget.AllOtherPokemon},
            {"SelectedPokemon", MoveTarget.SelectedPokemon},
            {"AllOpponents", MoveTarget.AllOpponents},
            {"EntireField", MoveTarget.EntireField}
        };
    }
}