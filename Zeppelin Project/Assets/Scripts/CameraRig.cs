using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{

    #region Déclaration des variables

    public Transform cameraObject;
    public GameObject playerGroup;
    private List<Transform> players;

    private Vector3 meanPosition;
    public Vector3 offset; // Eloigne la caméra des joueurs
    private Vector3 cameraRigVelocity = new Vector3(0f, 0f, 0f);
    public float cameraRigDamp;

    public float xRotation = 25f;

    private float meanDistance;
    // Distances pour lesquelles on est en plan serré
    public float minPlayerDistance;
    public float maxPlayerDistance;

    public float minRotation;
    public float maxRotation;

    public float minCameraDistance;
    public float maxCameraDistance;

    public float cameraDamp;
    private float cameraVelocity = 0f;

    #endregion

    #region Initialisation

    private void Start()
    {
        cameraObject = FindObjectOfType<Camera>().gameObject.transform;

        players = new List<Transform> { };

        for (int i = 0; i < Settings.numberOfPlayers; i++)
        {
            players.Add(playerGroup.transform.GetChild(i).transform);
        }
    }

    #endregion

    #region Contrôles caméra

    void FixedUpdate()
    {
        ComputeMeanTransform(); // Point milieu des joueurs
        MoveCamera(meanPosition); // Déplace la caméra en fonction du point milieu
        ZoomCamera(meanDistance); // Ajuste le zoom selon la distance entre les joueurs
    }

    void ComputeMeanTransform()
    {
        var sumOfPositions = new Vector3(0f, 0f, 0f);

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == null)
            {
                players.RemoveAt(i);
            }
            else
            {
                sumOfPositions += players[i].position;
            }
        }

        if (players.Count > 0)
        {
            meanPosition = sumOfPositions / players.Count;
        }
        else
        {
            meanPosition = transform.position;
        }

        if (players.Count > 0)
        {
            foreach (Transform player in players)
            {
                if (player != null)
                {
                    meanDistance += Mathf.Abs(meanPosition.x - player.position.x) + Mathf.Abs(meanPosition.z - player.position.z);
                }
            }
            meanDistance = meanDistance / players.Count;
        }

    }

    void MoveCamera(Vector3 centralPosition)
    {
        if (players.Count == 0)
        {
            offset.y = 0f;
        }

        transform.position = Vector3.SmoothDamp(transform.position, centralPosition + offset, ref cameraRigVelocity, cameraRigDamp);
    }

    void ZoomCamera(float distance)
    {
        float zoomProgress = Mathf.Clamp(distance, minPlayerDistance, maxPlayerDistance) / maxPlayerDistance;

        xRotation = Mathf.SmoothDamp(xRotation, Mathf.Lerp(minRotation, maxRotation, zoomProgress), ref cameraVelocity, cameraDamp); // Au delà de maxPlayerDistance, on est en plan large (x Rotation au max)
        transform.rotation = Quaternion.Euler(xRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        float zPosition = Mathf.SmoothDamp(cameraObject.localPosition.z, Mathf.Lerp(-minCameraDistance, -maxCameraDistance, zoomProgress), ref cameraVelocity, cameraDamp); // Au delà de maxPlayerDistance, on est en plan large (y position au max)
        cameraObject.localPosition = new Vector3(0f, 0f, zPosition); 
    }

    #endregion

    #region Rotation caméra après franchissement d'un checkpoint

    public IEnumerator RotateCamera(float yRotation, float timeToRotate, bool clockwise)
    {
        float baseRotation = transform.rotation.eulerAngles.y;

        if ((clockwise && (yRotation < transform.rotation.eulerAngles.y)) || (!clockwise && (yRotation > transform.rotation.eulerAngles.y))) // Clockwise est déterminé par le checkpoint qui initie la rotation
        {
            yRotation += clockwise ? 360f : -360f;
        }

        float t = 0f;

        while (t < timeToRotate)
        {
            transform.rotation = Quaternion.Euler(xRotation, Mathf.Lerp(baseRotation, yRotation, t / timeToRotate), 0f);
            t += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f); // Réinitialise la rotation en y pour avoir 0° au lieu de 360°
    }

    #endregion

}
