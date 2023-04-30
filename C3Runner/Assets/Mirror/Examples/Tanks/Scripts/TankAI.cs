using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Mirror.Examples.Tanks
{
    public class TankAI : NetworkBehaviour
    {
        [Header("Components")]
        public NavMeshAgent agent;
        public Animator animator;
        public TextMesh healthBar;
        public Transform turret;

        [Header("Movement")]
        public float rotationSpeed = 100;

        [Header("Firing")]
        public KeyCode shootKey = KeyCode.Space;
        public GameObject projectilePrefab;
        public Transform projectileMount;

        [Header("Stats")]
        [SyncVar] public int health = 4;


        public List<Transform> targets = new List<Transform>();
        void Start()
        {
            if (isServer)
                InvokeRepeating("CmdFire", 1, 1);
        }

        //void Update()
        //{
        //    // always update health bar.
        //    // (SyncVar hook would only update on clients, not on server)
        //    healthBar.text = new string('-', health);

        //    // movement for local player
        //    if (isLocalPlayer)
        //    {
        //        // rotate
        //        float horizontal = Input.GetAxis("Horizontal");
        //        transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);

        //        // move
        //        float vertical = Input.GetAxis("Vertical");
        //        Vector3 forward = transform.TransformDirection(Vector3.forward);
        //        agent.velocity = forward * Mathf.Max(vertical, 0) * agent.speed;
        //        animator.SetBool("Moving", agent.velocity != Vector3.zero);
        //        RotateTurret();
        //        // shoot
        //        if (Input.GetKeyDown(shootKey))
        //        {
        //            CmdFire();
        //        }
        //    }
        //}

        // this is called on the server
        //[Command]
        [ClientRpc]
        void CmdFire()
        {
            if (targets.Count > 0)
            {
                RotateTurret();
                GameObject projectile = Instantiate(projectilePrefab, projectileMount.position, projectileMount.rotation);
                NetworkServer.Spawn(projectile);
                RpcOnFire();
            }
        }

        // this is called on the tank that fired for all observers
        [ClientRpc]
        void RpcOnFire()
        {
            animator.SetTrigger("Shoot");
        }

        [ServerCallback]
        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Projectile>() != null)
            {
                --health;
                if (health == 0)
                    NetworkServer.Destroy(gameObject);
            }

            if (other.GetComponent<Tank>() != null)
            {
                targets.Add(other.gameObject.transform);
            }
        }


        [ServerCallback]
        void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Tank>() != null)
            {
                targets.Remove(other.gameObject.transform);
            }
        }

        int randomIndex;
        void RotateTurret()
        {

            randomIndex = Random.Range(0, targets.Count);

            Vector3 tdummy = targets[randomIndex].position;

            turret.transform.LookAt(tdummy);

        }
    }
}