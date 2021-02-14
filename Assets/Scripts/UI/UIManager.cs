using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{


    public GameObject[] endPanels;//panels for win and loose 
    public GameObject gamePanel;//current game panel
    public GameObject pausePanel;//pause menu panel
    [SerializeField]
    private GameObject winPanel;//pause menu panel
    [SerializeField]
    private Text coinsT;//coins text
    [SerializeField]
    private Text coinsP;//coins parent text

    /// <summary>
    /// function for main menu
    /// </summary>
    public void MainMenu()
    {
        GameManager.Instance.onGamePaused = false;//change to game is not paused
        Time.timeScale = 1f;//set timescale to normal timescale
        winPanel.SetActive(false);
        SceneManager.LoadScene(0);
    }
    /// <summary>
    /// Resume game
    /// </summary>
    public virtual void Resume()
    {
        GameManager.Instance.onGamePaused = false;//change to game is not paused
        Time.timeScale = 1f;//set timescale to normal timescale
        pausePanel.SetActive(false);//deactivate pause panel
    }
    /// <summary>
    /// function to pause game
    /// </summary>
    public void Pause()
    {
        Time.timeScale = 0f;//set timescale to zero
        pausePanel.SetActive(true);//activate pause menu
    }
    int fpsCo = 0;
    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//check for input to pause game
        {
            GameManager.Instance.onGamePaused = true;//change value to game paused
        }
        if (GameManager.Instance.onGamePaused == true)//check for game pause
        {
            Pause();//pause game
        }
    }
    public void UpdateCoins()
    {
        coinsT.text = GameManager.Instance.coins.ToString();
    }
    public void WinOrLost()
    {
        GameManager.Instance.onGamePaused = false;//change to game is not paused
        Time.timeScale = 1f;//set timescale to normal timescale
        winPanel.SetActive(false);
        SD.ins.ResetAllData();
        SceneManager.LoadScene(0);
    }
    public Text CoinText()
    {
        return coinsT;
    }
    public Text CoinParent()
    {
        return coinsP;
    }
    public GameObject WinPanel()
    {
        return winPanel;
    }
}
