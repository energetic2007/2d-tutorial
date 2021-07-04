using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingMonster : Entity
{
    // private float speed = 3.5f;
    private Vector3 dir;
    private SpriteRenderer sprite;

    [SerializeField] float collisionX;
    [SerializeField] float collisionY;
    [SerializeField] int setLives;
    private void Start()
    {
        dir = transform.right;
        lives = setLives;
    }

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * collisionY + transform.right * dir.x * collisionX, 0.1f);
        if (colliders.Length > 0)
        {
            dir *= -1f;
            sprite.flipX = dir.x < 0.0f;
        }
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, Time.deltaTime);
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + transform.up * collisionY + transform.right * dir.x * collisionX, 0.1f);
    }

    private void Update()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Debug.Log(lives);
            Hero.Instance.GetDamage();
        }
    }
}

