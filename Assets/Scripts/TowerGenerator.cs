using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    //serialized
    [Header("Piece")]
    [SerializeField] private GameObject piecePrefab; 

    //private
    private GameObject pieceObj;
    private Vector3 spawnPosition;
    private int rotationPicker = 0;
    private GameObject towerParent;
    private int currentHeight = 0;

    public void GenerateTower(int TowerHeight)
    {
        //destroy tower 
        Destroy(towerParent);
        //reset variables
        currentHeight = 0;
        spawnPosition = transform.position; // Initial spawn position
        towerParent = new GameObject("Tower"); // Create a parent object for the entire tower
        towerParent.tag = "Tower";

        //create a layer for each level of tower
        for (int i = 0; i < TowerHeight; i++)
        {
            //create layer
            GameObject layerParent = new GameObject("Layer " + i); 
            layerParent.transform.SetParent(towerParent.transform);
            //rotation will be 0 initially
            if (rotationPicker == 0)
            {
                //tag so we know what direction to push in interactor
                layerParent.tag = "o";
                //create 3 pieces for layer
                for (int j = 0; j < 3; j++)
                {
                    //calculate spawn pos
                    spawnPosition = new Vector3(0,currentHeight,j);
                    //create piece
                    pieceObj = Instantiate(piecePrefab, spawnPosition, Quaternion.identity);
                    pieceObj.transform.SetParent(layerParent.transform); 
                    //rotate gameObject
                    pieceObj.transform.rotation = Quaternion.Euler(0, 0, 0);

                    //create mesh with correct rotation
                    PieceGenerator pieceGenerator = pieceObj.GetComponent<PieceGenerator>();
                    if (pieceGenerator != null)
                    {
                        pieceGenerator.CreatePiece(Quaternion.Euler(0, 0, 0));
                    }
                }
                //set rotation for next layer
                rotationPicker = 1;
                //increment height
                currentHeight += 1;
            }
            //every other layer
            else if (rotationPicker == 1)
            {
                //tag for interactor
                layerParent.tag = "x";
                for (int o = 0; o < 3; o++)
                {
                    spawnPosition = new Vector3(o-1,currentHeight,1);
                    pieceObj = Instantiate(piecePrefab, spawnPosition, Quaternion.identity);
                    pieceObj.transform.SetParent(layerParent.transform);
                    pieceObj.transform.rotation = Quaternion.Euler(0, 90.0f, 0);

                    PieceGenerator pieceGenerator = pieceObj.GetComponent<PieceGenerator>();
                    if (pieceGenerator != null)
                    {
                        pieceGenerator.CreatePiece(Quaternion.Euler(0, 90.0f, 0));
                    }
                }
                rotationPicker = 0;
                currentHeight += 1;
            }
        }
    }
}
