using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoughManager : MonoBehaviour
{
    public float timeLimit = 60.0f;
    float currTime = 0.0f;
    float limitTimer = 0.0f;
    //[SerializeField] GameObject go_hand;
    [SerializeField] ScreenInOut screenInOut;
    [SerializeField] DoughHand handInfo;
    [SerializeField] float spawnTime;
    [SerializeField] int maxDoughCutline;
    [SerializeField] LevelSetting levelSetting;

    [SerializeField] GameObject doughBackground;
    [SerializeField] GameObject normalDough;
    [SerializeField] GameObject greenTeaDough;
    [SerializeField] GameObject sweetPotatoDough;
    [SerializeField] List<GameObject> smallDoughs;
    [SerializeField] List<GameObject> smallGreenTeaDoughs;
    [SerializeField] List<GameObject> smallSweetPotatoDoughs;

    [SerializeField] Dough[] doughInfo;
    [SerializeField] Slider timeSlider;
    [SerializeField]  TMP_Text timeText;
    [SerializeField]  TMP_Text doughText;

    // UI Section
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject endSign;
    [SerializeField] GameObject DoughUI;
    [SerializeField] GameObject baseDoughUI;
    [SerializeField] GameObject secondDoughUI;

    [SerializeField] List<Sprite> rankImages;
    [SerializeField] List<Sprite> secondContainer;
    // Base Dough UI
    [SerializeField] Image baseRank;
    [SerializeField] TMP_Text baseDoughPercent;
    [SerializeField] TMP_Text baseDoughResult;

    // Second Dough UI
    [SerializeField] TMP_Text secondDoughTitle;
    [SerializeField] Image secondDoughContainer;
    [SerializeField] Image secondRank;
    [SerializeField] TMP_Text secondDoughPercent;
    [SerializeField] TMP_Text secondDoughResult;

    [SerializeField] GameObject OKButton;

    public bool canSpawn = true;
    bool isStartDough = false;
    bool isShowStartButton = false;
    bool isTwo = false;
    bool isFinished = false;
    bool isWaiting = false;
    bool showUI = false;
    bool showOK = false;
    float waitTime = 3.0f;
    float currWaitTime = 0.0f;
    //bool StartDough = false;
    int lastDough = 0;
    int currSmallDough = 0;
    float currDoughPercent = 0.0f;
    Sequence mySequence;

    // Start is called before the first frame update
    void Start()
    {
        screenInOut = FindAnyObjectByType<ScreenInOut>();
        levelSetting = FindAnyObjectByType(typeof(LevelSetting)) as LevelSetting;
        lastDough = 0;
        screenInOut.VertCutIn();
        StartDough();
        currTime = 0.0f;
        limitTimer = timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if(isWaiting)
        {
            currWaitTime += Time.deltaTime;

            if (currWaitTime > waitTime)
            {
                isWaiting = false;
            }
            return;
        }
        if(isShowStartButton)
        {
            isShowStartButton = false;
            ShowStartButton();
        }
        if (!isStartDough)
        {
            return;
        }

        if (!isFinished)
        {
            float doughRatio = (float)handInfo.CollideCount / (float)maxDoughCutline;

            currDoughPercent = (doughRatio * 100.0f);
            doughText.text = currDoughPercent.ToString("F1") + "% ¿Ï·áµÊ";
            SpawnSmallDough();

            limitTimer -= Time.deltaTime;
            timeSlider.value = limitTimer;
            timeText.text = limitTimer.ToString("F1") + "s";

            if (handInfo.CollideCount > maxDoughCutline
            || limitTimer < 0.0f)
            {
                EndWork();
                //Debug.Log(handInfo.CollideCount);
                if (!isTwo)
                {
                    currSmallDough = 0;
                    currTime = 0.0f;
                    limitTimer = timeLimit;
                    if (levelSetting.m_gameData.activeFoods[0] == true) //greentea
                    {
                        

                        isTwo = true;
                        lastDough = 1;
                        normalDough.SetActive(false);
                        greenTeaDough.SetActive(true);
                        isShowStartButton = true;
                    }
                    else if (levelSetting.m_gameData.activeFoods[1] == true) // sweetPotato
                    {
                        isTwo = true;
                        lastDough = 2;
                        normalDough.SetActive(false);
                        sweetPotatoDough.SetActive(true);
                        isShowStartButton = true;
                    }
                    else
                    {
                        FinishDough();
                    }
                }
                else
                {
                    FinishDough();
                }
            }

            // Background
            Color currCol = doughBackground.gameObject.GetComponentInParent<SpriteRenderer>().color;
            doughBackground.gameObject.GetComponentInParent<SpriteRenderer>().color
             = new Color(currCol.r, currCol.g, currCol.b, 1.0f - doughRatio);

        }
        else
        {
            ShowUI();
            ShowOKButton();
        }
    }

    void SpawnSmallDough()
    {
        if(canSpawn)
        {
            if (currSmallDough > 3)
            {
                canSpawn = false;
                return;
            }
            currTime += Time.deltaTime;

            if (currTime > spawnTime)
            {
                currTime = 0.0f;
                canSpawn = false;
                switch (lastDough)
                {
                    case 0:
                        smallDoughs[currSmallDough].SetActive(true);
                        smallDoughs[currSmallDough].transform.localPosition = new Vector3(Random.Range(-1.8f, 1.8f), Random.Range(-1.8f, 1.8f), -1.0f);

                        foreach (var sj in smallDoughs[currSmallDough].GetComponentsInChildren<SpringJoint2D>())
                        {
                            sj.autoConfigureConnectedAnchor = true;
                            //sj.autoConfigureConnectedAnchor = false;
                        }
                        break;
                    case 1:
                        smallGreenTeaDoughs[currSmallDough].SetActive(true);
                        smallGreenTeaDoughs[currSmallDough].transform.localPosition = new Vector3(Random.Range(-1.8f, 1.8f), Random.Range(-1.8f, 1.8f), -1.0f);

                        foreach (var sj in smallGreenTeaDoughs[currSmallDough].GetComponentsInChildren<SpringJoint2D>())
                        {
                            sj.autoConfigureConnectedAnchor = true;
                            //sj.autoConfigureConnectedAnchor = false;
                        }
                        break;
                    case 2:
                        smallSweetPotatoDoughs[currSmallDough].SetActive(true);
                        smallSweetPotatoDoughs[currSmallDough].transform.localPosition = new Vector3(Random.Range(-1.8f, 1.8f), Random.Range(-1.8f, 1.8f), -1.0f);

                        foreach (var sj in smallSweetPotatoDoughs[currSmallDough].GetComponentsInChildren<SpringJoint2D>())
                        {
                            sj.autoConfigureConnectedAnchor = true;
                            //sj.autoConfigureConnectedAnchor = false;
                        }
                        break;

                    default:
                        break;
                }
                currSmallDough++;
            }
        }

    }

    void FinishDough()
    {
        isFinished = true;
        showUI = true;

        levelSetting.m_gameData.doughScore[lastDough] = doughInfo[lastDough].doughScore;
        if (isTwo)
        {
            levelSetting.m_gameData.doughScore[0] = doughInfo[0].doughScore;
        }


    }

    void SaveDoughInfo(int index_)
    {
        doughInfo[index_].doughPercent = currDoughPercent;

        if(doughInfo[index_].doughPercent >= 100.0f)
        {
            doughInfo[index_].doughScore = 1.5f;
        }
        else if(doughInfo[index_].doughPercent >= 70.0f)
        {
            doughInfo[index_].doughScore = 1.25f;
        }
        else if(doughInfo[index_].doughPercent >= 50.0f)
                {
                    doughInfo[index_].doughScore = 1.1f;
        }
        else
        {
            doughInfo[index_].doughScore = 1.0f;
        }
    }

    void StartDough()
    {
        normalDough.SetActive(true);
        normalDough.transform.DOLocalMoveY(0.0f, 1.0f).SetEase(Ease.OutBounce);
    }

    void WaitTimeSet(float time_)
    {
        currWaitTime = 0.0f;
        waitTime = time_;
        isWaiting = true;
    }

    void ShowUI()
    {
        if (showUI)
        {
            showUI = false;

            DoughUI.SetActive(true);


            baseDoughResult.text = "È£¶± °¡°Ý ";
            if (doughInfo[0].doughPercent >= 100.0f)
            {
                doughInfo[0].doughPercent = 100.0f;
                baseRank.sprite = rankImages[0];
                baseDoughResult.text += "50";
            }
            else if (doughInfo[0].doughPercent >= 70.0f)
            {
                baseRank.sprite = rankImages[1];
                baseDoughResult.text += "25";
            }
            else if (doughInfo[0].doughPercent >= 50.0f)
            {
                baseRank.sprite = rankImages[2];
                baseDoughResult.text += "10";
            }
            else
            {
                baseRank.sprite = rankImages[3];
                baseDoughResult.text += "0";
            }
            baseDoughResult.text += "% Áõ°¡ ";
            baseDoughPercent.text = doughInfo[0].doughPercent.ToString("F0") + "% ¹ÝÁ× ¿Ï·á";


            // Base Dough Anim
            mySequence = DOTween.Sequence();
            baseDoughPercent.DOFade(0.0f, 0.0f);
            baseDoughResult.DOFade(0.0f, 0.0f);
            baseRank.transform.DOScale(0.0f, 0.0f);
            mySequence.Append(baseDoughPercent.DOFade(1.0f, 0.1f).SetDelay(1.0f));
            mySequence.Insert(1.5f, baseDoughResult.DOFade(1.0f, 0.1f));
            mySequence.Insert(2.0f, baseRank.transform.DOScale(1.5f, 0.5f));
            mySequence.Insert(3.0f, baseRank.transform.DOScale(1.0f, 0.1f));


            if (isTwo)
            {
                secondDoughUI.SetActive(true);

                if(lastDough == 1)
                {
                    secondDoughTitle.text = "³ìÂ÷ ¹ÝÁ×";
                    secondDoughContainer.sprite = secondContainer[0];
                }
                else if(lastDough == 2)
                {
                    secondDoughTitle.text = "°í±¸¸¶ ¹ÝÁ×";
                    secondDoughContainer.sprite = secondContainer[1];
                }
                secondDoughResult.text = "È£¶± °¡°Ý ";
                if (doughInfo[lastDough].doughPercent >= 100.0f)
                {
                    doughInfo[lastDough].doughPercent = 100.0f;
                    secondRank.sprite = rankImages[0];
                    secondDoughResult.text += "50";
                }
                else if (doughInfo[lastDough].doughPercent >= 70.0f)
                {
                    secondRank.sprite = rankImages[1];
                    secondDoughResult.text += "25";
                }
                else if (doughInfo[lastDough].doughPercent >= 50.0f)
                {
                    secondRank.sprite = rankImages[2];
                    secondDoughResult.text += "10";
                }
                else
                {
                    secondRank.sprite = rankImages[3];
                    secondDoughResult.text += "0";
                }
                secondDoughResult.text += "% Áõ°¡ ";
                secondDoughPercent.text = doughInfo[lastDough].doughPercent.ToString("F0") + "% ¹ÝÁ× ¿Ï·á";

                secondDoughPercent.DOFade(0.0f, 0.0f);
                secondDoughResult.DOFade(0.0f, 0.0f);
                secondRank.transform.DOScale(0.0f, 0.0f);
                mySequence.Append(secondDoughPercent.DOFade(4.0f, 0.1f).SetDelay(1.0f));
                mySequence.Insert(4.5f, secondDoughResult.DOFade(1.0f, 0.1f));
                mySequence.Insert(5.0f, secondRank.transform.DOScale(1.5f, 0.5f));
                mySequence.Insert(6.0f, secondRank.transform.DOScale(1.0f, 0.1f));

                WaitTimeSet(7.0f);

            }
            else
            {
                baseDoughUI.GetComponent<RectTransform>().localPosition = 
                    new Vector3(0,
                    baseDoughUI.GetComponent<RectTransform>().localPosition.y,
                    baseDoughUI.GetComponent<RectTransform>().localPosition.z);
                WaitTimeSet(3.5f);
            }
            showOK = true;
        }
    }
    void ShowOKButton()
    {
        if(showOK)
        {
            showOK = false;

            OKButton.SetActive(true);
        }
    }
    public void StartButtonPressed()
    {
        isStartDough = true;
        handInfo.gameObject.SetActive(true);

        timeSlider.gameObject.SetActive(true);
        doughText.transform.parent.gameObject.SetActive(true);
        endSign.SetActive(true);
    }

    void EndWork()
    {
        foreach (GameObject smD in smallDoughs)
        {
            smD.SetActive(false);
        }

        handInfo.GetComponent<CircleCollider2D>().radius = 2.0f;
        Color currCol = doughBackground.GetComponentInParent<SpriteRenderer>().color;
        doughBackground.GetComponentInParent<SpriteRenderer>().color
            = new Color(currCol.r, currCol.g, currCol.b, 1.0f);
        currTime = 0.0f;

        SaveDoughInfo(lastDough);
        timeSlider.gameObject.SetActive(false);
        doughText.transform.parent.gameObject.SetActive(false);

        handInfo.CollideCount = 0;
        handInfo.gameObject.SetActive(false);
        endSign.transform.DOLocalMoveX(1200.0f, 0.0f);

        mySequence = DOTween.Sequence();
        mySequence.Append(endSign.transform.DOLocalMoveX(0.0f, 0.5f));
        mySequence.Insert(2.0f, endSign.transform.DOLocalMoveX(-1200.0f, 0.5f));
        WaitTimeSet(2.5f);
    }

    public void ShowStartButton()
    {
        isStartDough = false;
        startButton.SetActive(true);
    }
    [System.Serializable]
    public class Dough
    {
        public float doughScore;
        public float doughPercent;
    }
}
