using Robust.Shared.GameStates;

namespace Content.Stellar.Shared.CosmicCult.Components.Examine;

/// <summary>
/// Marker component for The Unknown. We also use this to detect its spawn through CultRule!
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class CosmicGodComponent : Component;
