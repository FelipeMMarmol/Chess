using Godot;

namespace Game.Other;

public partial class SoundHandler : Node
{
    static AudioStreamPlayer2D Move;
    static AudioStreamPlayer2D Capture;

    public override void _Ready()
    {
        Move = GetChild<AudioStreamPlayer2D>(0);
        Capture = GetChild<AudioStreamPlayer2D>(1);
    }

    public static void PlayCaptureSound()
    {
        Capture.Play();
    }

    public static void PlayMoveSound()
    {
        Move.Play();
    }
}