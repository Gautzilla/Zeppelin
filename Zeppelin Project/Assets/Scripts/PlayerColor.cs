using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    #region Déclaration des variables

    int playerIdentifier;

    public MaterialPropertyBlock playerBlock;
    public MaterialPropertyBlock instancesBlock; // Définit la couleur des instances faites par le joueur (projectiles, débris...)
    private Renderer playerRenderer;

    // Apparition du joueur
    public float EdgeIntensity;
    public float DissolveSpeed;

    #endregion

    #region Initialisation

    void Start()
    {
        playerBlock = new MaterialPropertyBlock();
        instancesBlock = new MaterialPropertyBlock();

        playerRenderer = gameObject.GetComponent<Renderer>();
        playerIdentifier = GetComponent<PlayerMovement>().playerIdentifier;

        Color playerColor = Settings.configurableColors[Settings.playerColors[playerIdentifier]]; // Récupère la couleur du joueur dans les paramètres

        float intensityFactor = Mathf.Pow(2, EdgeIntensity);
        Color playerEdgeColor = new Color(playerColor.r * intensityFactor, playerColor.g * intensityFactor, playerColor.b * intensityFactor); // Couleurs HDR en bordure de l'animation


        // Paramètres du shader dans Shader Graph
        playerBlock.SetColor("_MainColor", playerColor);
        playerBlock.SetColor("_EdgeColor", playerEdgeColor);
        playerRenderer.SetPropertyBlock(playerBlock);

        instancesBlock.SetColor("_BaseColor", playerColor);

        StartCoroutine("FadeInPlayer");
    }

    #endregion

    #region Animation d'apparition du joueur

    IEnumerator FadeInPlayer()
    {
        float fadeProgress = 1f;

        while (fadeProgress > -0.2f)
        {
            playerRenderer.GetPropertyBlock(playerBlock);
            playerBlock.SetFloat("_DissolveProgression", fadeProgress); // Paramètre du shader, voir Shader Graph
            playerRenderer.SetPropertyBlock(playerBlock);

            fadeProgress -= DissolveSpeed;

            yield return null;
        }
    }

    #endregion
}
