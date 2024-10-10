using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
* PersistentSingleton.cs
* Created by: Evan Robertson
* Date Created: 2024-10-10
* 
* Description: Abstract class used for creating singleton classes that persist between scenes
* 
* Last Changed by: Evan Robertson
* Last Date Changed: 2024-10-10
* 
* 
*   -> 1.0 - Created PersistentSingletons.cs
*   
*   v1.0
*/
public abstract class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    public bool AutoUnparentOnAwake = true;

    protected static T instance;
    public static bool HasInstance => instance != null;
    public static T TryGetInstance() => HasInstance ? instance : null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    var go = new GameObject(typeof(T).Name + " Generated");
                    instance = go.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        InitializeSingleton();
    }

    protected virtual void InitializeSingleton()
    {
        if (!Application.isPlaying) return;
        if (AutoUnparentOnAwake) transform.SetParent(null);
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}

