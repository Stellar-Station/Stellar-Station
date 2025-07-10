using Content.Shared.Actions;
using Content.Shared.Chemistry.Components;
using Content.Shared.DoAfter;
using Content.Shared.Implants.Components;
using Content.Shared.Store;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Stellar.Shared.Mindflayer;

[RegisterComponent, NetworkedComponent]
public sealed partial class MindflayerComponent : Component
{
    [DataField]
    public TimeSpan SiphonDuration = TimeSpan.FromMilliseconds(500);

    [DataField]
    public int SiphonQuantity = 1;

    [DataField]
    public ProtoId<CurrencyPrototype> SiphonCurrency = "RecoveredMind";
}

public sealed partial class MindflayerShopActionEvent : InstantActionEvent;

public sealed partial class MindflayerAddSolutionToBloodstreamEvent : InstantActionEvent
{
    [DataField(required: true)]
    public Solution Solution;
}

public sealed partial class MindflayerSiphonMindActionEvent : EntityTargetActionEvent;

[Serializable, NetSerializable]
public sealed partial class MindflayerSiphonMindDoAfterEvent : SimpleDoAfterEvent;

[DataDefinition, Serializable, NetSerializable]
public sealed partial class MindflayerAddImplantEvent : EntityEventArgs
{
    [DataField(required: true)]
    public EntProtoId<SubdermalImplantComponent> Implant;
}
