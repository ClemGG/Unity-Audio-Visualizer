using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Son[] sons;
    AudioListener thisAudioListener;

    public static AudioManager instance;

#if UNITY_EDITOR

    private void OnValidate()
    {
        for (int i = 0; i < sons.Length; i++)
        {
            sons[i].tag = sons[i].clip.name;
        }
    }

#endif




    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        thisAudioListener = (AudioListener)gameObject.AddComponent(typeof(AudioListener));

        for (int i = 0; i < sons.Length; i++)
        {
            if(sons[i].source == null)
            {
                sons[i].source = gameObject.AddComponent<AudioSource>();
                sons[i].source.clip = sons[i].clip;
                sons[i].source.volume = sons[i].volume;
                sons[i].source.pitch = sons[i].pitch;
                sons[i].source.loop = sons[i].loop;
                sons[i].source.playOnAwake = sons[i].playOnAwake;

                if (sons[i].playOnAwake)
                {
                    Play(sons[i].clip.name);
                }
            }
        }
    }


    public void Play(string name)
    {
        Son s = Array.Find(sons, son => son.clip.name == name);

        if (s != null)
            s.source.Play();
        else
            Debug.LogError($"Erreur : Le nom \"{name}\" n'existe pas dans la liste des sons.");
    }
    public void Stop(string name)
    {
        Son s = Array.Find(sons, son => son.clip.name == name);

        if (s != null)
            s.source.Stop();
        else
            Debug.LogError($"Erreur : Le nom \"{name}\" n'existe pas dans la liste des sons.");
    }
    public void Pause(string name, bool pause)
    {
        Son s = Array.Find(sons, son => son.clip.name == name);

        if (s != null)
            if (pause)
                s.source.Pause();
            else
                s.source.UnPause();
        else
            Debug.LogError($"Erreur : Le clip \"{name}\" n'existe pas dans la liste des sons.");
    }
    public void Mute(string name, bool mute)
    {
        Son s = Array.Find(sons, son => son.clip.name == name);

        if (s != null)
            s.source.mute = mute;
        else
            Debug.LogError($"Erreur : Le clip \"{name}\" n'existe pas dans la liste des sons.");
    }
    public void Play(AudioClip clip)
    {
        Son s = Array.Find(sons, son => son.clip == clip);

        if (s != null)
            s.source.Play();
        else
            Debug.LogError($"Erreur : Le clip \"{clip.name}\" n'existe pas dans la liste des sons.");
    }
    public void Stop(AudioClip clip)
    {
        Son s = Array.Find(sons, son => son.clip == clip);

        if (s != null)
            s.source.Stop();
        else
            Debug.LogError($"Erreur : Le clip \"{clip.name}\" n'existe pas dans la liste des sons.");
    }
    public void Pause(AudioClip clip, bool pause)
    {
        Son s = Array.Find(sons, son => son.clip == clip);

        if (s != null)
            if (pause)
                s.source.Pause();
            else
                s.source.UnPause();
        else
            Debug.LogError($"Erreur : Le clip \"{clip.name}\" n'existe pas dans la liste des sons.");
    }
    public void Mute(AudioClip clip, bool mute)
    {
        Son s = Array.Find(sons, son => son.clip == clip);

        if (s != null)
            s.source.mute = mute;
        else
            Debug.LogError($"Erreur : Le clip \"{clip.name}\" n'existe pas dans la liste des sons.");
    }
    public void Play(int id)
    {
        try
        {
            sons[id].source.Play();
        }
        catch
        {
            Debug.LogError($"Erreur : Le clip n°\"{id}\" n'existe pas dans la liste des sons.");
        }
    }
    public void Stop(int id)
    {
        try
        {
            sons[id].source.Stop();
        }
        catch
        {
            Debug.LogError($"Erreur : Le clip n°\"{id}\" n'existe pas dans la liste des sons.");
        }
    }
    public void Pause(int id, bool pause)
    {
        try
        {
            if(pause)
                sons[id].source.Pause();
            else
                sons[id].source.UnPause();
        }
        catch
        {
            Debug.LogError($"Erreur : Le clip n°\"{id}\" n'existe pas dans la liste des sons.");
        }
    }
    public void Mute(int id, bool mute)
    {
        try
        {
            sons[id].source.mute = mute;
        }
        catch
        {
            Debug.LogError($"Erreur : Le clip n°\"{id}\" n'existe pas dans la liste des sons.");
        }
    }
    public void StopAll()
    {
        for (int i = 0; i < sons.Length; i++)
        {
            sons[i].source.Stop();
        }
    }
    public void PauseAll(bool pause)
    {
        for (int i = 0; i < sons.Length; i++)
        {
            if (pause)
                sons[i].source.Pause();
            else
                sons[i].source.UnPause();
        }
    }
    public void MuteAll(bool mute)
    {
        for (int i = 0; i < sons.Length; i++)
        {
            sons[i].source.mute = mute;
        }
    }




    public void PlayRandomSoundFromList(params int[] indexes)
    {
        int alea = UnityEngine.Random.Range(0, indexes.Length);
        Son s = sons[indexes[alea]];

        if (s != null)
            s.source.Play();
        else
            Debug.LogError($"Erreur : L'ID n° \"{indexes[alea]}\" n'existe pas dans la liste des sons.");
    }


    public void PlayRandomSoundFromList(params string[] noms)
    {
        int alea = UnityEngine.Random.Range(0, noms.Length);
        Son s = Array.Find(sons, son => son.clip.name == noms[alea]);

        if (s != null)
            s.source.Play();
        else
            Debug.LogError($"Erreur : Le nom \"{noms[alea]}\" n'existe pas dans la liste des sons.");
    }



    public Son GetSonFromClip(AudioClip clip)
    {
        Son s = Array.Find(sons, son => son.clip == clip);

        if (s != null)
            return s;
        else
            Debug.LogError($"Erreur : Le clip \"{clip.name}\" n'existe pas dans la liste des sons.");
        return null;
    }

    public Son GetSonFromName(string name)
    {
        Son s = Array.Find(sons, son => son.clip.name == name);

        if (s != null)
            return s;
        else
            Debug.LogError($"Erreur : Le clip \"{name}\" n'existe pas dans la liste des sons.");
        return null;
    }

    public Son GetSonFromTag(string tag)
    {
        Son s = Array.Find(sons, son => son.tag.Contains(tag));

        if (s != null)
            return s;
        else
            Debug.LogError($"Erreur : Aucun clip ne contient le tag \"{tag}\".");
        return null;
    }

    public Son GetSonFromID(int id)
    {
        try
        {
            return sons[id];
        }
        catch
        {
            Debug.LogError($"Erreur : Le clip n°\"{id}\" n'existe pas.");
            return null;
        }
            
    }


    public Son[] GetAllSonsFromTag(string tag)
    {
        List<Son> s = new List<Son>();

        for (int i = 0; i < sons.Length; i++)
        {
            Son sonTrouvé = Array.Find(sons, son => son.tag.Contains(tag));

            if (sonTrouvé != null && !s.Contains(sonTrouvé))
            {
                s.Add(sonTrouvé);
            }
        }

        if (s.Count > 0)
            return s.ToArray();
        else
            Debug.LogError($"Erreur : Aucun clip avec le tag \"{tag}\" n'a été trouvé dans la liste des sons.");
        return null;
    }

    public Son[] GetAllSonsFromTags(params string[] tags)
    {
        List<Son> s = new List<Son>();

        for (int j = 0; j < tags.Length; j++)
        {
            for (int i = 0; i < sons.Length; i++)
            {
                Son sonTrouvé = Array.Find(sons, son => son.tag.Contains(tags[j]));

                if (sonTrouvé != null && !s.Contains(sonTrouvé))
                {
                    s.Add(sonTrouvé);
                }
            }
        }


        if (s.Count > 0)
            return s.ToArray();
        else
            Debug.LogError($"Erreur : Aucun clip avec les tags mentionnés n'a été trouvé dans la liste des sons.");
        return null;
    }
}