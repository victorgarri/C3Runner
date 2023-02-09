using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public GameObject player;
    private Rigidbody _rigidbody;
    [SerializeField] private float moveForce = 20f;
    private bool activado;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x - player.transform.position.x < 10f)
        {
            activado = true;
        }
        if (transform.position.x - player.transform.position.x > 10f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (activado)
        {
           Vector3 direction = (player.transform.position - transform.position).normalized;
           
            _rigidbody.AddForce(moveForce * direction, ForceMode.Force); 
        }
        
    }
}
