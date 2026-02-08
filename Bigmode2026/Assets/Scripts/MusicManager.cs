using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField] private AudioMixer MainMixer;
    [SerializeField]
    private MusicLibrary musicLibrary;
    [SerializeField]
    private AudioSource musicSource;
    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SOUNDFX = "SoundFXVolume";
    public const string MIXER_MASTER = "MasterVolume";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        LoadVolume();
    }

    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        StartCoroutine(AnimateMusicCrossfade(musicLibrary.GetClipFromName(trackName), fadeDuration));
    }

    IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration = 0.5f)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(1f, 0, percent);
            yield return null;
        }

        musicSource.clip = nextTrack;
        musicSource.Play();

        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / fadeDuration;
            musicSource.volume = Mathf.Lerp(0, 1f, percent);
            yield return null;
        }
    }
    void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat(MIXER_MUSIC, 1f);
        float soundfxVolume = PlayerPrefs.GetFloat(MIXER_SOUNDFX, 1f);
        float masterVolume = PlayerPrefs.GetFloat(MIXER_MASTER, 1f);
        MainMixer.SetFloat(SoundMixerManager.MIXER_MUSICS, Mathf.Log10(musicVolume) * 20);
        MainMixer.SetFloat(SoundMixerManager.MIXER_SOUNDFXS, Mathf.Log10(soundfxVolume) * 20);
        MainMixer.SetFloat(SoundMixerManager.MIXER_MASTERS, Mathf.Log10(masterVolume) * 20);
    }
}