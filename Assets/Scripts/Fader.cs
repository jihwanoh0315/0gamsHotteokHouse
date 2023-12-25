using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // for Image


public class Fader : MonoBehaviour
{
    public Image image;
    public float delay = 1.0f;
    // Start is called before the first frame update
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    void Start()
    {
        image.DOFade(0, 1.5f).SetLoops(-1, LoopType.Yoyo).SetDelay(delay);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
