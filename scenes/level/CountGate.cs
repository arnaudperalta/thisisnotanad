using Godot;
using System;

public partial class CountGate : Node3D
{
    [Signal]
    public delegate void CountGateEnteredEventHandler(int value);

    private uint choiceAValue;
    private uint choiceBValue;

    public override void _Ready()
    {
        var choiceA = GetNode<Label3D>("ChoiceA/Area3D/Label3D");
        var choiceB = GetNode<Label3D>("ChoiceB/Area3D/Label3D");
        choiceAValue = GD.Randi() % 10;
        choiceBValue = GD.Randi() % 10;
        if (GD.Randi() % 2 == 0)
        {
            Rotate(new Vector3(0, 1, 0), Mathf.DegToRad(180));
            choiceA.Rotate(new Vector3(0, 1, 0), Mathf.DegToRad(180));
            choiceB.Rotate(new Vector3(0, 1, 0), Mathf.DegToRad(180));
            choiceA.Text = $"-{choiceAValue}";
            choiceB.Text = $"+{choiceBValue}";
        }
        choiceA.Text = $"+{choiceAValue}";
        choiceB.Text = $"-{choiceBValue}";
    }

    private void OnPositiveGateBodyEntered(StaticBody3D body)
    {
        EmitSignal(SignalName.CountGateEntered, choiceAValue);
        QueueFree();
    }

    private void OnNegativeGateBodyEntered(StaticBody3D body)
    {
        EmitSignal(SignalName.CountGateEntered, choiceBValue * -1);
        QueueFree();
    }

}
