using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private int enemiesOnScene;
    public static LevelController Instance { get; set; }
    public GameObject WinMenu;
    private void Awake()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        Debug.Log(enemies.Length);
        Instance = this;

    }
    public void EnemiesCount()
    {
        // GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        // enemiesOnScene = enemies.Length;
        // Debug.Log("enemies " + enemiesOnScene);

        // if (enemiesOnScene == 0)
        // {
        //     //Hero.Instance.Invoke("SetWinPanel", 1.1f);

        //     Debug.Log("WIN WIN");
        //     WinMenu.SetActive(true);
        // }

        if (GameObject.FindGameObjectsWithTag("enemy").Length == 0)
            WinMenu.SetActive(true);


    }


}
