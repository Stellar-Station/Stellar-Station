using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Stellar.Shared.Dropshadow.Components;

/// <summary>
/// Component applying a sprited drop shadow to entities. Specify the sprite and state in YML.
/// </summary>
[NetworkedComponent, RegisterComponent]
[AutoGenerateComponentState]
public sealed partial class DropshadowComponent : Component
{
    /// <summary>
    /// Wether or not the drop shadow is visible.
    /// </summary>
    [DataField]
    [AutoNetworkedField]
    public bool Visible = true;

    /// <summary>
    /// The file path for the associated sprite RSI.
    /// </summary>
    [DataField(required: true)]
    [AutoNetworkedField]
    public string Sprite = default!;

    /// <summary>
    /// The state inside the sprite RSI.
    /// </summary>
    [DataField(required: true)]
    [AutoNetworkedField]
    public string State = default!;

    /// <summary>
    /// The distance you need the dropshadow to be offset from the entity it's applied to. Format is "#, #". X (horizontal) and Y (vertical) axis.
    /// </summary>
    [DataField]
    [AutoNetworkedField]
    public Vector2 Offset = Vector2.Zero;
}

[Serializable, NetSerializable]
public enum DropshadowKey
{
    Key
}
