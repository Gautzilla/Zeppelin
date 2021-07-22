using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    #region Initialisation

    [Header("Speed")]
    public Rigidbody rb;
    public float initialForce = 5f;
    public float forwardForce = 0.2f;

    [Header("Rotation")]
    public float rotationSpeedModifier = 0f;
    private float rotationSpeed;

    [Header("Radar")]
    public float radarLength = 2f;
    public float radarRadius = 2f;
    public float focusSpeed = 2f;

    Collider[] targetFound;
    Transform foundTarget;
    Vector3 directionTowardsPlayer;
    Quaternion rotationTowardsPlayer;
    float distanceToPlayer;

    [Header("Effects")]
    public GameObject explosion;
    public Light torpedoLight;

    #endregion

    #region Initialisation

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    #endregion

    #region Déplacement

    void FixedUpdate()
    {
        rotationSpeed = transform.InverseTransformVector(rb.velocity).z * rotationSpeedModifier;

        rb.AddRelativeForce(0f, 0f, forwardForce * Time.deltaTime, ForceMode.VelocityChange);
        transform.Rotate(new Vector3(0f, 0f, rotationSpeed * Time.deltaTime), Space.Self);

        Radar();
    }

    #endregion

    #region Contact et explosion

    private void OnBecameInvisible()
    {
        Explode(); // Détruit l'objet si il sort du champ de vision de la caméra
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
        Instantiate(explosion, transform);
        Explode();
    }

    private void Explode()
    {
        torpedoLight.enabled = false;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject, 5f);
        }

        transform.DetachChildren(); // Se détache de l'explosion avant d'être détruit
        Destroy(gameObject);
    }

    #endregion

    #region Radar

    private void Radar()
    {
        targetFound = Physics.OverlapCapsule(transform.position + 1f * transform.forward, transform.position + (radarLength + 1f) * transform.forward, radarRadius, 1 << 8 | 1 << 9); // Détecte les objets dans les layers "joueurs" et "projectiles"

        if (targetFound.Length > 0)
        {
            int t = FindTarget();

            if (t >= 0)
            {
                foundTarget = targetFound[t].transform;
                directionTowardsPlayer = (foundTarget.position - transform.position).normalized;
                rotationTowardsPlayer = Quaternion.LookRotation(directionTowardsPlayer);

                distanceToPlayer = Vector3.Distance(transform.position, foundTarget.position);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotationTowardsPlayer, Time.deltaTime * focusSpeed / distanceToPlayer); // La torpille se focalise d'autant plus sur la cible si elle est proche
            }
        }
    }

    private int FindTarget()
    {
        int t = 0;

        foreach (Collider target in targetFound)
        {
            if (target.gameObject != gameObject)
            {
                return t;
            }
            t++;
        }

        return -1;
    }

    #endregion
}
