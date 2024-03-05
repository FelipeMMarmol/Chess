using Godot;

namespace Game;

public partial class Player : Node
{
    [Export] public bool Human {set; get;}
    [Export] public bool White {set; get;}

    public override void _Ready()
    {
        GD.Print($"Human: {Human}, White: {White}, Script: {GetScript().ToString()}");
    }
}