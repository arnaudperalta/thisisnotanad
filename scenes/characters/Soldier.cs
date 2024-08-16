using Godot;
using System;

public partial class Soldier : Node3D
{

  private AnimationPlayer animationPlayer;

  public override void _Ready()
  {
    animationPlayer = GetNode<Node3D>("Model").GetNode<AnimationPlayer>("AnimationPlayer");
    animationPlayer.Play("sprint");
  }
}
