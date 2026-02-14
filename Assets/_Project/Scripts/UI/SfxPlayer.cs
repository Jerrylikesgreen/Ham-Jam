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
    public AudioClip towerAttack;
    public AudioClip minionAttack;
    public AudioClip hamMinion;
    public AudioClip frogMinion;
    public AudioClip baconMinion;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxMixerGroup; // Route SFX through mixer
        audioSource.volume = 0.5f;
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
    public void PlayTowerAtk() => PlaySfx(towerAttack);
    public void PlayMinionAtk() => PlaySfx(minionAttack);
    public void PlayHamMinion() => PlaySfx(hamMinion);
    public void PlayBaconMinion() => PlaySfx(baconMinion);
    public void PlayFrogMinion() => PlaySfx(frogMinion);
}
