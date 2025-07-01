using Content.Stellar.Server.CosmicCult.Abilities;

namespace Content.Stellar.Server.CosmicCult.Components;

[RegisterComponent, Access(typeof(CosmicReturnSystem))]
public sealed partial class CosmicAstralBodyComponent : Component
{
    [DataField]
    public EntityUid OriginalBody;
}
