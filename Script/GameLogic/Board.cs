namespace Match3.GameLogic;
using Utilities;
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class Board : Node
{
	[Export] private int _width;
	[Export] private int _height;

	private float _spacingX;
	private float _spacingY;

	[Export] private PackedScene[] _candiesPrefabs;
	private Tile[,] _tiles;
	[Export] private Candy _selectedCandy;
	private List<Candy> _candiesToRemove = new();
	
	private void InitializeBoard()
	{
		//Variables that control where should the candies be placed
		_tiles = new Tile[_width, _height];
		_spacingX = (float)(_width - 1) / 2 - 3; //-0.5
		_spacingY = (float)(_height - 1) / 2; //3.5

		for (int y = 0; y < _height; y++)
		{
			for (int x = 0; x < _width; x++)
			{
				Vector2 position;

				float xPos = x - _spacingX;
				float yPos = y - _spacingY;

				position = new Vector2(xPos, yPos);

				CreateCandies(position, x, y);
			}
		}
	}
	
	private void CreateCandies(Vector2 position, int x, int y)
	{
		int randomIndex;
		Candy candy;
		    
		randomIndex = GD.RandRange(0, _candiesPrefabs.Length-1);
		
		candy = _candiesPrefabs[randomIndex].Instantiate<Candy>();
		candy.Position = new Vector2(120 + x * 130, 80 + y * 130);
		candy.Name = "Candy" + x + "_" + y;
		
		candy.XIndex = x;
		candy.YIndex = y;
		candy.x = x;
		candy.y = y;
		_tiles[x, y] = new Tile(true, candy);
		GD.Print(candy.Position);
		AddChild(candy);
		// //Creating the background
		// background = Instantiate(_background, position, Quaternion.identity);
		// background.transform.SetParent(_backroundParent.transform);
		// _backgrounds.Add(background);
		// 
		// _tiles[x, y] = new BackgroundTile(true, candy);
		// _candies.Add(candy);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitializeBoard();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	#region Matching Logic

	private bool CheckBoard()
	{
		var hasMatched = false;
		_candiesToRemove.Clear();
		
		foreach (Tile tile in _tiles)
			if (tile.Candy != null)
				tile.Candy.IsMatched = false;

		//check every candy inside the board 
		for (var x = 0; x < _width; x++)
		{
			for (var y = 0; y < _height; y++)
			{
				//Checking if the tile is usable and have a candy
				if (_tiles[x, y].IsUsable != true || _tiles[x, y].Candy == null)
					continue;
                
				var candy = _tiles[x, y].Candy;
				candy.WasSelected = false;
				
				if (candy.IsMatched)
					return hasMatched;

				var match = FindMatch(candy);
				if (match.connectedCandies.Count < 3)
					continue;
				
				match = CheckSuper(match);
				_candiesToRemove.AddRange(match.connectedCandies);
				foreach(var cand in match.connectedCandies)
					cand.IsMatched = true;
				
				hasMatched = true;
			
			}
		}
		return hasMatched;
	}

	private MatchResult FindMatch(Candy candy)
	{
		var candies = new List<Candy>();
		candies.AddRange(CheckDirection(candy, Vector2I.Right));
		candies.AddRange(CheckDirection(candy, Vector2I.Left));

		if (candies.Count >= 3)
		{
			var direction = MatchDirection.Horizontal;
			GD.Print(candies);
			return new MatchResult(candies, direction);
		}
		
		candies.Clear();
		candies.AddRange(CheckDirection(candy, Vector2I.Up));
		candies.AddRange(CheckDirection(candy, Vector2I.Down));

		if (candies.Count >= 3)
		{
			var direction = MatchDirection.Vertical;
			GD.Print(candies);
			return new MatchResult(candies, direction);
		}

		return new MatchResult(candies, MatchDirection.None);
	}
	//Checking after we found the main line of connected candies.
	//Here we are looking for matches in a T or L shape.
	private MatchResult CheckSuper(MatchResult match)
	{
		foreach (Candy candy in match.connectedCandies)
		{
			List<Candy> extra = new List<Candy>();
			if (match.direction == MatchDirection.Horizontal)
			{
				extra.AddRange(CheckDirection(candy, Vector2I.Down));
				extra.AddRange(CheckDirection(candy, Vector2I.Up));
			}
			else
			{
				extra.AddRange(CheckDirection(candy, Vector2I.Left));
				extra.AddRange(CheckDirection(candy, Vector2I.Right));
			}

			//Checking L shape
			if (extra.Count < 2 || match.connectedCandies.Count < 2) 
				continue;
			
			var newMatch = extra.Concat(match.connectedCandies).Distinct().ToList();
			return new MatchResult(newMatch, MatchDirection.Super);
		}
		return match;
	}
	private List<Candy> CheckDirection(Candy candy, Vector2I direction)
	{
		List<Candy> candyList = new List<Candy>();

		var x = candy.XIndex + direction.X;
		var y = candy.YIndex + direction.Y;
		
		while (CheckBoundaries(x, y) && CheckTile(x, y))
		{
			Candy other = _tiles[x, y].Candy;
			if (other.color == candy.color)
			{
				candyList.Add(other);
				x += direction.X;
				y += direction.Y;
			}
		}

		return candyList;
	}

	private bool CheckTile(int x, int y) => _tiles[x, y].IsUsable &&  _tiles[x, y].Candy != null;
	private bool CheckBoundaries(int x, int y) => x >= 0 && x < _width && y >= 0 && y < _height;
	#endregion 
	
	
	#region Swapping Candies
	//Check for some variables of the candy to see if the player wants to mark the candy or the contrary 
	public void SelectCandy(Candy candy)
	{
		if (_selectedCandy == null)
		{
			_selectedCandy = candy;
			candy.IsClicked = true;
		}
		else if (_selectedCandy == candy)
		{
			_selectedCandy = null;
			candy.IsClicked = false;
		}
		//Attempt a swap if it's other candy
		else if (_selectedCandy != candy)
		{
			SwapCandy(_selectedCandy, candy);
			_selectedCandy = null;
		}
	}
	private void SwapCandy(Candy currentCandy, Candy targetCandy)
	{
		currentCandy.IsClicked = false;
		//if (!IsAdjacent(currentCandy, targetCandy))
		//	return;

		if(currentCandy != null && targetCandy != null)
			DoSwap(currentCandy, targetCandy); 
		
		ProcessMatches(currentCandy, targetCandy);
	}
	//Swapping the information between both candies 
	private void DoSwap(Candy currentCandy, Candy targetCandy)
	{
		Candy temp;
		int tempXIndex, tempYIndex;
        
		temp = _tiles[currentCandy.XIndex, currentCandy.YIndex].Candy;
		_tiles[currentCandy.XIndex, currentCandy.YIndex].Candy = _tiles[targetCandy.XIndex, targetCandy.YIndex].Candy;
		_tiles[targetCandy.XIndex, targetCandy.YIndex].Candy = temp;

		//Swap the indexes of each candy 
		tempXIndex = currentCandy.XIndex;
		tempYIndex = currentCandy.YIndex;
        
		currentCandy.XIndex = targetCandy.XIndex;
		currentCandy.YIndex = targetCandy.YIndex;
        
		targetCandy.XIndex = tempXIndex;
		targetCandy.YIndex = tempYIndex;

		//Make each candy move 
		currentCandy.MoveToTarget(targetCandy.Position);
		targetCandy.MoveToTarget(currentCandy.Position);
	}

	private async void ProcessMatches(Candy currentCandy, Candy targetCandy)
	{
		SignalAwaiter moveCurrent = ToSignal(currentCandy, "Move");
		await moveCurrent;
		GD.Print("-----");
		//If we find a match process the points and move on, otherwise swap to their positions again 
		//if (CheckBoard())
		//{
		//	targetCandy.WasSelected = true;
		//	currentCandy.WasSelected = true;

		//	StartCoroutine(DoMatch(true, false));
		//}
		//else
		//DoSwap(currentCandy, targetCandy);
	}

	#endregion
}
