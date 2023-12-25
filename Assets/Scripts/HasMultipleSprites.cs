using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasMultipleSprites : MonoBehaviour
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
        if(sprites.Count > index_)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[index_];
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
            GetComponent<SpriteRenderer>().sprite = sprites[currSprite];
            currSprite++;
        }
    }
}
