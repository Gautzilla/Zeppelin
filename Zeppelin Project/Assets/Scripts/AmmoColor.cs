using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoColor : MonoBehaviour
{

    #region Déclaration des variables

    private Renderer ammoRenderer;
    public MaterialPropertyBlock ammoBlock;
    private Color playerColor;

    #endregion

    #region Paramétrage de la couleur du projectile

    // Cette méthode est appelée avant Start() lorsqu'on istancie un projectile ou un résidu de projectile
    public void SetAmmoColor(MaterialPropertyBlock playerBlock)
    {
        playerColor = playerBlock.GetColor("_BaseColor");
    }

    #endregion

    #region Application de la couleur lors de l'instanciation

    void Start()
    {
        ammoRenderer = gameObject.GetComponent<Renderer>();
        ammoBlock = new MaterialPropertyBlock();
        ammoBlock.SetColor("_BaseColor", playerColor); // Donne aux projectiles la même couleur que celle du joueur
        ammoRenderer.SetPropertyBlock(ammoBlock);
    }

    #endregion

}
