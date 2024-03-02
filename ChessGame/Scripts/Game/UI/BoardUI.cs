using Godot;
using Game.Engine;

namespace Game;

public partial class BoardUI : Node2D
{
	[ExportGroup("Board Style", "Color")]
	[Export] Color ColorLightSquares;
	[Export] Color ColorDarkSquares;
	[Export] Color ColorHighlight;
	
	public static float SquareSize {set; get;}
	public static float XOffset {set; get;}
	public static bool WhiteSide {set; get;} = true;

	private static int _selectedSquare = -1;

	private PackedScene _pieceScene;

    public override void _Ready()
    {
		Board.InitializeBoard();

        _pieceScene = GD.Load<PackedScene>("res://Entities/Piece.tscn");
		StartBoard();
    }

    public override void _Draw()
    {
		DrawBoard();

		PieceSprite.UpdateSVGTexture();
		
		foreach (var child in GetChildren())
		{
			foreach (var childPiece in child.GetChildren())
			{
				if (childPiece is PieceSprite pieceSprite)
				{
					pieceSprite.DrawPiece();
				}
			}
		}
    }

	public static Vector2 GetPositionFromSquare(int square)
	{
		float xPosition = square%8 * SquareSize + XOffset;
		float yPosition = (1 + square/8) * SquareSize;

		if (!WhiteSide)
		{
			xPosition = SquareSize * (7 - square%8) + XOffset;
			yPosition = SquareSize * 9 - yPosition;		
		}

		return new(xPosition, yPosition);
	}

	public static int GetSquareFromPosition(Vector2 position)
	{
		int row = (int)((position.X - XOffset)/SquareSize);
		int rank = (int)(position.Y/SquareSize - 1);

		if (!WhiteSide)
		{
			return 63 - row - rank*8;
		}

		return row + rank*8;
	}

	// Draws board squares
	private void DrawBoard()
	{
		SquareSize = GetViewportRect().Size.Y / 10;
		XOffset = (GetViewportRect().Size.X - SquareSize * 8) / 2;

        for (int i = 0; i < 64; i++)
		{
			float xPosition = i%8 * SquareSize + XOffset;
			float yPosition = (1 + i/8) * SquareSize;

			Color squareColor;
			if (i != _selectedSquare)
			{
				bool lightSquare = (i%8 + i/8)%2 == 0;
				squareColor = lightSquare ? ColorLightSquares : ColorDarkSquares;
			}
			else
			{
				squareColor = ColorHighlight;
			}

			DrawRect(new Rect2(xPosition, yPosition, SquareSize, SquareSize), squareColor);
		}
	}

	private void StartBoard()
	{
		var white = GetNode<Player>("White");
		var black = GetNode<Player>("Black");

		PieceSprite.BoardUINode = GetNode<BoardUI>(this.GetPath());

		for (int i = 0; i < 64; i ++)
		{
			Piece piece = Board.Square[i]; 
			if (piece.Type != PieceType.Empty)
			{
				PieceSprite pieceInstance = _pieceScene.Instantiate<PieceSprite>();
				pieceInstance.PieceData = piece;
				pieceInstance.BoardPosition = i;

				if (piece.Color == PieceColor.White)
				{
					white.AddChild(pieceInstance);
					continue;
				}
				
				black.AddChild(pieceInstance);
			}
		}
	}

	public void OnPieceClicked(int boardPosition)
	{
		_selectedSquare = boardPosition;
		QueueRedraw();
	}
}
