using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    #region Déclaration des variables

    public GameObject bomb;

    [Header("Explosion Properties")]
    public float explosionVelocity = 5f;
    public float explosionSmoothTime = 1f;
    public float explosionRadius = 10f;

    [Header("Explosion Fade Properties")]
    public float fadeDelay;
    public float fadeVelocity = 1f;
    public float fadeSmoothTime = 1f;

    private MaterialPropertyBlock expBlock;
    private Renderer expRenderer;

    #endregion

    #region Initialisation

    private void Start()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);
        StartCoroutine("Grow");

        expRenderer = gameObject.GetComponent<Renderer>();
        expBlock = new MaterialPropertyBlock();
        expRenderer.GetPropertyBlock(expBlock);
    }

    #endregion

    #region Evolution de l'onde de choc

    IEnumerator Grow()
    {
        while (transform.localScale.x < explosionRadius - 0.5f)
        {
            float newScale = Mathf.SmoothDamp(transform.localScale.x, explosionRadius, ref explosionVelocity, explosionSmoothTime);
            transform.localScale = new Vector3(newScale, 1f, newScale);

            yield return null;
        }

        StartCoroutine("Fade");
    }


    IEnumerator Fade()
    {
        float alpha = 1f;
        float delay = 0f;

        expRenderer.GetPropertyBlock(expBlock);

        while (delay < fadeDelay)
        {
            delay += 1 * Time.deltaTime;
            yield return null;
        }

        while (alpha > 0.01f)
        {
            alpha = Mathf.SmoothDamp(alpha, 0f, ref fadeVelocity, fadeSmoothTime);

            expBlock.SetFloat("_Alpha", alpha);
            expRenderer.SetPropertyBlock(expBlock);

            yield return null;
        }

        Destroy(bomb.gameObject);
    }

    #endregion

    #region Gestion des collisions

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponentInParent<DestroyOnCrash>().Destroy();
        }
    }

    #endregion
}
