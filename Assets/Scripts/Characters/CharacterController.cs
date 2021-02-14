using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public abstract class CharacterController : MonoBehaviour
{
    //player animator to change ik values
    protected Animator animator;

    [Header("Tilemaps")]
    [SerializeField]
    protected Tilemap groundTilemap;//ground tilemap to move
    [SerializeField]
    protected Tilemap collisionTilemap;//collision tilemap to block player movement
    [SerializeField]
    protected Tilemap detailsTilemap;//details tilemap for pick ups and enemies

    public Vector2 gridSize;//size of every grid
    protected bool isMoving = false;//variable to check is player moving
    
    Vector2 origPos, targetPos;//position of the character
    /// <summary>
    /// function to move player
    /// </summary>
    /// <param name="direction"></param>
    protected IEnumerator Move(Vector2 direction, Action onSlideStart, Action onSlideComplete)
    {
        if (!GameManager.Instance.inBattle)//player is not in battle
        {
            if (GameManager.Instance.onGamePaused == false)//game is not paused
            {
                if (CheckMove(direction))//has moving tile
                {
                    isMoving = true;//player starts to move

                    onSlideStart();//call first action 

                    origPos = transform.position;//take game object position
                    targetPos = origPos + direction * gridSize;//target moving position
                    float elapsed = 0, duration = 1;//time to move
                    while (elapsed < duration)//if elapsed time is less than duration
                    {
                        elapsed += Time.deltaTime;//add to elapsed time
                        
                        if (elapsed >= duration)//check elapsed time is not equal or bigger than duration
                            elapsed = duration;//if it is, equalize elapsed time to duration

                        transform.position = Vector2.Lerp(origPos, targetPos, elapsed / duration);//move game object

                        yield return null;//leave
                    }

                }

                animator.SetBool("Move", false);//stop animation

                isMoving = false;//game object is not moving
                onSlideComplete();//call second action
                yield return new WaitForSeconds(2f);//wait
            }
        }
    }
    
    /// <summary>
    /// function to check moving space
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private bool CheckMove(Vector2 direction)
    {
        Vector3Int gridPos = groundTilemap.WorldToCell(transform.position + (Vector3)direction);//grid position to move
        if (collisionTilemap.HasTile(gridPos))//check can player move to the next grid
            return false;
        else
            return true;
    }
    
    /// <summary>
    /// convert vector3 to vector2int
    /// </summary>
    /// <param name="v3"></param>
    /// <returns></returns>
    protected Vector2Int ToV2TI(Vector3 v3)
    {
        Vector2Int v2i = new Vector2Int(Mathf.FloorToInt(v3.x), Mathf.FloorToInt(v3.y));
        return v2i;
    }
    
    /// <summary>
    /// convert  vector 2 to vector2int
    /// </summary>
    /// <param name="v2"></param>
    /// <returns></returns>
    protected Vector2Int ToV2TI(Vector2 v2)
    {
        Vector2Int v2i = new Vector2Int(Mathf.FloorToInt(v2.x), Mathf.FloorToInt(v2.y));
        return v2i;
    }
    
    /// <summary>
    /// check which object is next to characters
    /// </summary>
    /// <param name="v2i"></param>
    /// <returns></returns>
    protected bool isNotCollisionTile(Vector2Int v2i) => collisionTilemap.HasTile((Vector3Int)v2i);
    
    /// <summary>
    /// call function for saving data
    /// </summary>
    protected void SendData()
    {
        SD.ins.AddData(new Data{name = name, posX = transform.position.x, posY = transform.position.y, posZ = transform.position.z,
            isActive =true, coins = GameManager.Instance.coins, health = GameManager.Instance.Health,
            damage = GameManager.Instance.playerDamage, turns = GameManager.Instance.turns, cPosX = Camera.main.transform.position.x,
            cPosY= Camera.main.transform.position.y, cPosZ = Camera.main.transform.position.z
        });//save data at constructor
    }
    
    public Data data = null;
    /// <summary>
    /// load data from saved file
    /// </summary>
    protected void SetData()
    {
        data = SD.ins.GetData(name);//taking game object name
        
        if (data != null)//check if data is not null
        {
            //load data
            transform.position = new Vector3(data.posX, data.posY, data.posZ);
            
            GameManager.Instance.coins = data.coins;
            GameManager.Instance.Health = data.health;
            GameManager.Instance.playerDamage = data.damage;
            GameManager.Instance.turns = data.turns;
            Camera.main.transform.position = new Vector3(data.cPosX, data.cPosY, data.cPosZ);
            GameManager.Instance.damageV = data.damageValue;
        }
        else
            data = new Data();
    }
    
    /// <summary>
    /// calling and initialising delegates
    /// </summary>
    protected void Start()
    {
        SD.ins.onLoaded += SetData;
        SD.ins.onSaving += SendData;
    }

}
/// <summary>
/// saving data 
/// </summary>
[System.Serializable]
public class Data
{
    //saving enemy stats
    [Header("Enemy")]
    public string name;
    public float posX;
    public float posY;
    public float posZ;
    public bool isActive;
    //saving player stats
    [Header("Player Stats")]
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

    public Data()
    {
        isActive = true;
    }
}