using UnityEngine;

public enum BattleState
{
    Start,
    Attack,
    Regen,
    Stun
}
public class BattleHandler : MonoBehaviour
{

    public BattleState gameState = BattleState.Start;//set game state

    [SerializeField]
    private CharacterBattle playerBattle;//get player 
    [SerializeField]
    private CharacterBattle enemyBattle;//get enemy
    private CharacterBattle active;//get active character
    private PlayerTurnState state;//who`s turn now

    enum PlayerTurnState
    {
        Waiting,
        Busy
    }

    private void Start()
    {
        playerBattle.healthSystem = new HealthSystem(GameManager.Instance.Health);//set player health
        print(GameManager.Instance.Health);//set player health
        enemyBattle.healthSystem = new HealthSystem(100);//set enemy health
        SetActive(playerBattle);//set active player
    }
    /// <summary>
    /// choice of player
    /// call when player presses button
    /// </summary>
    /// <param name="victim"></param>
    public void PlayerChoice(GameObject victim)
    {
        if (GameManager.Instance.onGamePaused == false)//is not paused
        {
            victim = enemyBattle.gameObject;//set victim to enemy
            if (state == PlayerTurnState.Waiting)//check is it players turn
            {
                switch (gameState)
                {
                    case BattleState.Attack://attack 
                        state = PlayerTurnState.Busy;//set player is doing action
                        playerBattle.Attack(enemyBattle, () =>//call attack function
                        {
                            Battle.Instance.SetEnemyHealth(enemyBattle.healthSystem.GetHealth());//update enemy`s health
                            ChooseNextActive();//activate next character
                        });
                        break;

                    case BattleState.Stun://stun enemy
                        state = PlayerTurnState.Busy;//set player is doing action
                        playerBattle.Stun(enemyBattle, () =>//call stun function
                        {
                            ChooseNextActive();//activate next
                        });
                        break;
                    case BattleState.Regen://regeneration 
                        state = PlayerTurnState.Busy;//change to busy mode
                        playerBattle.Heal();//call heal function to add health
                        Battle.Instance.SetPlayerHealth(playerBattle.healthSystem.GetHealth());//update player health
                        ChooseNextActive();//activate next
                        break;
                }
            }
        }
    }

    /// <summary>
    /// function to set active character
    /// </summary>
    /// <param name="characterBattle"></param>
    void SetActive(CharacterBattle characterBattle)
    {
        active = characterBattle;//active character is returned character
    }
    /// <summary>
    /// next active actor choosing
    /// </summary>
    void ChooseNextActive()
    {
        if (IsBattleOver())//check battle is not over
            return;

        if (active == playerBattle)//check player is active character
        {
            if (enemyBattle.stun == true)//check enemy is stunned
            {
                if (enemyBattle.tmpStun >= 0)//check temporary stun is bigger than 0
                {
                    
                    enemyBattle.tmpStun--;//decrease temprorary stun value
                    state = PlayerTurnState.Waiting;//change to waiting for action

                    if(enemyBattle.tmpStun == 0)//check tmp stun is equal to zero
                    {
                        enemyBattle.stun = false;//change value of stun
                        return;//leave function
                    }
                    SetActive(playerBattle);//activate player
                }
            }
            else//if enemy is not stunned
            {
             
                state = PlayerTurnState.Busy;//change action to busy mode
                enemyBattle.Attack(playerBattle, () =>//attack player
                {
                    Battle.Instance.SetPlayerHealth(playerBattle.healthSystem.GetHealth());//update player health
                    ChooseNextActive();//activate next
                }); 
                SetActive(enemyBattle);//activate enemy
            }
        }
        else // if enemy is active
        {
            
            state = PlayerTurnState.Waiting;//change mode to waiting
            SetActive(playerBattle);//set player active
        }
    }
    /// <summary>
    /// fucntion to check battle status
    /// </summary>
    /// <returns></returns>
    private bool IsBattleOver() 
    {
        if (playerBattle.IsDead())//player is dead
        {
            Battle.Instance.endPanels[1].SetActive(true);//activate lost panel
            Battle.Instance.gamePanel.SetActive(false);//deactivate game panel
            return true;//return player is dead
        }
        else if (enemyBattle.IsDead())//enemy is dead
        {
            Battle.Instance.endPanels[0].SetActive(true);//activate win panel
            Battle.Instance.gamePanel.SetActive(false);//deactivate game panel
            //get data of enemy and delete it 
            SD.ins.GetData(GameManager.Instance.tmpEnemy.name).isActive = false;
            Destroy(GameManager.Instance.tmpEnemy.gameObject);
            
            return true;//return enemy is dead
        }
        return false;//leave function
    }
}
