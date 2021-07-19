using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    #region Déclaration des variables

    private CameraRig cameraRig;

    public float yRotation;
    public float timeToRotate;
    public bool clockwise;

    public CheckpointGroup checkpointGroup;
    public int checkpointIdentifier;

    public bool lastCheckpoint = false;

    #endregion

    #region Initialisation

    private void Start()
    {
        cameraRig = GameObject.Find("CameraRig").GetComponent<CameraRig>();
    }

    #endregion

    #region Camera Rotation trigger

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int playerIdentifier = other.GetComponent<PlayerMovement>().playerIdentifier;
            int lastActiveCheckpoint = checkpointGroup.lastActiveCheckpoint;

            if (checkpointIdentifier == checkpointGroup.playersCheckpoints[playerIdentifier] + 1) // Si le checkpoint franchi est le checkpoint actif
            {
                if (checkpointIdentifier == lastActiveCheckpoint + 1) // On active le checkpoint suivant
                {
                    cameraRig.StopAllCoroutines();
                    cameraRig.StartCoroutine(cameraRig.RotateCamera(yRotation, timeToRotate, clockwise));
                    checkpointGroup.SetActiveCheckpoint(lastCheckpoint ? 0 : checkpointIdentifier);
                }
                checkpointGroup.playersCheckpoints[playerIdentifier] = lastCheckpoint ? 0 : checkpointIdentifier; // Chaque joueur doit passer par les checkpoints dans l'ordre
            }
        }
    }

    #endregion

}
