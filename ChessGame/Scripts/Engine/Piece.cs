using System;
using System.Collections.Generic;

namespace Game.Engine;

[Flags]
public enum PieceType : byte
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
public enum PieceColor : byte
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

    static readonly Dictionary<PieceType, char> PieceSymbol = new()
    {
        [PieceType.Empty] = ' ',
        [PieceType.Pawn] = 'p',
        [PieceType.Knight] = 'n',
        [PieceType.Bishop] = 'b',
        [PieceType.Rook] = 'r',
        [PieceType.Queen] = 'q',
        [PieceType.King] = 'k',
    };

    public override readonly string ToString()
    {
        char pieceChar = Color == PieceColor.White ? char.ToUpper(PieceSymbol[Type]) : PieceSymbol[Type];

        return pieceChar.ToString();
    }
}