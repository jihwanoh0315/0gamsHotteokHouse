using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMTester : MonoBehaviour
{
    [SerializeField]
    BGMManager BGM;

    public int playMusicTrack;

    // Start is called before the first frame update
    void Start()
    {
        BGM = FindObjectOfType<BGMManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            BGM.PlayBGM(playMusicTrack);
            this.gameObject.SetActive(false);
        }
    }
}
