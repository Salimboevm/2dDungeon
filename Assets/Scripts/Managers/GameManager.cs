using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum Turns
{
    player,
    enemy
}
public class GameManager : MonoBehaviour
{
    #region Instance
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }


/* Unmerged change from project 'Assembly-CSharp.Player'
Before:
    public int Health { get => health; set => health = value; }
After:
    public int Health { get => Health1; set => health = value; }
*/
    

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        maxHealth = 100;
        Health = maxHealth;
    }
    #endregion
    public Turns turns;

    [Header("Player Battle")]
    private int health = 100;

    public int Health { 
        get => health;
        set => health = value;
    }

    public int maxHealth;
    public int playerDamage =10;
    public short playerHeal = 3;
    public short stunTurnTime = 2;
    [HideInInspector]
    public GameObject tmpEnemy;
    [Header("Player Stats. Do not fill")]
    public int coins = 0;
    public float damageV = 0;
    public bool onGamePaused = false;

    public bool inBattle = false;

    private void Start()
    {
        turns = Turns.player;//set turn to player
        AudioManager.instance.PlaySound("BackgroundMusic");//play music
        
    }
    /// <summary>
    /// func for loading scene
    /// </summary>
    /// <param name="sceneNum"></param>
    public void LoadScene(int sceneNum)
    {   
        SceneManager.LoadScene(sceneNum,LoadSceneMode.Additive);//load scene
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));//set active scene
    }
    
}
public class GeneralData
{
    //saving player stats
    [Header("Player Stats")]
    public int level;
    public float xPos;
    public float yPos;
    public int coins;
    public int health;
    public int damage;
    public Turns turns;
    [Header("Camera")]
    public float cPosX;
    public float cPosY;
    public float cPosZ;
    [Header("Inventory")]
    public float damageValue;
}
