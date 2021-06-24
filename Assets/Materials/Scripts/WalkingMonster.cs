using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingMonster : Entity
{
    // private float speed = 3.5f;
    private Vector3 dir;
    private SpriteRenderer sprite;



    private void Start()
    {
        dir = transform.right;
        lives = 4;
    }

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5f + transform.right * dir.x * 0.5f, 0.01f);
        if (colliders.Length > 0)
        {
            dir *= -1f;
            sprite.flipX = dir.x > 0.0f;
        }
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, Time.deltaTime);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position + transform.up * 0.5f + transform.right * dir.x * 0.4f, 0.01f);
        //Gizmos.color = Color.cyan;
        //Gizmos.DrawWireSphere(transform.position, 0.05f);


    }

    private void Update()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            lives--;
            Hero.Instance.GetDamage();
        }
        if (lives < 1)
            Die();
    }
}
