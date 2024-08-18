using Godot;
using System;

public partial class ZombieWave : Node3D
{
    private PackedScene zombieScene = GD.Load<PackedScene>("res://scenes/characters/zombie.tscn");

    private static float SPAWN_RADIUS = 1f;

    public void AddZombies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float randomX = GD.Randf() * SPAWN_RADIUS;
            float randomZ = GD.Randf() * SPAWN_RADIUS;

            var instance = zombieScene.Instantiate<Zombie>();
            instance.Position = new Vector3(randomX, 0, randomZ);
            GetNode<Node3D>("Zombies").AddChild(instance);
        }
    }

}
