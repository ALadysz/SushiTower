using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    //private
    private Color clickColour = new Color(0.592f, 1f, 0.965f);
    private static List<ClickableFace> clickablePieces = new List<ClickableFace>();
    private GameManager gameManager;
        
    private class ClickableFace
    {
        public GameObject faceObj;
        public Renderer renderer;
        public Color originalColor;
        public bool isSelected = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Camera CameraObj = gameObject.GetComponent<Camera>(); 
            Ray ray = CameraObj.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Clickable"))
                {
                    ClickableFace clickedFace = GetClickableFace(hit.collider.gameObject);

                    if (clickedFace != null)
                    {
                        if (!clickedFace.isSelected)
                        {
                            DeselectAllFaces();
                            
                            clickedFace.isSelected = true;
                            clickedFace.renderer.material.color = clickColour;
                        }
                    }
                }
            }
        }
    }

    public void PushFacesPiece()
    {
        foreach (ClickableFace obj in clickablePieces)
        {
            if (obj.isSelected)
            {
                Rigidbody rb = obj.faceObj.GetComponentInParent<Rigidbody>();
                if (rb != null)
                {
                    Transform parent = obj.faceObj.transform.parent.parent;

                    if (parent.CompareTag("x"))
                    {
                        rb.AddForce(Vector3.forward * 250.0f, ForceMode.Impulse);
                        GameObject managerObj = GameObject.FindGameObjectWithTag("Manager");
                        gameManager = managerObj.GetComponent<GameManager>();
                        gameManager.IncreaseSuccesfulPushes();
                    }
                    else if (parent.CompareTag("o"))
                    {
                        rb.AddForce(Vector3.right * 250.0f, ForceMode.Impulse);
                        GameObject managerObj = GameObject.FindGameObjectWithTag("Manager");
                        gameManager = managerObj.GetComponent<GameManager>();
                        gameManager.IncreaseSuccesfulPushes();
                    }
                    else
                    {
                        Debug.Log("Wrong Parent: " + parent.name);
                    }
                }
            }
        }

    }

    public void ResetChoice()
    {
        DeselectAllFaces();
    }

    private void DeselectAllFaces()
    {
        foreach (ClickableFace obj in clickablePieces)
        {
            if (obj.isSelected)
            {
                obj.isSelected = false;
                obj.renderer.material.color = obj.originalColor;
            }
        }
    }

    private ClickableFace GetClickableFace(GameObject obj)
    {
        foreach (ClickableFace clickableObj in clickablePieces)
        {
            if (clickableObj.faceObj == obj)
            {
                return clickableObj;
            }
        }

        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            ClickableFace newClickableFace = new ClickableFace
            {
                faceObj = obj,
                renderer = renderer,
                originalColor = renderer.material.color
            };
            clickablePieces.Add(newClickableFace);
            return newClickableFace;
        }

        return null;
    }
}