using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : CharacterController
{
    public int canMoveGreedSize = 1;//game object`s maximum moving tile
    private Vector2 v2p => transform.position;//game object`s position
    private Vector2[] fwd => new Vector2[] {Vector2.left, Vector2.up, Vector2.right, Vector2.down};//directions to move
    private Vector2 originPos;//original position of gameobject

    static System.Random random;//using system random
    private IEnumerator Start()
    {
        
        base.Start();
        
        yield return new WaitWhile(()=>(data == null));//wait until getting data
        if (data.isActive==false)//if gameobject from data is not acitve
        {
            //destroy current enemy and leave 
            Destroy(gameObject);
            yield break;
            
        }
        originPos = transform.position;
        animator = GetComponent<Animator>();
        Player.move += Move;//attach delegate with move function
       
        random = new System.Random();//take random from system
    }

    /// <summary>
    /// Moving game object
    /// </summary>
    private void Move()
    {
        int ri = random.Next(0, pmds.Length);//randomise possible directions to move
        //start moving

        StartCoroutine(Move(pmds[ri],//give possible moving directions from taken random num
            () =>
              {
                  animator.SetBool("Move", true);//start animation
              },
            () =>
            {

            }));
        GameManager.Instance.turns = Turns.player;//change turn to player

    }
    //when game object destroyed
    private void OnDestroy()
    {
        Player.move -= Move;//delete this object from moving delegate
        SD.ins.AddData(new Data { name = name, posX = transform.position.x,posY = transform.position.y, posZ = transform.position.z, isActive = false });//save data and set object as a not active one
        //delete this object from loading and saving delegates
        SD.ins.onLoaded -= SetData;
        SD.ins.onSaving -= SendData;
        
    }
    /// <summary>
    /// take possible moving directions
    /// </summary>
    private Vector2[] pmds => fwd.Where(a => !isNotCollisionTile(ToV2TI(v2p+a)) && (Vector2.Distance(v2p + a, originPos) <= canMoveGreedSize + .5f)).ToArray();//check next tile is movable
                                                                                                                                                               //and game object is in moving distance
}