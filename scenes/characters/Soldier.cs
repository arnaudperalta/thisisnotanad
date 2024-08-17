using Godot;
using System;

public partial class Soldier : Node3D
{

    private PackedScene projectileScene = GD.Load<PackedScene>("res://scenes/characters/gun_projectile.tscn");
    private PackedScene rifleScene = GD.Load<PackedScene>("res://graphics/weapons/rifle.glb");


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
        if (gunType == "rifle")
        {
            GetNode<Node3D>("EquippedWeapon/WeaponModel").QueueFree();
            var instance = rifleScene.Instantiate<Node3D>();
            instance.Name = "WeaponModel";
            GetNode<Node3D>("EquippedWeapon").AddChild(instance);
            GetNode<Timer>("EquippedWeapon/Timer").WaitTime = 0.25f;
        }
    }
}
