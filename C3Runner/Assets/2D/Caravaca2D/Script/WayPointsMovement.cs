using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointsMovement : MonoBehaviour
{
    private SpawnProfesores _spawnProfesores;
    
    [SerializeField]private List<Transform> waypoints;
    // Start is called before the first frame update
    public float speed = 0f;
    private float changeDistance = 0.2f;
    private byte nextPosition = 0;
    
    
    void Start()
    {
        _spawnProfesores = GameObject.FindGameObjectWithTag("Spawn").GetComponent<SpawnProfesores>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            waypoints[nextPosition].transform.position,
            speed * Time.deltaTime);

        if (Vector2.Distance(transform.position,waypoints[nextPosition].transform.position)<changeDistance)
        {
            nextPosition++;
            if (nextPosition >= waypoints.Count)
            {
                nextPosition = 0;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Puerta")
        {
            Destroy(gameObject);
            _spawnProfesores.disminuirProfesor();
        }
    }
}
