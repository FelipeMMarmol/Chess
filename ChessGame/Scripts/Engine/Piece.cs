using System;

namespace Game;

[Flags]
public enum PieceType
{
    Empty = 0,
    Pawn = 1,
    Knight = 2,
    Bishop = 4,
    Rook = 8,
    Queen = 16,
    King = 32
}

[Flags]
public enum PieceColor
{
    White = 0,
    Black = 1
}

public struct Piece
{
    public PieceType Type { get; set; }
    public PieceColor Color { get; set; }

    public Piece(PieceType type = PieceType.Empty, PieceColor color = PieceColor.White)
    {
        Type = type;
        Color = color;
    }
}