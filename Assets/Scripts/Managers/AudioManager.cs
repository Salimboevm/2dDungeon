using UnityEngine.Audio;
using UnityEngine;
using System;

[System.Serializable]
public class Sound
{
    public string name;//name of sound
    public AudioClip clip;//clip to play
    [Range(0,1)]
    public float volume= 0.7f; //volume of the clip
    public bool loop;
    [HideInInspector]
    public AudioSource source;//source to play

}


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;//get delegate

    [SerializeField]
    Sound[] sounds;//make list of sounds
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        //make new game objects with source and call set source
        //for (int i = 0; i < sounds.Length; i++)
        //{
        //    GameObject _go = new GameObject("Sound_" + i + sounds[i].name);
        //    _go.transform.SetParent(this.transform);
        //    sounds[i].SetSource(_go.AddComponent<AudioSource>());
        //
        //}
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }
    /// <summary>
    /// func to play sound
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name)
    {
        //search for sounds
        Sound s = Array.Find(sounds, sound => sound.name == name);
        //check s is not null
        if (s == null)
        {
            Debug.LogWarning("There is no sound");
            return;
        }
        //play
        s.source.Play();
    }
    /// <summary>
    /// function to stop sound
    /// </summary>
    /// <param name="name"></param>
    public void StopSound(string name)
    {
        //Search for sounds
        Sound s = Array.Find(sounds, sound => sound.name == name);
        //check s is not null
        if (s == null)
        {
            Debug.LogWarning("There is no sound");
            return;
        }
        //Stop
        s.source.Stop();
    }
}
