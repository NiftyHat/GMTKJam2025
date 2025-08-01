using Godot;

namespace GMTKJam2025.Audio;

[GlobalClass]
public partial class SoundEffectSample : Resource
{
    [Export] public AudioStream Stream { get; set; }

    [Export(PropertyHint.Range, "-80,24,or_greater,suffix:dB")]
    public float VolumeDb { get; set; } = 0;

    [Export(PropertyHint.Range, "0.01,4,or_greater")]
    public float PitchScale { get; set; } = 1;
}