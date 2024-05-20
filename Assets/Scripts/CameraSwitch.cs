using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//script for switching between cameras in scene
public class CameraSwitch : MonoBehaviour
{
    //serialized
    [Header("Cameras")]
    [SerializeField] private Camera initialCamera;
    [SerializeField] private Camera otherCamera;
    [SerializeField] private Camera transitionCamera;
    
    [Header("Other needed bits")]
    [SerializeField] private Button switchButton;
    [SerializeField] private Transform lookAtTarget;

    //private
    //transition settings
    private float transitionDuration = 1.0f;
    private Transform transitionCentre;
    private Camera currentCamera;
    private bool isSwitched = false;
    private bool rotateClockwise = true;

    private void Start()
    {
        //set starting variables
        currentCamera = initialCamera;
        initialCamera.gameObject.SetActive(true);
        otherCamera.gameObject.SetActive(false);
        transitionCamera.gameObject.SetActive(false);
    }


    //function that calls the switch
    public void SwitchCameras()
    {
        //set the transition centre to the tower if its not already set
        if(transitionCentre == null)
        {
            transitionCentre = GameObject.FindGameObjectWithTag("Tower").transform;
        }
        //start switch
        StartCoroutine(TransitionCoroutine(currentCamera, isSwitched ? initialCamera : otherCamera));
        //flip bools
        isSwitched = !isSwitched;
        rotateClockwise = !rotateClockwise;

    }

    private IEnumerator TransitionCoroutine(Camera fromCamera, Camera toCamera)
    {
        //get rid of button so player cant switch again whilst one is happening
        switchButton.gameObject.SetActive(false);
        //set transition cam to exact pos + rotation of source cam
        transitionCamera.transform.SetPositionAndRotation(fromCamera.transform.position, fromCamera.transform.rotation);
        //disable current cam and activate transition cam
        transitionCamera.gameObject.SetActive(true);
        fromCamera.gameObject.SetActive(false);

        //get starting point and finishing point
        Vector3 startPos = fromCamera.transform.position;
        Vector3 endPos = toCamera.transform.position;
        
        //calculate radius of circle camera will go round in
        float radius = Vector3.Distance(startPos, transitionCentre.position);
        float elapsedTime = 0f;

        //while we're within duration of transition
        while (elapsedTime < transitionDuration)
        {
            //calculate how far along transition
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);
            
            //depending on rotate clockwise acquire angle
            float angle = Mathf.Lerp(0f, rotateClockwise ? 180.0f : -180.0f, t);
            
            //calculates how far to offset camera rotation from previously calculated angle
            Vector3 offset = Quaternion.Euler(0, angle, 0) * (startPos - transitionCentre.position);
            
            //combines previous calculations
            Vector3 circularPos = transitionCentre.position + offset;
            //makes sure camera stays on the same y
            circularPos.y = startPos.y;

            //set cam pos            
            transitionCamera.transform.position = circularPos;
            
            //rotate so it centres on tower
            transitionCamera.transform.rotation = Quaternion.LookRotation((lookAtTarget.position - transitionCamera.transform.position).normalized, Vector3.up);
            
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        //set transition camera to position of finishing camera
        transitionCamera.transform.SetPositionAndRotation(endPos, toCamera.transform.rotation);
        
        //set stuff up for next transition
        transitionCamera.gameObject.SetActive(false);
        toCamera.gameObject.SetActive(true);
        currentCamera = toCamera;
        switchButton.gameObject.SetActive(true);

    }
}
