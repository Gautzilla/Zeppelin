using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{

    #region Déclaration des variables

    private Mesh ground;

    public Vector3[] vertices;
    private Vector2[] uvs; // Coordonnées sur la texture pour couleur des tuiles
    private int[] triangles;

    public bool useRandomSettings = true;

    [Header("Heightmap")] // Contrôle du relief (valeurs : contrôle manuel)
    private float[,] heightMap;
    public float noiseScale = 10f; // Zoome sur le bruit de Perlin : points proches plus liés
    public bool discreteSteps = true; 
    public int numberOfSteps = 10; // Si hauteurs discrètes : nombre de valeurs possibles
    private int noiseResolution = 1; // Résolution du bruit généré
    public float heightStrenght = 1f; // Dynamique de hauteur

    // Coordonnées dans le bruit de Perlin
    private float xOffset; 
    private float zOffset;
    private int width;
    private int height;

    [Header("Hexagons")] // Paramètres des tuiles
    public int GroundSize = 32;
    private float hexagonRadius = 1f;
    public Vector3 centerPosition = new Vector3(0f, 0f, 0f);
    public int hexagonMatrixSize = 1; // Nombre de rangées de tuiles
    public int numberOfHexagons; 
    private int[] hexagonTriangles;
    private int[,,] hexagonIndexes;
    public float colorRange;

    [Header("VerticalTriangles")] // Faces qui relient les tuiles
    private List<int> southTriangles = new List<int>();
    private List<int> northWestTriangles = new List<int>();
    private List<int> southWestTriangles = new List<int>();

    #endregion

    #region Création de la grille -- Appel des méthodes
    void Awake()
        {
            ground = new Mesh();
            GetComponent<MeshFilter>().mesh = ground;

            if (useRandomSettings)
            {
                RandomizeMapSettings();
            }

            SetUp();

            heightMap = GenerateHeights();

            hexagonMatrixSize -= 1;

            CreateHexagons();

            ConcatenateTriangles();
            UpdateMesh();

            GetComponent<MeshCollider>().sharedMesh = ground;
        }

    #endregion

    #region Paramètres aléatoires
    private void RandomizeMapSettings()
        {
            hexagonMatrixSize = Random.Range(3, 21);

            colorRange = (Random.Range(1, 9) * 2f - 1f) / 16f; // Les coordonnées des UV sont comprises entre 0 et 1
            discreteSteps = Random.Range(0, 2) == 0;
            discreteSteps = false; // Paramètre désactivé car ça ne rend pas très bien
            if (discreteSteps)
            {
                numberOfSteps = Random.Range(10, 20);
            }
            noiseScale = Random.Range(1f, 5f);
            heightStrenght = Random.Range(2f, 6f);
        }
    #endregion

    #region Taille et relief

    private void SetUp()
    {
        hexagonIndexes = new int[2 * hexagonMatrixSize - 1, 2 * hexagonMatrixSize - 1, 2 * hexagonMatrixSize - 1]; // Coordonnées cubiques

        numberOfHexagons = 1;
        for (int i = 1; i < hexagonMatrixSize; i++)
        {
            numberOfHexagons += 6 * i;
        }

        hexagonRadius = (float)GroundSize / (3 * hexagonMatrixSize - 1); // Calcule la taille des hexagones pour que le sol ait la taille GroundSize quelle que soit la valeur de hexagonMatrixSize

        if (!useRandomSettings)
        {
            colorRange = (colorRange * 2f - 1f) / 16f;
        }

        // Largeur et hauteur totale du sol

        width = Mathf.CeilToInt((3f * hexagonMatrixSize - 1f) * hexagonRadius * noiseResolution);
        height = Mathf.CeilToInt((2f * hexagonMatrixSize - 1f) * Mathf.Sqrt(3f) * hexagonRadius * noiseResolution);
    }

    private float[,] GenerateHeights()
    {
        xOffset = Random.Range(0f, 9999f); // Coordonnées sur le bruit de Perlin
        zOffset = Random.Range(0f, 9999f);

        float[,] heights = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                heights[x, z] = CalculateHeights(x, z);
            }
        }

        return heights;
    }

    private float CalculateHeights(int x, int z)
    {
        float xCoord = (float)x / width * noiseScale + xOffset;
        float zCoord = (float)z / height * noiseScale + zOffset;
        float value = Mathf.PerlinNoise(xCoord, zCoord);

        value = Mathf.Clamp(value, 0f, 1f);

        if (discreteSteps)
        {
            float step = 1f / numberOfSteps;

            if (value < step)
            {
                value = 0f;
            }
            else
            {
                while (value > step)
                {
                    step += 1f / numberOfSteps;
                }

                value = (step * numberOfSteps - 1f) / (numberOfSteps - 1f);
            }
        }

        return value;
    }

    #endregion

    #region Création des vertices et des faces

    private void CreateHexagons() // Calcul des coordonnées des vertices
    {
        vertices = new Vector3[7 * numberOfHexagons]; // Un vertice au centre, un par sommet
        uvs = new Vector2[vertices.Length]; // Même UV pour tous vertices d'un même hexagone
        hexagonTriangles = new int[18 * numberOfHexagons]; // Triangles formés de 3 vertices : 3*6 vertices pour décrire les 6 triangles d'un hexagone

        int hexagonIndex = 0;

        // Calcul des coordonnées de chaque vertice dans un repère cubique

        for (int x = -hexagonMatrixSize; x <= hexagonMatrixSize; x++)
        {
            for (int y = -hexagonMatrixSize; y <= hexagonMatrixSize; y++)
            {
                for (int z = -hexagonMatrixSize; z <= hexagonMatrixSize; z++)
                {
                    if (x + y + z == 0)
                    {
                        int xCoord = x + hexagonMatrixSize;
                        int yCoord = y + hexagonMatrixSize;
                        int zCoord = z + hexagonMatrixSize;

                        hexagonIndexes[xCoord, yCoord, zCoord] = hexagonIndex;

                        // Passage des coordonnées cubiques aux coordonnées cartésiennes

                        float xCart = 3f / 2f * hexagonRadius * (float)x;
                        float zCart = Mathf.Sqrt(3f) * ((float)x / 2f + (float)y) * hexagonRadius;
                        float yCart = heightMap[Mathf.RoundToInt(xCart * noiseResolution + width / 2f), Mathf.RoundToInt(zCart * noiseResolution + height / 2f)];

                        vertices[hexagonIndex * 7] = new Vector3(centerPosition.x + xCart, centerPosition.y + yCart * heightStrenght, centerPosition.z + zCart);  // Place les vertices centraux

                        for (int i = 1; i < 7; i++)
                        {
                            float angle = i * 60f * Mathf.Deg2Rad;
                            float xVert = vertices[hexagonIndex * 7].x + hexagonRadius * Mathf.Cos(angle);
                            float zVert = vertices[hexagonIndex * 7].z + hexagonRadius * Mathf.Sin(angle);

                            vertices[hexagonIndex * 7 + i] = new Vector3(xVert, vertices[hexagonIndex * 7].y, zVert); // Place les sommets
                        }

                        // Calcul des coordonnées des triangles : classe les vertices 3 à 3 pour les relier en triangles

                        for (int i = 0; i < 6; i++)
                        {
                            switch (i)
                            {
                                case 5:
                                    hexagonTriangles[hexagonIndex * 18 + i * 3] = 0 + hexagonIndex * 7;
                                    hexagonTriangles[hexagonIndex * 18 + i * 3 + 1] = 1 + hexagonIndex * 7; // Pour le 6ème triangle, les index ne se suivent plus
                                    hexagonTriangles[hexagonIndex * 18 + i * 3 + 2] = i + 1 + hexagonIndex * 7;
                                    break;
                                default:
                                    hexagonTriangles[hexagonIndex * 18 + i * 3] = 0 + hexagonIndex * 7;
                                    hexagonTriangles[hexagonIndex * 18 + i * 3 + 1] = i + 2 + hexagonIndex * 7;
                                    hexagonTriangles[hexagonIndex * 18 + i * 3 + 2] = i + 1 + hexagonIndex * 7;
                                    break;
                            }
                        }

                        // Assignation de la couleur de la tuile en fonction de sa hauteur

                        for (int i = 0; i < 7; i++)
                        {
                            yCart = Mathf.Clamp(yCart, 0f, 0.99f);
                            uvs[hexagonIndex * 7 + i] = new Vector2(yCart * heightStrenght / 6f, colorRange);
                        }

                        // Création des faces qui relient les différentes tuiles (sauf si tuile de bordure du sol)

                        if (y != -hexagonMatrixSize && z != hexagonMatrixSize)
                        {
                            CreateSouthTriangles(xCoord, yCoord, zCoord);
                        }

                        if (x != -hexagonMatrixSize && y != hexagonMatrixSize)
                        {
                            CreateNorthWestTriangles(xCoord, yCoord, zCoord);
                        }

                        if (x != -hexagonMatrixSize && z != hexagonMatrixSize)
                        {
                            CreateSouthWestTriangles(xCoord, yCoord, zCoord);
                        }

                        hexagonIndex++;
                    }
                }
            }
        }
    }


    private void CreateSouthTriangles(int x, int y, int z)
    {
        int hexagonIndex = hexagonIndexes[x, y, z];
        int southernHexagonIndex = hexagonIndexes[x, y - 1, z + 1];

        Vector3 currentHexagonPosition = vertices[hexagonIndex * 7];
        Vector3 southernHexagonPosition = vertices[southernHexagonIndex * 7];

        if (currentHexagonPosition.y != southernHexagonPosition.y) // Si les hexagones sont à la même hauteur : pas besoin de les relier par une face verticale
        {
            southTriangles.Add(southernHexagonIndex * 7 + 1);
            southTriangles.Add(hexagonIndex * 7 + 4);
            southTriangles.Add(hexagonIndex * 7 + 5);

            southTriangles.Add(southernHexagonIndex * 7 + 1);
            southTriangles.Add(southernHexagonIndex * 7 + 2);
            southTriangles.Add(hexagonIndex * 7 + 4);
        }
    }
    private void CreateNorthWestTriangles(int x, int y, int z)
    {
        int hexagonIndex = hexagonIndexes[x, y, z];
        int northWesternHexagonIndex = hexagonIndexes[x - 1, y + 1, z];

        Vector3 currentHexagonPosition = vertices[hexagonIndex * 7];
        Vector3 northWesternHexagonPosition = vertices[northWesternHexagonIndex * 7];

        if (currentHexagonPosition.y != northWesternHexagonPosition.y)
        {
            northWestTriangles.Add(northWesternHexagonIndex * 7 + 6);
            northWestTriangles.Add(hexagonIndex * 7 + 2);
            northWestTriangles.Add(hexagonIndex * 7 + 3);

            northWestTriangles.Add(northWesternHexagonIndex * 7 + 5);
            northWestTriangles.Add(northWesternHexagonIndex * 7 + 6);
            northWestTriangles.Add(hexagonIndex * 7 + 3);
        }
    }

    private void CreateSouthWestTriangles(int x, int y, int z)
    {
        int hexagonIndex = hexagonIndexes[x, y, z];
        int southWesternHexagonIndex = hexagonIndexes[x - 1, y, z + 1];

        Vector3 currentHexagonPosition = vertices[hexagonIndex * 7];
        Vector3 southWesternHexagonPosition = vertices[southWesternHexagonIndex * 7];

        if (currentHexagonPosition.y != southWesternHexagonPosition.y)
        {
            southWestTriangles.Add(southWesternHexagonIndex * 7 + 1);
            southWestTriangles.Add(hexagonIndex * 7 + 3);
            southWestTriangles.Add(hexagonIndex * 7 + 4);

            southWestTriangles.Add(southWesternHexagonIndex * 7 + 6);
            southWestTriangles.Add(southWesternHexagonIndex * 7 + 1);
            southWestTriangles.Add(hexagonIndex * 7 + 4);
        }
    }

    private void ConcatenateTriangles() // Après avoir calculé les coordonnées de tous les vertices, on les concatène pour les regrouper en tant que triangles du mesh ground
    {
        int numbOfTri = hexagonTriangles.Length + southTriangles.Count + northWestTriangles.Count + southWestTriangles.Count;

        triangles = new int[numbOfTri];

        for (int i = 0; i < hexagonTriangles.Length; i++)
        {
            triangles[i] = hexagonTriangles[i];
        }

        for (int i = 0; i < southTriangles.Count; i++)
        {
            triangles[i + hexagonTriangles.Length] = southTriangles[i];
        }

        for (int i = 0; i < northWestTriangles.Count; i++)
        {
            triangles[i + hexagonTriangles.Length + southTriangles.Count] = northWestTriangles[i];
        }

        for (int i = 0; i < southWestTriangles.Count; i++)
        {
            triangles[i + hexagonTriangles.Length + southTriangles.Count + northWestTriangles.Count] = southWestTriangles[i];
        }
    }

    private void UpdateMesh()
    {
        ground.Clear();

        ground.vertices = vertices;
        ground.uv = uvs;
        ground.triangles = triangles;

        ground.RecalculateNormals();
    }

    #endregion

}
