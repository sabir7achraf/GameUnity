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
    public UnityEngine.UI.Button retryButton; 
    public UnityEngine.UI.Button quitButton; 


   // Start is called before the first frame update
   void Start()
   {
        GameOver.SetActive(false);
       UIDocument uiDocument = GetComponent<UIDocument>();
       m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealtBar");
       SetHealthValue(1.0f);
        retryButton.onClick.AddListener(OnRetryButton);
        quitButton.onClick.AddListener(OnQuitButton);
   }

    public void ShowGameOver()
    {
        GameOver.SetActive(true); // Montrer le panneau de Game Over
    }

    public void OnRetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recharger la scène actuelle
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


}