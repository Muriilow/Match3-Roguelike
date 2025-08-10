namespace Match3.Script;
using Godot;
using System;

public partial class Board : Node
{
	[Export] private int _width;
	[Export] private int _height;

	private float _spacingX;
	private float _spacingY;

	[Export] private PackedScene[] _candiesPrefabs;
	private Tile[,] _tiles;
	[Export] private Candy _selectedCandy;
	
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

	#region Select Candies

	

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

		//StartCoroutine(ProcessMatches(currentCandy, targetCandy));
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
	#endregion
}
