using Content.Stellar.Shared.Mindflayer;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Wires;

namespace Content.Stellar.Shared.Mindflayer;

public sealed class RemoteWiresHackerSystem : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedUserInterfaceSystem _ui = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RemoteWiresHackerComponent, BeforeRangedInteractEvent>(OnBeforeRangedInteract);
        SubscribeLocalEvent<MindflayerComponent, InRangeOverrideEvent>(OnInRangeOverride);
        SubscribeLocalEvent<MindflayerComponent, AccessibleOverrideEvent>(OnAccessibleOverride);
    }

    private void OnBeforeRangedInteract(Entity<RemoteWiresHackerComponent> ent, ref BeforeRangedInteractEvent args)
    {
        if (args.Target is not { } target || !HasComp<WiresPanelComponent>(target))
            return;

        _ui.OpenUi(target, WiresUiKey.Key, args.User);
        args.Handled = true;
    }

    private void CheckRemoteWires(Entity<MindflayerComponent> ent, EntityUid target, ref bool handled, ref bool inRange)
    {
        if (!HasComp<WiresPanelComponent>(target))
            return;

        if (!TryComp<RemoteWiresHackerComponent>(_hands.GetActiveItem(ent.Owner), out var remoteWireHacker))
            return;

        handled = true;
        inRange = _transform.InRange(ent.Owner, target, remoteWireHacker.InteractionRange);
    }

    private void OnInRangeOverride(Entity<MindflayerComponent> ent, ref InRangeOverrideEvent args)
    {
        CheckRemoteWires(ent, args.Target, ref args.Handled, ref args.InRange);
    }

    private void OnAccessibleOverride(Entity<MindflayerComponent> ent, ref AccessibleOverrideEvent args)
    {
        CheckRemoteWires(ent, args.Target, ref args.Handled, ref args.Accessible);
    }
}
