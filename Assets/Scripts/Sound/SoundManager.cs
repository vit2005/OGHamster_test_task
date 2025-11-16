using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager Instance => _instance;

    [SerializeField] private SoundConfig config;

    [Header("Audio Source Settings")]
    [SerializeField] private float pitchMin = 0.95f;
    [SerializeField] private float pitchMax = 1.05f;

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        _instance = this;
    }

    public void Play(SoundType type)
    {
        var clip = config.GetClip(type);
        if (clip == null)
        {
            Debug.LogWarning($"[SoundManager] No clip for sound type {type}");
            return;
        }

        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.PlayOneShot(clip);
    }
}
