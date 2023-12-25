using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEmoji : MonoBehaviour
{
    public GameObject m_goEmote;
    public SpriteRenderer m_sprEmoji;
    //private DialogueManager theDM;
    private EmoteManager theEM;
    //[SerializeField] Sprite[] spr_emoteList;
    public enum Emoji
    {
        Smile = 0,
        Sad,
        Angry,
        Happy,
        Exclamation,
        Ellipsis,




        Other
    }
    // Start is called before the first frame update
    void Start()
    {
        theEM = FindObjectOfType<EmoteManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEmoji(Emoji emoji, bool isFlip = false)
    {
        if(isFlip)
        {
            m_goEmote.transform.localPosition = new Vector3(-0.4f, 1.5f, -2f);
            m_goEmote.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            m_goEmote.transform.localPosition = new Vector3(0.4f, 1.5f, -2f);
            m_goEmote.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        m_goEmote.SetActive(true);
        if (emoji != Emoji.Other)
        {
            m_sprEmoji.sprite = theEM.emoteDic[1000 + (int)emoji];
        }

        switch (emoji)
        {
            case Emoji.Smile:
                m_goEmote.transform.DOLocalJump(m_goEmote.transform.localPosition, 0.2f, 2, 0.4f).SetEase(Ease.OutCirc);
                break;
            case Emoji.Sad:
                m_goEmote.transform.DOLocalJump(m_goEmote.transform.localPosition, -0.2f, 1, 0.2f);
                break;
            case Emoji.Angry:
                m_goEmote.transform.DOLocalJump(m_goEmote.transform.localPosition, -0.2f, 1, 0.2f);
                break;
            case Emoji.Happy:
                m_goEmote.transform.DOLocalJump(m_goEmote.transform.localPosition, 0.2f, 2, 0.8f);
                break;
            case Emoji.Exclamation:
                m_goEmote.transform.DOLocalJump(m_goEmote.transform.localPosition, 0.2f, 1, 0.2f);
                break;
            case Emoji.Ellipsis:
                m_goEmote.transform.DOLocalJump(m_goEmote.transform.localPosition, 0.2f, 1, 0.2f);
                break;
            default:
                m_goEmote.transform.DOLocalJump(m_goEmote.transform.localPosition, 0.2f, 1, 0.2f);
                break;
        }

    }
    public void ActiveEmoji(bool active)
    {
        if(active)
        {
            m_goEmote.SetActive(true);
        }
        else
        {
            m_goEmote.SetActive(false);
        }
    }
}
