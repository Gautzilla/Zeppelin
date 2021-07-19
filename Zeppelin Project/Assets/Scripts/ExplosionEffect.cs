using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{

    #region Déclaration des variables

    [Header ("Prefabs")]
    public GameObject sphere;
    public ParticleSystem particle;
    public Light explosionLight;

    private Renderer sphereRenderer;
    private MaterialPropertyBlock block;

    [Header("Light Parameters")]

    public float explosionLightStrength = 10f;
    public float explosionLightDecay = 20f;

    [Header("Size Parameters")]

    public float maxSize = 0f;
    public float growthSpeed = 0f;
    public float growthDuration = 0f;
    public float progressionSpeed = 0f;
    private float explosionDuration;

    #endregion

    #region Initialisation

    private void Start()
    {
        sphereRenderer = sphere.GetComponent<Renderer>();
        block = new MaterialPropertyBlock();

        explosionDuration = particle.main.duration;
        sphere.transform.localScale = new Vector3(0f, 0f, 0f);

        StartCoroutine("ExplosionGrowth");
    }

    #endregion

    #region Evolution de l'explosion
    IEnumerator ExplosionGrowth()
    {
        float t = 0f;
        Vector3 sphereScale;
        float newScale = 0f;
        float progression = 1f;
        explosionLight.intensity = explosionLightStrength;

        while (t < explosionDuration)
        {
            newScale = Mathf.SmoothDamp(sphere.transform.localScale.x, maxSize, ref growthSpeed, growthDuration);
            sphereScale = new Vector3(newScale, newScale, newScale);
            sphere.transform.localScale = sphereScale;

            sphereRenderer.GetPropertyBlock(block);
            progression += progressionSpeed * Time.deltaTime;
            block.SetFloat("_Progression", progression); // Paramètre du Shader, voir ShaderGraph
            sphereRenderer.SetPropertyBlock(block);

            explosionLight.intensity = explosionLightStrength - t * explosionLightDecay;
            t += 1f * Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }

    #endregion

}
