using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
    private AudioSource source;

    public float volume;
    public bool loop;

    public void SetSource(AudioSource source_)
    {
        source = source_;
        source.clip = clip;
        source.volume = volume;
        source.loop = loop;
    }

    public void PlaySound()
    {
        source.Play();
    }

    public void StopSound() 
    {
        source.Stop();
    }

    public void SetLoop(bool loop)
    {
        source.loop = loop;
    }
    public void SetVolume()
    {
        source.volume = volume;
    }
}

public class AudioManager : MonoBehaviour
{
    static public AudioManager instance;

    [SerializeField]
    public Sound[] sounds;
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
        for(int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObject = new GameObject("Sound File Name : " + i + " = " + sounds[i].name);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlaySound(string name_)
    {
        for( int i = 0;i < sounds.Length;i++)
        {
            if(name_ == sounds[i].name)
            {
                sounds[i].PlaySound();
                return;
            }
        }
    }
    public void StopSound(string name_)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (name_ == sounds[i].name)
            {
                sounds[i].StopSound();
                return;
            }
        }
    }

    public void SetLoop(string name_,  bool loop_)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (name_ == sounds[i].name)
            {
                sounds[i].SetLoop(loop_);
                return;
            }
        }
    }

    public void SetVolume(string name_, float volume_)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (name_ == sounds[i].name)
            {
                sounds[i].volume = volume_;
                sounds[i].SetVolume();
                return;
            }
        }
    }
}
