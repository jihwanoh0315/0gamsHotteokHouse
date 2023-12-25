using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DoughManager;

public class HotteokCup : HotteokContainers
{
    [SerializeField] List<Sprite> sprites = new List<Sprite>();

    void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[9];
    }
    public void AddHotteok(HotteokOnRack ht_)
    {
        SetCupTexture(ht_.m_dough, ht_.m_textureIndex);
        m_price = ht_.m_price;
        m_dough = ht_.m_dough;
        ++m_count;
    }

    public void SetCupTexture(int dough_, int index_)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[dough_  + index_ * 3];
    }

    public void ResetCup()
    {
        m_price = 0;
        m_count = 0;
        m_dough = 0;
        GetComponent<SpriteRenderer>().sprite = sprites[9];
    }

    public override int GetPrice()
    {
        return m_price;
    }
    public override void RemoveHotteok()
    {
        ResetCup();
        m_count = 0;
    }

    public override void ResetContainer()
    {
        ResetCup();
    }
}
