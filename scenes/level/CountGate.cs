using Godot;
using System;

public partial class CountGate : Node3D
{

    public override void _Ready()
    {
        var choiceA = GetNode<Label3D>("ChoiceA/Area3D/Label3D");
        var choiceB = GetNode<Label3D>("ChoiceB/Area3D/Label3D");
        if (GD.Randi() % 2 == 0)
        {
            Rotate(new Vector3(0, 1, 0), Mathf.DegToRad(180));
            choiceA.Rotate(new Vector3(0, 1, 0), Mathf.DegToRad(180));
            choiceB.Rotate(new Vector3(0, 1, 0), Mathf.DegToRad(180));
            choiceA.Text = $"-{GD.Randi() % 10}";
            choiceB.Text = $"+{GD.Randi() % 10}";
        }
        choiceA.Text = $"+{GD.Randi() % 10}";
        choiceB.Text = $"-{GD.Randi() % 10}";
    }
}
