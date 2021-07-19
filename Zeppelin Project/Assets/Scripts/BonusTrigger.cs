using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTrigger : MonoBehaviour
{

    #region Déclaration des variables

    public float rotationSpeed = 100f;
    public int bonusIdentifier;

    private MaterialPropertyBlock block;
    private Renderer bonusRenderer;

    public GameObject bonusSpawner;

    #endregion

    #region Initialisation

    private void Start()
    {
        block = new MaterialPropertyBlock();
        bonusRenderer = GetComponent<Renderer>();
        bonusSpawner = GameObject.Find("BonusSpawner");

        // Place le bonus à la hauteur des joueurs (dépendant de la hauteur de la tuile)

        RaycastHit hit;
        Ray downRay = new Ray (transform.position, -bonusSpawner.transform.up);

        if (Physics.Raycast(downRay, out hit))
        {
            transform.position = new Vector3 (transform.position.x, transform.position.y - hit.distance + Settings.playerHeight, transform.position.z);
        }

    }

    #endregion

    #region Apparition

    public void MakeBonusReady()
    {
        StartCoroutine("MakeBonusAppear");
    }

    IEnumerator MakeBonusAppear()
    {
        float progress = 1.2f;

        while (progress >= -0.1f)
        {
            block.SetFloat("_Appearing", progress); // Propriété du shader - voir ShaderGraph
            bonusRenderer.SetPropertyBlock(block);

            progress -= 1f * Time.deltaTime;

            yield return null;
        }

        GetComponent<Collider>().enabled = true;
    }

    #endregion

    #region Rotation et trigger

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponentInParent<BonusPlayer>().ActivateBonus(bonusIdentifier);
            Destroy(gameObject);
        }
    }

    #endregion
}
