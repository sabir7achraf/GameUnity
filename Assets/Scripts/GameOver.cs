using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject MainMenu;
    public Canvas gameOverCanvas; 

    
    public void ShowMainMenu()
    {
        gameOverCanvas.gameObject.SetActive(false); 
        MainMenu.SetActive(true); 
    }
}
