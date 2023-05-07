using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    private SpawnProfesores _spawnProfesores;

    private Puntuacion _puntuacion;

    public float force = 15;
    private AudioSource audioSource;
    //public AudioClip ballSound;

    private void Start()
    {
        _spawnProfesores = GameObject.FindGameObjectWithTag("Spawn").GetComponent<SpawnProfesores>();
        _puntuacion = GameObject.FindGameObjectWithTag("Aciertos").GetComponent<Puntuacion>();
        
        this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * force, ForceMode2D.Impulse);
        
        //audioSource.GetComponent<AudioSource>().PlayOneShot(ballSound, 1);
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * force * Time.deltaTime, ForceMode2D.Force);
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Profesor")
        {
            //Debug.Log("mandooooooo");
            Destroy(gameObject);
            Destroy(col.gameObject);
            _spawnProfesores.disminuirProfesor();
            _puntuacion.acierto();
        }

        if (col.gameObject.tag == "ProfesorEnfadado")
        {
            Destroy(gameObject);
            Destroy(col.gameObject);
            _spawnProfesores.disminuirProfesor();
            _puntuacion.fallo();
        }

        Destroy(gameObject);
    }
}