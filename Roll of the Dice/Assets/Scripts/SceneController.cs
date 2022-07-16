using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    public void LoadSceneWithIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void GoToNextScene()
    {
        int currIndex = SceneManager.GetActiveScene().buildIndex;

        if (currIndex > SceneManager.sceneCountInBuildSettings -1)    
            SceneManager.LoadScene(currIndex + 1);
        else
        {
            Debug.LogError("Cannot go to next scene when on the last scene!");
        }
    }
}
