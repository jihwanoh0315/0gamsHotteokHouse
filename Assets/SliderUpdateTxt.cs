using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderUpdateTxt : MonoBehaviour
{
    public TMP_Text sliderValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sliderValue.text = gameObject.GetComponent<Slider>().value.ToString();
    }
}
