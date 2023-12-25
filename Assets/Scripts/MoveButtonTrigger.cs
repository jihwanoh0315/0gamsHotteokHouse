using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveButtonTrigger : MonoBehaviour
{
    public GameObject moveButton;
    public GameObject moveButtonSpace;
    public GameObject gamChar;
    public GameObject selectOne;
    public GameObject supOne;
    public GameObject selectTwo;
    public GameObject supTwo;


    public bool isCollide;
    private bool isMoveOn;
    private bool isSelectOn;
    private int currButton;
    // Start is called before the first frame update
    void Start()
    {
        moveButton.GetComponent<CanvasRenderer>().SetAlpha(0.5f);
        currButton = 0;
        isCollide = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isCollide)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                switch (currButton)
                {
                    case 1:
                        selectOne.SetActive(true);
                        moveButton.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
                        moveButtonSpace.SetActive(false);
                        supOne.GetComponent<Button>().Select();
                        currButton = 3;
                        break;
                    case 2:
                        selectTwo.SetActive(true);
                        moveButton.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
                        moveButtonSpace.SetActive(false);
                        supTwo.GetComponent<Button>().Select();
                        currButton = 3;
                        break;
                    default:
                        break;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TurnOffMoveUI();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {

            if(collision.gameObject.tag == "Player")
            {
                moveButton.GetComponent<CanvasRenderer>().SetAlpha(0.5f);
                moveButton.SetActive(true);
                moveButtonSpace.SetActive(true);
                currButton = 1;
                isCollide = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if(collision.gameObject.tag == "Player")
            {
                TurnOffMoveUI();
            }
        }
    }

    private void TurnOffMoveUI()
    {
        moveButton?.SetActive(false);
        selectOne?.SetActive(false);
        selectTwo?.SetActive(false);

        currButton = 0;
        isCollide = false;
    }
}
