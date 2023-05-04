using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProfesores : MonoBehaviour
{

    public GameObject[] profesores;

    private int randomSpawn;

    [SerializeReference] private float numProfesorSala;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("Spawn", 1.5f, 2);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Numero de Profesores en la Sala: " + numProfesorSala);
    }

    private void Spawn()
    {

        if (numProfesorSala < 5)
        {
            randomSpawn = Random.Range(0, profesores.Length);

            Instantiate(profesores[randomSpawn], new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), profesores[randomSpawn].transform.rotation);
            aumentarProfesor();
        }

    }

    public void disminuirProfesor()
    {
        numProfesorSala--;
    }

    public void aumentarProfesor()
    {
        numProfesorSala++;
    }

    void OnEnable()
    {
        InvokeRepeating("Spawn", 1.5f, 2);
    }

    void OnDisable()
    {
        CancelInvoke("Spawn");
    }
}
