using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject GameEntitiesUI;
    // Update is called once per frame
    void Update()
    {

        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }

            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        GameEntitiesUI.SetActive(true);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        GameEntitiesUI.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("LoadMenu");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }

    public void PauseMenuForAndroid()
    {
        if (Application.isMobilePlatform)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (Input.touchCount == 1)
                {
                    {
                        if (GameIsPaused)
                        {
                            Resume();
                        }
                        else
                        {
                            Pause();
                        }
                    }
                }
            }
        }
    }
}
