using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixerGroup musicMixerGroup; // Drag Music group here

    [Header("Music Tracks")]
    public AudioClip mainTrack;
    public AudioClip rushTrack;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = musicMixerGroup;
        audioSource.loop = true; // Music usually loops
    }

    private void Start()
    {
        // Start playing the main track automatically
        PlayMainTrack();
    }

    /// <summary>
    /// Play the main background music
    /// </summary>
    public void PlayMainTrack()
    {
        PlayTrack(mainTrack);
    }

    /// <summary>
    /// Play the rush music track
    /// </summary>
    public void PlayRushTrack()
    {
        PlayTrack(rushTrack);
    }

    /// <summary>
    /// Stops current music
    /// </summary>
    public void StopMusic()
    {
        audioSource.Stop();
    }

    /// <summary>
    /// Generic track player
    /// </summary>
    private void PlayTrack(AudioClip track)
    {
        if (track == null)
        {
            Debug.LogWarning("MusicPlayer: Track not assigned!");
            return;
        }

        if (audioSource.clip == track && audioSource.isPlaying)
            return; // Already playing

        audioSource.clip = track;
        audioSource.Play();
    }
}
