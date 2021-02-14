using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Battle : UIManager
{
    #region Instance
    private static Battle _instance;

    public static Battle Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    public Slider playerHealth;//player health slider
    public Slider enemyHealth;//enemy health slider

/// <summary>
/// set player health
/// </summary>
/// <param name="health"></param>
    public void SetPlayerHealth(int health)
    {
        playerHealth.value = health;
    }
    /// <summary>
    /// set enemy health
    /// </summary>
    /// <param name="health"></param>
    public void SetEnemyHealth(int health)
    {
        enemyHealth.value = health;
    }
    /// <summary>
    /// get coins reward
    /// </summary>
    public void CoinsReward()
    {
        GameManager.Instance.coins += 10;//add 10 coins 
        Destroy(GameManager.Instance.tmpEnemy);//destroy temp enemy game object
        FindObjectOfType<UIManager>().CoinParent().enabled = true;//activate coins parent text 
        FindObjectOfType<UIManager>().CoinText().enabled = true;
        FindObjectOfType<UIManager>().UpdateCoins();//update coins 
        endPanels[0].SetActive(false);//deactivate win panel
        GameManager.Instance.inBattle = false;//set player is not in battle
        AudioManager.instance.StopSound("DungeonCrawl");//stop currently playing music
        AudioManager.instance.PlaySound("BackgroundMusic");//start battle music
        SD.ins.Save();
        SceneManager.UnloadSceneAsync(2);//destroy current scene
    }
    /// <summary>
    /// function to health reward
    /// </summary>
    public void HealthReward()
    {
        GameManager.Instance.Health += 10;//add 10 to health
        if (GameManager.Instance.Health > GameManager.Instance.maxHealth)//check current health to max health
            GameManager.Instance.Health = GameManager.Instance.maxHealth;//if it is bigger, set current health to max health
        endPanels[0].SetActive(false);//deactivate win panel
        GameManager.Instance.inBattle = false;//set player is not in battle
        AudioManager.instance.StopSound("DungeonCrawl");//stop currently playing music
        AudioManager.instance.PlaySound("BackgroundMusic");//start battle music
        SD.ins.Save();

        FindObjectOfType<UIManager>().CoinParent().enabled = true;
        FindObjectOfType<UIManager>().CoinText().enabled = true;
        SceneManager.UnloadSceneAsync(2);//destroy current scene
    }
    public void DamageReward()
    {
        GameManager.Instance.playerDamage += 5;//add 5 to player damage
        endPanels[0].SetActive(false);//deactivate win panel
        GameManager.Instance.inBattle = false;//set player is not in battle
        AudioManager.instance.StopSound("DungeonCrawl");//stop currently playing music
        AudioManager.instance.PlaySound("BackgroundMusic");//start battle music
        SD.ins.Save();

        FindObjectOfType<UIManager>().CoinParent().enabled = true;
        FindObjectOfType<UIManager>().CoinText().enabled = true;
        SceneManager.UnloadSceneAsync(2);//destroy current scene
    }
}
