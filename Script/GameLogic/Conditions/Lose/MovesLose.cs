using Godot;
using System;
using Match3.Script.GameLogic.Conditions.Interfaces;

namespace Match3.Script.GameLogic.Conditions.Lose;
public class MovesLose(ushort movesRemaining): ILoseCondition
{
    public void UseMove() => movesRemaining--;
    public bool HasLost() => movesRemaining <= 0;
} 
