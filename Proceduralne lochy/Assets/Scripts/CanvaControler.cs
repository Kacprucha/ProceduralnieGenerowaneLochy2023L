using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvaControler : MonoBehaviour
{
    [SerializeField]
    public GameObject canvasObj;

    [SerializeField]
    public GameObject defeatCanvas;

    //This will check if your game is paused (we'll set it)
    bool gamePaused = false;


    void Update ()
    {
        //Reading input for ESCAPE key, and by saying gamePaused = !gamePaused, we switch the bool on and off each time the Keycode is registered!
        if (Input.GetKeyDown (KeyCode.Escape))
            gamePaused = !gamePaused;

        //Now we enable and disable the game object!
        if (gamePaused)
            canvasObj.SetActive (true);
        else
            canvasObj.SetActive (false);

    }
}
