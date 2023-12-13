using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotteokSpriteManager : MonoBehaviour
{
    [Header("Base Hotteok")]
    [SerializeField] List<Sprite> rawHotteok;
    [SerializeField] List<Sprite> wellHotteok;
    [SerializeField] List<Sprite> burnHotteok;
    [SerializeField] Animator pressAnim;

    [Header("GreenTea Hotteok")]
    [SerializeField] List<Sprite> gr_rawHotteok;
    [SerializeField] List<Sprite> gr_wellHotteok;
    [SerializeField] List<Sprite> gr_burnHotteok;
    [SerializeField] Animator gr_pressAnim;

    [Header("SweetPotato Hotteok")]
    [SerializeField] List<Sprite> sp_rawHotteok;
    [SerializeField] List<Sprite> sp_wellHotteok;
    [SerializeField] List<Sprite> sp_burnHotteok;
    [SerializeField] Animator sp_pressAnim;

    [Header("Oil Animation")]
    [SerializeField] Animator anim_oilSmall;
    [SerializeField] Animator anim_oilBig;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
