using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Entity
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int health;

    [SerializeField] private float jumpForce = 0.3f;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite aliveHeart;
    [SerializeField] private Sprite deadHeart;


    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    public SpriteRenderer AttackCircle;
    private bool isGrounded = false;
    public bool isAttacking = false;
    public bool isRecharged = true;



    private Vector2 pos;
    //pos.y -= 1.0f;

    //public Transform groundPos;



    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;

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
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        isRecharged = true;
    }
    private void Update()
    {
        if (isGrounded && !isAttacking) State = States.idle;
        if (Input.GetButton("Horizontal"))
            Run();
        if (Input.GetButton("Jump") && isGrounded)
            Jump();
        if (Input.GetButtonDown("Fire1"))
            Attack();

        if (health > lives)
            health = lives;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
                hearts[i].sprite = aliveHeart;
            else

                hearts[i].sprite = deadHeart;



            // if (i < lives)
            //  hearts[i].enabled = true;
            // else
            //  hearts[i].enabled = false;

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


        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sprite.flipX = dir.x < 0.0f;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    private void CheckGround()
    {

        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.05f);
        isGrounded = collider.Length > 1;

    }

    //private void OnDrawGizmosSelected()
    // {
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(pos, 0.3f);
    // }
    /*
    private void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
    */

    public override void GetDamage()
    {
        health -= 1;
        if (health == 0)
        {
            foreach (var h in hearts)
                h.sprite = deadHeart;
            Die();
        }
    }
    private void Attack()
    {
        if (isGrounded && isRecharged)
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

}
public enum States
{
    idle,
    Run,
    Jump,
    Attack
}