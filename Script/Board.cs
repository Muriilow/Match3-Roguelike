namespace Match3.Script;
using Godot;
using System;

public partial class Board : Node2D
{
	[Export] private int _width;
	[Export] private int _height;

	private float _spacingX;
	private float _spacingY;

	[Export] private PackedScene[] _candiesPrefabs; 
	private void InitializeBoard()
	{
		//Variables that control where should the candies be placed
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
		candy.Position = new Vector2(100 + x * 120, 50 + y * 120);
		candy.Name = "Candy" + x + "_" + y;
		
		candy.XIndex = x;
		candy.YIndex = y;

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
}
