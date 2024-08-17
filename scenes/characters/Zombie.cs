using Godot;
using System;

public partial class Zombie : Node3D
{
    private static int DEFAULT_ZOMBIE_HP = 5;

    private int healthPoint = DEFAULT_ZOMBIE_HP;

    public override void _Ready()
    {
        GetNode<Node3D>("Model").GetNode<AnimationPlayer>("AnimationPlayer").Play("sprint");
    }

    private void OnBodyEntered(StaticBody3D body)
    {
        if (body.CollisionLayer == 2) // Projectiles
        {
            --healthPoint;
            if (healthPoint <= 0)
            {
                QueueFree();
            }
        }
    }
}
