namespace Content.Stellar.Server.CosmicCult.Components;

[RegisterComponent]
public sealed partial class CosmicEntropyConditionComponent : Component
{
    /// <summary>
    ///     The amount of entropy this objective would like to be siphoned
    /// </summary>
    [DataField]
    public int Siphoned;
}
