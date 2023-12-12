using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public LevelSetting levelSetting = null;
    public List<GameObject> TitleUI = new List<GameObject>();

    public GameObject InitialMenuSet = null;
    public GameObject MainMenuUI = null;

    public float delay = 3.0f;
    private float currTime = 0.0f;

    bool clicked = false;

    // Start is called before the first frame update
    void Start()
    {
        currTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (levelSetting == null)
            return;

        currTime += Time.deltaTime;

        if (currTime < delay)
        {
            return;
        }

        if (Input.anyKeyDown && !clicked)
        {
            clicked = true;

            foreach (GameObject go in TitleUI)
            {
                if (go != null)
                {
                    RectTransform rectTransform = go.GetComponent<RectTransform>();
                    rectTransform.DOAnchorPosY(rectTransform.localPosition.y + 100, .5f, true);
                    go.transform.DOScale(0.75f, 0.5f);
                }
            }

            if(InitialMenuSet != null)
            {
                InitialMenuSet.SetActive(false);
            }

            if(MainMenuUI != null)
            {
                MainMenuUI.SetActive(true);
                RectTransform rectTransform = MainMenuUI.GetComponent<RectTransform>();
                rectTransform.DOAnchorPosY(300, 0.5f, true);
            }

            //if (!levelSetting.m_gameData.isWorking && levelSetting.m_gameData.currMission == 0)
            //{
            //    SceneManager.LoadScene("GameStage");
            //}

        }


    }
}
