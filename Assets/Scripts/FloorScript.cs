using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script for checking when pieces fall on the floor
public class FloorScript : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        //get game manager
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //checks if piece has been counted before
        if (!gameManager.HasPieceBeenCounted(collision.gameObject))
        {
            //counts piece if it hasnt
            gameManager.IncreaseAmountOnFloor(collision.gameObject);
        }
    }
}
