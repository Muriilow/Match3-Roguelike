using Godot;
using System;

namespace Match3.Script.GameLogic.Conditions.Interfaces
{
	public interface IWinCondition
	{
		bool HasWon();
	}

	public interface ILoseCondition
	{
		bool HasLost();
	}
}
