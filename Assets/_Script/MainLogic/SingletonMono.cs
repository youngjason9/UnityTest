using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> :MonoBehaviour where T : MonoBehaviour
{
    //1.判断当前实例是否存在
    public static bool IsExisted {get;private set;} = false;
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent<T>();
                }
            }
            DontDestroyOnLoad(instance);
            IsExisted = true;
            return instance;
        }
    }

    protected SingletonMono(){}

     private void OnDestroy(){
         IsExisted = false;
     }
    
}
