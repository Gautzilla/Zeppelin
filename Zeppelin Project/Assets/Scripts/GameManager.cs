using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{

    #region Déclaration des variables

    [Header("Players")]
    public int numberOfPlayers;
    public GameObject players;
    public GameObject playerPrefab;
    static public string[] playerNames = new string[4] { "Player 1", "Player 2", "Player 3", "Player 4" };
    public GameObject[] playersUI = new GameObject[4];
    public PlayerInput controls;
    int numberOfKeyboardUsed = 0;
    int numberOfGamepadUsed = 0;
    public int deadPlayers = 0;
    Vector3[] playerPosition;
    Vector3[] playerRotation;

    [Header("GameManagement")]
    public bool gameHasEnded = false;
    public GameObject pauseMenu;
    bool gameIsPaused = false;
    public GameObject gameResults;

    int numberOfDestroyedAmmo = 0;
    public List<GameObject> destroyedAmmoList = new List<GameObject>();
    bool pauseDelayOk = true;

    #endregion

    #region Création des joueurs
    private void Awake()
    {
        numberOfDestroyedAmmo = 0;

        numberOfPlayers = Settings.numberOfPlayers;

        for (int i = 0; i < numberOfPlayers; i++)
        {
            CreatePlayer(i);
        }

        for (int i = numberOfPlayers; i < 4; i++)
        {
            playersUI[i].SetActive(false);
        }
    }


    void CreatePlayer(int i)
    {
        int scheme = 0; // Type de contrôles (clavier ou manette)
        if (i < Gamepad.all.Count)
        {
            scheme = 1;
        }

        switch (scheme) // Assignation du contrôleur au joueur
        {
            case 0:
                controls = PlayerInput.Instantiate(playerPrefab, i, "Keyboard" + numberOfKeyboardUsed.ToString(), pairWithDevices: Keyboard.current);
                numberOfKeyboardUsed++;
                break;
            case 1:
                controls = PlayerInput.Instantiate(playerPrefab, i, "Gamepad", pairWithDevices: Gamepad.all[numberOfGamepadUsed]);
                numberOfGamepadUsed++;
                break;
            default:
                break;
        }

        controls.gameObject.GetComponent<PlayerMovement>().playerIdentifier = i;
        controls.gameObject.transform.parent = players.transform; // Assignation du joueur en enfant du GameObject players

        // Positions initiales des joueurs (Arène ou Course).

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                playerPosition = new Vector3[] { new Vector3(0f, 6.5f, -13f), new Vector3(0f, 6.5f, 13f), new Vector3(13f, 6.5f, 0f), new Vector3(-13f, 6.5f, 0f) };
                playerRotation = new Vector3[] { new Vector3(0f, 0f, 0f), new Vector3(0f, 180f, 0f), new Vector3(0f, -90f, 0f), new Vector3(0f, 90f, 0f) };
                break;
            case 2:
                playerPosition = new Vector3[] { new Vector3(-2f, 6.5f, 0f), new Vector3(1f, 6.5f, 0f), new Vector3(-1f, 6.5f, -5f), new Vector3(2f, 6.5f, -5f) };
                playerRotation = new Vector3[] { new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f) };
                break;
            default:
                break;
        }

        controls.gameObject.transform.position = playerPosition[i];
        controls.gameObject.transform.rotation = Quaternion.Euler(playerRotation[i]);
    }

    #endregion

    #region Gestion de la partie

    public void PlayerIsDead()
    {
        deadPlayers++;

        if (deadPlayers >= numberOfPlayers - 1)
        {
            Invoke("EndGame", 2f);
        }
    }

    public void EndGame()
    {
        gameHasEnded = true;
        gameResults.SetActive(true);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        if (!pauseDelayOk) // Délai entre deux pauses consécutives car tous les joueurs claviers ont la même touche pause
            return;

        StartCoroutine(PauseDelay());

        if (!gameIsPaused)
        {
            Time.timeScale = 0f;
            gameIsPaused = true;
            pauseMenu.SetActive(true);

        } else
        {
            Time.timeScale = 1f;
            gameIsPaused = false;
            pauseMenu.SetActive(false);
        }
    }

    IEnumerator PauseDelay()
    {
        pauseDelayOk = false;
        yield return new WaitForSecondsRealtime(0.1f); // RealTime pour ne pas dépendre de Time.timeScale
        pauseDelayOk = true;
        yield break;
    }

    public void RemoveDestroyedAmmo(GameObject destroyedAmmo)
    {

        destroyedAmmoList.Add(destroyedAmmo);
        numberOfDestroyedAmmo++;
        if (numberOfDestroyedAmmo >= Settings.maxNumberOfDestroyedAmmo) // Détruit les résidus de munitions pour libérer de la ressource
        {
            Destroy(destroyedAmmoList[0]);
            destroyedAmmoList.RemoveAt(0);
        }

    }

    #endregion

}
