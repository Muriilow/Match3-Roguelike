using Match3.Script.GameLogic.Conditions.Interfaces;

namespace Match3.Script.GameLogic;
using Godot;
using System;

public class GameState
{
	public int Score { get; set; }
	public int MovesRemaining { get; set; }
	public float TimeRemaining { get; set; }
}

public partial class GameManager: Node
{
	private IWinCondition winCondition;
	private ILoseCondition loseCondition;

	public GameManager(IWinCondition win, ILoseCondition lose)
	{
		winCondition = win;
		loseCondition = lose;
	}
	
	public void SetWinCondition(IWinCondition win) => winCondition = win;
	public void SetLoseCondition(ILoseCondition lose) => loseCondition = lose;
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (winCondition.HasWon())
		{
			EndGame(true);
			return;
		}

		if (loseCondition.HasLost())
			EndGame(false);
	}

	private void EndGame(bool victory)
	{
		if (victory)
			GD.Print("Você venceu!");
		else
			GD.Print("Você perdeu!");
	}
}
