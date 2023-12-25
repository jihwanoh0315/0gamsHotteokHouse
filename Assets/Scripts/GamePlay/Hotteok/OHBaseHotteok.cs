using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OHBaseHotteok : OnHand
{
    public int currStep = 0;
    public List<Sprite> steps;
    SpriteRenderer SpriteRenderer;

    protected bool isFolding = false;

    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(currStep == 1)
        {
            if (!isFolding)
                StartFoldingGame();

        }
    }

    public void OnClick(ForHand otherObj)
    {
        switch (currStep)
        {
            case 0:
                if(otherObj.name == "SugarContainer")
                {
                    ++currStep;
                    SpriteRenderer.sprite = steps[currStep];
                }
                break;
            case 1:

                break;
            case 2:

                break;
            case 3:

                break;
        }
    }

    public void StartFoldingGame()
    {

    }
    public void PlayFoldingGame()
    {

    }
}

