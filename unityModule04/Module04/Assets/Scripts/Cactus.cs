using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cactus : MonoBehaviour
{
    public GameObject   ammo = null;
    public float        damage = 1;
    public float        bulletForce = 1;
    public float        bulletLifeTime = 1;
    public float        fireRate = 1;

    private Animator    anim;
    private int         id;
    private Vector3     target;

    void Start()
    {
        this.anim = this.GetComponent<Animator>();
        this.anim.speed = this.fireRate;
        this.id = Animator.StringToHash("Attack");
        this.target = Vector3.zero;
    }

    void UpdateTarget(Collider2D col)
    {
        Vector2 closest = col.ClosestPoint(
            new Vector2(
                this.transform.position.x,
                this.transform.position.y
            )
        );
        this.target = new Vector3(
            closest.x - this.transform.position.x,
            closest.y - this.transform.position.y,
            0
        );
        this.target.y += this.target.magnitude * 0.1f;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            this.anim.SetBool(this.id, true);
            this.UpdateTarget(col);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            this.UpdateTarget(col);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            this.anim.SetBool(this.id, false);
        }
    }

    public void Fire()
    {
        GameObject  bullet = GameObject.Instantiate(this.ammo, this.transform);

        if (bullet.TryGetComponent<Spit>(out Spit spit))
        {
            spit.SetDamage(this.damage);
            spit.SetLifeTime(this.bulletLifeTime);
            if (bullet.TryGetComponent<Rigidbody2D>(out Rigidbody2D rig))
            {
                bullet.transform.right = this.target.normalized
                    * Mathf.Sign(this.transform.localScale.x);
                rig.AddForce(this.bulletForce * this.target.normalized,
                    ForceMode2D.Impulse);
            }
        }
    }
}
