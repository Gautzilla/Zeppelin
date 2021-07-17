using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{

    int playerIdentifier;

    public MaterialPropertyBlock playerBlock;
    public MaterialPropertyBlock instancesBlock;

    private Renderer playerRenderer;
    public float EdgeIntensity;
    public float DissolveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerBlock = new MaterialPropertyBlock();
        instancesBlock = new MaterialPropertyBlock();

        playerRenderer = gameObject.GetComponent<Renderer>();

        playerIdentifier = GetComponent<PlayerMovement>().playerIdentifier;

        Color playerColor = Settings.configurableColors[Settings.playerColors[playerIdentifier]];

        float intensityFactor = Mathf.Pow(2, EdgeIntensity);
        Color playerEdgeColor = new Color(playerColor.r * intensityFactor, playerColor.g * intensityFactor, playerColor.b * intensityFactor);

        playerBlock.SetColor("_MainColor", playerColor);
        playerBlock.SetColor("_EdgeColor", playerEdgeColor);
        playerRenderer.SetPropertyBlock(playerBlock);

        instancesBlock.SetColor("_BaseColor", playerColor);

        StartCoroutine("FadeInPlayer");
    }

    IEnumerator FadeInPlayer()
    {
        float fadeProgress = 1f;

        while (fadeProgress > -0.2f)
        {
            playerRenderer.GetPropertyBlock(playerBlock);
            playerBlock.SetFloat("_DissolveProgression", fadeProgress);
            playerRenderer.SetPropertyBlock(playerBlock);

            fadeProgress -= DissolveSpeed;

            yield return null;
        }
    }
}
