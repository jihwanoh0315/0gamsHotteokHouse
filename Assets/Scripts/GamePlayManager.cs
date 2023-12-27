using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayManager : MonoBehaviour
{
    public enum HandMode
    {
        HAND = 0,
        OIL,
        PRESSER,
        TONGS
    }

    enum HoldingObj
    {
        NOTHING = 0,
        DOUGH,
        CUP,
        TRAY
    }

    enum Dough
    {
        BASE = 0,
        GREENTEA,
        SWEETPOTATO
    }

    HotteokSpriteManager theHSM;
    LevelSetting levelSetting;
    [SerializeField] Hotteok hotteok;
    [SerializeField] Hotteok greenteaHotteok;
    [SerializeField] Hotteok sweetPotatoHotteok;
    [SerializeField] GameObject oilOnPlate;

    [SerializeField] List<Sprite> handTextures;
    [SerializeField] List<Sprite> handUITextures;

    [SerializeField] bool isFever;

    [SerializeField] Transform hand; // main char in here
    [SerializeField] SpriteRenderer handSprite;
    [SerializeField] Image handUI;
    [SerializeField] SpriteRenderer handUnderSprite; // main char in here

    // On Hand Section
    // dough
    [Header("Hand Secton")]
    [SerializeField] GameObject dough_Hand;
    Vector3 dHandOrigPos = Vector3.zero;
    Vector3 dHandOrigScale = Vector3.zero;
    [SerializeField] GameObject dough_DoughOnHand;
    [SerializeField] List<Sprite> baseDough;
    [SerializeField] List<Sprite> greenTeaDough;
    [SerializeField] List<Sprite> sweetPotatoDough;
    Dictionary<Dough, List<Sprite>> doughMap = new();
    [SerializeField] int currStep = 0;

    // dough folding game
    [SerializeField] GameObject doughFoldUI;
    [SerializeField] GameObject feverDoughFoldUI;
    [SerializeField] List<GameObject> doughArrows;
    [SerializeField] List<Sprite> doughArrowsSprite;
    List<KeyCode> doughKeys = new List<KeyCode>(4);
    List<KeyCode> myKeys = new List<KeyCode>(4);
    int currKeyStep = 0;
    bool isGaming = false;
    bool isPlacing = false;
    bool canPlace = false;

    // dough placing
    [SerializeField] GameObject doughToPlace;
    [SerializeField] HotteokBoardStatus m_hotteokPlate;
    int hotteokLayerMask;
    int workingSpaceLayerMask;
    int interactiveSpaceLayerMask;
    int hotteokBoardLayerMask;
    int ingameUILayerMask;
    int rackLayerMask;
    int wallLayerMask;
    int containersMask;



    [SerializeField] HotteokCup cupOnHand;
    [SerializeField] HotteokTray trayOnHand;
    [SerializeField] GameObject BaseRackUI;
    [SerializeField] GameObject GRTRackUI;
    [SerializeField] GameObject SWPRackUI;
    ForHand otForHand; // For Hand Component in other obj
    ForHand prevForHand; // otForHand in previous frame for anime
    int addedHotteok = 0;

    GameObject prevHandObj = null;

    // Oil Section
    [Header("Oil Section")]
    [SerializeField] GameObject oilBottle;
    [SerializeField] GameObject oilTrack;
    [SerializeField] GameObject oilEnd;
    [SerializeField] Sprite oilDir_Red;
    [SerializeField] Sprite oilDir_Base;

    public int oilCount = 0;

    // Presser Section
    [Header("Presser Section")]
    Hotteok currHotteok;

    // Tongs Section
    [Header("Tongs Section")]
    [SerializeField] BoxCollider2D m_hotteokRackCollider;
    [SerializeField] GameObject pref_hotteokOnRack;
    Dictionary<Dough, List<HotteokOnRack>> hotteokRack = new();
    [SerializeField] List<HotteokOnRack> visualRack = new();
    List<HotteokOnRack> entireRackList = new();

    [SerializeField] List<GameObject> RackUI = new();
    [SerializeField] List<TMP_Text> RackUIText = new();

    // IngameUI
    [SerializeField] GameObject giveToPetUI;
    [SerializeField] GameObject putToTrashCanUI;
    [SerializeField] GameObject coolingRackUI;
    [SerializeField] GameObject sellingUI;
    GameObject prevObj = null;

    int t_currPickedIndex = 0;
    public int t_MaxVisualRackCount = 0;
    int t_currVisualRackCount = 0;
    int t_currSpawnCount = 0;
    Dough t_prevPickedDough = Dough.BASE;
    Dough t_currPickedDough = Dough.BASE;
    HotteokOnRack t_currPickedHotteok = null;

    ForHand currClicked = null;

    bool m_movingContainer = false;
    float m_movingTimer = 0.0f;

    /// <summary>
    /// In Game Section
    /// </summary>
    // Customer Section
    [Header("Customer Section")]
    int maxTakeOutCount = 6;
    CustomerManager m_customerManager = null;


    [Header("Fee UI Section")]
    // 정산표
    [SerializeField] GameObject w_baseHotteok;
    [SerializeField] TMP_Text wb_rawCount;
    [SerializeField] TMP_Text wb_wellCount;
    [SerializeField] TMP_Text wb_greatCount;
    [SerializeField] TMP_Text wb_doughBonus;
    [SerializeField] TMP_Text wb_petBonus;
    [SerializeField] TMP_Text wb_profit;



    [SerializeField] GameObject w_greenteaHotteok;
    [SerializeField] TMP_Text wg_rawCount;
    [SerializeField] TMP_Text wg_wellCount;
    [SerializeField] TMP_Text wg_greatCount;
    [SerializeField] TMP_Text wg_doughBonus;
    [SerializeField] TMP_Text wg_petBonus;
    [SerializeField] TMP_Text wg_profit;

    [SerializeField] GameObject w_sweetPotatoHotteok;
    [SerializeField] TMP_Text ws_rawCount;
    [SerializeField] TMP_Text ws_wellCount;
    [SerializeField] TMP_Text ws_greatCount;
    [SerializeField] TMP_Text ws_doughBonus;
    [SerializeField] TMP_Text ws_petBonus;
    [SerializeField] TMP_Text ws_profit;

    [SerializeField] GameObject w_entireProfit;
    [SerializeField] TMP_Text we_baseHotteokProfit;
    [SerializeField] TMP_Text we_secondHTName;
    [SerializeField] TMP_Text we_secondHTProfit;
    [SerializeField] TMP_Text we_sideProfit;
    [SerializeField] TMP_Text we_etcProfit;

    [SerializeField] TMP_Text we_ingredientFee;
    [SerializeField] TMP_Text we_itemFee;
    [SerializeField] TMP_Text we_placeFee;
    [SerializeField] TMP_Text we_buildingFee;

    [SerializeField] GameObject w_okayButton;



    HandMode handMode = 0; // 0 hand, 1 oil, 2 presser, 3 tongs
    // for mode hand
    HoldingObj holdingObject; // 0 nothing, 1 dough, 2 cup, 3 tray
    Dough currDough; // 0 base, 1 grt, 2 swp
    // for mode oil
    bool isClicked = false;
    Vector2 firstPos = Vector2.zero;
    Vector2 secPos = Vector2.zero;


    Vector3 mousePos = Vector3.zero;


    int toTween = 0;
    Dough activeSecondDough = Dough.BASE;
    GameObject activeSecondRackUI;
    bool onlyOneDough = false;


    void ShowHandChangeWithUI()
    {
        handSprite.sprite = handTextures[(int)handMode];
        handUI.sprite = handUITextures[(int)handMode];
        handUI.DOFade(0.7f, 0.0f);
        handUI.DOFade(0.0f, 1.0f);
    }

    // Start is called before the first frame update
    void Awake()
    {
        // Get informations from the level setting
        levelSetting = FindAnyObjectByType<LevelSetting>();

        Cursor.lockState = CursorLockMode.Confined;

        m_hotteokPlate = FindAnyObjectByType<HotteokBoardStatus>();

        handSprite = hand.GetComponent<SpriteRenderer>();
        ShowHandChangeWithUI();
        otForHand = null;

        theHSM = FindObjectOfType<HotteokSpriteManager>();

        hotteokLayerMask = (1 << LayerMask.NameToLayer("Hotteok"));
        workingSpaceLayerMask = (1 << LayerMask.NameToLayer("WorkingSpace"));
        interactiveSpaceLayerMask = (1 << LayerMask.NameToLayer("InteractiveWorkingSpace"));
        ingameUILayerMask = (1 << LayerMask.NameToLayer("IngameUI"));
        rackLayerMask = (1 << LayerMask.NameToLayer("Rack"));
        wallLayerMask = (1 << LayerMask.NameToLayer("Wall"));
        containersMask = (1 << LayerMask.NameToLayer("Containers"));

        // hand section
        doughKeys.Add(KeyCode.W);
        doughKeys.Add(KeyCode.A);
        doughKeys.Add(KeyCode.S);
        doughKeys.Add(KeyCode.D);
        myKeys.Add(KeyCode.W);
        myKeys.Add(KeyCode.A);
        myKeys.Add(KeyCode.S);
        myKeys.Add(KeyCode.D);

        doughMap.Add(Dough.BASE, baseDough);
        doughMap.Add(Dough.GREENTEA, greenTeaDough);
        doughMap.Add(Dough.SWEETPOTATO, sweetPotatoDough);

        dHandOrigPos = dough_Hand.transform.localPosition;
        dHandOrigScale = dough_Hand.transform.localScale;


        // oil section
        firstPos = oilBottle.transform.localPosition;

        // Rack section
        hotteokRack.Add(Dough.BASE, new());
        hotteokRack.Add(Dough.GREENTEA, new());
        hotteokRack.Add(Dough.SWEETPOTATO, new());

        //DOTween.To(() => toTween, value => toTween = value, 100, 10.0f);
        if (levelSetting.activeFoods[0])
        {
            RackUI[1].SetActive(true);
            activeSecondDough = Dough.GREENTEA;
            activeSecondRackUI = GRTRackUI;
        }
        else if (levelSetting.activeFoods[1])
        {
            RackUI[2].SetActive(true);
            activeSecondDough = Dough.SWEETPOTATO;
            activeSecondRackUI = SWPRackUI;
        }
        else
        {
            onlyOneDough = true;
        }

        // Set Arrow dir
        for (int i = 0; i < 4; ++i)
        {
            doughArrows[i].GetComponent<Image>().DOFade(0.0f, 0.0f);
        }

        m_customerManager = FindObjectOfType<CustomerManager>();
        m_currMoney = 0;

        Game_Initialize();
        GenerateSpawnPositions();
        FeverInit();
    }

    // Update is called once per frame
    void Update()
    {
        ////////////
        /// TOOL SECTION
        //Debug.Log(toTween);
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (!m_isFeverTime)
        {
            if (!g_doDailyCalculation)
            {
                Sequence mySequence = DOTween.Sequence();

                // Change Hand Mode
                if (Input.GetKeyDown(KeyCode.Alpha1)) // hand
                {
                    ResetCurrSequence();
                    handMode = HandMode.HAND;
                    ShowHandChangeWithUI();
                }
                if (Input.GetKeyDown(KeyCode.Alpha2)) // oil
                {
                    ResetCurrSequence();
                    handMode = HandMode.OIL;
                    ShowHandChangeWithUI();
                }
                if (Input.GetKeyDown(KeyCode.Alpha3)) // presser
                {
                    ResetCurrSequence();
                    handMode = HandMode.PRESSER;
                    ShowHandChangeWithUI();
                }
                if (Input.GetKeyDown(KeyCode.Alpha4)) // tongs
                {
                    ResetCurrSequence();
                    handMode = HandMode.TONGS;
                    ShowHandChangeWithUI();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ResetCurrSequence();
                    ShowHandChangeWithUI();
                }


                // Play Hand Role
                switch (handMode)
                {
                    case HandMode.HAND:
                        Mode_HandUpdate();
                        break;
                    case HandMode.OIL:
                        Mode_OilUpdate();
                        break;
                    case HandMode.PRESSER:
                        Mode_PresserUpdate();
                        break;
                    case HandMode.TONGS:
                        Mode_TongsUpdate();
                        break;
                    default:
                        break;
                }

                hand.gameObject.SetActive(levelSetting.handActive);
            }
            Game_Update();
        }

        FeverUpdate();
    }


    void Mode_HandUpdate()
    {
        otForHand = null;
        if (isGaming)
            PlayFoldingGame();

        if (!isPlacing)
        {
            if ((hand.localPosition.x < -5.0f || hand.localPosition.x > 5.0f) && hand.localPosition.y < 3.5f)
            {
                RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 15.0f, interactiveSpaceLayerMask);

                if (hit.collider != null &&
                    hit.collider.CompareTag("ForHand") &&
                    ((int)handMode == hit.collider.GetComponent<ForHand>().reactNum
                        && (hit.collider.GetComponent<ForHand>().currStep == 0 || hit.collider.GetComponent<ForHand>().currStep == currStep)))
                {
                    otForHand = hit.collider.GetComponent<ForHand>();
                    if (prevForHand != otForHand)
                    {
                        hand.GetComponent<Animator>().runtimeAnimatorController = otForHand.handAnimator.runtimeAnimatorController;
                        //otForHand.MouseOn(true);
                        if (prevForHand != null)
                        {
                            prevForHand.GetComponent<ForHand>().MouseOn(false);
                        }
                        otForHand.GetComponent<ForHand>().MouseOn(true);
                        prevForHand = otForHand;
                    }
                }
                else
                {
                    // 감지된 오브젝트가 없거나 태그가 일치하지 않으면 기본 커서 이미지로 변경
                    hand.GetComponent<Animator>().runtimeAnimatorController = null;
                    if (prevForHand)
                    {
                        prevForHand.GetComponent<ForHand>().MouseOn(false);
                        prevForHand = null;
                    }
                }
            }
        }

        switch (holdingObject)
        {
            case HoldingObj.NOTHING:
                if (hand.localPosition.x < 0)
                    hand.localScale = new Vector3(-1.0f, 1.0f, 0.0f);
                else
                    hand.localScale = new Vector3(1.0f, 1.0f, 0.0f);

                break;
            case HoldingObj.DOUGH:
                switch (currStep)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3: // Place Dough
                        if (isGaming)
                            break;
                        CheckingIngameUI();
                        canPlace = false;
                        if ((hand.localPosition.x > -5.0f && hand.localPosition.x < 5.0f) && hand.localPosition.y < 1.0f)
                        {
                            // Check collision to place
                            RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 15.0f, hotteokLayerMask | wallLayerMask);

                            if (hit.collider != null)
                            {
                                //Debug.Log(hit.collider.name);
                                // don't need to check again
                                canPlace = false;
                            }
                            else
                            {
                                CanPlaceHotteok(mousePos);
                            }

                        }

                        if (hand.localPosition.x < -6.0f && hand.localPosition.y < 1.0f)
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                isPlacing = false;
                                ResetHand();
                                return;
                            }
                        }

                        if (canPlace)
                        {
                            handSprite.color = new Color(0.8f, 1.0f, 0.8f, 0.7f);
                            Hotteok newHotteok;
                            if (Input.GetMouseButtonDown(0))
                            {
                                switch (currDough)
                                {
                                    case Dough.BASE:
                                        newHotteok = Instantiate(hotteok, new Vector3(mousePos.x, mousePos.y, -5.0f), Quaternion.identity);
                                        break;
                                    case Dough.GREENTEA:
                                        newHotteok = Instantiate(greenteaHotteok, new Vector3(mousePos.x, mousePos.y, -5.0f), Quaternion.identity);
                                        break;
                                    case Dough.SWEETPOTATO:
                                        newHotteok = Instantiate(sweetPotatoHotteok, new Vector3(mousePos.x, mousePos.y, -5.0f), Quaternion.identity);
                                        break;
                                    default:
                                        newHotteok = Instantiate(hotteok, new Vector3(mousePos.x, mousePos.y, -5.0f), Quaternion.identity);
                                        break;
                                }
                                m_hotteokPlate.PutHotteok();
                                m_hotteokPlate.hotteoks.Add(newHotteok);


                                isPlacing = false;
                                ResetHand();
                            }
                        }
                        else
                        {
                            handSprite.color = new Color(1.0f, 0.8f, 0.8f, 0.6f);
                        }

                        break;

                    default:
                        break;
                }
                break;
            case HoldingObj.CUP:
                switch (currStep)
                {
                    case 0: // initCup
                        break;
                    case 1: // Time to pick but also can give
                       

                        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, transform.forward, 15.0f, rackLayerMask);

                        bool activeCoolingRackUI = false;
                        bool needHand = false;
                        bool showRackHTUI = true;
                        foreach (RaycastHit2D currHit in hits)
                        {
                            if (currHit.collider != null)
                            {
                                if (currHit.collider.gameObject.name == "Hotteok_CoolingRack")
                                {
                                    activeCoolingRackUI = true;
                                    if(t_currPickedIndex < 1)
                                    {
                                        if (mousePos.x > 0.0f)
                                        {
                                            if (hotteokRack[activeSecondDough].Count > t_currPickedIndex)
                                            {
                                                handUnderSprite.sprite = hotteokRack[activeSecondDough][t_currPickedIndex].GetComponent<SpriteRenderer>().sprite;
                                                needHand = true;
                                                showRackHTUI = false;
                                            }
                                        }
                                        else
                                        {
                                            if (hotteokRack[Dough.BASE].Count > t_currPickedIndex)
                                            {
                                                handUnderSprite.sprite = hotteokRack[Dough.BASE][t_currPickedIndex].GetComponent<SpriteRenderer>().sprite;
                                                needHand = true;
                                                showRackHTUI = false;
                                            }
                                        }
                                    }

                                    if (Input.GetMouseButtonDown(0))
                                    {
                                        if(t_currPickedIndex < 1)
                                        {
                                            if (mousePos.x > 0.0f)
                                            { // if there is second dough
                                                SetPickingHotteok(activeSecondDough);
                                            }
                                            else // just add base
                                            {
                                                SetPickingHotteok(Dough.BASE);
                                            }
                                            if (hotteokRack[t_currPickedDough].Count > 0)
                                            {
                                                if (t_currPickedDough == t_prevPickedDough)
                                                {
                                                    if (PickHotteok())
                                                    {
                                                        cupOnHand.AddHotteok(t_currPickedHotteok);
                                                        t_prevPickedDough = t_currPickedDough;

                                                    }

                                                }
                                                else
                                                {
                                                    ReturnToVisualRack();
                                                    cupOnHand.ResetCup();
                                                    UpdateRackUI(t_prevPickedDough);
                                                    if (PickHotteok())
                                                    {
                                                        cupOnHand.AddHotteok(t_currPickedHotteok);
                                                        t_prevPickedDough = t_currPickedDough;
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                ShakeErrorObject(hand.gameObject);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    handUnderSprite.gameObject.SetActive(false);
                                }
                            }
                        }
                        coolingRackUI.SetActive(activeCoolingRackUI);
                        if (!onlyOneDough)
                        {
                            BaseRackUI.SetActive(showRackHTUI);
                            activeSecondRackUI.SetActive(showRackHTUI);
                        }

                        handUnderSprite.gameObject.SetActive(needHand);


                        bool collidingWithCup = false;
                        if (cupOnHand.CanMove())
                        {
                            RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 15.0f, containersMask);
                            {
                                if (hit.collider != null)
                                {
                                    //Debug.Log(hit.collider.gameObject);
                                    if (hit.collider.name == cupOnHand.name)
                                    {
                                        collidingWithCup = true;
                                        if (Input.GetMouseButtonDown(0))
                                        {
                                            m_movingContainer = true;
                                            sellingUI.SetActive(true);
                                            m_movingTimer = 0.0f;
                                        }
                                    }
                                }
                            }
                        }

                        if (m_movingContainer)
                        {
                            collidingWithCup = true;
                            m_movingTimer += Time.deltaTime;
                            if (m_movingTimer > 0.12f)
                            {
                                cupOnHand.gameObject.transform.position = new Vector3(mousePos.x, mousePos.y, gameObject.transform.position.z);
                                if (Input.GetMouseButtonUp(0))
                                {
                                    //Debug.Log(m_movingTimer + "s");
                                    RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 15.0f, ingameUILayerMask);

                                    if (hit.collider != null && hit.collider.name == sellingUI.name)
                                    {
                                        ServePickedHotteoks(cupOnHand);
                                        ResetHand();
                                        // Return to origin pos
                                    }
                                    cupOnHand.transform.position = new Vector3(0, -5.0f, 0);
                                    m_movingContainer = false;
                                    sellingUI.SetActive(false);
                                    m_movingTimer = 0.0f;
                                }
                            }
                            else
                            {
                                if (Input.GetMouseButtonUp(0))
                                {
                                    if (t_currPickedIndex > 0)
                                    {
                                        UnpickHotteok();
                                        cupOnHand.RemoveHotteok();
                                    }
                                    m_movingContainer = false;
                                    sellingUI.SetActive(false);
                                    m_movingTimer = 0.0f;
                                }
                            }
                        }

                        if (collidingWithCup)
                        {
                            handSprite.sprite = handTextures[0];
                        }
                        else
                        {
                            handSprite.sprite = handTextures[3];
                        }
                        CheckingIngameUI();
                        break;
                    case 2: // sent
                    default:
                        break;
                }
                break;
            case HoldingObj.TRAY:
                switch (currStep)
                {
                    case 0: // init Tray
                        break;
                    case 1: // Time to pick but also can give
                        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, transform.forward, 15.0f, rackLayerMask);

                        bool activeCoolingRackUI = false;
                        bool needHand = false;
                        bool showRackHTUI = true;
                        foreach (RaycastHit2D currHit in hits)
                        {
                            if (currHit.collider != null)
                            {
                                if (currHit.collider.gameObject.name == "Hotteok_CoolingRack")
                                {
                                    activeCoolingRackUI = true;

                                    if (t_currPickedIndex < 4)
                                    {
                                        if (mousePos.x > 0.0f)
                                        {
                                            if (hotteokRack[activeSecondDough].Count > t_currPickedIndex)
                                            {
                                                handUnderSprite.sprite = hotteokRack[activeSecondDough][t_currPickedIndex].GetComponent<SpriteRenderer>().sprite;
                                                needHand = true;
                                                showRackHTUI = false;
                                            }
                                        }
                                        else
                                        {
                                            if (hotteokRack[Dough.BASE].Count > t_currPickedIndex)
                                            {
                                                handUnderSprite.sprite = hotteokRack[Dough.BASE][t_currPickedIndex].GetComponent<SpriteRenderer>().sprite;
                                                needHand = true;
                                                showRackHTUI = false;
                                            }
                                        }
                                    }

                                    if (Input.GetMouseButtonDown(0))
                                    {
                                        bool needShake = true;

                                        if(t_currPickedIndex < 4)
                                        {
                                            if (mousePos.x > 0.0f)
                                            { // if there is second dough
                                                SetPickingHotteok(activeSecondDough);
                                            }
                                            else // just add base
                                            {
                                                SetPickingHotteok(Dough.BASE);
                                            }
                                            //Debug.Log("Curr : " + t_currPickedDough + ", Prev : " + t_prevPickedDough);


                                            if (t_currPickedDough == t_prevPickedDough)
                                            {
                                                if (hotteokRack[t_currPickedDough].Count > t_currPickedIndex)
                                                {
                                                    if (PickHotteok())
                                                    {
                                                        trayOnHand.AddHotteok(t_currPickedHotteok);
                                                        needShake = false;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ReturnToVisualRack();
                                                trayOnHand.ResetTray();
                                                UpdateRackUI(t_prevPickedDough);
                                                if (PickHotteok())
                                                {
                                                    trayOnHand.AddHotteok(t_currPickedHotteok);
                                                    t_prevPickedDough = t_currPickedDough;
                                                    needShake = false;
                                                }
                                            }
                                        }
                                        
                                        if(needShake)
                                        {
                                            ShakeErrorObject(hand.gameObject);
                                        }
                                    }
                                }
                            }
                        }
                        coolingRackUI.SetActive(activeCoolingRackUI);
                        handUnderSprite.gameObject.SetActive(needHand);
                        if (!onlyOneDough)
                        {
                            BaseRackUI.SetActive(showRackHTUI);
                            activeSecondRackUI.SetActive(showRackHTUI);
                        }

                        bool collidingWithTray = false;
                        if (trayOnHand.CanMove())
                        {
                            RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 15.0f, containersMask);
                            {
                                if (hit.collider != null)
                                {
                                    //Debug.Log(hit.collider.gameObject);
                                    if (hit.collider.name == trayOnHand.name)
                                    {
                                        collidingWithTray = true;
                                        if (Input.GetMouseButtonDown(0))
                                        {
                                            m_movingContainer = true;
                                            sellingUI.SetActive(true);
                                            m_movingTimer = 0.0f;
                                        }
                                    }
                                }
                            }
                        }

                        if (m_movingContainer)
                        {
                            collidingWithTray = true;
                            m_movingTimer += Time.deltaTime;
                            if(m_movingTimer > 0.12f)
                            {
                                trayOnHand.gameObject.transform.position = new Vector3 (mousePos.x, mousePos.y, gameObject.transform.position.z);
                                if (Input.GetMouseButtonUp(0))
                                {
                                    //Debug.Log(m_movingTimer + "s");
                                    RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 15.0f, ingameUILayerMask);

                                    if (hit.collider != null && hit.collider.name == sellingUI.name)
                                    {
                                        ServePickedHotteoks(trayOnHand);
                                        ResetHand();
                                    }
                                    trayOnHand.transform.position = new Vector3(0, -4.5f, 0);
                                    m_movingContainer = false;
                                    sellingUI.SetActive(false);
                                    m_movingTimer = 0.0f;
                                }
                            }
                            else
                            {
                                if (Input.GetMouseButtonUp(0))
                                {
                                    if (t_currPickedIndex > 0)
                                    {
                                        UnpickHotteok();
                                        trayOnHand.RemoveHotteok();
                                    }
                                    m_movingContainer = false;
                                    sellingUI.SetActive(false);
                                    m_movingTimer = 0.0f;
                                }
                            }
                        }

                        if (collidingWithTray)
                        {
                            handSprite.sprite = handTextures[0];
                        }
                        else
                        {
                            handSprite.sprite = handTextures[3];
                        }

                        CheckingIngameUI();
                        break;
                    case 2: // sent
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        // Change
        if (Input.GetMouseButtonDown(0))
        {
            if (otForHand)
            {
                if (otForHand.currStep == currStep || otForHand.currStep == 0)
                {
                    SetHoldingObject();
                    otForHand.MouseOn(false);

                    if (currClicked == otForHand)
                    {
                        ResetHand();
                        currClicked = null;
                    }
                    else
                    {
                        currClicked = otForHand;
                    }

                }
            }
        }

    }
    void SetHoldingObject()
    {
        currStep = otForHand.currStep;
        if (currStep == 0)
        {
            ResetHand();
            holdingObject = (HoldingObj)otForHand.holdingObject;
            currDough = (Dough)otForHand.dough;
        }

        switch (holdingObject)
        {
            case HoldingObj.NOTHING:
                break;
            case HoldingObj.DOUGH:
                switch (currStep)
                {
                    case 0:
                        dough_Hand.SetActive(true);
                        dough_DoughOnHand.GetComponent<SpriteRenderer>().sprite = doughMap[currDough][currStep];
                        ++currStep;
                        break;
                    case 1: // dough wrapping
                        dough_DoughOnHand.GetComponent<SpriteRenderer>().sprite = doughMap[currDough][currStep];
                        switch (currDough)
                        {
                            case Dough.BASE:
                                StartFoldingGame();
                                break;  
                            case Dough.GREENTEA:
                            case Dough.SWEETPOTATO:
                                ++currStep;
                                break;
                            default:
                                break;
                        }
                        break;
                    case 2:
                        dough_DoughOnHand.GetComponent<SpriteRenderer>().sprite = doughMap[currDough][currStep];
                        //++currStep;
                        StartFoldingGame();
                        break;
                    case 3:
                        break;
                    default:
                        break;
                }

                break;
            case HoldingObj.CUP:
                //Debug.Log("Cup Clicked " + currStep);
                switch (currStep)
                {
                    case 0:
                        cupOnHand.gameObject.SetActive(true);
                        ++currStep;
                        handSprite.sprite = handTextures[3];
                        break;
                    default:
                        break;
                }

                break;
            case HoldingObj.TRAY:
                switch (currStep)
                {
                    case 0:
                        trayOnHand.gameObject.SetActive(true);
                        ++currStep;
                        handSprite.sprite = handTextures[3];
                        break;
                    default:
                        break;
                }

                break;
            default:
                break;
        }
    }

    public void StartFoldingGame()
    {

        // Show Folding Game UI
        doughFoldUI.SetActive(true);
        // Set Arrow dir
        SetFoldingDirection();
        isGaming = true;
    }

    void SetFoldingDirection()
    {
        //Debug.Log("Curr Step = " + currStep);
        currKeyStep = 0;
        dough_DoughOnHand.GetComponent<SpriteRenderer>().sprite = doughMap[currDough][currStep];
        doughFoldUI.transform.DOShakePosition(.1f, 5, 5);
        isResetting = true;
    }
    bool isResetting = false;
    float m_foldResetTimer = 0.1f;

    void ShakeErrorObject(GameObject gO_)
    {
        Sequence myColSequence = DOTween.Sequence();
        myColSequence.Append(gO_.GetComponent<SpriteRenderer>().DOColor(new Color(1.0f, 0.8f, 0.8f), 0.05f));
        myColSequence.Insert(0.0f, gO_.transform.DOShakePosition(.1f, 0.2f, 5));
        myColSequence.Insert(0.05f, gO_.GetComponent<SpriteRenderer>().DOColor(Color.white, 0.01f));
    }
    void ShakeObject(GameObject gO_, float strenght_ = 0.2f)
    {
        Sequence myColSequence = DOTween.Sequence();
        myColSequence.Insert(0.0f, gO_.transform.DOShakePosition(.1f, strenght_, 5));
    }
    public void PlayFoldingGame()
    {
        if (isResetting)
        {
            m_foldResetTimer += Time.deltaTime;
            if (m_foldResetTimer > 0.1f)
            {
                // Set Arrow dir
                for (int i = 0; i < 4; ++i)
                {
                    int ranVal = Random.Range(0, 3);
                    doughKeys[i] = myKeys[ranVal];
                    doughArrows[i].GetComponent<Image>().sprite = doughArrowsSprite[ranVal];
                    doughArrows[i].GetComponent<Image>().DOFade(1.0f, 0.0f);
                }
                isResetting = false;
            }
            return;
        }

        if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
        {
            if (Input.GetKey(doughKeys[currKeyStep]) && !isResetting)
            {
                doughArrows[currKeyStep].GetComponent<Image>().DOFade(0.0f, 0.1f);
                ++currKeyStep;
                dough_DoughOnHand.GetComponent<SpriteRenderer>().sprite = doughMap[currDough][currStep + currKeyStep];
            }
            else
            {
                m_foldResetTimer = 0.0f;
                currKeyStep = 0;
                ShakeErrorObject(dough_Hand);
                SetFoldingDirection();
            }

        }

        if (currKeyStep == 4)
        {
            currKeyStep = 0;
            currStep = 3;
            doughFoldUI.SetActive(false);
            // place dough
            isPlacing = true;
            isGaming = false;
            doughToPlace.SetActive(true);
            hand.GetComponent<Animator>().runtimeAnimatorController = null;
            dough_Hand.SetActive(false);

            ActiveHandsIngameUI(true);

            switch (currDough)
            {
                case Dough.BASE:
                    doughToPlace.GetComponent<SpriteRenderer>().sprite = theHSM.rawHotteok[0];
                    handSprite.sprite = theHSM.rawHotteok[0];
                    break;
                case Dough.GREENTEA:
                    doughToPlace.GetComponent<SpriteRenderer>().sprite = theHSM.gr_rawHotteok[0];
                    handSprite.sprite = theHSM.gr_rawHotteok[0];
                    break;
                case Dough.SWEETPOTATO:
                    doughToPlace.GetComponent<SpriteRenderer>().sprite = theHSM.sp_rawHotteok[0];
                    handSprite.sprite = theHSM.sp_rawHotteok[0];
                    break;
                default:
                    break;
            }
        }
    }
    void ResetHand()
    {

        currStep = 0;
        currClicked = null;
        handSprite.sprite = handTextures[(int)handMode];
        hand.GetComponent<Animator>().runtimeAnimatorController = null;
        holdingObject = HoldingObj.NOTHING;

        dHandOrigPos = dough_Hand.transform.localPosition;
        dHandOrigScale = dough_Hand.transform.localScale;
        // inactive hand
        dough_Hand.SetActive(false);
        dough_Hand.transform.position = new Vector3(0, -3.0f, -5.0f);
        // turn off Arrow Game UI
        doughFoldUI.SetActive(false);
        isGaming = false;
        isPlacing = false;
        handSprite.color = Color.white;

        // cup tray section
        cupOnHand.ResetCup();
        cupOnHand.gameObject.SetActive(false);
        cupOnHand.transform.position = new Vector3(0, -5.0f, 0);

        trayOnHand.ResetTray();
        trayOnHand.gameObject.SetActive(false);
        trayOnHand.transform.position = new Vector3(0, -4.5f, 0);

        m_movingContainer = false;
        sellingUI.SetActive(false);
        m_movingTimer = 0.0f;

        handUnderSprite.gameObject.SetActive(false);
        sellingUI.SetActive(false);

        ReturnToVisualRack();
        t_currPickedIndex = 0;

        BaseRackUI.SetActive(false);
        activeSecondRackUI.SetActive(false);

        ActiveHandsIngameUI(false);
        if (prevForHand)
        {
            prevForHand.GetComponent<ForHand>().MouseOn(false);
            prevForHand = null;
        }
    }
    void CanPlaceHotteok(Vector2 position)
    {
        // 물체의 Collider 크기를 고려하여 겹치는지 여부를 확인
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, hotteok.GetComponent<CircleCollider2D>().radius, hotteokLayerMask | wallLayerMask);
        // 자기 자신을 제외한 Collider2D들에 대한 처리
        canPlace = true;
        foreach (Collider2D col in colliders)
        {
            // 자기 자신과 겹치는 경우는 제외
            if (col.gameObject != hand)
            {
                canPlace = false;
                //Debug.Log("물체가 이미 존재합니다." + col.name);
            }
        }
    }
    void Mode_OilUpdate()
    {
        secPos = mousePos;

        // 대상의 위치와 현재 객체의 위치를 뺀 벡터를 얻어옵니다.
        Vector2 direction = secPos - firstPos;
        direction.Normalize(); // 방향 벡터를 정규화합니다.
        // 아크탄젠트를 이용하여 각도를 계산하고, 이를 degrees로 변환합니다.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (isClicked)
        {
            //hand.gameObject.SetActive(false);
            levelSetting.handActive = false;
            oilTrack.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if (oilEnd.transform.position.x > -5.5f && oilEnd.transform.position.x < 5.5f && oilEnd.transform.position.y > -6.0f && oilEnd.transform.position.y < 2.0f)
            {
                oilTrack.GetComponent<SpriteRenderer>().sprite = oilDir_Base;

                if (Input.GetMouseButtonUp(0) && oilCount < 10)
                {
                    levelSetting.handActive = true;
                    GameObject spO = Instantiate(oilOnPlate, firstPos, oilTrack.transform.rotation);
                    m_hotteokPlate.PutOil();
                    // Reset
                    ResetOil();
                    firstPos = secPos;
                    ++oilCount;
                }
            }
            else
            {
                oilTrack.GetComponent<SpriteRenderer>().sprite = oilDir_Red;
                if (Input.GetMouseButtonUp(0))
                {
                    levelSetting.handActive = true;
                    // Reset
                    ResetOil();
                    firstPos = secPos;
                }
            }
        }
        else
        {
            hand.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            if (mousePos.x > -5.5f && mousePos.x < 5.5f && mousePos.y > -6.0f && mousePos.y < 2.0f)
            {
                hand.GetComponent<SpriteRenderer>().sprite = oilDir_Base;
                if (Input.GetMouseButtonDown(0))
                {
                    isClicked = true;
                    firstPos = mousePos;
                    oilTrack.SetActive(true);
                    oilTrack.transform.localPosition = new Vector3(firstPos.x, firstPos.y, oilTrack.transform.localPosition.z);
                }
            }
            else
            {
                hand.GetComponent<SpriteRenderer>().sprite = oilDir_Red;
            }
        }
    }

    void ResetOil()
    {
        oilTrack.SetActive(false);
        isClicked = false;
        firstPos = oilBottle.transform.localPosition;
    }

    void Mode_PresserUpdate()
    {
        if ((hand.localPosition.x > -5.0f && hand.localPosition.x < 5.0f) && hand.localPosition.y < 1.0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (levelSetting.leftPresser > 0)
                {
                    // Check collision to place
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 15.0f, hotteokLayerMask);

                    if (hit.collider != null)
                    {
                        currHotteok = hit.collider.GetComponent<Hotteok>();
                        currHotteok.PressFace();
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            currHotteok?.ReleaseFace();
        }
    }

    private void ResetPresser()
    {
        currHotteok?.ReleaseFace();
        currHotteok = null;
    }

    void Mode_TongsUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if ((hand.localPosition.x > -5.0f && hand.localPosition.x < 5.0f) && hand.localPosition.y < 1.0f)
            {
                //Debug.Log("mouseDOWN");
                if (levelSetting.leftPresser > 0)
                {
                    // Check collision to place
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 15.0f, hotteokLayerMask);

                    if (hit.collider != null)
                    {
                        currHotteok = hit.collider.GetComponent<Hotteok>();
                        if(currHotteok.isPressing)
                        {
                            currHotteok = null;
                        }
                        else
                        {
                            currHotteok.HoldHotteok();
                            if (currHotteok.currStatus != 0 || currHotteok.isBurned)
                                ActiveTongsIngameUI(true);
                        }
                    }
                }
            }
        }
        if (currHotteok)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, transform.forward, 15.0f, rackLayerMask);

            bool activeCoolingRackUI = false;
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.name == "Hotteok_CoolingRack")
                    {
                        activeCoolingRackUI = true;
                    }
                }
            }

            if((mousePos.x > -5.0f && mousePos.x < 5.0f) && mousePos.y > 2.0f)
            {
                activeCoolingRackUI = true;
            }

            coolingRackUI.SetActive(activeCoolingRackUI);

            CheckingIngameUI();
        }

        if (Input.GetMouseButtonUp(0))
        {

            if (currHotteok)
            {
                // 물체의 Collider 크기를 고려하여 겹치는지 여부를 확인
                //Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePos, currHotteok.GetComponent<CircleCollider2D>().radius, rackLayerMask);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 15.0f, rackLayerMask);

                if ((hit.collider != null || ((mousePos.x > -5.0f && mousePos.x < 5.0f) && mousePos.y > 2.0f)) && currHotteok.CanRack())
                { 
                    AddToRack();
                }
                else
                {
                    currHotteok.ReleaseHotteok();
                    levelSetting.handActive = true;
                }

                currHotteok = null;

                ActiveTongsIngameUI(false);
                coolingRackUI.SetActive(false);
            }
        }
    }
    int addRackHotteok = 0;
    void AddToRack()
    {
        HotteokOnRack htForRack = Instantiate(pref_hotteokOnRack).GetComponent<HotteokOnRack>();
        htForRack.name = htForRack.name + addedHotteok++.ToString();
        htForRack.SetRackHotteok(currHotteok);
        htForRack.GetComponent<SpriteRenderer>().sortingOrder = t_currSpawnCount;
        t_currSpawnCount++;
        htForRack.m_price = currHotteok.GetPrice();
        htForRack.m_stats = currHotteok.GetStats();
        currHotteok.AddToRack(); // reset currhotteok

        hotteokRack[(Dough)currHotteok.GetDoughType()].Add(htForRack.GetComponent<HotteokOnRack>());
        entireRackList.Add(htForRack);
        AddToVisualRack(htForRack);

        if (t_MaxVisualRackCount < visualRack.Count)
        {
            htForRack.gameObject.SetActive(false);
        }

        UpdateRackUI((Dough)currHotteok.GetDoughType());
    }

    void AddToRack(Hotteok ht_)
    {
        HotteokOnRack htForRack = Instantiate(pref_hotteokOnRack).GetComponent<HotteokOnRack>();
        htForRack.name = htForRack.name + addedHotteok++.ToString();
        htForRack.SetRackHotteok(ht_);
        htForRack.GetComponent<SpriteRenderer>().sortingOrder = t_currSpawnCount;
        t_currSpawnCount++;
        htForRack.m_price = ht_.GetPrice();
        htForRack.m_stats = ht_.GetStats();
        ht_.AddToRack(); // reset hotteok

        hotteokRack[(Dough)ht_.GetDoughType()].Add(htForRack.GetComponent<HotteokOnRack>());
        entireRackList.Add(htForRack);
        AddToVisualRack(htForRack);

        if (t_MaxVisualRackCount < visualRack.Count)
        {
            htForRack.gameObject.SetActive(false);
        }

        UpdateRackUI((Dough)ht_.GetDoughType());
    }

    void UpdateRackUI(Dough dough_)
    {
        RackUIText[((int)dough_)].text = hotteokRack[dough_].Count.ToString();
    }
    void UpdateRackPickedUI(Dough dough_)
    {
        RackUIText[((int)dough_)].text = (hotteokRack[dough_].Count - t_currPickedIndex).ToString();
    }

    void AddToVisualRack(HotteokOnRack htForRack)
    {
        if (t_MaxVisualRackCount + maxTakeOutCount > visualRack.Count)
        {
            visualRack.Add(htForRack);
            htForRack.m_visualIndex = visualRack.Count - 1;
            htForRack.transform.position = new Vector3(-4.2f + htForRack.m_visualIndex * 0.6f, 1.6f);

        }
    }

    void SetPickingHotteok(Dough doughType_)
    {
        t_currPickedDough = doughType_;
    }
   
    bool PickHotteok()
    {
        if(hotteokRack[t_currPickedDough].Count > 0)
        {
            t_currPickedHotteok = hotteokRack[t_currPickedDough][t_currPickedIndex];
            if (t_currPickedHotteok.m_visualIndex != -1)
            {
                t_currPickedHotteok.gameObject.SetActive(false);
                if(visualRack.Count >  t_MaxVisualRackCount + t_currPickedIndex)
                {
                    //Debug.Log( "In rack : " + visualRack.Count+ ", Active " + (t_MaxVisualRackCount + t_currPickedIndex));
                    visualRack[t_MaxVisualRackCount + t_currPickedIndex].gameObject.SetActive(true);
                }

                for (int i = t_currPickedHotteok.m_visualIndex; i < visualRack.Count; i++)
                {
                    Transform currVRT = visualRack[i].transform;
                    currVRT.position
                        = new Vector3(currVRT.position.x - 0.6f, currVRT.position.y, currVRT.position.z);
                }
            }

            ++t_currPickedIndex;
            UpdateRackPickedUI(t_currPickedDough);
            return true;
        }
        else
        {
            return false;
        }
    }

    void UnpickHotteok()
    {
        int indexToRemove = t_currPickedIndex - 1;
        Debug.Log("indexToRemove = " + indexToRemove.ToString());
        t_currPickedHotteok = hotteokRack[t_prevPickedDough][indexToRemove];
        if (t_currPickedHotteok.m_visualIndex != -1)
        {
            //Debug.Log("지금 빼는 호떡은 " + (t_currPickedHotteok.m_visualIndex+1).ToString() + "번째 호떡이고, 전체 호떡 중에서는 " +   t_currPickedIndex.ToString() + "번째임");

            t_currPickedHotteok.gameObject.SetActive(true);

            if(visualRack.Count     >  t_MaxVisualRackCount + indexToRemove)
            {
                    visualRack[t_MaxVisualRackCount + indexToRemove].gameObject.SetActive(false);
            }

            //string toPrint = "Rack에서 " + t_currPickedHotteok.m_visualIndex + "번째 호떡이라서 ";
            for (int i = t_currPickedHotteok.m_visualIndex; i < visualRack.Count; i++)
            {
                //toPrint += i.ToString() + ", ";
                Transform currVRT = visualRack[i].transform;
                currVRT.position
                    = new Vector3(currVRT.position.x + 0.6f, currVRT.position.y, currVRT.position.z);
            }
            //toPrint += "를 앞으로 0.6씩";
            //Debug.Log(toPrint);
        }
        //Debug.Log("Unpick " + t_currPickedIndex.ToString() + "번째 호떡");
        if(t_currPickedIndex > 0)
        {
            --t_currPickedIndex;
        }
        UpdateRackPickedUI(t_currPickedDough);
    }

    bool CanServeHotteoks()
    {
   
        return false;
    }

    int m_currMoney = 0;

    void ServePickedHotteoks(HotteokContainers containers_)
    {
        if (m_customerManager.GiveHotteokToCustomer(containers_))
        {
            m_feverGage += containers_.m_count * 50.0f;

            containers_.ResetContainer();
            containers_.gameObject.SetActive(false);

            // Real remove curr picked hotteok
            for (int i = t_currPickedIndex-1; i >= 0; --i)
            {
                HotteokOnRack r_currRackHotteok = hotteokRack[t_currPickedDough][i];
                if (r_currRackHotteok.m_visualIndex != -1)
                {
                    visualRack.RemoveAt(r_currRackHotteok.m_visualIndex);
                }
                entireRackList.Remove(r_currRackHotteok);
                hotteokRack[t_currPickedDough].Remove(r_currRackHotteok);
                Destroy(r_currRackHotteok);
                Debug.Log("Delete " + r_currRackHotteok.name);
            }

            t_currPickedIndex = 0;

            // Fill the left visual rack
            if (entireRackList.Count > t_MaxVisualRackCount)
            {
                for (int i = visualRack.Count; i < t_MaxVisualRackCount + maxTakeOutCount; ++i)
                {
                    if (entireRackList.Count <= i)
                        break;
                    HotteokOnRack htNotInVisualRack = entireRackList[i];
                    AddToVisualRack(htNotInVisualRack);
                }
            }

            for (int i = 0; i < visualRack.Count; ++i)
            {
                visualRack[i].m_visualIndex = i;
                Transform currVRH = visualRack[i].transform;
                currVRH.position
                    = new Vector3(-4.2f + 0.6f * visualRack[i].m_visualIndex, currVRH.position.y, currVRH.position.z);
            }
        }
    }

    void ReturnToVisualRack()
    {
        if(t_currPickedIndex > 0)
        {
            for (int i = 0; t_currPickedIndex > 0; ++i)
            {
                UnpickHotteok();
            }
        }
    }

    // Ingame UI section
    void ActiveHandsIngameUI(bool active_)
    {
        putToTrashCanUI.SetActive(active_);
    }


    void ActiveTongsIngameUI(bool active_)
    {
        if (levelSetting.m_gameData.currMission > 8) // if there is pet
        {
            giveToPetUI.SetActive(active_);
        }
        else
        {
            putToTrashCanUI.SetActive(active_);
        }
    }

    void CheckingIngameUI()
    {
        RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 15.0f, ingameUILayerMask);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject != prevObj)
            {
                if (prevObj != null)
                {
                    // reset prev obj
                    prevObj.GetComponent<SpriteRenderer>().color = new Color(0.22f, 0.22f, 0.22f, 0.5f);
                }
                // update curr obj
                prevObj = hit.collider.gameObject;
                prevObj.GetComponent<SpriteRenderer>().color = new Color(0.22f, 0.22f, 0.22f, 0.8f);
            }
        }
        else
        {
            if (prevObj != null)
            {
                prevObj.GetComponent<SpriteRenderer>().color = new Color(0.22f, 0.22f, 0.22f, 0.5f);
                prevObj = null;
            }
        }
    }

    void ResetTongs()
    {
        currHotteok?.ReleaseHotteok();
        currHotteok = null;
        ActiveHand();
        ActiveTongsIngameUI(false);
    }

    void ResetCurrSequence()
    {
        hand.localScale = new Vector3(1.0f, 1.0f, 0.0f);
        hand.localRotation = Quaternion.identity;

        switch (handMode)
        {
            case HandMode.HAND:
                ResetHand();
                break;
            case HandMode.OIL:
                ResetOil();
                break;
            case HandMode.PRESSER:
                ResetPresser();
                break;
            case HandMode.TONGS:
                ResetTongs();
                break;
            default:
                break;
        }
    }

    void ActiveHand()
    {
        hand.gameObject.SetActive(levelSetting.handActive);
    }

    ////////////////////////////////
    ///
    ///      INGAME SECTION
    /// 
    ////////////////////////////////

    [Header("Ingame Section")]
    [SerializeField] float g_maxTime = 600.0f;
    float g_currTime = 0.0f;

    // day time control
    float g_morningTime = 200.0f;
    float g_eveningTime = 400.0f;
    uint g_dayTimeBackground = 0; // 0 = morning, 1 = evening, 1 = night;

    void Game_Initialize()
    {
        // initialize playtime
        g_clockInside.fillAmount = 0.0f;


        g_morningTime = g_maxTime / 3.0f;
        g_eveningTime = g_morningTime * 2.0f;
        // fill the clock section
        float timeRatio = g_maxTime * 5.0f / 6.0f;

        g_clockInside.DOFillAmount(5.0f / 6.0f, g_maxTime).SetEase(Ease.Linear);
        g_minuteHand.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, -300.0f), g_maxTime, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);

        //Start game play
    }

    bool g_isOpened = true;
    bool g_doDailyCalculation = false;
    [SerializeField] List<GameObject> g_lamps = new List<GameObject>();
    [SerializeField] List<GameObject> g_lampLights = new List<GameObject>();
    [SerializeField] GameObject g_background;

    // clock
    [SerializeField] Image g_clockInside;
    [SerializeField] Image g_minuteHand;

    // 정산표
    [SerializeField] GameObject g_workingSheet;

    void Game_Update()
    {
        // When Open
        if (g_isOpened)
        {
            // When Time is 10min (600s)
            g_currTime += Time.deltaTime;

            // Close The Store
            if (g_currTime > g_maxTime)
            {
                // Then Stop Spawn the customer anymore
                g_isOpened = false;
                m_customerManager.CloseTheStore();
            }

            // Background changing
            if (g_dayTimeBackground == 1 && g_currTime > g_eveningTime)
            {
                g_dayTimeBackground = 2;
                g_background.GetComponent<HasMultipleSprites>().SetNextSprite();
                g_clockInside.color = new Color(0.3f, .4f, .7f);
                // lamps turn on
                foreach (GameObject lamp in g_lamps)
                {
                    lamp.GetComponent<HasMultipleSprites>().SetNextSprite();
                }
                foreach (GameObject light in g_lampLights)
                {
                    light.SetActive(true);
                }
            }
            else if (g_dayTimeBackground == 0 && g_currTime > g_morningTime)
            {
                g_dayTimeBackground = 1;
                g_background.GetComponent<HasMultipleSprites>().SetNextSprite();
                g_clockInside.color = new Color(1.0f, .75f, .75f);
            }
        }
        // When Close
        else
        {
            // do actual close when no customers in screen
            if (!g_doDailyCalculation)
            {
                if (m_customerManager.GetLeftCustomerNum() == 0)
                {
                    g_doDailyCalculation = true;
                    // 정산
                    //g_calculationChart.
                    g_workingSheet.SetActive(true);
                    hand.gameObject.SetActive(false);
                    Cursor.visible = true;
                }
            }
        }
    }

    // FEVER SECTION
    bool m_isFeverTime = false;
    public int m_maxCount = 15;
    float m_feverMaxGage = 100.0f;
    public float m_maxFeverTime = 15.0f;

    int m_feverCount = 0;
    float m_feverGage = 0.0f;
    float m_feverTimer = 0.0f;

    bool m_feverResetting = false;

    private List<Vector2> spawnPositions = new List<Vector2>();
    private List<Dough> htToSpawn = new List<Dough>();

    [Header("FeverSection")]
    [SerializeField] Image m_feverGageBar;
    [SerializeField] Sprite m_blackFever;
    [SerializeField] Image m_feverChar;
    [SerializeField] Animator m_feverAnim;
    [SerializeField] Image m_combo;
    [SerializeField] Image m_comboNumbers;

    Sequence feverAnimSequence;
    Sequence stampSequence;
    Sequence stampNumSequence;
    void FeverInit()
    {
        m_feverCount = 0;
        m_feverGage = 0.0f;
        m_feverTimer = 0.0f;

        m_combo.DOFade(0.0f, 0.0f);
        m_comboNumbers.DOFade(0.0f, 0.0f);
    }

    public void StartFeverGame()
    {
        m_feverGageBar.fillAmount = 1.0f;
        m_feverCount = 0;
        // Show Folding Game UI
        doughFoldUI.SetActive(true);
        feverDoughFoldUI.SetActive(true);
        // Set Arrow dir
        SetFeverFoldingDirection();
        m_feverResetting = true;
        m_isFeverTime = true;

        m_feverAnim.enabled = true;

        feverAnimSequence = DOTween.Sequence();
        feverAnimSequence.Append(m_feverGageBar.DOFade(0.3f, 0.5f).SetEase(Ease.Linear));
        feverAnimSequence.Append(m_feverGageBar.DOFade(1.0f, 0.5f).SetEase(Ease.Linear));
        feverAnimSequence.Insert(0.0f, m_feverGageBar.transform.DOShakePosition(0.1f, 10.0f).SetEase(Ease.Linear));
        feverAnimSequence.SetLoops(-1); // 무한 반복
        m_comboNumbers.GetComponent<HasMultipleImages>().SetSprite(0);


        // 손모양 보여주기
        dough_Hand.SetActive(true);
        dough_DoughOnHand.SetActive(true);

        m_hotteokPlate.IsFever(true);
    }
    void FeverUpdate()
    {
        if(!m_isFeverTime)
        {
            m_feverGageBar.fillAmount = m_feverGage / m_feverMaxGage;
            if (m_feverGage > m_feverMaxGage)
            {
                // active fever uis
                StartFeverGame();
            }
        }
        else
        {
            m_feverTimer += Time.deltaTime;
            m_feverGageBar.fillAmount = (m_maxFeverTime - m_feverTimer) / m_maxFeverTime;
            if (m_maxFeverTime < m_feverTimer || m_feverCount >= m_maxCount)
            {
                SpawnFeverHotteoks();
                ResetFeverTime();
                return;
            }
            GetFeverDoughInput();
            PlayingFeverGame();

            TakeShootenHotteok();
        }
    }

    void TakeShootenHotteok()
    {
        if(m_hotteokPlate.hotteoks.Count > 0)
        {
            List<Hotteok> htChecker = new List<Hotteok>(m_hotteokPlate.hotteoks);
            //hotteokRack
            foreach (Hotteok ht in htChecker)
            {
                CircleCollider2D htCol = ht.GetComponent<CircleCollider2D>();
                if (Physics2D.IsTouching(htCol, m_hotteokRackCollider))
                {
                    AddToRack(ht);
                }
            }
        }
    }

    void ResetFeverTime()
    {
        // inactive fever ui
        doughFoldUI.SetActive(false);
        feverDoughFoldUI.SetActive(false);

        // reset fever values
        m_isFeverTime = false;
        m_feverTimer = 0.0f;
        m_feverCount = 0;
        m_feverGage = 0.0f;
        htToSpawn.Clear();
        feverAnimSequence.Kill();
        m_feverGageBar.DOFade(1.0f, 0.0f);
        m_feverAnim.enabled = false;
        m_feverChar.sprite = m_blackFever;
        ResetHand();
        m_hotteokPlate.IsFever(false);
    }

    void GenerateSpawnPositions()
    {
        for (int i = 0; i < m_maxCount; i++)
        {
            Vector2 randomPosition = GenerateNonOverlappingPosition();
            spawnPositions.Add(randomPosition);
        }
    }

    void SetFeverFoldingDirection()
    {
        SetHandDoughWithCurrDough();
        //Debug.Log("Curr Step = " + currStep);
        currKeyStep = 0;
        dough_DoughOnHand.GetComponent<SpriteRenderer>().sprite = doughMap[currDough][currStep];
        doughFoldUI.transform.DOShakePosition(.1f, 5, 5);
        m_feverResetting = true;
    }

    void SetHandDoughWithCurrDough()
    {
        switch (currDough)
        {
            case Dough.BASE:
                currStep = 1;
                break;
            case Dough.GREENTEA:
            case Dough.SWEETPOTATO:
                currStep = 2;
                break;
            default:
                break;
        }
    }

    void GetFeverDoughInput()
    {
        if ((hand.localPosition.x < -5.0f || hand.localPosition.x > 5.0f) && hand.localPosition.y < 3.5f)
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 15.0f, interactiveSpaceLayerMask);

            if (hit.collider != null)
                if(hit.collider.name == "GreenTeaDoughContainer" ||
                hit.collider.name == "DoughContainer" ||
                hit.collider.name == "SweetPotatoDoughContainer")
            {
                otForHand = hit.collider.GetComponent<ForHand>();
                if (prevForHand != otForHand)
                {
                    Debug.Log("Onhand " + hit.collider.name);

                    hand.GetComponent<Animator>().runtimeAnimatorController = otForHand.handAnimator.runtimeAnimatorController;
                    //otForHand.MouseOn(true);
                    if (prevForHand != null)
                    {
                        prevForHand.GetComponent<ForHand>().MouseOn(false);
                    }
                    otForHand.GetComponent<ForHand>().MouseOn(true);
                    prevForHand = otForHand;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (otForHand)
                    {
                        switch (hit.collider.name)
                        {
                            case "DoughContainer":
                                currDough = Dough.BASE;
                                currStep = 1;
                                break;
                            case "GreenTeaDoughContainer":
                                currDough = Dough.GREENTEA;
                                currStep = 2;
                                break;
                            case "SweetPotatoDoughContainer":
                                currDough = Dough.SWEETPOTATO;
                                currStep = 2;
                                break;
                            default:
                                break;
                        }
                    dough_DoughOnHand.GetComponent<SpriteRenderer>().sprite = doughMap[currDough][currStep + currKeyStep];
                    }
                }
            }
            else
            {
                // 감지된 오브젝트가 없거나 태그가 일치하지 않으면 기본 커서 이미지로 변경
                hand.GetComponent<Animator>().runtimeAnimatorController = null;
                if (prevForHand)
                {
                    prevForHand.GetComponent<ForHand>().MouseOn(false);
                    prevForHand = null;
                }
            }
        }
    }
    void PlayingFeverGame()
    {
        ///
        if (m_feverResetting)
        {
            m_foldResetTimer += Time.deltaTime;
            if (m_foldResetTimer > 0.1f)
            {
                // Set Arrow dir
                int ranVal = Random.Range(0, 3);
                for (int i = 0; i < 4; ++i)
                {
                    doughKeys[i] = myKeys[ranVal];
                    doughArrows[i].GetComponent<Image>().sprite = doughArrowsSprite[ranVal];
                    doughArrows[i].GetComponent<Image>().DOFade(1.0f, 0.0f);
                }
                m_feverResetting = false;
            }
            return;
        }


        if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
        {
            if (Input.GetKey(doughKeys[currKeyStep]) && !m_feverResetting)
            {
                doughArrows[currKeyStep].GetComponent<Image>().DOFade(0.0f, 0.1f);
                ++currKeyStep;
                dough_DoughOnHand.GetComponent<SpriteRenderer>().sprite = doughMap[currDough][currStep + currKeyStep];
                ShakeObject(doughFoldUI, 15.0f);
                ShakeObject(dough_DoughOnHand, 1.0f);
                ShakeObject(dough_Hand, 1.0f);
            }
        }

        if (currKeyStep == 4)
        {
            ++m_feverCount;
            currKeyStep = 0;
            SetFeverFoldingDirection();
            m_foldResetTimer = 0.0f;
            htToSpawn.Add(currDough);
            m_comboNumbers.GetComponent<HasMultipleImages>().SetNextSprite();
            Stamp(m_combo.transform);
            if(m_feverCount > 1)
            {
                NumStamp(m_comboNumbers.transform);
            }
        }
    }

    private Vector2 GenerateNonOverlappingPosition()
    {
        while (true)
        {
            Vector2 randomPosition = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(0.0f, -4.0f));
            if (!spawnPositions.Contains(randomPosition))
            {
                return randomPosition;
            }
        }
    }

    private void SpawnFeverHotteoks()
    {
        for (int i = 0; i < m_feverCount; i++)
        {
            Vector2 currPos = spawnPositions[i];
            Hotteok newHotteok;
            switch (htToSpawn[i])
            {
                case Dough.BASE:
                    newHotteok = Instantiate(hotteok, new Vector3(currPos.x, currPos.y, -5.0f), Quaternion.identity);
                    break;
                case Dough.GREENTEA:
                    newHotteok = Instantiate(greenteaHotteok, new Vector3(currPos.x, currPos.y, -5.0f), Quaternion.identity);
                    break;
                case Dough.SWEETPOTATO:
                    newHotteok = Instantiate(sweetPotatoHotteok, new Vector3(currPos.x, currPos.y, -5.0f), Quaternion.identity);
                    break;
                default:
                    newHotteok = Instantiate(hotteok, new Vector3(currPos.x, currPos.y, -5.0f), Quaternion.identity);
                    break;
            }
            m_hotteokPlate.hotteoks.Add(newHotteok);
        }
    }

    void Stamp(Transform stamp)
    {
        Vector3 startScale = Vector3.one;
        Vector3 endScale = new Vector3(1.1f, 1.1f, 1f);

        stampSequence.Kill();
        stampSequence = DOTween.Sequence();
        stampSequence.Append(stamp.GetComponent<Image>().DOFade(1.0f, 0.0f));
        stampSequence.Append(stamp.DOScale(endScale, 0.5f).SetEase(Ease.OutQuad));
        stampSequence.Append(stamp.DOScale(startScale, 0.15f).SetEase(Ease.InQuad));
        stampSequence.Append(stamp.GetComponent<Image>().DOFade(0.0f, 3.0f).SetEase(Ease.Linear));
    }

    void NumStamp(Transform stamp)
    {
        Vector3 startScale = Vector3.one;
        Vector3 endScale = new Vector3(1.5f, 1.5f, 1f);

        stampNumSequence.Kill();
        stampNumSequence = DOTween.Sequence();
        stampNumSequence.Append(stamp.GetComponent<Image>().DOFade(1.0f, 0.0f));
        stampNumSequence.Append(stamp.DOScale(endScale, 0.5f).SetEase(Ease.OutQuad));
        stampNumSequence.Append(stamp.DOScale(startScale, 0.15f).SetEase(Ease.InQuad));
        stampNumSequence.Append(stamp.GetComponent<Image>().DOFade(0.0f, 3.0f).SetEase(Ease.Linear));
    }

}
