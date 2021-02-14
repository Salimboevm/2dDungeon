using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject[] shopPanel;//shop panel
    /// <summary>
    /// func to start game
    /// call from button 
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);//load battle scene
    }
    ///load data
    public void LoadData()
    {
        SD.ins.OnLoaded();//load data
    }
    /// <summary>
    /// function for shop menu
    /// </summary>
    public void ShopMenu()
    {
        shopPanel[1].SetActive(true);//activate shop menu
        shopPanel[0].SetActive(false);//deactivate current menu
    }
    /// <summary>
    /// quit game
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
    /// <summary>
    /// func for going back
    /// </summary>
    public void GoBack()
    {
        shopPanel[1].SetActive(false);//deactivate shop menu
        shopPanel[0].SetActive(true);//activate main menu
        SD.ins.Save();
    }

}
