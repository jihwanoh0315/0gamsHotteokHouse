using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEmoji : MonoBehaviour
{
    public GameObject m_goEmoji;
    public SpriteRenderer m_sprEmoji;
    private DialogueManager theDM;
    //[SerializeField] Sprite[] spr_emoteList;
    public enum Emoji
    {
        Smile = 0,
        Sad,
        Angry,
        Happy,
        Exclamation,
        Ellipsis
    }
    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEmoji(Emoji emoji, bool isFlip = false)
    {
        if(isFlip)
        {
            m_goEmoji.transform.localPosition = new Vector3(-0.4f, 1.5f, -2f);
            m_goEmoji.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            m_goEmoji.transform.localPosition = new Vector3(0.4f, 1.5f, -2f);
            m_goEmoji.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        m_goEmoji.SetActive(true);
        m_sprEmoji.sprite = theDM.spr_emoteList[(int)emoji];

        switch (emoji)
        {
            case Emoji.Smile:
                m_goEmoji.transform.DOLocalJump(m_goEmoji.transform.localPosition, 0.2f, 2, 0.4f).SetEase(Ease.OutCirc);
                break;
            case Emoji.Sad:
                m_goEmoji.transform.DOLocalJump(m_goEmoji.transform.localPosition, -0.2f, 1, 0.2f);
                break;
            case Emoji.Angry:
                m_goEmoji.transform.DOLocalJump(m_goEmoji.transform.localPosition, -0.2f, 1, 0.2f);
                break;
            case Emoji.Happy:
                m_goEmoji.transform.DOLocalJump(m_goEmoji.transform.localPosition, 0.2f, 2, 0.8f);
                break;
            case Emoji.Exclamation:
                m_goEmoji.transform.DOLocalJump(m_goEmoji.transform.localPosition, 0.2f, 1, 0.2f);
                break;
            case Emoji.Ellipsis:
                m_goEmoji.transform.DOLocalJump(m_goEmoji.transform.localPosition, 0.2f, 1, 0.2f);
                break;
            default:
                break;
        }

    }
    public void ActiveEmoji(bool active)
    {
        if(active)
        {
            m_goEmoji.SetActive(true);
        }
        else
        {
            m_goEmoji.SetActive(false);
        }
    }
}
