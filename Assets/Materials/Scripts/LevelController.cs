using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; set; }
    public GameObject WinMenu;
    public Text enemiesCountText;
    private string _enemiesCountTextStart = "Enemies left: ";
    private void Awake()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        Debug.Log(enemies.Length);
        Instance = this;
        enemiesCountText.text = _enemiesCountTextStart + enemies.Length.ToString();

    }
    public void EnemiesCount()
    {
        int enemiesOnScene = GameObject.FindGameObjectsWithTag("enemy").Length;

        enemiesCountText.text = _enemiesCountTextStart + enemiesOnScene.ToString();

        if (enemiesOnScene == 0)
        {
            Debug.Log("WIN WIN");
            WinMenu.SetActive(true);
        }

        //if (GameObject.FindGameObjectsWithTag("enemy").Length == 0)
        //    WinMenu.SetActive(true);


    }


}
