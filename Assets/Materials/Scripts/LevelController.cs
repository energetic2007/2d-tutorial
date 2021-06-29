using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private int enemiesOnScene;
    public static LevelController Instance { get; set; }
    private void Awake()
    {
        {
            Instance = this;
        }
    }
    public void EnemiesCount()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        enemiesOnScene = enemies.Length;
        Debug.Log("enemies " + enemiesOnScene);

        if (enemiesOnScene == 0)
            //Hero.Instance.Invoke("SetWinPanel", 1.1f);
            Debug.Log("WIN WIN");

    }
}
