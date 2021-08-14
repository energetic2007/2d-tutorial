using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Entity
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int health;
    [SerializeField] private float jumpForce = 0.7f;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite aliveHeart;
    [SerializeField] private Sprite deadHeart;
    [SerializeField] GameObject DeathMenu;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public SpriteRenderer AttackCircle;
    private bool isGrounded = false;
    public bool isAttacking = false;
    public bool isRecharged = true;
    private Vector2 pos;
    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;
    public Joystick joystick;

    public static Hero Instance { get; set; }

    private States State
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }

    private void Awake()
    {
        lives = 5;
        health = lives;
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        isRecharged = true;
    }
    private void Update()
    {
        if (isGrounded && !isAttacking) State = States.idle;
        if (!isAttacking && joystick.Horizontal != 0)
            Run();
        // if (!isAttacking && joystick.Vertical > 0.5f && isGrounded)
        //     Jump();


        if (health > lives)
            health = lives;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
                hearts[i].sprite = aliveHeart;
            else
                hearts[i].sprite = deadHeart;
            if (i < lives)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;

        }
    }

    private void FixedUpdate()
    {
        CheckGround();
        if (!isGrounded) State = States.Jump;
    }
    private void Run()
    {
        if (isGrounded) State = States.Run;


        Vector3 dir = transform.right * joystick.Horizontal;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sprite.flipX = dir.x < 0.0f;
    }

    public void Jump()
    {
        if (!isAttacking && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
        //rb.velocity = Vector2.up * jumpForce;
    }

    private void CheckGround()
    {

        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.05f);
        isGrounded = collider.Length > 1;

    }

    public override void GetDamage()
    {
        anim.SetTrigger("damage");
        health -= 1;

        if (health == 0)
        {
            foreach (var h in hearts)
                h.sprite = deadHeart;
            Die();
            DeathMenu.SetActive(true);
        }
    }
    public void Attack()
    {
        if (isRecharged)
        {
            State = States.Attack;
            AttackCircle.enabled = true;
            isAttacking = true;
            isRecharged = false;

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }
    }
    private void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);


        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Entity>().GetDamage();
            StartCoroutine(EnemyOnAttack(colliders[i]));
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
        //Gizmos.color = Color.cyan;
        //Gizmos.DrawWireSphere(transform.position, 0.05f);


    }

    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.4f);
        AttackCircle.enabled = false;
        isAttacking = false;
    }
    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }
    private IEnumerator EnemyOnAttack(Collider2D enemy)
    {
        SpriteRenderer enemyColor = enemy.GetComponentInChildren<SpriteRenderer>();

        Debug.Log(enemyColor);
        // меняет цвет
        enemyColor.color = Color.red;

        // ждем 0.2 сек
        yield return new WaitForSeconds(0.5f);

        // ставим старый цвет
        enemyColor.color = new Color(1, 1, 1);
    }

}
public enum States
{
    idle,
    Run,
    Jump,
    Attack,
    damage,
    dead
}