using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotteokTray : HotteokContainers
{
    [SerializeField] List<SpriteRenderer> m_hotteoks;
    [SerializeField] int m_currHotteokSpace = 0;
    [SerializeField] int[] m_trayPrice;

    void Awake()
    {
        m_currHotteokSpace = 0;
        ResetTray();
        m_trayPrice = new int[4];
    }
    public void AddHotteok(HotteokOnRack ht_)
    {
        if(m_currHotteokSpace == 0)
        {
            m_dough = ht_.m_dough;
        }
        m_hotteoks[m_currHotteokSpace].GetComponent<SpriteRenderer>().sprite = ht_.GetComponent<SpriteRenderer>().sprite;
        m_hotteoks[m_currHotteokSpace].gameObject.SetActive(true);
        m_trayPrice[m_currHotteokSpace] = ht_.m_price;

        ++m_currHotteokSpace;
        ++m_count;
    }

    public override void RemoveHotteok()
    {
        if(m_currHotteokSpace > 0)
        {
            --m_currHotteokSpace;
            m_hotteoks[m_currHotteokSpace].gameObject.SetActive(false);
            m_trayPrice[m_currHotteokSpace] = 0;
            --m_count;
        }
    }

    public void ResetTray()
    {
        foreach(SpriteRenderer ht in m_hotteoks)
        {
            ht.gameObject.SetActive(false);
        }
        m_currHotteokSpace = 0;
        m_price = 0;
        m_count = 0;
        m_dough = 0;
    }

    public override int GetPrice()
    {
        int entirePrice = 0;
        for(int i = 0; i < m_currHotteokSpace; ++i)
        {
            entirePrice += m_trayPrice[i];
        }
        return entirePrice;
    }

    public override void ResetContainer()
    {
        ResetTray();
    }
}
