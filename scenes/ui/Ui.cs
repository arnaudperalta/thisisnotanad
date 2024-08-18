using Godot;
using System;

public partial class Ui : Control
{
    private int score = 0;

    private double elapsedTimeInSeconds = 0;

    public override async void _Process(double delta)
    {
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

}