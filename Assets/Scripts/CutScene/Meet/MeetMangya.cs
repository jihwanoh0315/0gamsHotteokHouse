using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeetMangya : CutSceneBase
{
    [SerializeField] GameObject mangyaChar;
    [SerializeField] Animator mangyaAnimator;
    [SerializeField] List<GameObject> toHide;
    [SerializeField] GameObject CutBackground;
    [SerializeField] GameObject CutImage;
    [SerializeField] GameObject CutText;


    // Start is called before the first frame update
    void Start()
    {
        screenInOut.DiagonalCutOut();
        theDM = FindObjectOfType<DialogueManager>();
        m_timeLimit = 1.0f;
        m_goNextCut = true;
        mangyaChar.transform.localPosition = new Vector3(-7.3f, -2.6f, 0.0f);
        mangyaChar.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        gamChar.GetComponent<PlayerController>().IsMovable(false);
    }

    void Update()
    {
        if (m_goNextCut)
        {
            m_cutTimer += Time.deltaTime;
            if (m_cutTimer > m_timeLimit)
            {
                m_goNextCut = false;
                m_playCutScene = true;
                m_cutTimer = 0.0f;
                m_currCutScene++;
            }
        }

        if (m_dialogSection)
        {
            if (!theDM.isDialogue)
            {
                m_dialogSection = false;
                GoNextCut(1.0f);
            }
        }

        if (m_goNextScene)
        {
            m_cutTimer += Time.deltaTime;
            if (m_cutTimer > m_timeLimit)
            {
                //SceneManager.LoadScene("FirstLobby");
            }
        }



        //////////////////////////////////////
        ///
        ///  NEW CODE SECTION
        /// 
        /// //////////////////////////////////
        ///









        if (m_playCutScene)
        {
            m_playCutScene = false;
            Sequence mySequence = DOTween.Sequence();
            //////////////////////////////////////
            ///
            ///  NEW CODE SECTION
            /// 
            /// //////////////////////////////////
            ///

            switch (m_currCutScene)
            {
                case 1:
                    gamAnimator.SetBool("isWalking", false);
                    mySequence.Append(mangyaChar.transform.DOLocalMoveX(-7.4f, 0.2f).SetLoops(4, LoopType.Yoyo).SetEase(Ease.Linear));
                    mySequence.Insert(1.5f, mangyaChar.transform.DOLocalMoveX(-7.4f, 0.2f).SetLoops(4, LoopType.Yoyo));
                    GoNextCut(3.5f);
                    break;
                case 2:
                    mangyaChar.GetComponent<CharacterEmoji>().SetEmoji(CharacterEmoji.Emoji.Exclamation, true);
                    GoNextCut(1.0f);
                    break;
                case 3:
                    mangyaChar.GetComponent<CharacterEmoji>().ActiveEmoji(false);
                    mySequence.Append(mangyaChar.transform.DOScaleX(1.0f, 0.0f));
                    GoNextCut(1.5f);
                    break;
                case 4:
                    GoNextCut(0.5f);
                    break;
                case 5:
                    mangyaChar.GetComponent<CharacterEmoji>().ActiveEmoji(false);
                    mangyaAnimator.SetBool("isWalking", true);
                    mySequence.Append(mangyaChar.transform.DOLocalMoveX(-5.5f, 1.0f).SetEase(Ease.Linear));
                    GoNextCut(1.0f);
                    break;
                case 6:
                    mangyaAnimator.SetBool("isWalking", false);
                    GoNextCut(0.1f);
                    break;
                case 7:
                    foreach (GameObject go in toHide)
                    {
                        go.SetActive(false);
                    }
                    CutImage.SetActive(true);
                    CutBackground.SetActive(true);
                    CutText.SetActive(true);
                    mainCamera.GetComponent<Camera>().DOOrthoSize(4, 0.1f);
                    mainCamera.transform.DOMove(new Vector3(-1.5f, 0.5f, -10.0f), 0.1f);

                    CutImage.transform.DOLocalMoveX(-1.0f, .5f);
                    CutText.transform.DOLocalMoveX(8.0f, .5f);
                    GoNextCut(2.0f);
                    break;
                case 8:
                    CutImage.transform.DOLocalMoveX(20.0f, .25f);
                    CutText.transform.DOLocalMoveX(-16.0f, .25f);
                    GoNextCut(.25f);
                    break;
                case 9:
                    foreach (GameObject go in toHide)
                    {
                        go.SetActive(true);
                    }

                    mainCamera.GetComponent<Camera>().DOOrthoSize(5, 0.1f);
                    mainCamera.transform.DOMove(new Vector3(0.0f, 0.0f, -10.0f), 0.1f);
                    CutImage.SetActive(false);
                    CutBackground.SetActive(false);
                    CutText.SetActive(false);
                    GoNextCut(0.5f);
                    break;
                case 10:
                    m_dialogSection = true;
                    theDM.ShowDialogue(eventForTest.GetDialogueWithLines(0, 1, 6));
                    break;
                case 11:
                    mangyaChar.GetComponent<CharacterEmoji>().SetEmoji(CharacterEmoji.Emoji.Ellipsis);
                    GoNextCut(3.0f);
                    break;
                case 12:
                    mangyaChar.GetComponent<CharacterEmoji>().ActiveEmoji(false);
                    GoNextCut(0.1f);
                    break;
                case 13:
                    m_dialogSection = true;
                    theDM.ShowDialogue(eventForTest.GetDialogueWithLines(1, 1, 4));
                    break;

                default:
                    //mySequence.Append(screenInOut.VertCutOut());
                    gamChar.transform.DOScaleX(-1.0f, 0.0f);
                    mySequence.Append(screenInOut.DiagonalCutOut(2.0f).SetEase(Ease.OutBounce));
                    m_timeLimit = 3.0f;
                    m_cutTimer = 0.0f;
                    m_goNextScene = true;
                    break;
            }

        }
    }

}
