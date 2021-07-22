using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDisplayButtons : MonoBehaviour
{

    #region Déclaration des variables

    public MenuManager menuManager;
    public Animator animator;

    static public bool animationHasBeenPlayed = false;

    #endregion

    #region Initialisation

    private void Awake()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                if (!animationHasBeenPlayed) // L'animation du sol n'est jouée qu'au lancement du jeu (pas au retour sur le menu)
                {
                    animator.enabled = true;
                    animationHasBeenPlayed = true;
                }
                else
                {
                    gameObject.transform.position = new Vector3(0f, 0f, 0f); // Si l'animation a déja été jouée, le sol est directement placé au centre
                    if (!MenuManager.displaySettings)
                    {
                        DisplayButtons();
                    }
                }
            }
        }

    #endregion

    #region Affichage du menu

    // Fonction appelée par l'animation du sol du menu

    public void DisplayButtons()
    {
        menuManager.Menu();
    }

    #endregion
    
}
