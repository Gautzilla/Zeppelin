using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCrash : MonoBehaviour
{

    // Détruit le joueur si il frappe trop fort sur un obstacle (arrière du zeppelin plus fragile)

    #region Déclaration des variables

    [Header("Dependencies")]
    public PlayerMovement playerMovement;
    public Rigidbody rb;
    public GameObject destroyedPlayerPrefab;
    public PlayerColor playerColor;

    [Header("Impact Forces To Crash")]
    public float magnitudeToCrashSide = 7f;
    public float magnitudeToCrashFront = 14f;
    public float magnitudeToCrashGround = 10f;
    bool playerIsDestroyed = false;

    [Header("Shields")]
    public int shield = 0;
    private Renderer playerRenderer;
    private MaterialPropertyBlock block;
    private Color shieldColor = new Color(0f, 0.45f, 1f);
    private float shieldIntensityModifier = 0.35f;

    #endregion

    #region Initialisation
        private void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();
    }

    #endregion

    #region Gestion des collisions

    private void OnCollisionEnter(Collision collisionInfo)
    {
        if (!playerIsDestroyed)
        {
            switch (collisionInfo.collider.tag)
            {
                case "Ground":
                    if (playerMovement.isFalling == true || (collisionInfo.relativeVelocity.magnitude > magnitudeToCrashGround))
                    {
                        Destroy();
                    }
                    break;
                case "Ammo":
                case "Torpedo":
                    if (shield == 0)
                    {
                        GetComponent<PlayerMovement>().Fall();
                    }
                    else
                    {
                        ChangeShield(false);
                    }
                    break;
                default:
                    if (((collisionInfo.contacts[0].thisCollider.material.name == "PlayerSideMat (Instance)") && (collisionInfo.relativeVelocity.magnitude > magnitudeToCrashSide)) || ((collisionInfo.contacts[0].thisCollider.material.name == "PlayerFrontMat (Instance)") && (collisionInfo.relativeVelocity.magnitude > magnitudeToCrashFront)))
                    {
                        if (shield == 0)
                        {
                            Destroy();
                        }
                        else
                        {
                            ChangeShield(false);
                        }
                    }
                    break;
            }
        }
    }

    public void Destroy()
    {
        if (!playerIsDestroyed)
        {
            playerIsDestroyed = true;
            GameObject destroyedPlayer = Instantiate(destroyedPlayerPrefab, transform.position, transform.rotation);

            foreach (Transform child in destroyedPlayer.transform)
            {
                child.GetComponent<Rigidbody>().AddForce(rb.velocity, ForceMode.VelocityChange);
                child.GetComponent<AmmoColor>().SetAmmoColor(playerColor.instancesBlock);
            }
            FindObjectOfType<GameManager>().PlayerIsDead();
            Destroy(gameObject);
        }
    }

    #endregion

    #region Gestion des boucliers

    public void ChangeShield(bool gain) // Ajoute un boulier si true, enlève un bouclier si false
    {
        shield += gain ? 1 : -1;
        ShieldColor(shield * shieldIntensityModifier);
    }

    private void ShieldColor(float shieldIntensity)
    {
        playerRenderer.GetPropertyBlock(block);
        block.SetColor("_ShieldColor", new Color(shieldColor.r * shieldIntensity, shieldColor.g * shieldIntensity, shieldColor.b * shieldIntensity));
        playerRenderer.SetPropertyBlock(block);
    }

    #endregion

}
