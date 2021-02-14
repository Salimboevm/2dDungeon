using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : CustomerShop
{
    [Header("Item variables")]
    public int cost;//update cost
    public string name;//update name
    public Slider percentage;//slider percentage
    public Text text;//text cost

    [Header("General variables")]
    public Text coinsText;//coins text
    private void Start()
    {
        text.text = cost.ToString();
        UpdateCoins();
    }
    public void UpdateCoins()
    {
        coinsText.text = GameManager.Instance.coins.ToString();
    }
    public void TryToShop()
    {
        if (TryToShop(cost) == true)//check for enough coins
        {
            if (name.CompareTo(GameManager.Instance.playerDamage.ToString()) != 0)//check for name
            {
                if (percentage.value < 100)//check for percentage
                {
                    GameManager.Instance.coins -= cost;//deduct coins
                    GameManager.Instance.playerDamage += 3;//increase damage
                    GameManager.Instance.damageV += 100 / 4f;//increase damage value
                    percentage.value = GameManager.Instance.damageV;//initialise slider value to damage value
                    UpdateCoins();//update coins


                }
            }
        }
    }
}
