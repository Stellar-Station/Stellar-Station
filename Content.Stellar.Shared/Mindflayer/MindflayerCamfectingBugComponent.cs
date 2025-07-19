using Content.Shared.Actions;
using Content.Shared.DoAfter;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Stellar.Shared.Mindflayer;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class MindflayerCamfectingBugComponent : Component
{
    [DataField]
    public EntProtoId ViewCamerasAction = "ActionMindFlayerViewCameras";

    [DataField, AutoNetworkedField]
    public EntityUid? ViewCamerasActionEntity;

    [DataField, AutoNetworkedField]
    public HashSet<EntityUid> TrackedEntities = new();

    // store names so we don't have to do a request/response deal
    // or PVS shenanigans with the tracked entities
    [DataField, AutoNetworkedField]
    public Dictionary<EntityUid, string> TrackedLabels = new();

    [DataField, AutoNetworkedField]
    public EntityUid? CurrentlyViewing;

    [DataField, AutoNetworkedField]
    public int MaximumCameras = 6;
}

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class MindflayerInfectedCameraComponent : Component
{
    [DataField, AutoNetworkedField]
    public HashSet<EntityUid> Hackers = new();
}

public sealed partial class MindflayerCamfectingBugEvent : EntityTargetActionEvent
{
    [DataField]
    public TimeSpan HackingDuration = TimeSpan.FromSeconds(5);
}

[Serializable, NetSerializable]
public sealed partial class MindflayerCamfectingBugDoAfterEvent : SimpleDoAfterEvent;

public sealed partial class MindflayerOpenHackedCamerasEvent : InstantActionEvent;

[Serializable, NetSerializable]
public sealed class CamfectingBugViewCameraMessage : BoundUserInterfaceMessage
{
    public NetEntity? Camera;

    public CamfectingBugViewCameraMessage(NetEntity? camera)
    {
        Camera = camera;
    }
}

[Serializable, NetSerializable]
public enum CamfectingBugUiKey : byte
{
    Key
}
