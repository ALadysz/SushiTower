using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class GameSettings : MonoBehaviour
{
    //Serialized
    [Header("Panels")]
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject ButtonPanel;

    [Header("Input Fields")]
    [SerializeField] TMP_InputField towerHeightInput;
    [SerializeField] TMP_InputField winAmountInput;
    //public but hidden
    [HideInInspector] public int TowerHeight;    
    [HideInInspector] public int WinAmount;    

    public void Start()
    {
        //make sure correct stuff is active
        ButtonPanel.SetActive(false);
        SettingsPanel.SetActive(true);
        //initialise input fields
        winAmountInput.onEndEdit.AddListener(SetWinAmount);
        towerHeightInput.onEndEdit.AddListener(SetTowerHeight);
    }

    //sets tower height dependent on input
    public void SetTowerHeight(string height)
    {
        if (int.TryParse(height, out int result))
        {
            TowerHeight = result;
        }
        else
        {
            Debug.LogError("Wrong Input");
        }
    }

    //sets win amount dependent on input
    public void SetWinAmount(string amount)
    {
        if (int.TryParse(amount, out int result))
        {
            WinAmount = result;
        }
        else
        {
            Debug.LogError("Wrong Input");
        }
    }

    //start game
    public void Continue()
    {
        //acquire manager to tell it to start
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        GameManager managerscript = manager.GetComponent<GameManager>();
        managerscript.succesfulPushesNeeded = WinAmount;
        //create tower
        managerscript.SummonTower(TowerHeight);
        //game start so in game
        managerscript.inGame = true;
        //deactivate settings and activate game buttons
        ButtonPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }

}
