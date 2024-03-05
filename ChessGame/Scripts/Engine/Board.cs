using System.Collections.Generic;
using System.Text;

namespace Game.Engine;

// Handles the game, it contains the current state of the game and contains methods
// to move and capture pieces.

public static class Board
{
    public static Piece[] Square {set; get;}
    public static bool IsWhitePlaying {set; get;} = true;

    public static void InitializeBoard()
    {
        Square = new Piece[64];

        string StartPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        LoadFENPosition(StartPosition);
    }

    public static void MovePiece(int fromSquare, int toSquare)
    {
        Square[toSquare] = Square[fromSquare];
        Square[fromSquare] = default;
    }

    public static void LoadFENPosition(string FEN)
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

    public static string GenerateFEN()
    {
        StringBuilder fenBuilder = new();

        int blankSpaces = 0;
        for (int i = 0; i < 64; i++)
        {
            Piece piece = Square[i];

            if (i % 8 == 0 && i > 0)
            {
                if (blankSpaces > 0)
                {
                    fenBuilder.Append(blankSpaces);
                    blankSpaces = 0;
                }
                fenBuilder.Append('/');
            }

            if (piece.Type != PieceType.Empty)
            {
                if (blankSpaces > 0)
                {
                    fenBuilder.Append(blankSpaces);
                    blankSpaces = 0;
                }
                fenBuilder.Append(piece);
            }
            else
            {
                blankSpaces++;
            }
        }

        // Handle any remaining blank spaces at the end of the rank
        if (blankSpaces > 0)
        {
            fenBuilder.Append(blankSpaces);
        }

        return fenBuilder.ToString();
    }
}