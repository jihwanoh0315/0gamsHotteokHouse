using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // for Image
using UnityEngine.SceneManagement;

public class IntroCutScene : MonoBehaviour
{
    public Image BackBoard;

    public ScreenInOut screenInOut;
    // Images for cutscene
    [Header("Cutscene Objects")]
    public GameObject BackgroundSky;
    public GameObject RoadTile;
    public GameObject Restaurant;
    public GameObject OMGSign;

    public GameObject ygamKicked;
    public GameObject ygamSit;
    public GameObject ygamSpotlight;


    [Header("Text Bar")]
    public GameObject TextSet;
    public GameObject TextArrow;

    public List<string> SceneComments;

    [SerializeField] TMP_Text txt_Dialogue;
    //Sequence mySequence;

    bool m_goNext = false;
    bool m_playCutScene = true;
    bool m_goNextIntro= false;
    float m_cutTimer = 0.0f;
    int m_currCutScene = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_currCutScene = 0;
        m_playCutScene = false;
        m_goNext = true;

        // CutScene 0
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(screenInOut.HorizCutIn());
        RoadTile.transform.localPosition = new Vector3(30.0f, -800.0f, 0.0f);
        mySequence.Append(RoadTile.transform.DOLocalMove(new Vector3(30.0f, -260.0f, 0.0f), 2.0f)).SetEase(Ease.OutBack);
        Restaurant.transform.localPosition = new Vector3(2000.0f, 60.0f, 0.0f);
        //Restaurant.transform.localScale = new Vector3(1.5f,1.5f, 0.0f);
        mySequence.Insert(0.5f, Restaurant.transform.DOLocalMove(new Vector3(200.0f, 60.0f, 0.0f), 2.0f).SetDelay(.5f)).SetEase(Ease.OutBack);
        mySequence.Insert(0.5f, Restaurant.transform.DOScale(new Vector3(0.8f, 0.8f, 1.0f), 2.0f).SetDelay(.2f)).SetEase(Ease.OutBack);
        txt_Dialogue.text = "";
        StartCoroutine(TextTyping());
        //mySequence.Append(screenInOut.HorizCutOut());
        TextArrow.transform.DOLocalMoveY(-10.0f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetMouseButton(0))
        //     Debug.Log("Curr CutScene = " + m_currCutScene.ToString());
        if (m_goNextIntro)
        {
            m_cutTimer += Time.deltaTime;
            if (m_cutTimer > 2.3f)
            {
                SceneManager.LoadScene("Intro2CutScene");
            }
        }
        else
        {
            if (m_goNext)
            {
                m_cutTimer += Time.deltaTime;
                if (m_cutTimer > 1.0f)
                {
                    if (Input.GetMouseButton(0))
                    {
                        m_playCutScene = true;
                        m_goNext = false;
                        ++m_currCutScene;
                    }
                }
            }
        }

        if (m_playCutScene)
        {
            m_playCutScene = false;
            Sequence mySequence = DOTween.Sequence();
            txt_Dialogue.text = "";

            switch (m_currCutScene)
            {
                case 1:
                    OMGSign.SetActive(true);
                    mySequence.Insert(0.0f, OMGSign.transform.DOShakeScale(0.5f));
                    break;
                case 2:
                    mySequence.Insert(0.5f, Restaurant.transform.DOLocalMove(new Vector3(450.0f, 60.0f, 0.0f), .5f)).SetEase(Ease.InBack);
                    mySequence.Insert(0.5f, RoadTile.transform.DOLocalMove(new Vector3(250.0f, -260.0f, 0.0f), .5f)).SetEase(Ease.InBack);
                    OMGSign.SetActive(false);
                    break;
                case 3:
                    ygamKicked.SetActive(true);
                    ygamKicked.transform.localPosition = new Vector3(260.0f, -160.0f, 0.0f);
                    ygamKicked.transform.localScale = new Vector3(0.1f, 0.1f, 0.0f);
                    m_cutTimer = 0.0f;

                    mySequence.Insert(0.0f, ygamKicked.transform.DOLocalMove(new Vector3(-300.0f, -230.0f, 0.0f), .5f));
                    mySequence.Insert(0.0f, ygamKicked.transform.DORotate(new Vector3(0.0f, 0.0f, 360.0f), 0.5f, RotateMode.FastBeyond360)).SetEase(Ease.Linear);
                    mySequence.Insert(0.0f, ygamKicked.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), .5f));
                    break;
                case 4:
                    mySequence.Insert(0.0f, ygamKicked.GetComponent<Image>().DOFade(0, 0.5f));
                    mySequence.Append(BackBoard.DOFade(1, 0.5f));
                    ygamSpotlight.SetActive(true);
                    mySequence.Insert(0.0f, ygamSpotlight.GetComponent<Image>().DOFade(1, 0.5f)).SetDelay(0.5f);
                    break;
                case 5:
                    ygamSit.SetActive(true);
                    mySequence.Insert(0.0f, ygamSit.transform.DOLocalMoveY(43.0f, 1.0f)).SetEase(Ease.OutBounce) ;
                    //mySequence.Insert(0.0f, ygamKicked.transform.DORotate(new Vector3(0.0f, 0.0f, 360.0f), 0.5f, RotateMode.FastBeyond360)).SetEase(Ease.Linear);

                    break;
                default:
                    //mySequence.Append(screenInOut.HorizCutOut());
                    mySequence.Append(screenInOut.EntireFadeOut(2.0f));
                    m_cutTimer = 0.0f;
                    m_goNextIntro = true;
                    break;
            }

            if(m_currCutScene < SceneComments.Count)
            {
                if (SceneComments[m_currCutScene] != "")
                {
                    TextSet.SetActive(true);
                    TextArrow.SetActive(false);
                    StartCoroutine(TextTyping());
                }
                else
                {
                    TextSet.SetActive(false);
                    m_goNext = true;
                    m_cutTimer = 0.0f;
                }
            }
            else
            {
                TextSet.SetActive(false);
                m_goNext = true;
                m_cutTimer = 0.0f;
            }
        }




    }

    IEnumerator TextTyping()
    {
        string t_ReplaceText = SceneComments[m_currCutScene];
        for (int i = 0; i < t_ReplaceText.Length; i++)
        {
            txt_Dialogue.text += t_ReplaceText[i];
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    txt_Dialogue.text = t_ReplaceText;
            //    break;
            //}
            yield return new WaitForSeconds(0.05f);
        }

        if (txt_Dialogue.text.Length == t_ReplaceText.Length)
        {
            yield return new WaitForSeconds(1.0f);
            TextArrow.SetActive(true);
            m_goNext = true;
        }

        yield return null;
    }

}
