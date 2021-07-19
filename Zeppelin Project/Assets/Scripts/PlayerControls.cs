using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Shoot))]
public class PlayerControls : MonoBehaviour
{
    #region Déclaration des variables

    PlayerInput controls;
    PlayerMovement playerMovement;
    Shoot playerShoot;

    float forwardSpeed = 0f;
    float rotationSpeed = 0f;

    #endregion

    #region Initialisation
    void Awake()
    {
        controls = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShoot = GetComponent<Shoot>();
    }

    #endregion

    #region Acquisition des contrôles
    private void FixedUpdate()
    {
        playerMovement.MoveForward(forwardSpeed);
        playerMovement.Rotate(rotationSpeed);
    }

    private void OnMoveForward(InputValue speed)
    {
        forwardSpeed = speed.Get<float>();
    }

    private void OnRotate(InputValue speed)
    {
        rotationSpeed = speed.Get<float>();
    }

    private void OnShoot(InputValue value)
    {
        playerShoot.ShootAction(value.Get<float>());
    }

    private void OnPauseGame()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().PauseGame();
    }

    #endregion
}
