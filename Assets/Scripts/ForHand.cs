using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForHand : MonoBehaviour
{
    public int reactNum = 0;
    public int holdingObject = 0;
    public int dough = 0;
    public int currStep = 0;
    public Animator handAnimator;

    SpriteRenderer spriteRenderer;
    Sprite originalSprite;
    [SerializeField] Sprite hoverSprite;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
    }

    public void MouseOn(bool isOn_)
    {
        if(isOn_)
        {
            spriteRenderer.sprite = hoverSprite;
        }
        else
        {
            spriteRenderer.sprite = originalSprite;
        }
    }
}
