using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HasMultipleImages : MonoBehaviour
{

    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    int currSprite = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSprite(int index_)
    {
        if (sprites.Count > index_)
        {
            GetComponent<Image>().sprite = sprites[index_];
            SetCurrSprite(index_);
        }
    }

    public void SetCurrSprite(int index_)
    {
        if (sprites.Count > index_)
        {
            currSprite = index_;
        }
    }

    public void SetNextSprite()
    {
        if (sprites.Count > currSprite + 1)
        {
            currSprite++;
            GetComponent<Image>().sprite = sprites[currSprite];
        }
    }
}
