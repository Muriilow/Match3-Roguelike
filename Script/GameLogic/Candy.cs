namespace Match3.GameLogic;
using Match3.GameLogic.Utilities;
using Godot;
using System;

public partial class Candy : Node2D
{
    // [Export] private asas _animation;
    [Export] public CandyColor color;
    
    //My x and y position
    [Export] private int _xIndex;
    [Export] private int _yIndex;
    public int x, y;
    [Signal] public delegate void MoveEventHandler();
    public int XIndex
    {
        get => _xIndex;
        set => _xIndex = value;
    }

    public int YIndex
    {
        get => _yIndex;
        set => _yIndex = value;
    }

    //If the candy is going to show up the points
    public bool WasSelected { get; set; }
    public bool IsMatched { get; set; } 
    public bool IsClicked { get; set; }
    public override void _Ready()
    {
        
    }
    public void MoveToTarget(Vector2 targetPos)
    {
        // Cria um Tween para animação suave
        var tween = CreateTween();
        tween.SetEase(Tween.EaseType.InOut);
        tween.SetTrans(Tween.TransitionType.Quad);
    
        tween.TweenProperty(this, "position", targetPos, 0.2f);
        tween.TweenCallback(Callable.From(() => EmitSignal("Move")));
    }
}