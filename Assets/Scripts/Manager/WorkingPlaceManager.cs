using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkingPlaceManager : MonoBehaviour
{
    [Tooltip("손에 들 수 있는 도구")]
    public GameObject m_hotteokTray;
    public GameObject m_hotteokCup; //
    public GameObject m_pressTool; // 누르개
    public GameObject m_tongs; // 집게

    /*****************
     *  Tool on the hand
     * 
     *  0 Tray
     *  1 Cup
     *  2 Tool
     *  3 Tongs
     * *****************/
    public int m_handlingTool; // Tool on the hand




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
