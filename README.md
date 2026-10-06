# Advize_ValheimMods
Game modifications for Valheim

## PlantEasily harvest controls

When `EnableBulkHarvest` is enabled, Use harvests every eligible resource in the harvest radius. Hold the harvest modifier and press Use to harvest only the targeted resource.

- Keyboard: Use = bulk. `KeyboardHarvestModifierKey` + Use = single (default Left Shift). Vanilla alt-place + Use = single.
- Gamepad: Use = bulk. Alt-place (alt keys) + Use = single.

## PlantEasily grid layout

`GridLayout` is under `[Grid]` and defaults to `Square`. Set it to `Staggered` to pack crops, saplings, and other cultivator pieces in a hex grid:

```
[Grid]
GridLayout = Staggered
```

Staggered offsets every other row by half the column spacing and sets the row distance to that spacing times √3/2. Column spacing is still the per-piece distance from `GetPieceSpacing`, so `MinimizeGridSpacing`, `ExtraCropSpacing`, and `ExtraSaplingSpacing` apply unchanged. `PreventInvalidPlanting` still rejects a ghost that does not have room to grow.

Snapping the piece you are holding onto an existing staggered field uses the six hex neighbors of a detected plant. A lone plant still snaps the root around the existing spacing circle. Cells that would overlap are left invalid instead of forcing a new grid solver.
