using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;

enum State
{
    Idle = 0,
    Jump = 2,
    Walk = 4,
    Damage = 8,
    Turn = 16,
    Death = 32,
    Respawn = 64
}

public class PlayerController : MonoBehaviour
{
    public float    speed = 1;
    public float    jumpSpeed = 1;

    [SerializeField] private Rigidbody2D    rg;
    [SerializeField] private Animator       anim;
    [SerializeField] private Vector2        vectorialSpeed;
    [SerializeField] private Vector2        vectorialJump;
    [SerializeField] private int            orientation;
    [SerializeField] private int            state;
    [SerializeField] private int            idleID;
    [SerializeField] private int            jumpID;
    [SerializeField] private int            walkID;
    [SerializeField] private int            damageID;
    [SerializeField] private int            turnID;
    [SerializeField] private int            deathID;
    [SerializeField] private int            respawnID;

    public void ReverseOrientation() => this.orientation *= -1;
    public void SetIdle() => this.state = 0;
    public void SetJump() => this.state |= (int)State.Jump;
    public void SetWalk() => this.state |= (int)State.Walk;
    public void SetDamage() => this.state |= (int)State.Damage;
    public void SetTurn() => this.state |= (int)State.Turn;
    public void SetDeath() => this.state |= (int)State.Death;
    public void SetRespawn() => this.state |= (int)State.Respawn;
    public void UnsetJump() => this.state &= ~(int)State.Jump;
    public void UnsetWalk() => this.state &= ~(int)State.Walk;
    public void UnsetDamage() => this.state &= ~(int)State.Damage;
    public void UnsetTurn() => this.state &= ~(int)State.Turn;
    public void UnsetDeath() => this.state &= ~(int)State.Death;
    public void UnsetRespawn() => this.state &= ~(int)State.Respawn;
    public bool IsIdle() => (this.state == 0);
    public bool IsJumping() => ((this.state >> 1) % 2 == 1);
    public bool IsWalking() => ((this.state >> 2) % 2 == 1);
    public bool IsDamaged() => ((this.state >> 3) % 2 == 1);
    public bool IsTurning() => ((this.state >> 4) % 2 == 1);
    public bool IsDead() => ((this.state >> 5) % 2 == 1);
    public bool IsRespawning() => ((this.state >> 6) % 2 == 1);

    public void UpdateStates()
    {
        this.anim.SetBool(this.idleID, this.state == 0);
        this.anim.SetBool(this.jumpID, (this.state >> 1) % 2 == 1);
        this.anim.SetBool(this.walkID, (this.state >> 2) % 2 == 1);
        this.anim.SetBool(this.damageID, (this.state >> 3) % 2 == 1);
        this.anim.SetBool(this.turnID, (this.state >> 4) % 2 == 1);
        this.anim.SetBool(this.deathID, (this.state >> 5) % 2 == 1);
        this.anim.SetBool(this.respawnID, (this.state >> 6) % 2 == 1);
    }
    
    void Start()
    {
        this.rg = this.gameObject.GetComponent<Rigidbody2D>();
        this.anim = this.GetComponent<Animator>();
        this.vectorialSpeed = Vector2.right;
        this.vectorialJump = Vector2.up * this.jumpSpeed;
        this.orientation = 1;
        this.SetIdle();
        this.idleID = Animator.StringToHash("Idle");
        this.jumpID = Animator.StringToHash("Jump");
        this.walkID = Animator.StringToHash("Walk");
        this.turnID = Animator.StringToHash("Turn");
        this.deathID = Animator.StringToHash("Death");
        this.damageID = Animator.StringToHash("Damage");
        this.respawnID = Animator.StringToHash("Respawn");
    }

    void Controls()
    {
        float   move = Input.GetAxis("Horizontal");
        float   jump = Input.GetAxis("Jump");

        
        if (!this.IsJumping() && jump != 0 && Mathf.Abs(this.rg.velocity.y) < this.jumpSpeed)
        {
            this.rg.AddForce(this.vectorialJump, ForceMode2D.Impulse);
            this.SetJump();
        }
        if (move != 0)
        {
            this.rg.AddForce((move * this.speed - this.rg.velocity.x) * this.vectorialSpeed);
            if (!this.IsJumping())
            {
                if (move * this.orientation < 0)
                    this.SetTurn();
                else
                    this.SetWalk();
            }
        }
        else
            this.UnsetWalk();
        if (!this.IsWalking() && !this.IsJumping() && !this.IsTurning())
            this.SetIdle();
    }

    void Update()
    {
        this.UpdateStates();
        this.Controls();
    }

    void OnCollisionStay2D(Collision2D other)
    {
        ContactPoint2D[]    points = new ContactPoint2D[10];
        int                 amount = other.GetContacts(points);

        for (int i = 0; i < points.Length; ++i)
        {
            if (Vector2.Dot(points[i].normal, Vector2.up) > 0.5f)
            {
                this.UnsetJump();
                return ;
            }
        }
    }
}