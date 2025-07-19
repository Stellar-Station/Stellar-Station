using Content.Shared.Actions;
using Content.Shared.DoAfter;
using Robust.Shared.Utility;

namespace Content.Stellar.Shared.Mindflayer;

public abstract class SharedMindflayerCamfectingBugSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedUserInterfaceSystem _userInterface = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MindflayerCamfectingBugComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<MindflayerCamfectingBugComponent, BoundUIClosedEvent>(OnCamfectingBugClosed);
        SubscribeLocalEvent<MindflayerComponent, MindflayerCamfectingBugEvent>(OnCamfectingBug);
        SubscribeLocalEvent<MindflayerCamfectingBugComponent, MindflayerCamfectingBugDoAfterEvent>(OnCamfectingBugDoAfter);
        SubscribeLocalEvent<MindflayerCamfectingBugComponent, MindflayerOpenHackedCamerasEvent>(OnOpenHackedCameras);
        SubscribeLocalEvent<MindflayerCamfectingBugComponent, AfterAutoHandleStateEvent>(OnCamfectingAfterState);
        SubscribeLocalEvent<MindflayerInfectedCameraComponent, ComponentShutdown>(OnHackedCameraShutdown);

        Subs.BuiEvents<MindflayerCamfectingBugComponent>(CamfectingBugUiKey.Key,
            sub =>
            {
                sub.Event<CamfectingBugViewCameraMessage>(OnViewCamera);
            });
    }

    private void OnComponentInit(Entity<MindflayerCamfectingBugComponent> ent, ref ComponentInit args)
    {
        _actions.AddAction(ent, ref ent.Comp.ViewCamerasActionEntity, ent.Comp.ViewCamerasAction);
        Dirty(ent);
    }

    private void OnCamfectingBugClosed(Entity<MindflayerCamfectingBugComponent> ent, ref BoundUIClosedEvent args)
    {
        if (!args.UiKey.Equals(CamfectingBugUiKey.Key))
            return;

        if (ent.Comp.CurrentlyViewing is { } viewing)
            StopViewing(ent, viewing);
    }

    private void OnCamfectingBug(Entity<MindflayerComponent> ent, ref MindflayerCamfectingBugEvent args)
    {
        EnsureComp<MindflayerCamfectingBugComponent>(ent);

        var doArgs = new DoAfterArgs(EntityManager, ent, args.HackingDuration, new MindflayerCamfectingBugDoAfterEvent(), ent, args.Target)
        {
            DistanceThreshold = 2f,
            BreakOnHandChange = true,
            BreakOnDamage = true,
            BreakOnMove = true,
            BreakOnDropItem = true,
        };
        args.Handled = _doAfter.TryStartDoAfter(doArgs);
    }

    protected virtual string CameraName(EntityUid target)
    {
        return Name(target);
    }

    private void OnCamfectingBugDoAfter(Entity<MindflayerCamfectingBugComponent> ent, ref MindflayerCamfectingBugDoAfterEvent args)
    {
        if (args.Args.Target is not { } target)
            return;

        ent.Comp.TrackedEntities.Add(target);
        ent.Comp.TrackedLabels[target] = CameraName(target);
        EnsureComp<MindflayerInfectedCameraComponent>(target).Hackers.Add(ent);
        Dirty(ent);
    }

    private void OnHackedCameraShutdown(Entity<MindflayerInfectedCameraComponent> ent, ref ComponentShutdown args)
    {
        foreach (var entity in ent.Comp.Hackers)
        {
            if (TryComp<MindflayerCamfectingBugComponent>(entity, out var camfecting))
            {
                camfecting.TrackedEntities.Remove(ent);
                camfecting.TrackedLabels.Remove(ent);

                if (camfecting.CurrentlyViewing == ent)
                {
                    StopViewing((entity, camfecting), ent);
                }
            }
        }
    }

    private void OnOpenHackedCameras(Entity<MindflayerCamfectingBugComponent> ent, ref MindflayerOpenHackedCamerasEvent args)
    {
        _userInterface.OpenUi(ent.Owner, CamfectingBugUiKey.Key, ent.Owner);
    }

    private void OnCamfectingAfterState(Entity<MindflayerCamfectingBugComponent> ent, ref AfterAutoHandleStateEvent args)
    {
        if (!_userInterface.TryGetOpenUi(ent.Owner, CamfectingBugUiKey.Key, out var bui))
            return;

        bui.Update();
    }

    protected virtual void StopViewing(Entity<MindflayerCamfectingBugComponent> ent, EntityUid viewing)
    {
        DebugTools.AssertEqual(ent.Comp.CurrentlyViewing, viewing);
        ent.Comp.CurrentlyViewing = null;
        Dirty(ent);
    }

    protected virtual void StartViewing(Entity<MindflayerCamfectingBugComponent> ent, EntityUid viewing)
    {
        DebugTools.AssertEqual(ent.Comp.CurrentlyViewing, null);
        ent.Comp.CurrentlyViewing = viewing;
        Dirty(ent);
    }

    private void OnViewCamera(Entity<MindflayerCamfectingBugComponent> ent, ref CamfectingBugViewCameraMessage args)
    {
        if (ent.Comp.CurrentlyViewing is { } viewing)
            StopViewing(ent, viewing);

        if (GetEntity(args.Camera) is not { } entity)
            return;

        if (!ent.Comp.TrackedEntities.Contains(entity))
            return;

        StartViewing(ent, entity);
    }
}
