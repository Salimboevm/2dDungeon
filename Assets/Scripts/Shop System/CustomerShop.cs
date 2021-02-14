using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerShop : MonoBehaviour
{
    /// <summary>
    /// func to check 
    /// do character have enough money to buy
    /// </summary>
    /// <param name="cost"></param>
    /// <returns></returns>
    public bool TryToShop(int cost)
    {
        if(cost <= GameManager.Instance.coins)
        {
            return true;

        }
        else
        {
            return false;
        }
    }
}
