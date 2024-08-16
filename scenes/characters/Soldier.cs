using Godot;
using System;

public partial class Soldier : Node3D
{

  private AnimationPlayer animationPlayer;
  private PackedScene projectileScene = GD.Load<PackedScene>("res://scenes/characters/gun_projectile.tscn");


  public override void _Ready()
  {
    animationPlayer = GetNode<Node3D>("Model").GetNode<AnimationPlayer>("AnimationPlayer");
    animationPlayer.Play("sprint");
  }

  public void OnEquippedWeaponTimerTimeout()
  {
    var instance = projectileScene.Instantiate<GunProjectile>();
    GetNode<Node3D>("EquippedWeapon/Projectiles").AddChild(instance);
  }
}
