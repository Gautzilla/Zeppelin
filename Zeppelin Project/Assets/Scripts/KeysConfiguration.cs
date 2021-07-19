using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeysConfiguration : MonoBehaviour
{

    #region Déclaration des variables

    public GameObject playerPrefab;
    public Text[] inputTexts = new Text[5];
    public Settings settings;

    PlayerInput controls;
    InputAction[] inputActions = new InputAction[3];   
    
    private int activePlayer;
    private string controlScheme;

    int offset; // Sélectionne le Control Scheme depuis lequel afficher l'affectation de touches

    #endregion

    #region Récupération et affichage des affectations de touches du joueur

    public void GetActivePlayer()
    {
        activePlayer = settings.playerToConfigure;

        if (activePlayer < Gamepad.all.Count) // Les joueurs manette ont les identifiants les plus faibles
        {
            controlScheme = "Gamepad";
        }
        else
        {
            controlScheme = "Keyboard" + (activePlayer - Gamepad.all.Count).ToString();
        }

        controls = playerPrefab.GetComponent<PlayerInput>();
        inputActions[0] = controls.actions.FindAction("MoveForward");
        inputActions[1] = controls.actions.FindAction("Rotate");
        inputActions[2] = controls.actions.FindAction("Shoot");

        DisplayBindings();
    }

    void DisplayBindings()
    {
        switch (controlScheme)
        {
            case ("Gamepad"):
                offset = 0;
                break;
            case ("Keyboard0"):
                offset = 1;
                break;
            case ("Keyboard1"):
                offset = 2;
                break;
            case ("Keyboard2"):
                offset = 3;
                break;
            case ("Keyboard3"):
                offset = 4;
                break;
            default:
                offset = 0;
                break;
        }

        for (int i = 0; i < 5; i++) // Structure : voir PlayerControls (InputAction)
        {
            switch (i)
            {
                case (0):
                    inputTexts[i].text = inputActions[0].GetBindingDisplayString(2 + 3 * offset);   // Move forward
                    break;
                case (1):
                    inputTexts[i].text = inputActions[0].GetBindingDisplayString(1 + 3 * offset);   // Move backwards
                    break;
                case (2):
                    inputTexts[i].text = inputActions[1].GetBindingDisplayString(1 + 3 * offset);   // Turn left
                    break;
                case (3):
                    inputTexts[i].text = inputActions[1].GetBindingDisplayString(2 + 3 * offset);   // Turn right
                    break;
                case (4):
                    inputTexts[i].text = inputActions[2].GetBindingDisplayString(offset);   // Shoot
                    break;
                default:
                    break;
            }
        }
    }

    #endregion

    #region Modification des affectations de touches

    public void SetKey(string id) // Méthode appelée par les boutons de l'interface. Paramètres : (int) action et (int) direction
    {
        if (id.Length == 2 && int.TryParse(id, out int num))
        {
            int action = int.Parse(id.Substring(0, 1));
            int direction = int.Parse(id.Substring(1, 1));

            inputActions[action].Disable();

            var rebindOperation = inputActions[action].PerformInteractiveRebinding(1 + direction + (action == 2 ? offset - 1 : 3 * offset)) // Action 2 : "shoot" : pas de direction
                       // To avoid accidental input from mouse motion
                       .WithControlsExcluding("Mouse")
                       .OnMatchWaitForAnother(0.1f)
                       .OnComplete(operation =>
                       {
                           DisplayBindings();
                           operation.Dispose();
                       })
                       .Start();
            inputActions[action].Enable();
        }
    }

    #endregion

}
