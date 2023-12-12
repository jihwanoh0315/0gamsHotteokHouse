using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenChanger : MonoBehaviour
{
    static public ScreenChanger instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu"))
            {
                SceneManager.LoadScene("GamePlay");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("GamePlay1"))
            {
                SceneManager.LoadScene("GamePlay1");
            }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("GamePlay2"))
            {
                SceneManager.LoadScene("GamePlay2");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("GameStage"))
            {
                SceneManager.LoadScene("GameStage");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu"))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MovementTestStage"))
            {
                SceneManager.LoadScene("MovementTestStage");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("SampleScene"))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("TeamLogo"))
            {
                SceneManager.LoadScene("TeamLogo");
            }
        }
    }
}
