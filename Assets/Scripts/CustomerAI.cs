using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

[System.Serializable]
public class CustomerStatus
{
    public SpriteRenderer EmoteBox;
    public SpriteRenderer Emoji;
    public SpriteRenderer OrderCount;

    public int orderCount;
}


public class CustomerAI : MonoBehaviour
{
    enum CustomerSequence
    {
        OnLine = 0,
        GoToCounter,
        OrderingHotteok,
        OrderingSidedish,
        ConsideringSidedish,
        OutFromWindow,
        Remove
    }
    public CustomerStatus m_status;
    [SerializeField]
    private EmoteManager m_theEM;
    [SerializeField]
    Animator m_animator;

    CharacterEmoji m_characterEmoji;

    public int m_hotteokDough = 0;
    public int m_sidePlace = 0;
    public int m_actualSide = 0; // for texture and price

    public int m_moneyToPay = 0;

    float m_nextStepTime = 5.0f;
    float m_currTime = 0.0f;
    float m_waitTime = 10.0f;

    bool m_isWaiting = false;
    bool m_isMoving = false;
    public bool m_getAllHotteok = false;
    public bool m_shoppingFinishied = false;
    public float m_movingSpeed = .3f;

    int m_currStep = 0;

    // for angry and run
    public float m_angryTime;
    public float m_angryMax;

    CustomerSequence m_currSequence = 0;

    // Start is called before the first frame update
    //void Start()
    //{
    //    m_theEM = FindObjectOfType<EmoteManager>();
    //    m_animator = GetComponent<Animator>();
    //}

    void Awake()
    {
        m_theEM = FindObjectOfType<EmoteManager>();
        m_animator = GetComponent<Animator>();
        m_currSequence = CustomerSequence.OnLine;
        m_status.EmoteBox.gameObject.SetActive(false);
        m_characterEmoji = GetComponent<CharacterEmoji>();
    }


    // Update is called once per frame
    void Update()
    {
        if (m_isWaiting)
        {
            m_currTime += Time.deltaTime;
            //Debug.Log("Wait " + m_currTime);
            if (m_waitTime < m_currTime)
            {
                //Debug.Log("Go NEXT!");
                m_isWaiting = false;
                m_currTime = 0.0f;
            }
            return;
        }

        if (m_isMoving)
        {
            DoMove();
            return;
        }
        //Debug.Log(m_currSequence);

        switch (m_currSequence)
        {
            case CustomerSequence.OnLine:
                // checke front customer

                break;
            case CustomerSequence.GoToCounter:
                switch (m_currStep)
                {
                    case 0:
                        WaitForNextStep(0.5f);
                        ++m_currStep;
                        break;
                    case 1:
                        GoTo(2.5f);
                        m_status.EmoteBox.gameObject.SetActive(true);
                        GoNextSequence();
                        break;
                }
                // walk to counter
                break;
            case CustomerSequence.OrderingHotteok:
                switch (m_currStep)
                {
                    case 0:
                        // wait until get proper num of hotteok
                        if (m_getAllHotteok)
                        {
                            m_characterEmoji.SetEmoji(CharacterEmoji.Emoji.Smile);
                            ++m_currStep;
                        }
                        break;
                    case 1:
                        CheckSideDish();
                        GoNextSequence();
                        break;
                    default:
                        break;
                }
                break;
            case CustomerSequence.OrderingSidedish:
                switch (m_currStep)
                {
                    case 0:
                        m_characterEmoji.SetEmoji(CharacterEmoji.Emoji.Ellipsis);
                        WaitForNextStep(1.0f);
                        m_currStep++;
                        break;
                    case 1:
                        m_status.Emoji.GetComponent<SpriteRenderer>().sprite = m_theEM.emoteDic[11000 + m_actualSide];

                        // if(levelSetting.pet.active)
                        //  Random.Range(1,5) == 1
                        //   pet.doAction();

                        m_characterEmoji.SetEmoji(CharacterEmoji.Emoji.Other);
                        WaitForNextStep(1.0f);
                        m_currStep++;
                        break;
                    case 2:
                        m_characterEmoji.SetEmoji(CharacterEmoji.Emoji.Happy);
                        GoNextSequence();
                        break;
                    default:
                        break;
                }
                //happy
                break;
            case CustomerSequence.ConsideringSidedish:
                switch (m_currStep)
                {
                    case 0:
                        m_characterEmoji.SetEmoji(CharacterEmoji.Emoji.Ellipsis);
                        WaitForNextStep(1.0f);
                        m_currStep++;
                        break;
                    case 1:
                        m_characterEmoji.ActiveEmoji(false);
                        GoNextSequence();
                        break;
                    default:
                        break;
                }
                break;
            case CustomerSequence.OutFromWindow:
                // walk to right
                GoTo(15.0f);
                GoNextSequence();
                break;
            case CustomerSequence.Remove:
                m_shoppingFinishied = true;
                break;
            default:
                break;
        }
    }

