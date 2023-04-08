using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    public GameObject bullet;
    public float force = 100;
    public float interval = 4;
    public float bulletDestroyTime = 12;
    public bool ableToShoot = true;
    public bool trackPlayer = true;

    List<Transform> targets = new List<Transform>();


    void Start()
    {
        InvokeRepeating("Shoot", 0, interval);
    }

    int randomIndex = 0;

    void FixedUpdate()
    {
        Aim();
    }

    void Shoot()
    {
        if (ableToShoot)
        {
            GameObject go = Instantiate(bullet, transform.position, transform.rotation);
            go.transform.parent = null;
            Aim();
            //Shoot forward
            go.GetComponent<Rigidbody>().AddForce(force * transform.forward, ForceMode.Impulse);

            Destroy(go, bulletDestroyTime);
        }

    }

    void Aim()
    {
        if (targets.Count > 0)
        {
            randomIndex = Random.Range(0, targets.Count - 1);
            transform.LookAt(targets[randomIndex]);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
        
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            targets.Add(c.gameObject.transform);


        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            targets.Remove(c.gameObject.transform);


        }
    }
}
