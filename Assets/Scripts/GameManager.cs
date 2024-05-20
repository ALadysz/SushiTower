using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//script for managing game logic
public class GameManager : MonoBehaviour
{
    //serialized
    [Header("Tower Creator")]
    [SerializeField] private TowerGenerator towerScript;

    [Header("Panels")]
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private AudioManager audioManager;

    //public but hidden
    [HideInInspector] public float succesfulPushes = 0.0f;
    [HideInInspector] public int succesfulPushesNeeded = 0;
    [HideInInspector] public int thisTowerHeight;
    [HideInInspector] public bool inGame = false;
    [HideInInspector] private int amountOnFloor = 0;
    [HideInInspector] private HashSet<GameObject> countedPieces = new HashSet<GameObject>();

    //private
    private int playOnlyOnce = 0;

    //create tower
    public void SummonTower(int TowerHeight)
    {
        towerScript.GenerateTower(TowerHeight);
        thisTowerHeight = TowerHeight;
    }

    public void Update()
    {
        //if in game
        if (inGame)
        {
            //calculate amount of bricks that need to hit the floor
            int loseAmount = (thisTowerHeight *3)/2;
            //win or lose depending on conditions
            if (loseAmount < amountOnFloor )
            {
                if(playOnlyOnce ==0)
                {
                    Lose();
                    playOnlyOnce++;
                }
            }
            else if(succesfulPushes >= succesfulPushesNeeded  )
            {
                if(playOnlyOnce ==0)
                {
                    Win();
                    playOnlyOnce++;
                }
            }
        }
    }

    //Floor Script functions
    //increases counter + adds piece to hash set (lose)
    public void IncreaseAmountOnFloor(GameObject piece)
    {
        amountOnFloor++;
        countedPieces.Add(piece);
    }

    //checks if piece is within hash set
    public bool HasPieceBeenCounted(GameObject piece)
    {
        return countedPieces.Contains(piece);
    }

    //Interactor function
    //increases succesful pushes (win)
    public void IncreaseSuccesfulPushes()
    {
        succesfulPushes = succesfulPushes +0.5f;
    }

    //GameLoop functions
    private void Lose()
    {
        audioManager.PlayAudioClip("Lose");
        buttonPanel.SetActive(false);
        losePanel.SetActive(true);        
    }

    private void Win()
    {
        audioManager.PlayAudioClip("Win");
        buttonPanel.SetActive(false);
        winPanel.SetActive(true);
    }

    //button function
    public void PlayORQuit(string option) 
    {
        if(option == "Play")
        {
            SceneManager.LoadScene("Game");
        }
        if(option == "Menu")
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if(option == "Quit")
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }  
}
