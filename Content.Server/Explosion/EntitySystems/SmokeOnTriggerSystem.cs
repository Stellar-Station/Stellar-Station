using Content.Shared.Explosion.Components;
using Content.Shared.Explosion.EntitySystems;
using Content.Server.Fluids.EntitySystems;
using Content.Server.Spreader;
using Content.Shared.Chemistry.Components;
using Content.Shared.Coordinates.Helpers;
using Content.Shared.Maps;
using Robust.Server.GameObjects;
using Robust.Shared.Map;

namespace Content.Server.Explosion.EntitySystems;

/// <summary>
/// Handles creating smoke when <see cref="SmokeOnTriggerComponent"/> is triggered.
/// </summary>
public sealed class SmokeOnTriggerSystem : SharedSmokeOnTriggerSystem
{
    [Dependency] private readonly IMapManager _mapMan = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;
    [Dependency] private readonly SmokeSystem _smoke = default!;
    [Dependency] private readonly TransformSystem _transform = default!;
    [Dependency] private readonly SpreaderSystem _spreader = default!;
    [Dependency] private readonly TurfSystem _turf = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SmokeOnTriggerComponent, TriggerEvent>(OnTrigger);
    }

    private void OnTrigger(EntityUid uid, SmokeOnTriggerComponent comp, TriggerEvent args)
    {
        var xform = Transform(uid);
        var mapCoords = _transform.GetMapCoordinates(uid, xform);
        if (!_mapMan.TryFindGridAt(mapCoords, out var gridUid, out var grid) ||
            !_map.TryGetTileRef(gridUid, grid, xform.Coordinates, out var tileRef) ||
            tileRef.Tile.IsEmpty)
        {
            return;
        }

        if (_spreader.RequiresFloorToSpread(comp.SmokePrototype.ToString()) && _turf.IsSpace(tileRef))
            return;

        var coords = _map.MapToGrid(gridUid, mapCoords);
        var ent = Spawn(comp.SmokePrototype, coords.SnapToGrid());
        if (!TryComp<SmokeComponent>(ent, out var smoke))
        {
            Log.Error($"Smoke prototype {comp.SmokePrototype} was missing SmokeComponent");
            Del(ent);
            return;
        }

        _smoke.StartSmoke(ent, comp.Solution, comp.Duration, comp.SpreadAmount, smoke);
    }
}
