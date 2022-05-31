namespace Drift
{
    public interface ISoundService
    {
        SoundDefinition[] Sounds { get; }
        SoundDefinition GetSoundById(int id);
    }
}