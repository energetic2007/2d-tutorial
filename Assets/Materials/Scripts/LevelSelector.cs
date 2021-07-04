using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelSelector : MonoBehaviour
{
    public Button[] levels;

    private void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < levels.Length; i++)
        {
            if (i > levelReached - 1) levels[i].interactable = false;
        }
    }

    public void Select(int numberInBuild)
    {
        int currentLevel = numberInBuild - 1;

        int levelReached = PlayerPrefs.GetInt("levelReached", 1);
        if (levelReached < currentLevel) PlayerPrefs.SetInt("levelReached", currentLevel);
        SceneManager.LoadScene(numberInBuild);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
