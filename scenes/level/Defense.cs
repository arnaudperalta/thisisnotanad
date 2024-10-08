using Godot;
using System;

public partial class Defense : Node3D
{

    private static float STEP_DISTANCE = 2f;
    private static float SPAWN_RADIUS = 1f;
    private static int MAX_DEFENSE_SIZE = 50;

    private static float MAX_TURN_DISTANCE = 3;

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
        var related_speed = direction == Direction.RIGHT ? STEP_DISTANCE : -STEP_DISTANCE;

        foreach (Soldier soldier in GetNode<Node3D>("Soldiers").GetChildren())
        {
            var position = soldier.Position;
            position.X += related_speed * (float)delta;
            if (position.X > -MAX_TURN_DISTANCE && position.X < MAX_TURN_DISTANCE) // If the new position is valid
            {
                soldier.Position = position;
            }
        }
    }

    public void SpawnSoldiers(int count)
    {
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                if (GetNode<Node3D>("Soldiers").GetChildCount() > MAX_DEFENSE_SIZE)
                {
                    break;
                }
                float randomX = GD.Randf() * SPAWN_RADIUS;
                float randomZ = GD.Randf() * SPAWN_RADIUS;

                var instance = soldierScene.Instantiate<Soldier>();
                instance.Position = new Vector3(randomX, 0, randomZ);
                instance.EquipWeapon(GetTree().Root.GetNode<GameState>("GameState").weaponTypeEquipped);
                GetNode<Node3D>("Soldiers").AddChild(instance);
            }
        }
        else
        {
            for (int i = 0; i < -count; i++)
            {
                for (int j = 0; j < GetNode<Node3D>("Soldiers").GetChildCount(); j++)
                {
                    var soldierToDelete = GetNode<Node3D>("Soldiers").GetChild(j);
                    if (!soldierToDelete.IsQueuedForDeletion())
                    {
                        soldierToDelete.QueueFree();
                        break;
                    }
                }
            }
        }
    }

    public void EquipWeapons(string gunType)
    {
        GetTree().Root.GetNode<GameState>("GameState").weaponTypeEquipped = gunType;
        for (int i = 0; i < GetNode<Node3D>("Soldiers").GetChildCount(); i++)
        {
            var soldier = GetNode<Node3D>("Soldiers").GetChild<Soldier>(i);
            soldier.EquipWeapon(gunType);
        }
    }

    public void OnAreaEntered(Area3D area)
    {
        GetNode<Node3D>("Soldiers").GetChild<Soldier>(-1).QueueFree();
        area.GetParent().QueueFree();
    }
}