    void CheckFrontCustomer()
    {

    }

    public void GoOrdering()
    {
        m_currSequence = CustomerSequence.GoToCounter;
    }

    public void SetOrderCount(int maxCount_/*must < 6*/)
    {
        int currCount = Random.Range(1, maxCount_);
        m_status.orderCount = currCount;
        // ID = 12000
        m_status.OrderCount.GetComponent<SpriteRenderer>().sprite = m_theEM.emoteDic[12000 + currCount - 1];
    }

    public void ResetOrderCountTexture()
    {
        // ID = 12000
        m_status.OrderCount.GetComponent<SpriteRenderer>().sprite = m_theEM.emoteDic[12000 + m_status.orderCount - 1];
    }

    public void SetDough(int anotherDough, int ratio_)
    {
        int spawnVal = Random.Range(0, 100);
        if (spawnVal < ratio_) // Spawn Another Dough
        {
            m_hotteokDough = anotherDough;
        }
        m_status.Emoji.GetComponent<SpriteRenderer>().sprite = m_theEM.emoteDic[10000 + m_hotteokDough];
    }

    public bool GetHotteok(HotteokContainers containers_)
    {
        if (containers_.m_dough != m_hotteokDough)
        {
            return false;
        }

        // check count and do the thing
        if (containers_.m_count > m_status.orderCount)
        {
            return false;
        }
        else if (containers_.m_count < m_status.orderCount)
        {
            m_status.orderCount -= containers_.m_count;
            m_moneyToPay += containers_.GetPrice();
            ResetOrderCountTexture();
            return true;
        }
        else
        {
            m_status.orderCount = 0;
            m_moneyToPay += containers_.GetPrice();
            m_status.OrderCount.gameObject.SetActive(false);
            m_getAllHotteok = true;
            return true;
        }
    }

    public void SetSideInTex(int index_, int actualSide_)
    {
        m_sidePlace = index_;
        m_actualSide = actualSide_;
    }

    void CheckSideDish()
    {
        if (m_sidePlace == 1)
        {
            GoTo(4.8f);
            GoToSequence(CustomerSequence.OrderingSidedish);
            m_currSequence = CustomerSequence.OrderingSidedish;
        }
        else if (m_sidePlace == 2)
        {
            GoTo(5.9f);
            GoToSequence(CustomerSequence.OrderingSidedish);
            m_currSequence = CustomerSequence.OrderingSidedish;
        }
        else
        {
            if (Random.Range(1, 5) < 1)
            {
                GoToSequence(CustomerSequence.ConsideringSidedish);
            }
            else
            {
                GoToSequence(CustomerSequence.OutFromWindow);
            }
        }
    }

    void ArriveSideDish()
    {

    }

    public void GoTo(float nextPos_)
    {
        float movTime = (nextPos_ - transform.position.x) / m_movingSpeed;
        m_currTime = 0.0f;

        m_animator.SetBool("isWalking", true);
        transform.DOMoveX(nextPos_, movTime).SetEase(Ease.Linear);
        m_currTime = 0.0f;
        //Debug.Log("Go to " + nextPos_.ToString() + " in " + movTime.ToString() + "sec");
        m_nextStepTime = movTime;

        m_isMoving = true;
    }

    void DoMove()
    {
        m_currTime += Time.deltaTime;

        if (m_nextStepTime < m_currTime)
        {
            //Debug.Log("Stop moving cuz " + m_nextStepTime.ToString() + " < " + m_currTime.ToString() + "sec");

            m_animator.SetBool("isWalking", false);
            m_isMoving = false;
        }
    }

    void WaitForNextStep(float time_)
    {
        m_currTime = 0.0f;
        m_isWaiting = true;
        m_waitTime = time_;
    }

    public bool IsNotBusy()
    {
        return !m_isWaiting && !m_isMoving;
    }               
    
    public bool IsCanOrder()
    {
        return m_currSequence == CustomerSequence.OrderingHotteok;
    }

    void GoNextSequence()
    {
        ++m_currSequence;
        m_currStep = 0;
    }
    void GoToSequence(CustomerSequence customerSequence_)
    {
        m_currStep = 0;
        m_currSequence = customerSequence_;
    }
}                   
