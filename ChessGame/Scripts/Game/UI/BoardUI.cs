using Godot;
using Game.Engine;
using System.Collections.Generic;

namespace Game;

public partial class BoardUI : Node2D
{
	[ExportGroup("Board Style", "Color")]
	[Export] Color ColorLightSquares {set; get;} 
	[Export] Color ColorDarkSquares {set; get;} 
	[Export] Color ColorHighlight {set; get;} 
	[Export] Color ColorLastMove {set; get;} 
	
	public static float SquareSize {set; get;}
	public static float XOffset {set; get;}
	public static bool WhiteSide {set; get;} = true;

	private BoardOverlay boardOverlay;

	private PackedScene _pieceScene;
	private readonly Dictionary<int, PieceSprite> _pieces = new();

    public override void _Ready()
    {
		Board.InitializeBoard();

		boardOverlay = GetNode<BoardOverlay>("BoardOverlay");
		boardOverlay.ColorHighlight = ColorHighlight;
		boardOverlay.ColorLastMove = ColorLastMove;

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

			bool lightSquare = (i%8 + i/8)%2 == 0;
			Color squareColor = lightSquare ? ColorLightSquares : ColorDarkSquares;
			
			DrawRect(new Rect2(xPosition, yPosition, SquareSize, SquareSize), squareColor);
		}
	}

	// Initializes board and instantiates the pieces
	private void StartBoard()
	{
		var white = GetNode<Player>("White");
		var black = GetNode<Player>("Black");

		for (int i = 0; i < 64; i ++)
		{
			Piece piece = Board.Square[i]; 
			if (piece.Type != PieceType.Empty)
			{
				PieceSprite pieceInstance = _pieceScene.Instantiate<PieceSprite>();
				pieceInstance.PieceData = piece;
				pieceInstance.BoardPosition = i;

				pieceInstance.ClickEvent += OnPieceClicked;
				pieceInstance.MoveEvent += OnPieceMoved;

				_pieces.Add(i, pieceInstance);

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
		boardOverlay.SelectedSquare = boardPosition;
		boardOverlay.QueueRedraw();
	}

	public void OnPieceMoved(int from, int to)
	{
		Board.MovePiece(from, to);

		if (_pieces.ContainsKey(to))
		{
			_pieces[to].QueueFree();
			_pieces[to] = _pieces[from];
			Other.SoundHandler.PlayCaptureSound();
		}
		else
		{
			_pieces.Add(to, _pieces[from]);
			Other.SoundHandler.PlayMoveSound();
		}

		_pieces.Remove(from);

		boardOverlay.MoveFromSquare = from;
		boardOverlay.MoveToSquare = to;
		boardOverlay.SelectedSquare = -1;
		boardOverlay.QueueRedraw();
	}
}
