using Godot;
using System;

public partial class Defense : Node3D
{

    private static float STEP_DISTANCE = 2f;
    private static float SPAWN_RADIUS = 1f;

    private enum Direction { LEFT, RIGHT }

    private PackedScene soldierScene = GD.Load<PackedScene>("res://scenes/characters/soldier.tscn");

    public override async void _Process(double delta)
    {
        if (Input.IsActionPressed("move_right"))
        {
            MoveSoldiers(Direction.LEFT, delta);
        }
        if (Input.IsActionPressed("move_left"))
        {
            MoveSoldiers(Direction.RIGHT, delta);
        }
    }

    private void MoveSoldiers(Direction direction, double delta)
    {
        var related_speed = direction == Direction.RIGHT ? STEP_DISTANCE : STEP_DISTANCE * -1;

        foreach (Soldier soldier in GetNode<Node3D>("Soldiers").GetChildren())
        {
            var position = soldier.Position;
            position.X += related_speed * (float)delta;
            soldier.Position = position;
        }
    }

    public void SpawnSoldiers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float randomX = (float)(GD.Randf() * 2.0 - 1.0) * SPAWN_RADIUS;
            float randomZ = (float)(GD.Randf() * 2.0 - 1.0) * SPAWN_RADIUS;

            var instance = soldierScene.Instantiate<Soldier>();
            instance.Position = new Vector3(randomX, 0, randomZ);
            GetNode<Node3D>("Soldiers").AddChild(instance);
        }
    }

}
