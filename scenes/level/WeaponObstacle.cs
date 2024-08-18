using Godot;
using System;

public partial class WeaponObstacle : Node3D
{

    [Signal]
    public delegate void WeaponObstacleDestroyedEventHandler(string gunType);
    private PackedScene rifleScene = GD.Load<PackedScene>("res://graphics/weapons/rifle.glb");
    private PackedScene minigunScene = GD.Load<PackedScene>("res://graphics/weapons/minigun.glb");
    private uint obstacleHP = GD.Randi() % 30;
    private string weaponType;


    public override void _Ready()
    {
        var equippedWeapon = GetTree().Root.GetNode<GameState>("GameState").weaponTypeEquipped;
        if (equippedWeapon == "pistol")
        {
            weaponType = "rifle";
            obstacleHP += 10;
            var instance = rifleScene.Instantiate<Node3D>();
            GetNode("Weapon").AddChild(instance);
        }
        else if (equippedWeapon == "rifle")
        {
            weaponType = "minigun";
            obstacleHP += 200;
            var instance = minigunScene.Instantiate<Node3D>();
            GetNode("Weapon").AddChild(instance);
        }
        else if (equippedWeapon == "minigun")
        {
            weaponType = "none";
            obstacleHP += 1000;
        }
        GetNode<Label3D>("ObstacleHPLabel").Text = $"{obstacleHP}";
        var position = Position;
        position.X = Position.X + (GD.Randi() % 2 == 0 ? 1.5f : -1.5f);
        Position = position;
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
                if (weaponType != "none")
                {
                    EmitSignal(SignalName.WeaponObstacleDestroyed, weaponType);

                }
                QueueFree();
                return;
            }
            GetNode<Label3D>("ObstacleHPLabel").Text = $"{obstacleHP}";
            body.GetParent().QueueFree();
        }
    }
}
