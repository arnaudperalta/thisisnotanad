using Godot;
using System;

public partial class CountGate : Node3D
{
    [Signal]
    public delegate void CountGateEnteredEventHandler(int value);

    private static int CHOICE_HEALTH = 20;

    private uint choiceAValue;
    private uint choiceBValue;
    private Label3D choiceALabel;
    private Label3D choiceBLabel;

    private int choiceAHP = CHOICE_HEALTH;
    private int choiceBHP = CHOICE_HEALTH;

    public override void _Ready()
    {
        choiceALabel = GetNode<Label3D>("ChoiceA/Area3D/Label3D");
        choiceBLabel = GetNode<Label3D>("ChoiceB/Area3D/Label3D");
        choiceAValue = (GD.Randi() % 4) + 1;
        choiceBValue = (GD.Randi() % 4) + 1;
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
        if (body.CollisionLayer == 1) // If it's a soldier
        {
            EmitSignal(SignalName.CountGateEntered, choiceAValue);
            QueueFree();
        }
        else if (body.CollisionLayer == 2) // If it's a projectile
        {
            choiceAHP--;
            if (choiceAHP <= 0)
            {
                choiceAHP = CHOICE_HEALTH;
                choiceAValue++;
                choiceALabel.Text = $"+{choiceAValue}";
            }
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
        else if (body.CollisionLayer == 2) // If it's a projectile
        {
            choiceBHP--;
            if (choiceBHP <= 0)
            {
                choiceBHP = CHOICE_HEALTH;
                choiceBValue++;
                choiceBLabel.Text = $"-{choiceBValue}";
            }
            body.GetParent().QueueFree();
        }
    }

}
