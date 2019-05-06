using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public void ChangeSceneBtn(int sceneindex)
    {
        Application.LoadLevel(sceneindex);
        
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
