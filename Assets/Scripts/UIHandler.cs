using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIHandler : MonoBehaviour
{
    private VisualElement m_Healthbar;
    public GameObject GameOver;
    public GameObject PlayBar;
    public UnityEngine.UI.Button retryButton; 
    public UnityEngine.UI.Button quitButton; 
    public UnityEngine.UI.Button playButton; 

     private RoomFirstDungeonGenerator dungeonGenerator;


   // Start is called before the first frame update
   void Start()
   {
        PlayBar.SetActive(true);
         dungeonGenerator = FindObjectOfType<RoomFirstDungeonGenerator>();
       UIDocument uiDocument = GetComponent<UIDocument>();
       m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealtBar");
       SetHealthValue(1.0f);
        retryButton.onClick.AddListener(OnRetryButton);
        playButton.onClick.AddListener(OnplayButton);
        quitButton.onClick.AddListener(OnQuitButton);
   }

    public void ShowGameOver()
    {
        GameOver.SetActive(true); // Montrer le panneau de Game Over
    }

    public void OnRetryButton()
    {
        // Appelle la méthode pour relancer la génération de la carte
        dungeonGenerator.ReplayDungeon();

        // Cachez le bouton après avoir cliqué dessus
        GameOver.SetActive(false);
    }

    public void OnQuitButton()
    {
         Debug.Log("Quitter le jeu");
        Application.Quit();

    }
   public void SetHealthValue(float percentage)
   {
       m_Healthbar.style.width = Length.Percent(100 * percentage);
   }
   public void OnplayButton(){
    dungeonGenerator.ReplayDungeon();
    PlayBar.SetActive(false);
   }
}