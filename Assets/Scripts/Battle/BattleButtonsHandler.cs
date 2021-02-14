using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class BattleButtonsHandler : MonoBehaviour
{
    
    
    [SerializeField]
    private bool physical;
    [SerializeField]
    private GameObject player;
    
    private void Start()
    {       
        string name = gameObject.name;//set name
        gameObject.GetComponent<Button>().onClick.AddListener(() => AttachCallBack(name));//add button to gameobject

    }
    /// <summary>
    /// function to control buttons
    /// </summary>
    /// <param name="name"></param>
    private void AttachCallBack(string name)
    {
        //check for name 
        if (name.CompareTo("AttackBtn") == 0)
        {
            FindObjectOfType<BattleHandler>().gameState = BattleState.Attack;//if it is attack
            player.GetComponent<FighterAction>().SelectAction("attack");//do attacking
        }
        else if (name.CompareTo("StunBtn") == 0)
        {
            FindObjectOfType<BattleHandler>().gameState = BattleState.Stun;//if it is stun
            player.GetComponent<FighterAction>().SelectAction("stun");//cause for stunning
            
        }
        else
        {
            FindObjectOfType<BattleHandler>().gameState = BattleState.Regen;//if it is regen
            player.GetComponent<FighterAction>().SelectAction("regeneration");//heal player
        }

    }
    
}
