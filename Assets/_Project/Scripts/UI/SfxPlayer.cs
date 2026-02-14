using UnityEngine;
using UnityEngine.Audio;

public class SfxPlayer : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixerGroup sfxMixerGroup; // Drag SFX group here

    [Header("Audio Clips")]
    public AudioClip buttonPress;
    public AudioClip notificationGood;
    public AudioClip notificationBad;
    public AudioClip timerTick;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxMixerGroup; // Route SFX through mixer
    }

    public void PlaySfx(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("SfxPlayer: No AudioClip assigned!");
            return;
        }

        audioSource.PlayOneShot(clip);
    }

    public void PlayButtonPress() => PlaySfx(buttonPress);
    public void PlayNotificationGood() => PlaySfx(notificationGood);
    public void PlayNotificationBad() => PlaySfx(notificationBad);
    public void PlayTimerTick() => PlaySfx(timerTick);
}
