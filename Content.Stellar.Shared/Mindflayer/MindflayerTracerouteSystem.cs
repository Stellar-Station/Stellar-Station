using System.Numerics;
using Content.Shared.Actions;
using Robust.Shared.Map;
using Robust.Shared.Physics.Components;

namespace Content.Stellar.Shared.Mindflayer;

public sealed class MindflayerTracerouteSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MindflayerComponent, MindflayerTraceRouteActionEvent>(OnTraceRoute);
    }

    private void OnTraceRoute(Entity<MindflayerComponent> ent, ref MindflayerTraceRouteActionEvent args)
    {
        var traceroute = EnsureComp<MindflayerTraceRouteComponent>(ent);

        if (traceroute.TaggedEntity is not { } target)
        {
            _actions.SetToggled(args.Action.Owner, true);
            traceroute.TaggedEntity = args.Target;
            Dirty(ent.Owner, traceroute);

            args.Handled = true;
            return;
        }

        traceroute.TaggedEntity = null;
        Dirty(ent.Owner, traceroute);
        _actions.SetToggled(args.Action.Owner, false);

        var targetCoords = new EntityCoordinates(target, Vector2.Zero);

        if (TryComp<PhysicsComponent>(target, out var targetPhysics))
            targetCoords = targetCoords.Offset(targetPhysics.LocalCenter);

        _transform.SetCoordinates(ent, targetCoords);
        _transform.AttachToGridOrMap(ent);
        args.Handled = true;
    }
}
