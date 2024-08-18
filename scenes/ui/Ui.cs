using Godot;
using System;

public partial class Ui : Control
{
    private int score = 0;

    private double elapsedTimeInSeconds = 0;

    public bool gameIsFinished = false;

    private PackedScene levelScene = GD.Load<PackedScene>("res://scenes/level/level.tscn");

    public override async void _Process(double delta)
    {
        if (gameIsFinished)
        {
            return;
        }
        elapsedTimeInSeconds += delta;
        int minutes = (int)elapsedTimeInSeconds / 60;
        int seconds = (int)elapsedTimeInSeconds % 60;

        GetNode<Label>("TimeContainer/TimeLabel").Text = $"Elapsed time: {minutes:D2}:{seconds:D2}";
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        GetNode<Label>("ScoreContainer/ScoreLabel").Text = $"Score: {score}";
    }

    private void OnRetryButtonPressed()
    {
        GetNode<Control>("GameOverUI").Visible = false;
        GetTree().Root.GetNode("Level").QueueFree();
        var instance = levelScene.Instantiate<Node3D>();
        GetTree().Root.AddChild(instance);
    }

}