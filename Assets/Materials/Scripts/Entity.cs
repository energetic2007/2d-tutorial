using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected int lives;
    public Animator anim;
    [SerializeField] Collider2D col;

    private void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }
    public virtual void GetDamage()
    {
        lives--;
        if (lives < 1)
            Die();
    }

    public virtual void Die()
    {
        anim.SetTrigger("death");
        col.isTrigger = true;
        gameObject.tag = "enemy_dead";
        LevelController.Instance.EnemiesCount();

        // Убирает из игры (если будет надо)
        // Destroy(this.gameObject);
    }
}



