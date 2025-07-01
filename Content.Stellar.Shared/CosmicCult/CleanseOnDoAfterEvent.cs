using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Stellar.Shared.CosmicCult;

[Serializable, NetSerializable]
public sealed partial class CleanseOnDoAfterEvent : SimpleDoAfterEvent;
