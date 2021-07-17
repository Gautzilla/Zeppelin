using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDisplayButtons : MonoBehaviour
{
    public MenuManager menuManager;
    public Animator animator;

    static public bool animationHasBeenPlayed = false;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (!animationHasBeenPlayed)
            {
                animator.enabled = true;
                animationHasBeenPlayed = true;
            }
            else
            {
                gameObject.transform.position = new Vector3(0f, 0f, 0f);
                if (!MenuManager.displaySettings)
                {
                    DisplayButtons();
                }
            }
        }
    }

    public void DisplayButtons()
    {
        menuManager.Menu();
    }
}
