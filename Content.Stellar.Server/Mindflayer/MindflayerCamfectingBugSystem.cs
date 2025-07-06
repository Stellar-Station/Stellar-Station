using Content.Server.SurveillanceCamera;
using Content.Stellar.Shared.Mindflayer;

namespace Content.Stellar.Server.Mindflayer;

public sealed class MindflayerCamfectingBugSystem : SharedMindflayerCamfectingBugSystem
{
    [Dependency] private readonly SurveillanceCameraSystem _surveillanceCamera = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MindflayerCamfectingBugComponent, SurveillanceCameraDeactivateEvent>(OnCameraDeactivate);
    }

    protected override void StopViewing(Entity<MindflayerCamfectingBugComponent> ent, EntityUid viewing)
    {
        base.StopViewing(ent, viewing);

        _surveillanceCamera.RemoveActiveViewer(viewing, ent, ent);
    }

    protected override void StartViewing(Entity<MindflayerCamfectingBugComponent> ent, EntityUid viewing)
    {
        base.StartViewing(ent, viewing);

        _surveillanceCamera.AddActiveViewer(viewing, ent, ent);
    }

    private void OnCameraDeactivate(Entity<MindflayerCamfectingBugComponent> ent, ref SurveillanceCameraDeactivateEvent args)
    {
        if (ent.Comp.CurrentlyViewing == args.Camera)
        {
            StopViewing(ent, args.Camera);
        }
    }
}
