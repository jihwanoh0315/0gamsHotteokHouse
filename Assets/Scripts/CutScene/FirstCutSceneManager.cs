using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstCutSceneManager : CutSceneBase
{
    [SerializeField] GameObject movArrow;

    bool needToGo = false;
    float moveMaxTime = 60.0f;
    float moveTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        screenInOut.DiagonalCutOut();
        theDM = FindObjectOfType<DialogueManager>();
        m_goNextCut = true;
    }

    // Update is called once per frame
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


        //if (m_goNextScene)
        //{
        //    m_cutTimer += Time.deltaTime;
        //    if (m_cutTimer > m_timeLimit)
        //    {
        //        //SceneManager.LoadScene("FirstLobby");
        //    }
        //}

        if(needToGo)
        {
            if(moveTimer > moveMaxTime)
            {
                moveTimer = 0.0f;
                theDM.ShowDialogue(eventForTest.GetDialogueWithLines(1, 1, 2));
            }
            if (!theDM.isDialogue)
            {
                moveTimer += Time.deltaTime;
            }
        }

        if (m_playCutScene)
        {
            m_playCutScene = false;
            Sequence mySequence = DOTween.Sequence();
            switch (m_currCutScene)
            {
                case 1:
                    m_dialogSection = true;
                    movArrow.SetActive(false);
                    theDM.ShowDialogue(eventForTest.GetDialogueWithLines(0, 1, 2));
                    break;
                case 2:
                    needToGo = true;
                    m_goNextCut = false;
                    GoNextCut(0.0f);
                    break;
                default:
                    break;
            }

        }
    }
}
