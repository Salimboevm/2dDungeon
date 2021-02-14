using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    [SerializeField]
    private Vector3 cameraPos;//variable to set camera pos
    [SerializeField]
    private Vector3 playerPos;//variable to set player pos
    [SerializeField]
    private GameObject player;//player game object

    [SerializeField]
    private bool last =false;
    /// <summary>
    /// check for trigger
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9)//check for layer
        {
            if (last)
            {
                FindObjectOfType<UIManager>().WinPanel().SetActive(true);
                Time.timeScale = 0;
                return;
            }
            //if it is player 
            //increase lvl and start moving obj
            StartCoroutine(MoveObjects());
        }
    }
    /// <summary>
    /// func for moving camera and player
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveObjects()
    {
        yield return new WaitForSeconds(3f);//wait
        player.transform.position = playerPos;//set new pos of player
        Camera.main.transform.position = cameraPos;//set new pos of camera
        SD.ins.Save();//save game
        //SD.ins.Load();//load game


        yield return new WaitForSeconds(2f);//wait
    }
    private void Start()
    {
        AudioManager.instance.PlaySound("BackgroundMusic");//start bg sound
        
    }
}
