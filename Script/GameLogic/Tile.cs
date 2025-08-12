namespace Match3.Script.GameLogic;
using Godot;
using System;

public class Tile(bool isUsable, Candy candy)
{
    public bool IsUsable { get; set; } = isUsable;
    public Candy Candy { get; set; } = candy;
}
