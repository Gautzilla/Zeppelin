using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPlayer : MonoBehaviour
{

    #region Déclaration des variables

    public Shoot shoot;
    public DestroyOnCrash destroy;

    #endregion

    #region Bonus

    public void ActivateBonus(int bonusIdentifier)
    {
        if (bonusIdentifier <= 2)
        {
            shoot.ChangeAmmoType(bonusIdentifier);
        } else if (bonusIdentifier ==  3)
        {
            destroy.ChangeShield(true); // Ajoute un bouclier au joueur
        }
    }

    #endregion
}
