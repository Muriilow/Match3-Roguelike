namespace Match3.Script;
using Godot;
using System;

public class Tile
{
    public bool IsUsable { get; set; }
    public Candy Candy { get; set; }
    
    //Constructor
    public Tile(bool isUsable, Candy candy)
    {
        IsUsable = isUsable;
        Candy = candy;
    }
}
