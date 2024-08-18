using Godot;
using System;

public partial class Zombie : Node3D
{

    private int healthPoint = 1;

    public override void _Ready()
    {
        GetNode<Node3D>("Model").GetNode<AnimationPlayer>("AnimationPlayer").Play("walk");
    }

    public void SetHealth(int value)
    {
        healthPoint = value;
    }

    private void OnBodyEntered(StaticBody3D body)
    {
        if (body.CollisionLayer == 2) // Projectiles
        {
            --healthPoint;
            if (healthPoint <= 0)
            {
                if (!IsQueuedForDeletion())
                {
                    GetTree().Root.GetNode<Ui>("UI").AddScore(1);
                    QueueFree();
                }
            }
            body.GetParent().QueueFree();
        }
    }
}
