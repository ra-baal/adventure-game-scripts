using UnityEngine;
using System.Collections;

public class Player : Character
{ 

    public float Speed = 4.0f;
    public float Roll = 8.0f;

    private bool isRolling = false;
    private int currentAttack = 0;
    private float delayToIdle = 0.0f;

    override protected void Start()
    {
        base.Start();
        this.intervalBetweenAttacks = 0.5f;
    }

    override protected void Update()
    {
        base.Update();

        if (this.CurrentHealth > 0)
        {
            float horizontalDirection = Input.GetAxis("Horizontal"); // left/right - a/d keys

            this.move(horizontalDirection);

            this.selectActivity(horizontalDirection);
        }
    }

    private void flipX(bool flip)
    {
        this.spriteRenderer.flipX = flip;
        this.facingDirection = flip == false ? 1 : -1;
    }

    private void move(float horizontalDirection)
    {
        if (this.blocked)
            return;

        if (horizontalDirection > 0)
            this.flipX(false);
        else if (horizontalDirection < 0)
            this.flipX(true);

        if (!this.isRolling)
            rigidbody2d.velocity = new Vector2(horizontalDirection * Speed, rigidbody2d.velocity.y);

        animator.SetFloat("AirSpeedY", rigidbody2d.velocity.y);
    }

    private void selectActivity(float horizontalDirection)
    {
        if (Input.GetMouseButtonDown(0) 
            && timeSinceAttack > this.intervalBetweenAttacks 
            && !isRolling)
                this.swordAttack();
        else if (Input.GetMouseButtonDown(1) 
            && !isRolling)
                this.block();
        else if (Input.GetMouseButtonUp(1))
            this.blockOff();
        else if (Input.GetKeyDown("left shift") && !isRolling)
            this.roll();
        else if (Input.GetKeyDown("space") && isGrounded && !isRolling)
            this.jump();
        else if (Mathf.Abs(horizontalDirection) > Mathf.Epsilon)
            this.run();
        else
            this.idle();
    }


    private void swordAttack()
    {
        currentAttack++;

        // Loop back to one after third attack
        if (currentAttack > 3)
            currentAttack = 1;

        // Reset Attack combo if time since last attack is too large
        if (timeSinceAttack > 1.0f)
            currentAttack = 1;

        // Call one of three attack animations "Attack1", "Attack2", "Attack3"
        animator.SetTrigger("Attack" + currentAttack);

        // Reset timer
        timeSinceAttack = 0.0f;

        StartCoroutine(this.attackCoroutine("Opponents", this.DamagePoints, 0.2f));

    }

    private void run()
    {
        delayToIdle = 0.05f;
        animator.SetInteger("AnimState", 1);
    }

    private void idle()
    {
        delayToIdle -= Time.deltaTime;
        if (delayToIdle < 0)
            animator.SetInteger("AnimState", 0);
    }

    private void block()
    {
        this.animator.SetTrigger("Block");
        this.animator.SetBool("IdleBlock", true);

        this.blocked = true;
    }

    // ?? do czego to?
    private void blockOff()
    {
        this.animator.SetBool("IdleBlock", false);
        this.blocked = false;
    }

    private void roll()
    {
        isRolling = true;
        animator.SetTrigger("Roll");
        rigidbody2d.velocity = new Vector2(facingDirection * Roll, rigidbody2d.velocity.y);
    }

    // Called when roll animation is end.
    private void AE_ResetRoll()
    {
        isRolling = false;
    }

}
