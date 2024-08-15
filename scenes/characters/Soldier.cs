using Godot;
using System;

public partial class Soldier : Node3D
{
    public void Walk()
    {
        var animationPlayer = GetNode<Node3D>("Model").GetNode<AnimationPlayer>("AnimationPlayer");
        if (!animationPlayer.IsPlaying())
        {
            animationPlayer.Play("sprint");
        }
    }
}
