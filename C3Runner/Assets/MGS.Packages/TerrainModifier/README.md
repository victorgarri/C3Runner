[TOC]

﻿# MGS.TerrainModifier

## Summary
- Unity Terrain modifier.

## Environment

- Unity 5.0 or above.
- .Net Framework 3.5 or above.

## Version

- 1.0.1

## Demand
- Occasionally, you may encounter a catastrophic problem in Unity project, example the terrain has
  been brushed but the demand change to brush a river or basin on the original terrain. if the
  terrain was created base on height zero, the river or basin cannot be brushed in this case, because
  Unity does not allow any part of the terrain is brushed to be negative. therefore, we hope that we
  can modify the heightmap data to raise whole terrian to continue brush landforms.

## Prerequisite
- Unity provide the API(GetHeights and SetHeights methods of TerrainData class) to modify TerrainData.

## Scheme
- Create extend editor window, select target TerrainData file and set raise height value.
- Add the raise height value to TerrainData heightmap.

## Usage
1. Find the menu item "Tool/Terrain Modifier" in Unity editor menu bar and click it or press key combination Alt+T to open the editor window.
1. Set the target "Terrain Data" and "Raise Height".
1. Click the "Modify" button to open the dialog and make your choice.

## Warning
1. "Modify" operate can not be recovered, make sure you have a backup of target terrain data.
1. Don't set a negative value to the Raise Height unless you know the terrain data inside out.

## Source

- https://github.com/mogoson/MGS.TerrainModifier.

------

Copyright © 2021 Mogoson.	mogoson@outlook.com