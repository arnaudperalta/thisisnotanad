using Godot;
using System;

public partial class Level : Node3D
{
    private float GRID_STEP_DISTANCE = 3f;
    private int FLOOR_ITEM_ID = 29;
    private int ROAD_SIDE_WIDTH = 3;
    private GridMap gridMap;
    private Vector3 generationTreshold;
    private enum Obstacles { COUNT_GATE, ZOMBIES, WEAPONS_GATE };
    private PackedScene countGateScene = GD.Load<PackedScene>("res://scenes/level/count_gate.tscn");
    private PackedScene weaponObstacleScene = GD.Load<PackedScene>("res://scenes/level/weapon_obstacle.tscn");
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
            SpawnObstacle(gridMapRow.Z);
        }
    }

    private void SpawnObstacle(int zAxis)
    {
        if (GD.Randi() % 2 == 0)
        {
            var countGateInstance = countGateScene.Instantiate<CountGate>();
            countGateInstance.CountGateEntered += OnCountGateEntered;
            countGateInstance.Position = new Vector3(0, 0.3f, zAxis);
            gridMap.AddChild(countGateInstance);
        }
        else
        {
            var weaponObstacleInstance = weaponObstacleScene.Instantiate<WeaponObstacle>();
            weaponObstacleInstance.WeaponObstacleDestroyed += OnWeaponObstacleDestroyed;
            weaponObstacleInstance.Position = new Vector3(0, 0.3f, zAxis);
            gridMap.AddChild(weaponObstacleInstance);
        }
        lastRowGenerated = zAxis;
    }

    private void OnCountGateEntered(int value)
    {
        GetNode<Defense>("Defense").SpawnSoldiers(value);
    }

    private void OnWeaponObstacleDestroyed(string gunType)
    {
        GetNode<Defense>("Defense").EquipWeapons(gunType);
    }
}
