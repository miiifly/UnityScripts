using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    
    public static Scene scene;

    void Start()
    {
        scene = SceneManager.GetActiveScene();
    }
    
   public static void PlayAgain()
    {
        SceneManager.LoadScene(scene.name);
        MoneyText.SetMoney(0);
    }


   public void LoadNextLevel()
    {
        SceneManager.LoadScene(scene.buildIndex + 1);
       
    }

    public static void LoadFirstLevel()
    {
        SceneManager.LoadScene((1));
    }

    public static void PauseGame()
    {
        Time.timeScale = 0f;
    }
    
    public static void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public static void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public static bool IsLoaded(int SceneNumber)
    {


        IEnumerator LoadSceneAdditive(int bulidIndex)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(SceneNumber, LoadSceneMode.Additive);

            while (!operation.isDone)
            {
                yield return false;
            }

            

        }
        return true;
    }
    
   
}
