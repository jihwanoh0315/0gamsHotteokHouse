using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotteokOnRack : MonoBehaviour
{
    // Start is called before the first frame update
    public int m_price = 0;
    public int m_visualIndex = -1;
    public int m_dough = 0;
    public int m_textureIndex = 0; // texture when in a cup 0~2
    public int m_stats; // for ui

    SpriteRenderer spriteRenderer;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRackHotteok(Hotteok hotteok_)
    {
        spriteRenderer.sprite = hotteok_.gameObject.GetComponent<SpriteRenderer>().sprite;
        m_price = hotteok_.price;
        m_dough = hotteok_.GetDoughType();
        m_textureIndex = hotteok_.GetFrontIndex();
    }
}
