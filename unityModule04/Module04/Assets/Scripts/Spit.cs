using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Spit : MonoBehaviour
{
    private Rigidbody2D rig = null;
    private Animator    ani = null;
    private int         id = 0;
    private float       damage = 1;
    private float       lifeTime = 1;
    private float       t;

    void Start()
    {
        this.rig = this.GetComponent<Rigidbody2D>();
        this.ani = this.GetComponent<Animator>();
        this.id = Animator.StringToHash("Hit");
        this.t = 0;
    }

    void Update()
    {
        if (this.t >= this.lifeTime)
        {
            this.rig.isKinematic = true;
            this.rig.velocity = Vector3.zero;
            this.rig.angularVelocity = 0;
            this.ani.SetBool(this.id, true);
        }
        this.t += Time.deltaTime;
    }

    void FixedUpdate()
    {
        this.transform.right = this.rig.velocity.normalized * Mathf.Sign(this.transform.parent.localScale.x);
    }

    public void SetDamage(float val) => this.damage = val;
    public void SetLifeTime(float val) => this.lifeTime = val;

    void OnCollisionEnter2D(Collision2D col)
    {
        this.rig.isKinematic = true;
        this.rig.velocity = Vector3.zero;
        this.rig.angularVelocity = 0;
        this.ani.SetBool(this.id, true);
        if (col.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.TakeDamage(this.damage);
        }
    }
}
