using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseView : MonoBehaviour
{
    //控件的查找
    [SerializeField] private Text stepCountText;
    [SerializeField] private Text timeText;
    [SerializeField] private Text levelText;

    //控件的修改方法
    public void UpdateInfo(int stepCount, int level)
    {
        stepCountText.text = "剩余步数：" + stepCount.ToString();
        levelText.text = "关卡：" + level.ToString();
    }

    public void UpdateTime(float time)
    {
        timeText.text = "时间：" + time.ToString("F0");
    }
}
