using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Bouncer : MonoBehaviour
{
    public float force = 10;
    public float cooldownTime = .3f;
    public float timer = .3f;

    private void Start()
    {
        //force *= GameManager.gravityScale;
    }

    private void FixedUpdate()
    {
        if (timer < cooldownTime) timer += Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        var obj = collision.gameObject;
        if (obj.CompareTag("Player") && timer >= cooldownTime)
        {
            timer = 0;
            //Vector3 dir = obj.transform.position - transform.position;
            Vector3 dir = -collision.contacts[0].normal;

            dir.y = .4f;
            //obj.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);
            ApplyForce(obj, dir * force);
        }
    }

    
    void ApplyForce(GameObject obj, Vector3 force)
    {
        obj.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }
}
