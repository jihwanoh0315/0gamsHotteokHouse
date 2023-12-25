using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    // Levelsetting, 특히 건물들을 확인해서 배치함.

    [SerializeField] LevelSetting m_levelSetting;

    // Structure
    [SerializeField] HasMultipleSprites[] m_trees;
    [SerializeField] HasMultipleSprites[] m_streetLamps;
    [SerializeField] HasMultipleSprites m_wall;

    [SerializeField] GameObject m_building;
    [SerializeField] GameObject m_pojangmacha;
    [SerializeField] HasMultipleSprites m_tent;
    [SerializeField] HasMultipleSprites m_buildingDeco;
    [SerializeField] HasMultipleSprites m_sign;

    [SerializeField] GameObject m_sikye;
    [SerializeField] GameObject m_slush;
    [SerializeField] GameObject m_fishCake;

    [SerializeField] GameObject[] m_pets;

    // Working space
    [SerializeField] GameObject m_greenTeaDough;
    [SerializeField] GameObject m_sweetPotatoDough;
    [SerializeField] HasMultipleSprites m_oil;

    // Start is called before the first frame update
    void Awake()
    {
        m_levelSetting = FindObjectOfType<LevelSetting>();

        m_greenTeaDough.SetActive(m_levelSetting.activeFoods[0]);
        m_sweetPotatoDough.SetActive(m_levelSetting.activeFoods[1]);

        int activeSide = 0;
        if (m_levelSetting.activeFoods[2])
        {
            m_sikye.SetActive(true);
            ++activeSide;
        }
        if (m_levelSetting.activeFoods[3])
        {
            m_slush.SetActive(true);
            m_slush.transform.DOLocalMoveX(3.5f + (float)activeSide, 0.0f);
            ++activeSide;
        }
        if (m_levelSetting.activeFoods[4])
        {
            m_fishCake.SetActive(true);
            m_fishCake.transform.DOLocalMoveX(3.5f + (float)activeSide, 0.0f);
        }

        if (m_levelSetting.m_gameData.currBuilding == 2)
        {
            m_pojangmacha.SetActive(false);
            m_building.SetActive(true);
        }

        int activePet = 0;
        bool[] pets = m_levelSetting.m_gameData.pets;
        for (int i = 0; i < pets.Length; ++i)
        {
            if (pets[i])
            {
                m_pets[i].SetActive(true);
                if (activePet == 0)
                {
                    m_pets[i].transform.DOLocalMoveX(7.8f, 0.0f);
                }
                else
                {
                    m_pets[i].transform.DOLocalMoveX(9.0f, 0.0f);
                }
                ++activePet;
            }
        }

        foreach (HasMultipleSprites tr in m_trees)
        {
            tr.SetSprite(m_levelSetting.m_gameData.activeDecos[0]);
        }
        foreach (HasMultipleSprites sL in m_streetLamps)
        {
            sL.SetSprite(m_levelSetting.m_gameData.activeDecos[1]);
        }
        m_wall.SetSprite(m_levelSetting.m_gameData.activeDecos[2]);
        m_tent.SetSprite(m_levelSetting.m_gameData.activeDecos[3]);
        m_buildingDeco.SetSprite(m_levelSetting.m_gameData.activeDecos[4]);
        m_sign.SetSprite(m_levelSetting.m_gameData.activeDecos[5]);

        // Working Space
        m_oil.SetSprite(m_levelSetting.activeOil);
    }
}
