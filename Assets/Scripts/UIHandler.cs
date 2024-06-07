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
    public GameObject Grid;
    public GameObject PlayBar;
    public GameObject DijekstraBar;
    public GameObject AstarBar;

    public UnityEngine.UI.Button retryButton; 
    public UnityEngine.UI.Button quitButton; 
    public UnityEngine.UI.Button playButton; 
    public UnityEngine.UI.Button dijekstraButton; 
    public UnityEngine.UI.Button AstarButton; 

    private RoomFirstDungeonGenerator dungeonGenerator;
    private RoomPathFind dungeonPathFind;
    private FollowDjikstra followDijkstra;
    private FollowpathAstar followAstar; 
    public GameObject player;

    void Start()
    {
        PlayBar.SetActive(true);
        dungeonGenerator = FindObjectOfType<RoomFirstDungeonGenerator>();
        dungeonPathFind = FindObjectOfType<RoomPathFind>();
         followDijkstra = player.GetComponent<FollowDjikstra>();
         followAstar = player.GetComponent<FollowpathAstar>();
        UIDocument uiDocument = GetComponent<UIDocument>();
        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealtBar");
        // Désactiver la barre de santé au début
        m_Healthbar.style.display = DisplayStyle.None;
        retryButton.onClick.AddListener(OnRetryButton);
        playButton.onClick.AddListener(OnplayButton);
        quitButton.onClick.AddListener(OnQuitButton);
       dijekstraButton.onClick.AddListener(OndijekstraButton);
       AstarButton.onClick.AddListener(OnAstarButton);

    }
    public void ShowGameOver()
    {
        Grid.SetActive(false);
        AstarBar.SetActive(false);
        DijekstraBar.SetActive(false);
        GameOver.SetActive(true);
         // Montrer le panneau de Game Over
    }

    public void OnRetryButton()
    {
        Grid.SetActive(true);
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

    public void OnplayButton()
    {
       Grid.SetActive(true);
        dungeonGenerator.ReplayDungeon();
        PlayBar.SetActive(false);

        // Activer la barre de santé
        m_Healthbar.style.display = DisplayStyle.Flex;
    }

        public void OndijekstraButton()
    {
    Grid.SetActive(true);
    DijekstraBar.SetActive(true);
    PlayBar.SetActive(false);
            if (followDijkstra != null)
        {
            followDijkstra.enabled = true; // Active le script FollowDijkstra
        }
    }
            public void OnAstarButton()
    {
        Grid.SetActive(true);
    AstarBar.SetActive(true);
    PlayBar.SetActive(false);
        if (followAstar != null)
        {
            followAstar.enabled = true; // Active le script FollowDijkstra
        }
    }
}
