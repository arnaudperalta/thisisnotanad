using Godot;
using System;

public partial class Soldier : Node3D
{

    private PackedScene projectileScene = GD.Load<PackedScene>("res://scenes/characters/gun_projectile.tscn");
    private PackedScene pistolScene = GD.Load<PackedScene>("res://graphics/weapons/pistol.glb");
    private PackedScene rifleScene = GD.Load<PackedScene>("res://graphics/weapons/rifle.glb");
    private PackedScene minigunScene = GD.Load<PackedScene>("res://graphics/weapons/minigun.glb");

    public override void _Ready()
    {
        GetNode<Node3D>("Model").GetNode<AnimationPlayer>("AnimationPlayer").Play("sprint");
    }

    public void OnEquippedWeaponTimerTimeout()
    {
        var instance = projectileScene.Instantiate<GunProjectile>();
        GetNode<Node3D>("EquippedWeapon/Projectiles").AddChild(instance);
    }

    public void EquipWeapon(string gunType)
    {
        GetNode<Node3D>("EquippedWeapon/Model").GetChild(0).QueueFree();
        if (gunType == "rifle")
        {
            var instance = rifleScene.Instantiate<Node3D>();
            instance.Name = "WeaponModel";
            GetNode<Node3D>("EquippedWeapon/Model").AddChild(instance);
            GetNode<Timer>("EquippedWeapon/Timer").WaitTime = 0.25f;
        }
        else if (gunType == "minigun")
        {
            var instance = minigunScene.Instantiate<Node3D>();
            instance.Name = "WeaponModel";
            GetNode<Node3D>("EquippedWeapon/Model").AddChild(instance);
            GetNode<Timer>("EquippedWeapon/Timer").WaitTime = 0.1f;
        }
        else if (gunType == "pistol")
        {
            var instance = pistolScene.Instantiate<Node3D>();
            instance.Name = "WeaponModel";
            GetNode<Node3D>("EquippedWeapon/Model").AddChild(instance);
            GetNode<Timer>("EquippedWeapon/Timer").WaitTime = 0.5f;
        }
    }
}
