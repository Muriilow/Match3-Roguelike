using Godot;
using System.Collections;
using System.Collections.Generic;


namespace Match3.Script.GameLogic.Utilities
{
    //To get easy to read the match result
    public class MatchResult(List<Candy> connectedCandies, MatchDirection direction)
    {
        public List<Candy> connectedCandies = connectedCandies;
        public MatchDirection direction = direction;
    }

    public enum MatchDirection
    {
        Vertical,
        Horizontal,
        Super,
        None
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public enum CandyColor
    {
        PeDeMlk,
        Snickers,
        ChocolateTriang,
        MentaCandy,
        BolinhoCandy
    }
}