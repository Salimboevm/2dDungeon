using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CharacterBattle : MonoBehaviour
{
    static System.Random random;//using system random
    Animator anim;//animator for characters
    private State state;//state of characters
    Vector3 slideTargetPos;//target`s position
    Action onSlideComplete;//action to do 
    [SerializeField]
    private ParticleSystem stunEffect;//particle effect for stun
    [HideInInspector]
    public bool stun = false;//is enemy stunned
    [HideInInspector]
    public short tmpStun;//temp value of stun
    [HideInInspector]
    public HealthSystem healthSystem;//health sys of characters
    private enum State
    {
        Idle,
        Sliding,
        Busy
    }
    private void Awake()
    {
        state = State.Idle;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        tmpStun = GameManager.Instance.stunTurnTime;//temp stun is equal to game manager value
        stun = false;//is not stunned

        //set character`s health
        Battle.Instance.SetPlayerHealth(GameManager.Instance.Health);
        Battle.Instance.SetEnemyHealth(healthSystem.GetHealth());
        random = new System.Random();
    }

    private void Update()
    {
        switch (state)//check states
        {
            case State.Idle://if idle return
                break;
            case State.Busy://if busy return
                break;
            case State.Sliding://if sliding do action
                float slideSpeed = 2f;//slide speed for characters
                gameObject.transform.position += (slideTargetPos - GetPosition()) * slideSpeed * Time.deltaTime;//move character

                float reachedDistance = 1f;//reached distance to check
                if(Vector3.Distance(GetPosition(), slideTargetPos)< reachedDistance)//check pos of character
                {
                    transform.position = slideTargetPos;//set position to target pos
                    onSlideComplete();//call action
                }
                break;//leave function
        }
    }
    /// <summary>
    /// function to take damage
    /// call when player or enemy is attacking
    /// </summary>
    /// <param name="amount"></param>
    public void Damage(int amount) 
    {
        healthSystem.Damage(amount);//get damage

        if (healthSystem.IsDead())//check they are not dead
        {
            anim.Play("Death");//if character is dead play death anim
        }
    }
    /// <summary>
    /// function to check character`s health and death proccess
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        return healthSystem.IsDead();//return player`s health 
    }
    /// <summary>
    /// get pos from active character
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition()
    {
        return transform.position;//return cur position
    }
    /// <summary>
    /// attack function 
    /// call when player is attacking
    /// </summary>
    /// <param name="targetCharacter"></param>
    /// <param name="onAttackComplete"></param>
    public void Attack(CharacterBattle targetCharacter, Action onAttackComplete)
    {
       
        Vector3 slideTargetPosition = targetCharacter.GetPosition() + (GetPosition() - targetCharacter.GetPosition()).normalized * 10f;//get target`s position
        Vector3 startingPos = GetPosition();//get start pos
        anim.SetBool("Move", true);//start moving animation
        ///call sliding function
        SlideToPosition(targetCharacter.GetPosition(), () =>
        {
            state = State.Busy;//change state to busy
            if (targetCharacter.CompareTag("Enemy"))//check tag to enemy
            {
                targetCharacter.Damage(GameManager.Instance.playerDamage);//take damage
                

            }
            else if (targetCharacter.CompareTag("Player"))//check tag to player
            {
                
                targetCharacter.Damage(random.Next(5,10));//take damage

                GameManager.Instance.Health -= random.Next(5, 10);
                print(GameManager.Instance.Health);
            }
            //slide back to starting pos
            SlideToPosition(startingPos, () =>
            {
               
                anim.SetBool("Move", false); //stop moving anim
                state = State.Idle;//change state to idle
                onAttackComplete();//call action
            });
        });
        
    }
    /// <summary>
    /// func to heal player
    /// </summary>
    public void Heal()
    {
        healthSystem.Heal(GameManager.Instance.playerHeal);//add to player health
        
    }
    /// <summary>
    /// function to stun enemy
    /// call when player stuns enemy
    /// </summary>
    /// <param name="targetCharacter"></param>
    /// <param name="onAttackComplete"></param>
    public void Stun(CharacterBattle targetCharacter, Action onAttackComplete)
    {
        //move to enemy and set animation
        Vector3 startingPos = GetPosition();
        anim.SetBool("Move", true);
        //call sliding func
        SlideToPosition(targetCharacter.GetPosition(), () =>
        {
            state = State.Busy;//change state
            targetCharacter.Stun();//make and set enemy stunned
            //slide back
            SlideToPosition(startingPos, () =>
            {
                
                anim.SetBool("Move", false);
                state = State.Idle;
                onAttackComplete();
            });
        });
    }
    /// <summary>
    /// function to change values of enemy
    /// </summary>
    public void Stun()
    {
        stunEffect.Play();//play particle effect
        stun = true;//change value of stun
        tmpStun = GameManager.Instance.stunTurnTime;//set temp stun to game manager stun value
    }
    /// <summary>
    /// function to move characters
    /// </summary>
    /// <param name="slideTargetPos"></param>
    /// <param name="onSlideComplete"></param>
    private void SlideToPosition(Vector3 slideTargetPos, Action onSlideComplete)
    {
        this.slideTargetPos = slideTargetPos;//set target pos
        this.onSlideComplete = onSlideComplete;//get action
        state = State.Sliding;//change state to sliding
    }
}


