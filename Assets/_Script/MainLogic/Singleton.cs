using System;
using UnityEngine;
public class Singleton<T> where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null){
                instance = Activator.CreateInstance(typeof(T), true) as T;
            }
            return instance;
        }
    }

    //私有构造函数，不能在外部创建实例
    protected Singleton(){}
}


