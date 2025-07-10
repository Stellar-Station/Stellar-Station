using Content.Shared.Actions;
using Robust.Shared.GameStates;

namespace Content.Stellar.Shared.Mindflayer;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class MindflayerTraceRouteComponent : Component
{
    [DataField, AutoNetworkedField]
    public EntityUid? TaggedEntity;
}

public sealed partial class MindflayerTraceRouteActionEvent : EntityTargetActionEvent;

[RegisterComponent, NetworkedComponent]
public sealed partial class MindflayerTraceRouteTargetComponent : Component;
