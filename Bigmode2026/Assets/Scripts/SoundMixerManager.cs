using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider SoundFXVolumeSlider;
    public const string MIXER_MUSICS = "MusicVolume";
    public const string MIXER_SOUNDFXS = "SoundFXVolume";
    public const string MIXER_MASTERS = "MasterVolume";
    private void Awake()
    {
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        SoundFXVolumeSlider.onValueChanged.AddListener(SetSoundFXVolume);
    }
    private void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicManager.MIXER_MUSIC, 1f);
        masterVolumeSlider.value = PlayerPrefs.GetFloat(MusicManager.MIXER_MASTER, 1f);
        SoundFXVolumeSlider.value = PlayerPrefs.GetFloat(MusicManager.MIXER_SOUNDFX, 1f);
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(MusicManager.MIXER_MUSIC, musicVolumeSlider.value);
        PlayerPrefs.SetFloat(MusicManager.MIXER_MASTER, masterVolumeSlider.value);
        PlayerPrefs.SetFloat(MusicManager.MIXER_SOUNDFX, SoundFXVolumeSlider.value);
    }
    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat(MIXER_MUSICS, Mathf.Log10(level)*20f);
    }

    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat(MIXER_SOUNDFXS, Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat(MIXER_MASTERS, Mathf.Log10(level) * 20f);
    }
}
