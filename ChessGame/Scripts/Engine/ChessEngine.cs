using System.Collections.Generic;

namespace Game;

public class Board
{
    public static Piece[] Square {set; get;}

    public Board()
    {
        Square = new Piece[64];

        string StartPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        LoadFENPosition(StartPosition);
    }

    public void LoadFENPosition(string FEN)
    {
        var PieceSymbol = new Dictionary<char, PieceType>
        {
            ['p'] = PieceType.Pawn,
            ['n'] = PieceType.Knight,
            ['b'] = PieceType.Bishop,
            ['r'] = PieceType.Rook,
            ['q'] = PieceType.Queen,
            ['k'] = PieceType.King,
        };

        string boardFEN = FEN.Split(' ')[0];

        int cell = 0;
        foreach (char symbol in boardFEN)
        {
            if (symbol == '/')
                continue;

            if (char.IsDigit(symbol))
            {
                cell += (int)char.GetNumericValue(symbol);
                continue;
            }
            else
            {
                Piece piece = new()
                {
                    Color = char.IsUpper(symbol) ? PieceColor.White : PieceColor.Black,
                    Type = PieceSymbol[char.ToLower(symbol)]
                };
                Square[cell] = piece;
                cell ++;
            }
        }
    }
}