using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBonus : MonoBehaviour
{

    public GroundGenerator groundGenerator;

    [Header("Bonus popup settings")]
    public float maxRadius = 16f;
    public float minTime = 5f;
    public float maxTime = 15f;
    private bool bonusIsLoading = false;
    private float popupTime;
    public List<int> popupIndex;

    [Header("Bonus type ratio (out of 10 bonuses)")]
    public int torpedoRatio = 5;
    public int bombRatio = 3;
    public int shieldRatio = 2;
    
    [Header("Bonus GameObjects")]
    public GameObject[] bonus = new GameObject[1];
    public float bonusStartinHeight = 10f;

    private void Start()
    {
        RandomizeList();
    }

    void Update()
    {
        if(!bonusIsLoading)
        {
            bonusIsLoading = true;

            int popupHexagonIndex = popupIndex[0];
            popupIndex.RemoveAt(0);

            if (popupIndex.Count == 0)
            {
                RandomizeList();
            }

            Vector3 popupHexagonPosition = groundGenerator.vertices[popupHexagonIndex * 7];
            popupHexagonPosition = new Vector3(popupHexagonPosition.x, popupHexagonPosition.y + Settings.playerHeight, popupHexagonPosition.z);

            popupTime = Random.Range(minTime, maxTime);

            StartCoroutine(PopBonus(popupTime, popupHexagonPosition));
        }
    }

    IEnumerator PopBonus(float popupTime, Vector3 popupPosition)
    {
        float invokeTime = 0f;
        int bonusIdentifier = TypeOfBonus();

        while (invokeTime <= popupTime)
        {
            invokeTime += 1f * Time.deltaTime;
            yield return null;
        }

        Instantiate(bonus[bonusIdentifier], popupPosition, Quaternion.Euler(50f, 0f, 0f));
        bonusIsLoading = false;
    }

    private int TypeOfBonus()
    {
        int rand = Random.Range(1, 10);

        if (rand <= torpedoRatio)
        {
            return 0;
        } else if (rand <= torpedoRatio + bombRatio)
        {
            return 1;
        } else if (rand <= torpedoRatio + bombRatio + shieldRatio)
        {
            return 2;
        } else
        {
            return 0;
        }
    }

    private void RandomizeList()
    {
        popupIndex = new List<int> { };

        for (int i = 0; i < groundGenerator.numberOfHexagons; i++)
        {
            popupIndex.Add(i);
        }

        for (int i = 0; i < popupIndex.Count; i++)
        {
            int temp = popupIndex[i];
            int randomIndex = Random.Range(i, popupIndex.Count);

            popupIndex[i] = popupIndex[randomIndex];
            popupIndex[randomIndex] = temp;
        }
    }
}
