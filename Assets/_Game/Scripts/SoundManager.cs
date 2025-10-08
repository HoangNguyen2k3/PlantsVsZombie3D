using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Default Volumes")]
    [Range(0f, 1f)][SerializeField] private float musicVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float sfxVolume = 1f;

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SfxVolume";

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load volume settings from PlayerPrefs
        musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);

        InitializeAudioSources();
    }

    private void InitializeAudioSources()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    // ================== MUSIC ==================
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying) return; // tránh play lại cùng bài
        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        musicSource.volume = musicVolume;
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicVolume);
        PlayerPrefs.Save();
    }

    public float GetMusicVolume() => musicVolume;

    // ================== SFX ==================
    public void PlaySfx(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        sfxSource.volume = sfxVolume;
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
        PlayerPrefs.Save();
    }

    public float GetSfxVolume() => sfxVolume;

    // ================== TIỆN ÍCH ==================
    public void MuteAll(bool mute)
    {
        musicSource.mute = mute;
        sfxSource.mute = mute;
    }

    public bool IsMuted()
    {
        return musicSource.mute && sfxSource.mute;
    }

    public void StopAllSounds()
    {
        musicSource.Stop();
        sfxSource.Stop();
    }
}
