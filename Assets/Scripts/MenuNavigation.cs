using UnityEngine;

public class MenuNavigation : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject controls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenu.SetActive(true);
        controls.SetActive(false);
    }

    public void OpenControls()
    {
        mainMenu.SetActive(false);
        controls.SetActive(true);
    }

    public void CloseControls()
    {
        mainMenu.SetActive(true);
        controls.SetActive(false);
    }
}
