using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuSushi : MonoBehaviour
{
    //serialized
    [Header("Sprites")]
    [SerializeField] private Sprite[] Sushis;
    [SerializeField] private Sprite[] Faces;

    //private
    private Image Sushi;
    private Image Face;

    void Start()
    {
        //get images
        Sushi = GetComponent<Image>();
        Face = transform.GetChild(0).GetComponent<Image>();

        //set sushi from sushis
        RandomizeSushi(Sushis, Sushi);

        //initialise button if it exists
        Button sushiButton = GetComponent<Button>();
        if (sushiButton != null)
        {
            sushiButton.onClick.AddListener(OnSushiClick);
        }
        else
        {
            Debug.LogError("no button.");
        }
    }

    void RandomizeSushi(Sprite[] images, Image image)
    {
        if (images.Length > 0)
        {
            //get random number
            int randomIndex = Random.Range(0, images.Length);
            //set sushi to random sushi
            image.sprite = images[randomIndex];
        }
        else
        {
            Debug.LogError("no images :(.");
        }
    }

    void OnSushiClick()
    {
        //activate child and show face
        transform.GetChild(0).gameObject.SetActive(true);
        RandomizeSushi(Faces, Face);
    }

    //main menu buttons
    public void PlayORQuit(string option) 
    {
        if(option == "Play")
        {
            SceneManager.LoadScene("Game");
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