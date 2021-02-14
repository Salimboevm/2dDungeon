using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public delegate void Saving();//saving delegate
public delegate void Loading();//loading delegate
/// <summary>
/// class for saving and loading data
/// </summary>
public class SD : MonoBehaviour
{
    public static SD ins;//singleton of the class
    AllDatas datas;//all datas from game
    //saving and loading delegates
    public Saving onSaving;
    public Loading onLoaded;
    private static string FilePath//taking filpath
    {
        get { return Application.persistentDataPath + "/playerInfo.dat"; }//apps persistent data path + name of data
    }
    private void Awake()
    {
        if(ins != null)
        {
            Destroy(ins.gameObject);
        }
        ins = this;
        
    }
    private void Start()
    {
        Load();//Load everything from last pos
    }
    /// <summary>
    /// function to save data
    /// </summary>
    public void Save()
    {
        onSaving();//call saving delegate

        BinaryFormatter formatter = new BinaryFormatter();//making new binary format

        FileStream fileStream = new FileStream(FilePath, FileMode.Create);//creating file

        formatter.Serialize(fileStream, datas);//serializing this data
        fileStream.Close();//closing file

    }
    /// <summary>
    /// funtion to load data
    /// </summary>
    public void Load()
    {
        if (File.Exists(FilePath))//check file
        {
            BinaryFormatter formatter = new BinaryFormatter();//make binary formatter
            FileStream stream = new FileStream(FilePath, FileMode.Open);//open file

            datas = formatter.Deserialize(stream) as AllDatas;
            stream.Close();
            OnLoaded();
        }
        else
        {
            datas = new AllDatas();//else make new all game datas
        }
    }

    public void OnLoaded()
    {
        onLoaded();//call loading delegate
    }

    /// <summary>
    /// function to add data to all game datas
    /// </summary>
    /// <param name="data"></param>
    public void AddData(Data data)
    {
        Data d = datas.datas.FirstOrDefault(a => a.name == data.name);//check for name
        if (d != null)//if it is not null
        {
            //load old things to data
            d.posX = data.posX;
            d.posY = data.posY;
            d.posZ = data.posZ;
            d.isActive = data.isActive;
            d.coins = data.coins;
            d.health = data.health;
            d.damage = data.damage;
            d.turns = data.turns;
            d.damageValue = data.damageValue;
        }
        else
            datas.datas.Add(data);//if not add it to all game datas
    }
    /// <summary>
    /// function for getting data
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Data GetData(string name)
    {
        return datas.datas.FirstOrDefault(a => a.name == name);//return game object`s name
    }

    private void OnApplicationQuit()
    {
        Save();
    }
    public void ResetAllData()
    {
        File.Delete(FilePath);
    }
}
/// <summary>
/// all datas of game
/// </summary>
[System.Serializable]
public class AllDatas
{
    public List<Data> datas;

    public AllDatas()
    {
        datas = new List<Data>();
    }
}