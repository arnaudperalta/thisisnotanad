using Godot;
using System;

public partial class CountGate : Node3D
{
    [Signal]
    public delegate void CountGateEnteredEventHandler(int value);

    private uint choiceAValue;
    private uint choiceBValue;
    private Label3D choiceALabel;
    private Label3D choiceBLabel;

    public override void _Ready()
    {
        choiceALabel = GetNode<Label3D>("ChoiceA/Area3D/Label3D");
        choiceBLabel = GetNode<Label3D>("ChoiceB/Area3D/Label3D");
        choiceAValue = GD.Randi() % 10;
        choiceBValue = GD.Randi() % 10;
        if (GD.Randi() % 2 == 0)
        {
            Rotate(new Vector3(0, 1, 0), Mathf.DegToRad(180));
            choiceALabel.Rotate(new Vector3(0, 1, 0), Mathf.DegToRad(180));
            choiceBLabel.Rotate(new Vector3(0, 1, 0), Mathf.DegToRad(180));
            choiceALabel.Text = $"-{choiceAValue}";
            choiceBLabel.Text = $"+{choiceBValue}";
        }
        choiceALabel.Text = $"+{choiceAValue}";
        choiceBLabel.Text = $"-{choiceBValue}";
    }

    private void OnPositiveGateBodyEntered(StaticBody3D body)
    {
        if (body.CollisionLayer == 1)
        {
            EmitSignal(SignalName.CountGateEntered, choiceAValue);
            QueueFree();
        }
        else
        {
            choiceAValue++;
            choiceALabel.Text = $"+{choiceAValue}";
            body.GetParent().QueueFree();
        }
    }

    private void OnNegativeGateBodyEntered(StaticBody3D body)
    {
        if (body.CollisionLayer == 1)
        {
            EmitSignal(SignalName.CountGateEntered, -choiceBValue);
            QueueFree();
        }
        else
        {
            choiceBValue++;
            choiceBLabel.Text = $"-{choiceBValue}";
            body.GetParent().QueueFree();
        }
    }

}
