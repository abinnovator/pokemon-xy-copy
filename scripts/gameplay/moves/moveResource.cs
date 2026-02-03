namespace Game.Gameplay;

[GlobalClass]
[Tool]
public partial class MoveResource : Resource
{
    [ExportCategory("Basic Info")]
    [Export]
    public string Name ="";
    [Export]
    public MoveTarget Target = MoveTarget.SelectedPokemon;
    [Export]
    public MoveCategory Category = MoveCategory.Physical;
    
}