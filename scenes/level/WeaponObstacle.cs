using Godot;
using System;

public partial class WeaponObstacle : Node3D
{

    [Signal]
    public delegate void WeaponObstacleDestroyedEventHandler(string gunType);

    private uint obstacleHP = GD.Randi() % 200;


    public override void _Ready()
    {
        GetNode<Label3D>("ObstacleHPLabel").Text = $"{obstacleHP}";
    }

    private void OnBodyEntered(StaticBody3D body)
    {
        if (body.CollisionLayer == 1)// If it's a soldier
        {
            body.GetParent().QueueFree();
        }
        else if (body.CollisionLayer == 2) // If it's a projectile
        {
            obstacleHP--;
            if (obstacleHP == 0)
            {
                EmitSignal(SignalName.WeaponObstacleDestroyed, "rifle");
                QueueFree();
                return;
            }
            GetNode<Label3D>("ObstacleHPLabel").Text = $"{obstacleHP}";
            body.GetParent().QueueFree();
        }
    }
}
