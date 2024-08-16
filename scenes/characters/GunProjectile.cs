using Godot;
using System;

public partial class GunProjectile : Node3D
{
    private static int BULLET_LIFE_SPAN = 1;
    private static float BULLET_SPEED = 50f;

    public override void _Ready()
    {
        var timer = new Timer
        {
            WaitTime = BULLET_LIFE_SPAN,
            Autostart = true
        };
        timer.Timeout += QueueFree;
        AddChild(timer);
    }

    public override void _Process(double delta)
    {
        var position = Position;
        position.Z += BULLET_SPEED * (float)delta;
        Position = position;
    }
}
