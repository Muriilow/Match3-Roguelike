namespace Match3.Script.GameLogic;
using Godot;
using System;
public partial class ControlManager : Node
{
    [Export] private Board _board;
    //[Export] private GameManager _manager;
    
    public override void _Ready()
    {

    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && 
            mouseEvent.Pressed && 
            mouseEvent.ButtonIndex == MouseButton.Left)
        {
            GD.Print("Button pressed");
            CheckClick();
        }
    }
    
    private void CheckClick()
    {
        Vector2 mousePos = GetViewport().GetMousePosition();
        var spaceState = GetTree().Root.GetWorld2D().DirectSpaceState;
            
        var result = spaceState.IntersectPoint(new PhysicsPointQueryParameters2D
        {
            Position = mousePos,
            CollideWithAreas = true
        });

        if (result.Count > 0)
        {
            Candy candy = result[0]["collider"].As<Area2D>().GetParent<Candy>();
            GD.Print(candy.XIndex, candy.YIndex);
            _board.SelectCandy(candy);
        }
    }
}