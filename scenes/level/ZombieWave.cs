using Godot;
using System;

public partial class ZombieWave : Node3D
{
    private PackedScene zombieScene = GD.Load<PackedScene>("res://scenes/characters/zombie.tscn");

    public void AddZombies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var position = GetNode<Node3D>("SpawnPoints").GetChild<Marker3D>(i).Position;
            var instance = zombieScene.Instantiate<Node3D>();
            instance.Position = position;
            GetNode<Node3D>("Zombies").AddChild(instance);
        }
    }

}
