namespace Match3.Script;
using Godot;
using System;

public partial class Candy : Node2D
{
    // [Export] private asas _animation;
    // public CandyColor candyColor;
    
    //My x and y position
    public int XIndex { get; set; }
    public int YIndex { get; set; }

    //If the candy is going to show up the points
    public bool WasSelected { get; set; }
    public bool IsMatched { get; set; } 
    public bool IsClicked { get; set; }
    public bool IsMoving { get; private set; }

    public override void _Ready()
    {
        GD.Print(XIndex, YIndex);
    }
}