using Godot;
using System;

public partial class Defense : Node3D
{

    private static float STEP_DISTANCE = 2f;

    private enum Direction { LEFT, RIGHT }

    public override async void _Process(double delta)
    {
        if (Input.IsActionPressed("move_right"))
        {
            MoveSoldiers(Direction.LEFT, delta);
        }
        if (Input.IsActionPressed("move_left"))
        {
            MoveSoldiers(Direction.RIGHT, delta);
        }
    }

    private void MoveSoldiers(Direction direction, double delta)
    {
        var related_speed = direction == Direction.RIGHT ? STEP_DISTANCE : STEP_DISTANCE * -1;

        foreach (Soldier soldier in GetNode<Node3D>("Soldiers").GetChildren())
        {
            soldier.Walk();
            var position = soldier.Position;
            position.X += related_speed * (float)delta;
            soldier.Position = position;
        }
    }

}
