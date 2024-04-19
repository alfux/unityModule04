using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liana : MonoBehaviour
{
    public float        damage = 1;

    private Animator    anim = null;
    private int         id;

    void Start()
    {
        this.anim = this.GetComponent<Animator>();
        this.id = Animator.StringToHash("Attack");
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            this.anim.SetBool(this.id, true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            this.anim.SetBool(this.id, false);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController    player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
            player.TakeDamage(this.damage);
    }
}
