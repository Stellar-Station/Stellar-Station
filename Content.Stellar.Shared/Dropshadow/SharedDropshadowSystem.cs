using Content.Shared.Buckle.Components;
using Content.Shared.Standing;

namespace Content.Stellar.Shared.Dropshadow;

public abstract class SharedDropshadowSystem : EntitySystem
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
        _appearance.SetData(ent, DropshadowVisuals.Visible, false);
    }

    private void OnUnbuckled(Entity<DropshadowComponent> ent, ref UnbuckledEvent args)
    {
        _appearance.SetData(ent, DropshadowVisuals.Visible, true);
    }

    private void OnDowned(Entity<DropshadowComponent> ent, ref DownedEvent args)
    {
        _appearance.SetData(ent, DropshadowVisuals.Visible, false);
    }

    private void OnStood(Entity<DropshadowComponent> ent, ref StoodEvent args)
    {
        _appearance.SetData(ent, DropshadowVisuals.Visible, true);
    }
}
