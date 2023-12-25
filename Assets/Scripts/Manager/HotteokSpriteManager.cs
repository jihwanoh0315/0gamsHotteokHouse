using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotteokSpriteManager : MonoBehaviour
{
    [Header("Base Hotteok")]
    public List<Sprite> rawHotteok;
    public List<Sprite> wellHotteok;
    public List<Sprite> burnHotteok;
    public Animator pressAnim;
    public Animator pressAnim_Raw;
    public Animator pressAnim_Burn;

    [Header("GreenTea Hotteok")]
    public List<Sprite> gr_rawHotteok;
    public List<Sprite> gr_wellHotteok;
    public List<Sprite> gr_burnHotteok;
    public Animator gr_pressAnim;
    public Animator gr_pressAnim_Raw;
    public Animator gr_pressAnim_Burn;

    [Header("SweetPotato Hotteok")]
    public List<Sprite> sp_rawHotteok;
    public List<Sprite> sp_wellHotteok;
    public List<Sprite> sp_burnHotteok;
    public Animator sp_pressAnim;
    public Animator sp_pressAnim_Raw;
    public Animator sp_pressAnim_Burn;

    [Header("Oil Animation")]
    public Animator anim_oilSmall;
    public Animator anim_oilBig;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
