using System.Numerics;

namespace Content.Stellar.Shared.CosmicCult.Components;

/// <summary>
/// This is used to apply an offset to the star mark that shows up at cult tier 3.
/// </summary>
[RegisterComponent]
public sealed partial class CosmicStarMarkOffsetComponent : Component
{
    [DataField]
    public Vector2 Offset = Vector2.Zero;
}
