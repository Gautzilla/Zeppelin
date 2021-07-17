using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{

    #region Déclaration des variables

    public List<Animator> animators = new List<Animator> { }; // Animations des boutons à l'apparition
    public List<ButtonManager> otherManagers = new List<ButtonManager> { };
    public GameObject firstButtonToSelect;

    public bool buttonsAreDisplayed = false;
    public bool openButtonsOnStart = false; // Pour les boutons du menu

    public bool canGoBack = true; // Pour les managers de boutons imbriqués (exemple : dans les paramètres)
    public GameObject objectToGoBack;

    #endregion

    #region Initialisation

    private void Start()
    {
        if (openButtonsOnStart)
        {
            ManageAnimator(true);
        }   else
        {
            foreach (Animator animator in animators)
            {
                animator.SetFloat("Speed", 0f);
            }
        }

        foreach (Animator animator in animators)
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime; // Pour les boutons du menu pause
        }
    }

    #endregion

    #region Contrôle des boutons
    private void LateUpdate()
    {
        bool back = false;

        if (buttonsAreDisplayed && canGoBack)
        {
            foreach (Gamepad pad in Gamepad.all)
            {
                if (pad.bButton.wasPressedThisFrame)
                {
                    back = true;
                }
            }

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                back = true;
            }

            if (back)
            {
                SwitchAnimationState();
                EventSystem.current.SetSelectedGameObject(objectToGoBack);
            }
        }
    }

    public void SwitchAnimationState()
    {
        if (!buttonsAreDisplayed)
        {
            if (otherManagers.Count > 0)
            {
                foreach (ButtonManager otherManager in otherManagers)
                {
                    if (otherManager.buttonsAreDisplayed)
                    {
                        otherManager.ManageAnimator(false);
                    }
                }
            }
        }
        ManageAnimator(!buttonsAreDisplayed);
    }

    #endregion

    #region Apparition / Disparition des boutons

    public void ManageAnimator(bool state)
    {
        if (state)
        {
            foreach (Animator animator in animators)
            {
                animator.SetFloat("Speed", 1f);
                animator.Play("MenuButton", -1, 0f);
            }
            buttonsAreDisplayed = true;
            EventSystem.current.SetSelectedGameObject(firstButtonToSelect);
        }
        else
        {
            foreach (Animator animator in animators)
            {
                animator.SetFloat("Speed", -1f);
                animator.Play("MenuButton", -1, 1f);
            }
            buttonsAreDisplayed = false;
        }

    }

    #endregion
}
