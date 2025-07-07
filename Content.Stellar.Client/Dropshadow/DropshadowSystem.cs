using Robust.Client.GameObjects;
using Content.Stellar.Shared.Dropshadow.Components;
using Robust.Shared.Utility;
using Content.Stellar.Shared.Dropshadow;
using Content.Shared.Buckle.Components;
using Content.Shared.Standing;

namespace Content.Stellar.Client.Dropshadow;

public sealed partial class DropshadowSystem : SharedDropshadowSystem
{
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SpriteSystem _sprite = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DropshadowComponent, ComponentStartup>(OnDropshadowAdded);
        SubscribeLocalEvent<DropshadowComponent, ComponentShutdown>(OnDropshadowRemoved);

        SubscribeLocalEvent<DropshadowComponent, AppearanceChangeEvent>(OnAppearanceChanged);

        SubscribeLocalEvent<DropshadowComponent, BuckledEvent>(OnBuckled);
        SubscribeLocalEvent<DropshadowComponent, UnbuckledEvent>(OnUnbuckled);
        SubscribeLocalEvent<DropshadowComponent, DownedEvent>(OnDowned);
        SubscribeLocalEvent<DropshadowComponent, StoodEvent>(OnStood);
    }

    private void OnDropshadowAdded(Entity<DropshadowComponent> ent, ref ComponentStartup args)
    {
        if (!TryComp<SpriteComponent>(ent, out var sprite) || _sprite.LayerMapTryGet((ent, sprite), DropshadowKey.Key, out _, false))
            return;

        var newSprite = new SpriteSpecifier.Rsi(new(ent.Comp.Sprite), ent.Comp.State);
        var layer = _sprite.AddLayer((ent, sprite), newSprite, 0);
        _sprite.LayerMapSet((ent, sprite), DropshadowKey.Key, layer);
        _sprite.LayerSetOffset((ent, sprite), layer, ent.Comp.Offset);
        sprite.LayerSetShader(layer, "unshaded");
    }

    private void OnDropshadowRemoved(Entity<DropshadowComponent> ent, ref ComponentShutdown args)
    {
        if (!TryComp<SpriteComponent>(ent, out var sprite))
            return;
        _sprite.RemoveLayer((ent, sprite), DropshadowKey.Key, false);
    }

    private void OnAppearanceChanged(Entity<DropshadowComponent> ent, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        _sprite.LayerSetVisible((ent, args.Sprite), DropshadowKey.Key, ent.Comp.Visible);
        Dirty(ent);
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
