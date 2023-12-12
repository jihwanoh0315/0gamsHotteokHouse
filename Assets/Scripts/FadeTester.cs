using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTester : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(FadeOutTester());
        }
    }

    IEnumerator FadeOutTester()
    {
        BGM.FadeOutMusic();

        yield return new WaitForSeconds(3.0f);

        BGM.FadeInMusic();

    }

}
