using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro2CutScene : MonoBehaviour
{
    [SerializeField] ScreenInOut screenInOut;
    [SerializeField] Animator gamAnimator;
    [SerializeField] GameObject gamChar;
    [SerializeField] GameObject houChar;
    [SerializeField] GameObject buildDust;
    [SerializeField] GameObject building;



    [SerializeField] GameObject mainCamera;


    DialogueManager theDM;
    [SerializeField] InteractionEvent eventForTest;


    bool m_playCutScene = false;
    bool m_firstMoving = true;
    bool m_goNextCut = false;
    bool m_goNextIntro = false;
    bool m_dialogSection = false;

    int m_currCutScene = 0;

    float m_cutTimer = 0.0f;
    float m_timeLimit = 3.0f;
    //float m_walkingTime = 30.0f;
    float m_walkingTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        screenInOut.EntireFadeIn(1.0f);
        //screenInOut.HorizOpen();

        theDM = FindObjectOfType<DialogueManager>();

        if (gamAnimator == null)
            Debug.Log("No animator in Intro2");

        gamAnimator.SetBool("isWalking", true);
        gamAnimator.speed = 0.6f;
        gamChar.transform.DOMoveX(-3.95f, m_walkingTime).SetEase(Ease.Linear);
        mainCamera.transform.DOMoveX(-3.95f, m_walkingTime).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_firstMoving)
        {
            if (gamChar.transform.position.x >= -4.0f)
            {
                m_firstMoving = false;
                m_playCutScene = true;
                gamAnimator.SetBool("isWalking", false);
                gamAnimator.speed = 1.0f;
            }
            return;
        }

        if(m_goNextCut)
        {
            m_cutTimer += Time.deltaTime;
            if(m_cutTimer > m_timeLimit) 
            {
                m_goNextCut = false;
                m_playCutScene = true;
                m_cutTimer = 0.0f;
                m_currCutScene++;
            }
        }
        
        if(m_dialogSection)
        {
            if(!theDM.isDialogue)
            {
                m_dialogSection = false;
                GoNextCut(1.0f);
            }
        }

        if(m_goNextIntro)
        {
            m_cutTimer += Time.deltaTime;
            if (m_cutTimer > m_timeLimit)
            {
                SceneManager.LoadScene("FirstLobby");
            }
        }


        if (m_playCutScene)
        {
            m_playCutScene = false;
            Sequence mySequence = DOTween.Sequence();
            switch (m_currCutScene)
            {
                case 0:
                    screenInOut.HorizCutIn(1.0f);
                    m_dialogSection = true;
                    theDM.ShowDialogue(eventForTest.GetDialogueWithLines(0, 1, 1));
                    break;
                case 1:
                    mainCamera.transform.DOMoveX(0.0f, 2.0f).SetEase(Ease.Linear).SetDelay(1.0f);
                    gamChar.GetComponent<CharacterEmoji>().SetEmoji(CharacterEmoji.Emoji.Exclamation);

                    GoNextCut(2.5f);
                    break;
                case 2:
                    gamChar.GetComponent<CharacterEmoji>().ActiveEmoji(false);

                    GoNextCut(1.5f);
                    break;
                case 3:
                    m_dialogSection = true;
                    theDM.ShowDialogue(eventForTest.GetDialogueWithLines(0, 2, 7));
                    break;
                case 4:
                    m_dialogSection = true;
                    theDM.ShowDialogue(eventForTest.GetDialogueWithLines(1, 1, 6));
                    break;
                case 5:
                    mySequence.Append( houChar.transform.DOScaleX(0.8f, 0.0f));
                    mySequence.Insert(0.5f, houChar.GetComponent<SpriteRenderer>().DOFade(0.0f, 0.0f));
                    mySequence.Insert(0.5f, houChar.GetComponent<SpriteRenderer>().DOFade(0.0f, 0.0f));
                    mySequence.Insert(0.5f, buildDust.GetComponent<SpriteRenderer>().DOFade(1.0f, 0.0f));
                    
                    mySequence.Insert(2.5f, buildDust.GetComponent<SpriteRenderer>().DOFade(0.0f, 0.0f));
                    mySequence.Insert(2.5f, houChar.GetComponent<SpriteRenderer>().DOFade(1.0f, 0.0f));
                    mySequence.Insert(2.5f, building.GetComponent<SpriteRenderer>().DOFade(1.0f, 0.0f));

                    mySequence.Insert(2.75f, houChar.transform.DOScaleX(-0.8f, 0.0f));
                    GoNextCut(3.5f);
                    break;
                case 6:
                    m_dialogSection = true;
                    theDM.ShowDialogue(eventForTest.GetDialogueWithLines(2, 1, 3));
                    break;
                default:
                    //mySequence.Append(screenInOut.VertCutOut());
                    mySequence.Append(screenInOut.HorizClose(2.0f).SetEase(Ease.OutBounce));
                    m_timeLimit = 3.0f;
                    m_cutTimer = 0.0f;
                    m_goNextIntro = true;
                    break;
            }

        }

    }
    void GoNextCut(float time_)
    {
        m_goNextCut = true;
        m_cutTimer = 0.0f;
        m_timeLimit = time_;
    }
}
