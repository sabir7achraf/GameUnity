using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject OptionMenu;

    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        MainMenu.SetActive(true);
        OptionMenu.SetActive(false);
    }

    public void ShowOptionsMenu()
    {
        MainMenu.SetActive(false);
       OptionMenu.SetActive(true);
    }
}
