using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBrick : Brick      //最基础的基底类
{
    private Collider2D col2D;
    protected GameObject puzzle;  //在这个基底上的拼图，为null则表示上方没有任何拼图

    private void Awake()
    {
        col2D = GetComponent<Collider2D>();       
    }

    private void OnDestroy()
    {
       
    }

    public virtual void BaseFuncRun(){
        Debug.Log("基础基底功能运行，碰撞箱关闭");
        col2D.enabled = false;
        puzzle = _GameManager.Instance.Grid.GetPuzzle(X, Y);
    }

    public virtual void BaseFuncStop(){
        Debug.Log("基础基底功能停止，碰撞箱开启");
        col2D.enabled = true;
        puzzle = null;
    }
}
