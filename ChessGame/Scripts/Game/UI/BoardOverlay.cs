using Godot;

namespace Game;

public partial class BoardOverlay : Node2D
{

    public int SelectedSquare {set; get;} = -1;
    public int MoveFromSquare {set; get;} = -1;
    public int MoveToSquare {set; get;} = -1;

    public Color ColorHighlight {set; get;}
    public Color ColorLastMove {set; get;}

    public override void _Draw()
    {
        void DrawSquare(int square, Color color)
        {
            if (square > 0)
            {
                var squarePosition = BoardUI.GetPositionFromSquare(square);
                DrawRect(new Rect2(squarePosition.X, squarePosition.Y, BoardUI.SquareSize, BoardUI.SquareSize), color);
            }
        }

        DrawSquare(SelectedSquare, ColorHighlight);

        if (MoveFromSquare != SelectedSquare)
            DrawSquare(MoveFromSquare, ColorLastMove);

        if (MoveToSquare != SelectedSquare)
            DrawSquare(MoveToSquare, ColorLastMove);
    }
}