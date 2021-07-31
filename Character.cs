using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float JumpSpeed = 7.5f;
    public int CurrentHealth;
    public int MaxHealth = 100;
    public int DamagePoints = 10;
    public float intervalBetweenAttacks = 1f;
    public int Points = 0; // points earned when killed

    // sounds
    public AudioClip SwordSound;
    public AudioClip BlockSound;
    public AudioClip HurtSound;

    protected int facingDirection = 1;
    protected bool isGrounded = false;
    protected float timeSinceAttack = 0f;
    protected bool blocked = false; // block against weapons

    public bool isDead { private set; get; }

    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected Collider2D collider2d;
    protected Rigidbody2D rigidbody2d;
    protected ColliderSensor groundSensor;
    protected Transform transform;

    /* Common Start function for all inheriting classes. */
    virtual protected void Start()
    {
        this.isDead = false;
        this.CurrentHealth = this.MaxHealth;

        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
        this.collider2d = GetComponent<Collider2D>();
        this.rigidbody2d = GetComponent<Rigidbody2D>();
        this.transform = GetComponent<Transform>();

        this.groundSensor = this.GetComponentInChildren<ColliderSensor>();
    }

    /* Common Update function for all inheriting classes. */
    virtual protected void Update()
    {
        this.timeSinceAttack += Time.deltaTime;
        this.changeGrounded();

        // fallen off the map
        if (this.transform.position.y < -7)
            this.fallingOffTheMap();
    }

    protected void changeGrounded()
    {        
        if (!this.isGrounded && this.groundSensor.IsOn())
            this.isGrounded = true;
        else if (this.isGrounded && !this.groundSensor.IsOn())
            this.isGrounded = false;

        this.animator.SetBool("Grounded", isGrounded);
    }

    private void attack(string layer, int damagePoints)
    {
#nullable enable

        AudioSource.PlayClipAtPoint(this.SwordSound, this.transform.position, 1f);

        GameObject? r = this.inRange(new Vector2(facingDirection, 0f), 1f, LayerMask.GetMask(layer));

        if (r != null)
        {
            Character ch = r.GetComponentInChildren<Character>();
            if (!ch.blocked)
                ch.Damage(damagePoints);
            else
                ch.Block();
        }
#nullable disable
    }

    // delayed attack
    protected IEnumerator attackCoroutine(string layer, int damagePoints, float delay)
    {
        yield return new WaitForSeconds(delay);
        this.attack(layer, damagePoints);
    }

    protected void Block()
    {
        AudioSource.PlayClipAtPoint(this.BlockSound, this.transform.position, 1f);
        this.animator.SetTrigger("Block");
    }

#nullable enable
    protected GameObject? inRange(Vector2 direction, float maxRange, int layer)
    {
        RaycastHit2D r = Physics2D.Raycast(
            new Vector2(this.transform.position.x, this.transform.position.y + 0.5f),
            direction,
            maxRange,
            layer);

        return r.collider?.gameObject;
    }
#nullable disable

    protected void jump()
    {
        this.animator.SetTrigger("Jump");
        this.isGrounded = false;
        this.animator.SetBool("Grounded", isGrounded);
        this.rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, JumpSpeed);
        this.groundSensor.Disable(0.2f);
    }

    private void hurt()
    {
        AudioSource.PlayClipAtPoint(this.HurtSound, this.transform.position, 1f);
        this.animator.SetTrigger("Hurt");
    }

    private protected void death()
    {
        if (!isDead)
        {
            this.isDead = true;

            this.animator.SetTrigger("Death");
            this.rigidbody2d.gravityScale = 0;
            this.collider2d.enabled = false;

            this.rigidbody2d.velocity = Vector2.zero;

            PointsCounter.AddPoints(this.Points);

            Destroy(this.gameObject, 10);
        }
    }

    private protected void fallingOffTheMap()
    {
        if (!isDead)
        {
            this.isDead = true;
            Destroy(this.gameObject, 30);
        }
    }

    public void Damage(int damagePoints)
    {
        this.CurrentHealth -= damagePoints;
        this.hurt();

        if (this.CurrentHealth <= 0)
            this.death();
    }

    public void Heal()
    {
        this.CurrentHealth = this.MaxHealth;
    }

    /* Returns distance to a object. */
    public float distanceTo(GameObject gameObject)
    {
        return Vector2.Distance(this.transform.position, gameObject.transform.position);
    }

}
