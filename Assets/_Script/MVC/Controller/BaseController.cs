using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    //获取View
    private BaseView baseView;
    private BaseModel baseModel;

    private static BaseController instance;

    public static BaseController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BaseController();
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = GetComponent<BaseController>();
    }

    private void Start()
    {
        baseView = GetComponent<BaseView>();
        baseModel = BaseModel.Instance;
        if (baseView != null) {
            baseView.UpdateInfo(baseModel.StepCount, baseModel.Level);
        }
        baseModel.AddEvent(UpdateViewInfo);
    }

    private void OnDestroy()
    {
        baseModel.RemoveEvent(UpdateViewInfo);
    }

    //更新View
    public void UpdateViewInfo(BaseModel baseModel)
    {
        baseView.UpdateInfo(baseModel.StepCount, baseModel.Level);
        baseView.UpdateTime(baseModel.Time);
    }

    //public void ShowToast(BaseModel baseModel)
    //{
    //    if (baseModel.IsGameOver) {
    //        baseView.ShowGameOver();
    //    }
    //    else if (baseModel.IsWin) {
    //        baseView.ShowWin();
    //    }
    //}
}
