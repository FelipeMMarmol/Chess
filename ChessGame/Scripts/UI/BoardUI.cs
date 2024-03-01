using Godot;

namespace Game;

public partial class BoardUI : Node2D
{
	[ExportGroup("Board Style", "Color")]
	[Export] Color ColorLightSquares;
	[Export] Color ColorDarkSquares;
	[Export] Color ColorHighlight;
	
	public static float SquareSize {set; get;} = 64;
	public static float XOffset {set; get;}

	public Board board = new();

	private PackedScene pieceScene;

    public override void _Ready()
    {
        pieceScene = GD.Load<PackedScene>("res://Entities/Piece.tscn");
		StartBoard();
    }

    public override void _Draw()
    {
		DrawBoard();

		PieceSprite.UpdateSVGTexture();
		
		foreach (var child in GetChildren())
		{
			if (child is PieceSprite pieceSprite)
			{
				pieceSprite.DrawPiece();
			}
		}
    }

	public static Vector2 GetPositionFromSquare(int square)
	{
		float xPosition = square%8 * SquareSize + XOffset;
		float yPosition = (1 + square/8) * SquareSize;

		return new(xPosition, yPosition);
	}

	public static int GetSquareFromPosition(Vector2 position)
	{
		int row = (int)((position.X - XOffset)/SquareSize);
		int rank = (int)(position.Y/SquareSize - 1);

		return row + rank*8;
	}

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

	private void StartBoard()
	{
		for (int i = 0; i < 64; i ++)
		{
			Piece piece = Board.Square[i]; 
			if (piece.Type != PieceType.Empty)
			{
				PieceSprite pieceInstance = pieceScene.Instantiate<PieceSprite>();
				pieceInstance.PieceData = piece;
				pieceInstance.BoardPosition = i;

				AddChild(pieceInstance);
			}
		}
	}
}
