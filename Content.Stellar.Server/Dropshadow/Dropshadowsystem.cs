using Content.Stellar.Shared.Dropshadow.Components;
using Content.Shared.Buckle.Components;
using Content.Shared.Standing;

namespace Content.Stellar.Shared.Dropshadow;

public sealed class DropshadowSystem : SharedDropshadowSystem
{
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DropshadowComponent, BuckledEvent>(OnBuckled);
        SubscribeLocalEvent<DropshadowComponent, UnbuckledEvent>(OnUnbuckled);
        SubscribeLocalEvent<DropshadowComponent, DownedEvent>(OnDowned);
        SubscribeLocalEvent<DropshadowComponent, StoodEvent>(OnStood);
    }
    private void OnBuckled(Entity<DropshadowComponent> ent, ref BuckledEvent args)
    {
        ent.Comp.Visible = false;
        Dirty(ent);
        _appearance.SetData(ent, DropshadowKey.Key, false);
    }

    private void OnUnbuckled(Entity<DropshadowComponent> ent, ref UnbuckledEvent args)
    {
        ent.Comp.Visible = true;
        Dirty(ent);
        _appearance.SetData(ent, DropshadowKey.Key, true);
    }

    private void OnDowned(Entity<DropshadowComponent> ent, ref DownedEvent args)
    {
        ent.Comp.Visible = false;
        Dirty(ent);
        _appearance.SetData(ent, DropshadowKey.Key, false);
    }

    private void OnStood(Entity<DropshadowComponent> ent, ref StoodEvent args)
    {
        ent.Comp.Visible = true;
        Dirty(ent);
        _appearance.SetData(ent, DropshadowKey.Key, true);
    }
}
