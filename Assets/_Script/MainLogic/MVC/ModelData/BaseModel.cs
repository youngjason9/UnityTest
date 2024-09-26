using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseModel
{
    private int level;
    private float time;
    private int stepCount;

    public int Level { get => level; set => level = value; }
    public float Time { get => time; set => time = value; }
    public int StepCount { get => stepCount; set => stepCount = value; }

    private static BaseModel instance;
    public static BaseModel Instance
    {
        get {
            if (instance == null) {
                instance = new BaseModel();
            }
            return instance;
        }
    }

    private event Action<BaseModel> OnValueChanged;

    //初始化数据
    public void InitData(int level, float time, int stepCount)
    {
        this.level = PlayerPrefs.GetInt("LevelValue",1);
        this.time = PlayerPrefs.GetFloat("TimeValue",0);
        this.stepCount = PlayerPrefs.GetInt("StepCountValue",3);
    }

    //更新数据
    public void UpdateData(int level, int stepCount)
    {
        this.level = level;
        this.stepCount = stepCount;

        SaveData();
    }

    public void UpdateTime(float time)
    {
        this.time = time;

        SaveData();
    }

    //存储数据
    public void SaveData()
    {
        PlayerPrefs.SetInt("LevelValue", level);
        PlayerPrefs.SetFloat("TimeValue", time);
        PlayerPrefs.SetInt("StepCountValue", stepCount);

        TriggerEvent();
    }

    //注册事件
    public void AddEvent(Action<BaseModel> func)
    {
        OnValueChanged += func;
    }

    //移除事件
    public void RemoveEvent(Action<BaseModel> func)
    {
        OnValueChanged -= func;
    }

    //触发事件
    private void TriggerEvent()
    {
        OnValueChanged?.Invoke(this);
    }
}
