using Godot;
public partial class AudioManager : Node
{
	[Export] private AudioStreamPlayer musicPlayer;
    [Export] private AudioStream[] sounds;
    [Export] private string[] soundNames;
    [Export] private AudioStream[] musicTracks;
    [Export] private string[] musicNames;
    [Export] private PackedScene audioPlayer;

	public static AudioManager Instance
	{
		private set;
		get;
	}
	public AudioManager()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			QueueFree();
			return;
		}
	}

    public override void _Ready()
    {

    }


    public void PlaySound(string soundName)
    {
        for (int i = 0; i < soundNames.Length; i++)
        {
            if (soundNames[i] == soundName)
            {
                AudioStreamPlayer newSound = audioPlayer.Instantiate<AudioStreamPlayer>();

				newSound.Name = soundName + "#" + newSound.GetInstanceId(); // Gives object unique name
				AddChild(newSound);
				newSound.Bus = "Sound Effects";
				newSound.Stream = sounds[i];
				newSound.Play();
				newSound.Finished += newSound.QueueFree;

                return;
            }
        }
        GD.Print("Sound named " + soundName + " not found!");
    }
    public void PlayMusic(string musicName)
    {
        for (int i = 0; i < musicNames.Length; i++)
        {
            if (musicNames[i] == musicName)
            {
                if (musicPlayer.Playing)
                    musicPlayer.Stop();
                musicPlayer.Stream = musicTracks[i];
                musicPlayer.Play();
                return;
            }
        }
        GD.Print("Music track named " + musicName + " not found!");
    }
    public bool IsMusicPlaying(string musicName)
    {
        for (int i = 0; i < musicNames.Length; i++)
        {
            if (musicNames[i] == musicName)
            {
                if (musicPlayer.Stream == musicTracks[i])
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void PauseMusic()
    {
        if (musicPlayer.Playing)
        {
            musicPlayer.StreamPaused = true;
        }
    }
    public void ResumeMusic()
    {
        if (musicPlayer.Playing)
        {
            musicPlayer.StreamPaused = false;
        }
    }
    public void StopMusic()
    {
        musicPlayer.Stop();
    }
}