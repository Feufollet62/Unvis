using UnityEngine;
using Random = UnityEngine.Random;

public class DiskJockey : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    
    [SerializeField] private AudioClip[] clips;

    [SerializeField] private float averageTimeBetweenSongs;
    [SerializeField] private float timeVariation;

    private float timer;
    private float timeToNextSong;
    private int nextSongIndex = 0;

    void Start()
    {
        timeToNextSong = GetTimeToNextSong();
    }

    private void Update()
    {
        transform.forward = Vector3.forward;
        
        if (source.isPlaying) return;
        
        timer += Time.deltaTime;

        if (timer > timeToNextSong)
        {
            PlayNextSong();
        }
    }

    private void PlayNextSong()
    {
        source.clip = clips[nextSongIndex];
        source.Play();

        nextSongIndex = nextSongIndex == 0 ? 1 : 0;
        timer = 0;
        timeToNextSong = GetTimeToNextSong();
    }

    private float GetTimeToNextSong()
    {
        return averageTimeBetweenSongs + Random.Range(-timeVariation, timeVariation);
    }
}
