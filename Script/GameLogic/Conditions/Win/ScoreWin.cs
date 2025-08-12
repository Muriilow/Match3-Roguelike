using Match3.Script.GameLogic.Conditions.Interfaces;
using Match3.Script.GameLogic;
using Godot;
using System;

namespace Match3.Script.GameLogic.Conditions.Win;
public class ScoreWin(uint target, GameState state) : IWinCondition
{
    public bool HasWon() => state.Score >= target;
}