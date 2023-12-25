using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject go_DialogueBar;
    [SerializeField] GameObject go_DialogueNameBar;

    [SerializeField] GameObject go_PortraitRight;
    [SerializeField] GameObject go_PortraitLeft;
    [SerializeField] GameObject go_Arrow;

    [SerializeField] TMP_Text txt_Dialogue;
    [SerializeField] TMP_Text txt_Name;

    [SerializeField] Sprite[] spr_portraitList;
    [SerializeField] Dictionary<int, Sprite> portraitDic;

    Dialogue[] m_dialogues;
    LevelSetting m_levelSetting;

    public bool isDialogue = false; // 대화중이면 true.
    bool isNext = false; // 특정키 입력 대기.

    [Header("텍스트 출력 딜레이")]
    [SerializeField] float textDelay;
    [Header("텍스트 출력 속도(글자당 시간)")]
    [SerializeField] float textTerm;

    int lineCount = 0; // 대화카운트
    int contextCount = 0; // 대사카운트
    float currTime = 0.0f;
    private void Start()
    {
        m_levelSetting = FindObjectOfType<LevelSetting>();
        SettingUI(false);
        go_Arrow.transform.DOLocalMoveY(70.0f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        go_Arrow.SetActive(false);

        portraitDic = new Dictionary<int, Sprite>();
        // 영감 초상화
        for (int i = 0; i < 5; i++)
        {
            portraitDic.Add(100 + i, spr_portraitList[i]);
        }

        // 망야 초상화
        for(int i = 0; i < 5; i++)
        {
            portraitDic.Add(200 + i, spr_portraitList[15+i]);
        }

        // 호우 초상화
        portraitDic.Add(10000 + 0, spr_portraitList[5]);
        portraitDic.Add(10000 + 1, spr_portraitList[6]);
        portraitDic.Add(10000 + 2, spr_portraitList[7]);
        portraitDic.Add(10000 + 3, spr_portraitList[8]);
        portraitDic.Add(10000 + 4, spr_portraitList[9]);

        // ??? 초상화
        portraitDic.Add(11000 + 0, spr_portraitList[10]);
        portraitDic.Add(11000 + 1, spr_portraitList[11]);
        portraitDic.Add(11000 + 2, spr_portraitList[12]);
        portraitDic.Add(11000 + 3, spr_portraitList[13]);
        portraitDic.Add(11000 + 4, spr_portraitList[14]);


    }

    private void Update()
    {
        if(isDialogue)
        {
            if(isNext)
            {
                currTime += Time.deltaTime;
                if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    if (currTime < textDelay)
                        return;
                    currTime = 0.0f;

                    isNext = false;
                    txt_Dialogue.text = "";
                    ++contextCount;

                    if (contextCount < m_dialogues[lineCount].contexts.Length)
                    {
                        go_Arrow.SetActive(false);
                        StartCoroutine(TypeWriter());
                        
                    }
                    else 
                    {
                        ++lineCount;

                        contextCount = 0;
                        if (lineCount < m_dialogues.Length)
                        {
                            go_Arrow.SetActive(false);
                            SettingCurrentDialogue();
                            StartCoroutine(TypeWriter());
                        }
                        else
                        {
                            EndDialogue();
                        }
                    }
                }
            }
        }
    }

    public void ShowDialogue(Dialogue[] dialogues_)
    {
        m_levelSetting.canMovePlayer = false;
        isDialogue = true;
        txt_Dialogue.text = "";
        txt_Name.text = "";

        m_dialogues = dialogues_;

        SettingCurrentDialogue();
        StartCoroutine(TypeWriter());
    }
    void EndDialogue()
    {
        m_levelSetting.canMovePlayer = true;
        isDialogue = false;
        contextCount = 0;
        lineCount = 0;
        m_dialogues = null; 
        isNext = false;
        SettingUI(false);
    }

    IEnumerator TypeWriter()
    {
        SettingUI(true);
        string t_ReplaceText = m_dialogues[lineCount].contexts[contextCount];
        t_ReplaceText = t_ReplaceText.Replace("`", ",");

        txt_Name.text = m_dialogues[lineCount].name;
        for(int i = 0; i < t_ReplaceText.Length; i++)
        {
            txt_Dialogue.text += t_ReplaceText[i];
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    txt_Dialogue.text = t_ReplaceText;
            //    break;
            //}
            yield return new WaitForSeconds(textTerm);
        }

        if(txt_Dialogue.text.Length == t_ReplaceText.Length)
        {
            isNext = true;
            go_Arrow.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }

    void SettingUI(bool p_flag)
    {
        go_DialogueBar.SetActive(p_flag);
        go_DialogueNameBar.SetActive(p_flag);
    }

    void SettingCurrentDialogue()
    {
        Dialogue currDlg = m_dialogues[lineCount];
        // Portrait Direction
        if (currDlg.isLeft)
        {
            go_PortraitLeft.SetActive(true);
            go_PortraitRight.SetActive(false);
            // Portrait Face
            go_PortraitLeft.GetComponent<Image>().sprite = portraitDic[currDlg.ID + currDlg.portrait];

        }
        else
        {
            go_PortraitLeft.SetActive(false);
            go_PortraitRight.SetActive(true);
            // Portrait Face
            go_PortraitRight.GetComponent<Image>().sprite = portraitDic[currDlg.ID + currDlg.portrait];
        }

        if (textDelay != 0.0f)
            textDelay = currDlg.delay;
        else
            textDelay = 0.5f;

        if(textTerm != 0.0f)
            textTerm = currDlg.speechSpeed;
        else
            textTerm = 0.02f;

    }
}
