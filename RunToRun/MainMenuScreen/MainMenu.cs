using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject AuthorsPanel;
    public void Quit()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        LevelLoader.LoadFirstLevel();
        WaitTillGameLoaded();
    }
    
    public void WaitTillGameLoaded()
    {
        while (! LevelLoader.IsLoaded(1))
        {
            Time.timeScale = 0;
        }

        Time.timeScale = 1;
    }

    public void ShowAutors()
    {
        MainMenuPanel.SetActive(false);
        AuthorsPanel.SetActive(true);
    }

  

    public void BackToMainMenu()
    {
        AuthorsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

}
