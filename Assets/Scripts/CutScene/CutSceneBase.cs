using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneBase : MonoBehaviour
{
    [SerializeField] protected ScreenInOut screenInOut;
    [SerializeField] protected GameObject gamChar;
    [SerializeField] protected Animator gamAnimator;

    [SerializeField] protected GameObject mainCamera;


    protected DialogueManager theDM;
    [SerializeField] protected InteractionEvent eventForTest;

    public bool m_playCutScene = false;
    protected bool m_goNextCut = false;
    protected bool m_goNextScene = false;
    protected bool m_dialogSection = false;

    protected int m_currCutScene = 0;

    protected float m_cutTimer = 0.0f;
    protected float m_timeLimit = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        screenInOut.HorizOpen();
        theDM = FindObjectOfType<DialogueManager>();
    }
    protected void GoNextCut(float time_)
    {
        m_goNextCut = true;
        m_cutTimer = 0.0f;
        m_timeLimit = time_;
    }
}
