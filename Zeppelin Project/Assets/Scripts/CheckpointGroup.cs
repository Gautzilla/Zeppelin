using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointGroup : MonoBehaviour
{

    #region Déclaration des variables

    public int lastActiveCheckpoint; // Checkpoint actif pour le joueur en tête
    public int[] playersCheckpoints; // Checkpoint actif pour chaque joueur

    public int[] playersRank;

    private Transform[] checkpoints;
    public Transform[] players;

    private Vector3[] playersPosition;

    private List<int> playersRanked;

    #endregion

    #region Initialisation

    private void Start()
    {
        lastActiveCheckpoint = 0;
        playersCheckpoints = new int[Settings.numberOfPlayers];
        playersRank = new int[Settings.numberOfPlayers];
        checkpoints = new Transform[transform.childCount];

        playersPosition = new Vector3[players.Length];

        foreach (int i in playersCheckpoints)
        {
            playersCheckpoints[i] = 0;
            playersRank[i] = 0;
        }

        foreach (Transform child in transform)
        {
            if (child.GetComponentInChildren<Checkpoint>() != null)
            {
                checkpoints[child.GetComponentInChildren<Checkpoint>().checkpointIdentifier - 1] = child; // Ordonne les checkpoints dans le tableau checkpoints
            }
        }
    }

    #endregion

    #region Classement des joueurs

    private void LateUpdate()
    {
        CompareCheckpoints(); // Préclasse les joueurs par rapport à leur checkpoint actif

        playersRanked = new List<int>();

        for (int i = 0; i < playersRank.Length; i++)
        {
            if (playersRanked.Count < playersRank.Length)
            {
                List<int> playersWithSameCheckpoints = new List<int>();

                for (int j = 0; j < playersRank.Length; j++)
                {
                    if (i != j)
                    {
                        if (playersRank[i] == playersRank[j])
                        {
                            playersWithSameCheckpoints.Add(j);
                        }
                    }
                }

                if (playersWithSameCheckpoints.Count == 0)
                {
                    playersRanked.Add(i);
                }
                else
                {
                    playersWithSameCheckpoints.Add(i);
                    ComparePositions(playersWithSameCheckpoints);
                }

            }
        }
    }

    private void CompareCheckpoints() // Classe en premier les joueurs qui ont franchi le plus de checkpoints
    {
        for (int player = 0; player < playersRank.Length; player++)
        {
            int playPos = 1;

            for (int p = 0; p < playersRank.Length; p++)
            {
                if (playersCheckpoints[player] < playersCheckpoints[p])
                {
                    playPos++;
                }
            }

            playersRank[player] = playPos;
        }
    }

    private void ComparePositions(List<int> playersToCompare) // Classe en premier les joueurs qui sont le plus proche du checkpoint actif
    {
        int count = 0;

        foreach (Vector3 playerPosition in playersPosition)
        {
            if (players[count] != null)
            {
                playersPosition[count] = players[count].position; // Répertorie les positions des joueurs
            }

            count++;
        }

        for (int i = 0; i < playersToCompare.Count; i++) // Le joueur à partir duquel on compare
        {
            int rank = playersRank[playersToCompare[i]];

            for (int j = 0; j < playersToCompare.Count; j++) // Les joueurs auxquels on compare le joueur i
            {
                if (i != j)
                {
                    int nextCheckpoint = playersCheckpoints[playersToCompare[i]];

                    if ((checkpoints[nextCheckpoint].position - playersPosition[playersToCompare[j]]).magnitude < (checkpoints[nextCheckpoint].position - playersPosition[playersToCompare[i]]).magnitude) // Classe les joueurs qui sont au même checkpoint en fonction de leur distance au suivant
                    {
                        rank++;
                    }
                }
            }

            playersRank[playersToCompare[i]] = rank;
        }

        for (int i = 0; i < playersToCompare.Count; i++)
        {
            playersRanked.Add(playersToCompare[i]);
        }
    }

    #endregion

    #region Paramétrage du checkpoint actif

    public void SetActiveCheckpoint(int checkpoint)
    {
        lastActiveCheckpoint = checkpoint;
    }

    #endregion

}
