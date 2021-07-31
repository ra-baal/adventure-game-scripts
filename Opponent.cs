using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Opponent : Character
{
    public float Speed = 2.0f;

    public float SwordAttackDistance = 1;
    public float GoToAttackDistance = 4;
    public float CombatIdleDistance = 6;

    private GameObject playerGameObject;
    private Player player;
    private Slider slider;

    override protected void Start()
    {
        base.Start();

        this.playerGameObject = GameObject.Find("Player");
        this.player = this.playerGameObject.GetComponent<Player>();
        this.slider = this.GetComponentInChildren<Slider>();

    }

    override protected void Update()
    {
        base.Update();

        this.selectActivity();
    }

    private void selectActivity()
    {
        if (this.CurrentHealth > 0)
        {
            float distance = this.distanceTo(this.playerGameObject);
            Vector2 direction = this.directionToThePlayer();

            if (direction.y < 0.5 && direction.y > -0.5 && this.player.CurrentHealth > 0)
            {
                if (distance < this.SwordAttackDistance)
                {
                    if (this.timeSinceAttack > this.intervalBetweenAttacks)
                        this.swordAttack();
                    else
                        this.idle();
                }
                else if (distance < this.GoToAttackDistance)
                    this.goToAttack();
                else if (distance < this.CombatIdleDistance)
                    this.combatIdle();
                else
                    this.idle();
            }
            else
            {
                if (distance < this.CombatIdleDistance)
                    this.combatIdle();
                else
                    this.idle();
            }
        }
    }

    private void flipX(bool flip)
    {
        this.spriteRenderer.flipX = flip;
        this.facingDirection = flip == false ? -1 : 1;
    }
    
    /* Shows a health bar. */
    private void OnGUI()
    {
        //Debug.Log(CurrentHealth);
        if (this.CurrentHealth > 0 && this.CurrentHealth < this.MaxHealth)
            this.slider.value = this.CurrentHealth / (float)this.MaxHealth;
        else // when health is 0% or 100%
            this.slider.value = 0f;
    }

    private void move()
    {
        float horizontalDirection = this.directionToThePlayer().x < 0 ? -1f : 1f;

        if (horizontalDirection > 0)
            this.flipX(true);
            //this.transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (horizontalDirection < 0)
            this.flipX(false);
        //this.transform.localScale = new Vector3(1f, 1f, 1f);

        this.rigidbody2d.velocity = new Vector2(horizontalDirection * Speed, rigidbody2d.velocity.y);
        this.animator.SetFloat("AirSpeed", rigidbody2d.velocity.y);
    }

    private void recover() //switchDeath()
    {
        //if (!this.isDead)
        //    animator.SetTrigger("Death");
        //else
            animator.SetTrigger("Recover");

        //this.isDead = !this.isDead;
    }

    private void goToAttack()
    {
        this.run();
        this.move();
    }

    private void swordAttack()
    {
        this.animator.SetTrigger("Attack");
        this.timeSinceAttack = 0f;

        StartCoroutine(this.attackCoroutine("Player", this.DamagePoints, 0.5f));
    }

    private void run()
    {
        this.animator.SetInteger("AnimState", 2);
    }

    private void combatIdle()
    {
        //this.isCombatIdle = true;
        this.animator.SetInteger("AnimState", 1);
    }

    private void idle()
    {
        this.animator.SetInteger("AnimState", 0);
    }

    /* Zwraca wektor okreœlaj¹cy kierunek do gracza. */
    private Vector2 directionToThePlayer()
    {
        Vector2 opponent = this.transform.position;
        Vector2 player = this.playerGameObject.transform.position;

        return new Vector2(
            player.x - opponent.x, 
            player.y - opponent.y);
    }

}
