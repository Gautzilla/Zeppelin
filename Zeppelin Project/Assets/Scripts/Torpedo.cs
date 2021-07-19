using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    public Rigidbody rb;
    public float initialForce = 5f;
    public float forwardForce = 0.2f;

    public float rotationSpeedModifier = 0f;
    private float rotationSpeed;
    public GameObject explosion;
    public Light torpedoLight;

    [Header("Radar")]
    public float radarLength = 2f;
    public float radarRadius = 2f;
    public float focusSpeed = 2f;

    Collider[] playersFound;
    Transform foundPlayer;
    Vector3 directionTowardsPlayer;
    Quaternion rotationTowardsPlayer;
    float distanceToPlayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rotationSpeed = transform.InverseTransformVector(rb.velocity).z * rotationSpeedModifier;

        rb.AddRelativeForce(0f, 0f, forwardForce * Time.deltaTime,ForceMode.VelocityChange);
        transform.Rotate(new Vector3(0f, 0f, rotationSpeed * Time.deltaTime), Space.Self);

        Radar();
    }

    private void OnBecameInvisible()
    {
        Explode();
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
        // FAIT EXPLOSER LE JOUEUR TOUCHE

        //if (collisionInfo.collider.CompareTag("Player"))
        //{
        //    GameObject collider = collisionInfo.collider.gameObject;
        //    collider.GetComponentInParent<DestroyOnCrash>().Destroy();
        //}

        if (!collisionInfo.collider.CompareTag("Torpedo"))
        {
            Instantiate(explosion, transform);
            Explode();
        }
    }

    private void Explode()
    {
        torpedoLight.enabled = false;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject, 5f);
        }

        transform.DetachChildren();
        Destroy(gameObject);
    }

    private void Radar()
    {
        playersFound = Physics.OverlapCapsule(transform.position + 1f * transform.forward, transform.position + (radarLength + 1f) * transform.forward, radarRadius, 1 << 8 | 1 << 9);
        
        if (playersFound.Length > 0)
        {
            int t = FindTarget();

            if (t >= 0)
            {
                foundPlayer = playersFound[t].transform;
                directionTowardsPlayer = (foundPlayer.position - transform.position).normalized;
                rotationTowardsPlayer = Quaternion.LookRotation(directionTowardsPlayer);

                distanceToPlayer = Vector3.Distance(transform.position, foundPlayer.position);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotationTowardsPlayer, Time.deltaTime * focusSpeed / distanceToPlayer);
            }
        }
    }

    private int FindTarget()
    {
        int t = 0;

        foreach (Collider target in playersFound)
        {
            if (target.gameObject != gameObject)
            {
                return t;
            }
            t++;
        }

        return -1;
    }
}
