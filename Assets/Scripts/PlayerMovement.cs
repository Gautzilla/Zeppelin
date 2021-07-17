using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    #region Déclaration des variables

    [Header("Player Movement")]
        public Rigidbody rb;
        public float forwardForce = 50f;
        public float playerRotation = 50f;
        public bool isFalling = false;
        public DestroyOnCrash destroy;

        [Header("Thrusters")]
        public Transform thrusterLeft;
        public Transform thrusterRight;

        [Header("Height")]
        public float maxHeight = 8f;
        public float hoverDamp = 1f;
        public float meanHeight;
        public float hoverForce = 10f;
        public float jiggleFrequency = 1f;
        public float jiggleDepth = 1f;

        [Header("Player Keys")]
        public int playerIdentifier;

    #endregion

    

    private void Start()
    {
        meanHeight = Settings.playerHeight;
    }

    void FixedUpdate()
    {
        if (!isFalling)
        {

            RaycastHit hit;
            Ray downRay = new Ray(transform.position, -transform.up);

            if (Physics.Raycast(downRay, out hit))
            {
                float hoverError = meanHeight - hit.distance;

                if (hoverError > 0 && transform.position.y < maxHeight)
                {
                    float upwardSpeed = rb.velocity.y;
                    float lift = (hoverError * hoverForce - upwardSpeed * hoverDamp) * Time.deltaTime;

                    rb.AddForce(lift * transform.up, ForceMode.VelocityChange);
                }
            } else
            {
                rb.AddForce(hoverForce * Time.deltaTime * transform.up, ForceMode.VelocityChange);
            }

            rb.AddForce(0f, (Mathf.Sin(Time.time * jiggleFrequency) * jiggleDepth) * Time.deltaTime, 0f, ForceMode.VelocityChange);
        }
    }

    public void MoveForward(float value)
    {
        rb.AddRelativeForce(0, 0, value * forwardForce * Time.deltaTime, ForceMode.VelocityChange);
    }

    public void Rotate(float value)
    {
        if (value > 0f)
        {
            rb.AddForceAtPosition(Mathf.Abs(value) * thrusterRight.forward * playerRotation * Time.deltaTime, thrusterRight.position, ForceMode.VelocityChange);
        }
        else if (value < 0f)
        {
            rb.AddForceAtPosition(Mathf.Abs(value) * thrusterLeft.forward * playerRotation * Time.deltaTime, thrusterLeft.position, ForceMode.VelocityChange);
        }
    }

    public void Fall()
    {
        isFalling = true;
        rb.drag = 0.5f;
        rb.angularDrag = 0.5f;
        rb.useGravity = true;
        rb.freezeRotation = false;
        destroy.Invoke("Destroy", 3f);
    }
}
