using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public GameObject prevButton;
    public GameObject nextButton;
    public GameObject closeButton;

    public TMP_Text t_tutName;
    [Header("Images")]
    public Image currImage;
    public List<Sprite> imageList;
    [Header("Messages")]
    public List<string> messageList;
    public TMP_Text t_HelperMessage;


    public TMP_Text t_pageNum;
    public TMP_Text t_totalPageNum;

    int currImageNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetBaseUI();
    }

    void SetBaseUI()
    {
        currImageNum = 0;
        t_pageNum.text = "1";
        t_HelperMessage.text = messageList[currImageNum];
        t_totalPageNum.text = imageList.Count.ToString();
        currImage.sprite = imageList[currImageNum];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextPage()
    {
        if (currImageNum < imageList.Count - 1)
        {
            currImageNum++;
            currImage.sprite = imageList[currImageNum];
            t_HelperMessage.text = messageList[currImageNum];

            t_pageNum.text = (currImageNum+1).ToString();
        }
        prevButton.SetActive(true);
        if(currImageNum >= imageList.Count - 1)
        {
            nextButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true);
        }
    }
    public void PrevPage()
    {
        if (currImageNum > 0)
        {
            currImageNum--;
            currImage.sprite = imageList[currImageNum];
            t_HelperMessage.text = messageList[currImageNum];

            t_pageNum.text = (currImageNum+1).ToString();
        }
        nextButton.SetActive(true);
        closeButton.gameObject.SetActive(false);
        if (currImageNum <= 0)
        {
            prevButton.gameObject.SetActive(false);
        }
    }
}
