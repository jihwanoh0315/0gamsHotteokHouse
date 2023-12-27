using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotteokBoardStatus : MonoBehaviour
{
    [SerializeField] public List<Hotteok> hotteoks = new List<Hotteok>();
    [SerializeField] public BoxCollider2D hotteokRack;

    [SerializeField] public float oilTemprature = 1.0f;
    [SerializeField] public float toastAmount = 1.0f;
    [SerializeField] public float oilPerHotteok = 0.005f;

    [SerializeField] Image m_thermometer;
    [SerializeField] Image m_temperPart;
    [SerializeField] Image m_temperGrad;
    [SerializeField] Image m_dangerSign;

    LevelSetting m_levelSetting;
    int currOil = 0;
    //float burningPerSec = 0.01f;
    //float burningPerSec = 0.0015f;
    float burningPerSec = 0.0005f;
    float warmingPerSec = 0.005f;
    float prevTemp = 0.0f;
    float animUpdateTime = 0.0f;

    bool m_isFever = false;

    // Start is called before the first frame update
    void Awake()
    {
        m_levelSetting = FindAnyObjectByType<LevelSetting>();
        currOil = m_levelSetting.activeOil;
        burningPerSec *= Time.deltaTime;
        warmingPerSec *= Time.deltaTime;

        switch (currOil)
        {
            case 1:
                burningPerSec *= 0.7f;
                warmingPerSec *= 1.2f;
                oilPerHotteok *= 0.7f;
                break;
            case 2:
                burningPerSec *= 0.4f;
                break;

            default:
                break;
        }

        m_dangerSign.DOFade(0.0f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        m_dangerSign.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isFever)
        {
            oilTemprature = 0.5f;
            m_temperPart.color = Color.red;
        }
        else
        {
            if (oilTemprature < 1.0f)
            {
                if (oilTemprature < 0.4f)
                {
                    oilTemprature += warmingPerSec;
                    toastAmount = 0.5f;
                    m_temperPart.color = new Color(0.65f, 0.95f, 1.0f);
                }
                else if (oilTemprature > 0.8f)
                {
                    toastAmount = 1.0f;
                    oilTemprature += burningPerSec;
                    m_temperPart.color = Color.red;

                    animUpdateTime += Time.deltaTime;
                    if (animUpdateTime > 0.4f)
                    {
                        m_thermometer.transform.DOShakePosition(0.2f, new Vector3(5.0f, 0.0f), 10);
                        animUpdateTime = 0.0f;
                    }
                    m_dangerSign.gameObject.SetActive(true);
                }
                else if (oilTemprature > 0.7f)
                {
                    toastAmount = 1.0f;
                    oilTemprature += burningPerSec;
                    m_temperPart.color = Color.red;

                    animUpdateTime += Time.deltaTime;
                    if (prevTemp > 0.8f)
                        m_dangerSign.gameObject.SetActive(false);

                    if (animUpdateTime > 1.0f)
                    {
                        m_thermometer.transform.DOShakePosition(0.2f, new Vector3(5.0f, 0.0f), 10);
                        animUpdateTime = 0.0f;
                    }

                }
                else
                {
                    m_temperPart.color = new Color(1.0f, 0.3f, 0.2f);
                    toastAmount = 1.0f;
                    oilTemprature += burningPerSec;
                    if (prevTemp > 0.8f)
                        m_dangerSign.gameObject.SetActive(false);
                }

            }
        }
        m_temperPart.fillAmount = oilTemprature;
        m_temperGrad.fillAmount = oilTemprature;
        prevTemp = oilTemprature;
    }

    public void PutHotteok()
    {
        oilTemprature += burningPerSec;
    }

    public void PutOil()
    {
        switch (currOil)
        {
            case 0:
                oilTemprature -= 0.2f;

                break;
            case 1:
                oilTemprature -= 0.3f;
                break;
            case 2:
                oilTemprature = 0.5f;
                break;

            default:
                break;
        }

        if(oilTemprature < 0.2f)
        {
            oilTemprature = 0.2f;
        }
    }

    public void IsFever(bool active_)
    {
        m_isFever = active_;
        if(m_isFever)
        {
            m_temperPart.color = Color.red;
            FeverHotteokShooter();
        }
    }

    public void FeverHotteokShooter()
    {
        foreach(Hotteok ht in hotteoks)
        {
            // Hotteoks to fry
            ht.IsFeverTimeNow();
            ht.GetComponent<CircleCollider2D>().isTrigger = true;

            ht.transform.DOLocalMove(hotteokRack.transform.position, 2.0f).SetEase(Ease.InOutBack);
        }
    }
}
