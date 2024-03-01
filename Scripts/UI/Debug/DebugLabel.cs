using Godot;
using System;

namespace Game.Debug;

public partial class DebugLabel : Label
{
	public override void _Process(double delta)
	{
		Text = BoardUI.GetSquareFromPosition(GetGlobalMousePosition()).ToString();
	}
}
