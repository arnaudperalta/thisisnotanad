using Godot;
using System;

public partial class Level : Node3D
{
    private float GRID_STEP_DISTANCE = 2f;
    private int FLOOR_ITEM_ID = 29;
    private int ROAD_SIDE_WIDTH = 3;
    private GridMap gridMap;
    private Vector3 generationTreshold;
    private enum Obstacles { COUNT_GATE, ZOMBIES, WEAPONS_GATE };
    private PackedScene countGateScene = GD.Load<PackedScene>("res://scenes/level/count_gate.tscn");
    private int lastRowGenerated;

    public override void _Ready()
    {
        gridMap = GetNode<GridMap>("GridMap");
        generationTreshold = GetNode<Marker3D>("GenerationTreshold").Position;
    }

    public override void _Process(double delta)
    {
        // Moving backward the gridmap
        var gridMapPosition = gridMap.Position;
        gridMapPosition.Z -= GRID_STEP_DISTANCE * (float)delta;
        gridMap.Position = gridMapPosition;

        // Moving forward the generation treshold
        generationTreshold.Z += GRID_STEP_DISTANCE * (float)delta;

        // Generating new cells
        var gridMapRow = gridMap.LocalToMap(generationTreshold);

        for (int x = -ROAD_SIDE_WIDTH; x < ROAD_SIDE_WIDTH; x++)
        {
            gridMap.SetCellItem(new Vector3I(x, 0, gridMapRow.Z), FLOOR_ITEM_ID);
        }

        if (gridMapRow.Z % 10 == 0 && lastRowGenerated != gridMapRow.Z)
        {
            var countGateInstance = countGateScene.Instantiate<Node3D>();
            countGateInstance.Position = new Vector3(0, 0.3f, gridMapRow.Z);
            gridMap.AddChild(countGateInstance);
            lastRowGenerated = gridMapRow.Z;
        }
    }
}
