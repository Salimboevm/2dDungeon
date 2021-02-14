using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAction : MonoBehaviour
{

    private GameObject enemy;//get enemy
    private GameObject player;//get player

    //get actions
    [SerializeField]
    private BattleHandler attack;
    [SerializeField]
    private BattleHandler stun;
    [SerializeField]
    private GameObject regeneration;

    private void Awake()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");//find enemy
        player = GameObject.FindGameObjectWithTag("Player");//find player
    }
    /// <summary>
    /// function to do action of player
    /// </summary>
    /// <param name="btn"></param>
    public void SelectAction(string btn)
    {
        GameObject victim = player;//get victim
        if(CompareTag("Player"))//check tag
        {
            victim = enemy;//if it is player, set enemy victim
        }
        //check game object names
        if (btn.CompareTo("attack") == 0)//check for attack
        {
            attack.PlayerChoice(victim);//do attack
        }
        else if (btn.CompareTo("stun") == 0)//check for stun 
        {
            stun.PlayerChoice(victim);//do stun
        }
        else//check for heal
        {
            regeneration.GetComponent<BattleHandler>().PlayerChoice(victim);//do regen
        }
    }

}
