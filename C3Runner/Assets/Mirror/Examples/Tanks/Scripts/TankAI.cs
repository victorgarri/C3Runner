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


        [ServerCallback]
        void Start()
        {
            if (isServer)
                InvokeRepeating("CmdFire", 1, 1);
        }

        [ServerCallback]
        void OnEnable()
        {
            if (isServer)
                InvokeRepeating("CmdFire", 1, 1);
        }

        [ServerCallback]
        void OnDisable()
        {
            if (isServer)
                CancelInvoke("CmdFire");
        }

        // this is called on the server
        void CmdFire()
        {
            try
            {
                GetTargets();
                if (targets.Count > 0)
                {
                    PickTarget();
                    RotateTurret();
                    GameObject projectile = Instantiate(projectilePrefab, projectileMount.position, projectileMount.rotation);
                    NetworkServer.Spawn(projectile);
                    RpcOnFire();
                    //}
                }

            }
            catch (System.Exception e) { print(e); }
        }

        void GetTargets()
        {
            targets.Clear();
            var tanks = GameObject.FindObjectsOfType<Tank>();
            foreach (var item in tanks)
            {
                targets.Add(item.transform);
            }
        }


        // this is called on the tank that fired for all observers
        [ClientCallback]
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

        [SyncVar] public int randomIndex;

        void PickTarget()
        {
            randomIndex = Random.Range(0, targets.Count);
        }


        [ClientCallback]
        void RotateTurret()
        {
            //if (targets.Count > 0)
            //{

            Vector3 tdummy = targets[randomIndex].position;

            turret.transform.LookAt(tdummy);
            //}
        }
    }
}