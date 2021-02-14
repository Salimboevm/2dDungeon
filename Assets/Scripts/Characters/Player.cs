//Author: Mokhirbek Salimboev
//SID: 1919019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class for:
/// manage movement and pick ups
/// </summary>
public delegate void Move();//delegate to start enemy movement
public class Player : CharacterController
{
    public static Move move;//enemy movment delegate
    private void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        //player movement 
        if (Input.GetKeyDown(KeyCode.D))//right
        {
            MovePlayer(Vector2Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.A))//left
        {
            MovePlayer(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.W))//up
        {
            MovePlayer(Vector2Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.S))//down
        {
            MovePlayer(Vector2Int.down);
        }
    }

    /// <summary>
    /// call when player presses move buttons 
    /// </summary>
    /// <param name="direction"></param>
    private void MovePlayer(Vector2Int direction)
    {
        //check player is not moving, it is player`s turn and next tile is not colliding tile
        if (!isMoving && GameManager.Instance.turns == Turns.player && !isNotCollisionTile(ToV2TI(transform.position)+direction))
        {
            //start moving
            StartCoroutine(Move(direction, () =>
            {
                animator.SetBool("Move", true);//start animation
                
            }, () => { GameManager.Instance.turns = Turns.enemy; //change turn to enemy
                move();//call delegate and move enemies
            }));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            GameManager.Instance.coins++;//increase coin
            FindObjectOfType<UIManager>().UpdateCoins();//self explain
            AudioManager.instance.PlaySound("CoinSound");//play coin reward sound
            Destroy(collision.gameObject);//destroy coin gameobject
        }
        if (collision.gameObject.layer == 10)
        {
            AudioManager.instance.StopSound("BackgroundMusic");//stop currently playing music
            AudioManager.instance.PlaySound("DungeonCrawl");//start battle music
            GameManager.Instance.inBattle = true;//player is in battle
            GameManager.Instance.tmpEnemy = collision.gameObject;//get tmp 
            collision.gameObject.SetActive(false);
            FindObjectOfType<UIManager>().CoinText().enabled = false;
            FindObjectOfType<UIManager>().CoinParent().enabled = false;
            GameManager.Instance.LoadScene(2);//load battle scene
        }
    }

    
}
