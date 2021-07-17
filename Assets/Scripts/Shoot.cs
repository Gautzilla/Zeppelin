using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    #region Déclaration des variables

    // Info sur les armes/munitions
    public GameObject[] ammo = new GameObject[2];
    public int typeOfAmmo = 1;
    int specialAmmoShot = 0;
    int specialAmmoStock = 4;
    public Vector3 ammoVelocity;
    float ammoVelocityModifier = 1f;

    // UI
    GameObject UIDisplay;
    PlayerUIDisplay weaponDisplay;
    ShootLoading loadingDisplay;

    // Infos sur le joueur
    public Rigidbody playerRb;
    public PlayerColor playerColor;

    // Temps de recharge
    bool isReadyToShoot = true;
    bool isLoading = false;
    Coroutine load;
    bool stopLoading = false;
    public float reloadTimeAmmo = 1f;
    public float reloadTimeTorpedo = 0.5f;
    float reloadTime;


    #endregion

    #region Initialisation
    private void Start()
    {
        int playerIdentifier = GetComponent<PlayerMovement>().playerIdentifier;
        UIDisplay = GameObject.Find("PlayersUI");
        weaponDisplay = UIDisplay.transform.GetChild(playerIdentifier).GetComponent<PlayerUIDisplay>();
        loadingDisplay = UIDisplay.transform.GetChild(playerIdentifier).GetComponentInChildren<ShootLoading>();

        ChangeAmmoType(typeOfAmmo);
    }

    #endregion

    #region Gestion de l'action "tir", des munitions, du changement d'arme

    public void ShootAction(float value) // Arme de base : on charge le tir. Armes spéciales : on tire direct
    {
        if (value == 0)
        {
            stopLoading = true;
        }
        else
        {
            if (!isLoading)
            {
                switch (typeOfAmmo)
                {
                    case 0:
                        load = StartCoroutine(LoadShoot());
                        break;
                    case 1:
                        ShootTorpedo();
                        break;
                    case 2:
                        ShootBomb();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void CountSpecialAmmoShot()
    {
        if (specialAmmoShot++ >= specialAmmoStock)
        {
            ChangeAmmoType(0);
            reloadTime = 1f;
        }

        weaponDisplay.DisplayAmmo(specialAmmoStock, specialAmmoShot);
    }

    public void ChangeAmmoType(int ammoIdentifier)
    {
        if (isLoading)
        {
            StopCoroutine(load);
            isLoading = false;
        }

        // Initialisation des valeurs avec la nouvelle arme
        reloadTime = reloadTimeAmmo;
        specialAmmoShot = 0;
        typeOfAmmo = ammoIdentifier;

        switch (ammoIdentifier)
        {
            case 1:
                specialAmmoStock = 4;
                reloadTime = reloadTimeTorpedo;
                break;
            case 2:
                specialAmmoStock = 1;
                break;
            default:
                loadingDisplay.DisplayShootLoading(1f);
                break;
        }

        weaponDisplay.DisplayWeaponIcon(ammoIdentifier);
        weaponDisplay.DisplayAmmo(specialAmmoStock, specialAmmoShot);
    }

    #endregion

    #region Tir de base

    IEnumerator LoadShoot()
    {
        isLoading = true;
        stopLoading = false;
        ammoVelocityModifier = 1f;

        while (!stopLoading)
        {
            if (ammoVelocityModifier < 2f)
            {
                ammoVelocityModifier += 1f * Time.deltaTime;
            }
            loadingDisplay.DisplayShootLoading(ammoVelocityModifier);
            yield return null;
        }
        ShootBase(ammoVelocityModifier);
        loadingDisplay.DisplayShootLoading(1f);
        isLoading = false;
    }

    void ShootBase(float velocityModifier)
    {
        if (isReadyToShoot)
        {
            GameObject ammoFired = Instantiate(ammo[typeOfAmmo], transform.position + new Vector3(0, 1f, 0), transform.rotation);

            ammoFired.GetComponent<AmmoColor>().SetAmmoColor(playerColor.instancesBlock);

            ammoFired.GetComponent<Rigidbody>().AddRelativeForce(ammoVelocity * velocityModifier, ForceMode.VelocityChange);
            isReadyToShoot = false;
            loadingDisplay.fillColor(isReadyToShoot);
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        float reload = 0f;

        while (reload < reloadTime)
        {
            reload += 1f * Time.deltaTime;
            yield return null;
        }

        isReadyToShoot = true;
        loadingDisplay.fillColor(isReadyToShoot);
    }

    #endregion

    #region Tir de torpille
    void ShootTorpedo()
    {
        if (isReadyToShoot)
        {
            GameObject ammoFired = Instantiate(ammo[typeOfAmmo], transform.TransformPoint(new Vector3(0f, 0f, 1.8f)), new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));

            ammoFired.GetComponent<AmmoColor>().SetAmmoColor(playerColor.instancesBlock);

            ammoFired.GetComponent<Rigidbody>().AddRelativeForce(0f, 0f, playerRb.velocity.magnitude, ForceMode.VelocityChange);

            CountSpecialAmmoShot();
            isReadyToShoot = false;
            loadingDisplay.fillColor(isReadyToShoot);
            StartCoroutine(Reload());
        }
    }

    #endregion

    #region Tir de bombe
    void ShootBomb()
    {
        if (isReadyToShoot)
        {
            GameObject ammoFired = Instantiate(ammo[typeOfAmmo], transform.TransformPoint(new Vector3(0f, -0.9f, 0f)), new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));

            ammoFired.GetComponent<AmmoColor>().SetAmmoColor(playerColor.instancesBlock);

            ammoFired.GetComponent<Rigidbody>().AddRelativeForce(0f, -5f, 0f, ForceMode.VelocityChange);

            CountSpecialAmmoShot();
            isReadyToShoot = false;
            loadingDisplay.fillColor(isReadyToShoot);
            StartCoroutine(Reload());
        }
    }

    #endregion

}
