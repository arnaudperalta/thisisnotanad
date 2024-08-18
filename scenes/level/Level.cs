using Godot;
using System;

public partial class Level : Node3D
{
    private float GRID_STEP_DISTANCE = 3f;
    private int FLOOR_ITEM_ID = 29;
    private int WALL_ITEM_ID = 80;
    private int ROAD_SIDE_WIDTH = 4;
    private int ROW_BETWEEN_EVENTS = 5;
    private GridMap gridMap;
    private Vector3 generationTreshold;
    private enum Obstacles { COUNT_GATE, ZOMBIES, WEAPONS_GATE };
    private PackedScene countGateScene = GD.Load<PackedScene>("res://scenes/level/count_gate.tscn");
    private PackedScene weaponObstacleScene = GD.Load<PackedScene>("res://scenes/level/weapon_obstacle.tscn");
    private PackedScene zombieWaveScene = GD.Load<PackedScene>("res://scenes/level/zombie_wave.tscn");
    private int lastRowGenerated;
    private double elapsedTimeInSeconds = 0;
    private bool isGameOver = false;


    public override void _Ready()
    {
        gridMap = GetNode<GridMap>("GridMap");
        generationTreshold = GetNode<Marker3D>("GenerationTreshold").Position;
    }

    public override void _Process(double delta)
    {
        if (Name != "Level")
        {
            Name = "Level";
        }
        if (GetNode("Defense/Soldiers").GetChildCount() != 0)
        {
            isGameOver = false;
        }
        // If no soldiers are alive, we stop the game
        if (isGameOver) { return; }
        if (!isGameOver && GetNode("Defense/Soldiers").GetChildCount() == 0)
        {
            isGameOver = true;
            GetTree().Root.GetNode<Ui>("UI").gameIsFinished = true;
            GetTree().Root.GetNode<Control>("UI/GameOverUI").Visible = true;
            return;
        }

        elapsedTimeInSeconds += delta;
        // Moving backward the gridmap
        var gridMapPosition = gridMap.Position;
        gridMapPosition.Z -= GRID_STEP_DISTANCE * (float)delta;
        gridMap.Position = gridMapPosition;

        // Moving forward the generation treshold
        generationTreshold.Z += GRID_STEP_DISTANCE * (float)delta;

        // Generating new cells
        var gridMapRow = gridMap.LocalToMap(generationTreshold);

        // Drawing the road
        for (int x = -ROAD_SIDE_WIDTH; x < ROAD_SIDE_WIDTH; x++)
        {
            gridMap.SetCellItem(new Vector3I(x, 0, gridMapRow.Z), FLOOR_ITEM_ID);
        }
        // Drawing the walls
        gridMap.SetCellItem(new Vector3I(-ROAD_SIDE_WIDTH, 1, gridMapRow.Z), WALL_ITEM_ID, 16);
        gridMap.SetCellItem(new Vector3I(ROAD_SIDE_WIDTH - 1, 1, gridMapRow.Z), WALL_ITEM_ID, 16);

        if (gridMapRow.Z % ROW_BETWEEN_EVENTS == 0 && lastRowGenerated != gridMapRow.Z)
        {
            SpawnObstacle(gridMapRow.Z);
        }
    }

    private void SpawnObstacle(int zAxis)
    {
        var random = GD.Randi() % 10;
        if (random < 3)
        {
            var countGateInstance = countGateScene.Instantiate<CountGate>();
            countGateInstance.CountGateEntered += OnCountGateEntered;
            countGateInstance.Position = new Vector3(0, 0.3f, zAxis);
            gridMap.AddChild(countGateInstance);
        }
        else if (random < 8)
        {
            var zombieWaveInstance = zombieWaveScene.Instantiate<ZombieWave>();
            zombieWaveInstance.AddZombies((int)GD.Randi() % 1 + (int)(elapsedTimeInSeconds / 3));
            zombieWaveInstance.Position = new Vector3(0, 0.3f, zAxis);
            gridMap.AddChild(zombieWaveInstance);
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
