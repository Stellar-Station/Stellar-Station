using Robust.Shared.GameStates;

namespace Content.Stellar.Shared.Mindflayer;

/// <summary>
/// Allows remotely opening and hacking wire panels
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class RemoteWiresHackerComponent : Component
{
    /// <summary>
    /// The range that wire panels can be hacked from
    /// </summary>
    [DataField]
    public float InteractionRange = 10f;
}
