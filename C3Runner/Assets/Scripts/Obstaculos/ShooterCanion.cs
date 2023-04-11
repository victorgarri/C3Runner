using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterCanion : MonoBehaviour
{

    public GameObject bullet;
    public Transform pivot;
    public float force = 100;
    public float interval = 4;
    public float bulletDestroyTime = 12;
    public bool ableToShoot = true;
    public bool trackPlayer = true;

    List<Transform> targets = new List<Transform>();

    GameObject body, barrel;


    void Start()
    {
        body = transform.Find("Parts").Find("Body").gameObject;
        barrel = body.transform.Find("Barrel").gameObject;

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
            GameObject go = Instantiate(bullet, pivot.position, transform.rotation);
            go.transform.parent = null;
            Aim();
            //Shoot forward
            go.GetComponent<Rigidbody>().AddForce(force * -barrel.transform.forward, ForceMode.Impulse);

            Destroy(go, bulletDestroyTime);
        }

    }

    void Aim()
    {
        if (targets.Count > 0)
        {
            randomIndex = Random.Range(0, targets.Count - 1);

            Quaternion dummy;
            body.transform.LookAt(targets[randomIndex]);
            dummy = body.transform.rotation;
            dummy.x = 0;
            //dummy.y = dummy.y + Mathf.PI; //pi is 180º, but doesn't work somehow
            dummy.z = 0;
            body.transform.rotation = dummy;
            body.transform.Rotate(new Vector3(0, 180, 0));



            barrel.transform.LookAt(targets[randomIndex]);
            dummy = barrel.transform.rotation;
            dummy = Quaternion.Euler(-dummy.eulerAngles.x, 0, 0);

            barrel.transform.localRotation = dummy;
        }
        else
        {
            body.transform.rotation = Quaternion.identity;
            body.transform.Rotate(new Vector3(0, 180, 0));
            barrel.transform.rotation = Quaternion.Euler(0, 180, 0);
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
