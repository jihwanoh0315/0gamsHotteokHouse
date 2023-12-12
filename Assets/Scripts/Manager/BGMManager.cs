using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    static public BGMManager instance;

    public AudioClip[] clips;

    private AudioSource audioSource;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBGM(int musicTrack_)
    {
        audioSource.clip = clips[musicTrack_];
        audioSource.Play();
    }

    public void PauseBGM()
    {
        audioSource.Pause();
    }

    public void UnpauseBGM()
    {
        audioSource.UnPause();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    public void FadeOutMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutMusicCoroutine());
    }

    IEnumerator FadeOutMusicCoroutine()
    {
        for(float i = 1.0f; i >= 0.0f; i -= 0.01f)
        {
            audioSource.volume = i;
            yield return waitTime ;
        }
    }
    public void FadeInMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInMusicCoroutine());
    }
    IEnumerator FadeInMusicCoroutine()
    {
        for (float i = 0.0f; i <= 1.0f; i += 0.01f)
        {
            audioSource.volume = i;
            yield return waitTime;
        }
    }
}
