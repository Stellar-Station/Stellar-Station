using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Stellar.Shared.Dropshadow;

/// <summary>
/// Component applying a sprited drop shadow to entities. Specify the sprite and state in YML.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class DropshadowComponent : Component
{
    /// <summary>
    /// The sprite to use for the drop shadow
    /// </summary>
    [DataField(required: true)]
    public SpriteSpecifier Sprite = default!;

    /// <summary>
    /// The distance you need the dropshadow to be offset from the entity it's applied to. Format is "#, #". X (horizontal) and Y (vertical) axis.
    /// </summary>
    [DataField]
    public Vector2 Offset = Vector2.Zero;
}

[Serializable, NetSerializable]
public enum DropshadowVisuals
{
    Visible
}

[Serializable, NetSerializable]
public enum DropshadowLayers
{
    Shadow
}
