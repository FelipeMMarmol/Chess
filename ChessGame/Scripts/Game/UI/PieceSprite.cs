using Godot;
using Game.Engine;

namespace Game;

public partial class PieceSprite : Sprite2D
{
	public int BoardPosition {set; get;}
	public Piece PieceData {set; get;}

	private static readonly float _originalPixelSize = 45f;
	private static float _pieceScale = 1f;
	private static string _imageString;
	private static readonly Image _image = new();

	private bool _selected = false;
	private bool _holding = false;

	public delegate void OnPieceClickedEventHandler(int boardPosition);
	public event OnPieceClickedEventHandler ClickEvent;

	public delegate void OnPieceMovedEventHandler(int from, int to);
	public event OnPieceMovedEventHandler MoveEvent;

    public override void _Ready()
    {
        using var file = FileAccess.Open("res://Assets/Graphics/Chess_Pieces_Sprite.svg", FileAccess.ModeFlags.Read);
		_imageString = file.GetAsText();
    }

    public override void _Process(double delta)
    {
        if (_holding)
		{
			Position = GetGlobalMousePosition() - Vector2.One * _pieceScale *_originalPixelSize/2;
		}
    }

    public void DrawPiece()
    {
		Texture = ImageTexture.CreateFromImage(_image);
		Frame = GetFrame();

		Position = BoardUI.GetPositionFromSquare(BoardPosition);
		UpdateHitbox();
	}

	// Updates SVG when size changes
	public static void UpdateSVGTexture()
	{
		if (_pieceScale != BoardUI.SquareSize/_originalPixelSize)
		{
			_pieceScale = BoardUI.SquareSize/_originalPixelSize;
			_image.LoadSvgFromString(_imageString, _pieceScale);
		}
	}

	// Returns the correct frame (sprite) corresponding to the piece
	private int GetFrame()
	{
		int frame = 0;

		switch (PieceData.Type)
		{
			case PieceType.King:
				frame = 0;
				break;
			case PieceType.Queen:
				frame = 1;
				break;
			case PieceType.Bishop:
				frame = 2;
				break;
			case PieceType.Knight:
				frame = 3;
				break;
			case PieceType.Rook:
				frame = 4;
				break;
			case PieceType.Pawn:
				frame = 5;
				break;
		}

		return frame + 6 * (int)PieceData.Color;
	}

	private void UpdateHitbox()
	{
		var hitbox = GetNode<Area2D>("%Hitbox");

		Vector2 scale = new (_pieceScale, _pieceScale);
		hitbox.Scale = scale;
		hitbox.Position = scale*_originalPixelSize/2;
	}
	
	// Handles selecting and grabbing events
	public void OnHitboxInputEvent(Node viewport, InputEvent @event, int shapeIdx)
	{
		Label label = GetNode<Label>("Label");
		if (Input.IsActionJustPressed("Click"))
		{
			_holding = true;
			ZIndex = 2;
			ClickEvent.Invoke(BoardPosition);

			GetViewport().SetInputAsHandled();
		}
		else if (Input.IsActionJustReleased("Click") && _holding)
		{
			int newSquare = BoardUI.GetSquareFromPosition(GetGlobalMousePosition());

			if (BoardPosition != newSquare)
			{
				MoveEvent.Invoke(BoardPosition, newSquare);
				BoardPosition = newSquare;
			}
			
			Position = BoardUI.GetPositionFromSquare(BoardPosition);
			_holding = false;
			ZIndex = 1;

			GetViewport().SetInputAsHandled();
		}
	}
}
