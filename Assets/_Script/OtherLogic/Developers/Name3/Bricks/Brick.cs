using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BaseType{
    /// <summary>
    /// 未知基底
    /// </summary>
    None,
    /// <summary>
    /// 最基础普通的基底（拼图放上去之后可以拿下来）
    /// </summary>
    normalBase,
    /// <summary>
    /// 固定型基底（拼图放上去之后不能拿下来）
    /// </summary>
    FixedBase,
    /// <summary>
    /// 旋转型基底（拼图放上去之后在接收到信号后会旋转90度,可以拿下来）
    /// </summary>
    RotaBase,
    /// <summary>
    /// 固定型旋转型基底（拼图放上去之后不能拿下来，在接收到信号后会旋转90度）
    /// </summary>
    FiexedRotaBase
}

[System.Serializable]
public enum PuzzleType{
    /// <summary>
    /// 未知类型拼图
    /// </summary>
    None,
    /// <summary>
    /// 开始拼图（玩家出生位置的拼图）
    /// </summary>
    startPuzzle,
    /// <summary>
    /// 结束拼图（玩家需要到达的终点位置的拼图）
    /// </summary>
    EndPuzzle,
    /// <summary>
    /// 空气拼图（可以正常通过的拼图）
    /// </summary>
    airPuzzle,
    /// <summary>
    /// 半空气拼图（有一半可以通过的拼图）
    /// </summary>
    halfAirPuzzle,
    /// <summary>
    /// 普通拼图（玩家不能通过的拼图）
    /// </summary>
    normalPuzzle,
    /// <summary>
    /// 带刺拼图（上半部分带刺，生物触碰到之后会死亡）
    /// </summary>
    crowdedPuzzle,
    /// <summary>
    /// 按钮拼图（上面有按钮的拼图，当受到按下的力之后XY轴方向3格内的基底会持续接收到信号）
    /// </summary>
    buttonPuzzle,
    /// <summary>
    /// 射箭拼图（当接收到信号后，会持续固定间隔时间射出箭头）
    /// </summary>
    arrowPuzzle,
    /// <summary>
    /// 门拼图（当接收到信号后会打开，此时玩家可以通过，信号断开时会关闭）
    /// </summary>
    doorPuzzle,
    /// <summary>
    /// 弹簧拼图（当有生物挤压它的左右面时，会将生物弹飞到四格之外）
    /// </summary>
    springPuzzle
}


public class Brick : MonoBehaviour
{
    [Header("砖块的基本属性")]
    /// <summary>
    /// 是否能被拿起来
    /// </summary>
    [SerializeField]private bool canCatch;  
    /// <summary>
    /// 是否有旗子（是开始或者结束方块）
    /// </summary>
    [SerializeField]private bool hasFlag;   
    /// <summary>
    /// 是否是基底砖块
    /// </summary>
    [SerializeField]private bool isBase; 

    private bool isFixed;   //是否被固定
    private int x,y;    // 砖块的坐标

    public bool CanCatch{ 
        get
        { 
            if (isFixed) 
                return false;
            else
                return canCatch; 
        } 
        set
        { 
            canCatch = value; 
        } 
    }
    public bool HasFlag{ get{ return hasFlag; } set{ hasFlag = value; } }
    public bool IsBase{ get{ return isBase; } set{ isBase = value; } }
    public bool IsFixed{ get{ return isFixed; } set{ isFixed = value; } }
    public int X{ get{ return x; } }
    public int Y{ get{ return y; } }

    /// <summary>
    /// 玩家碰撞到砖块时，砖块不能被捡起
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("T_playerBody") && !hasFlag && !isBase) {
            CanCatch = false;
        }
    }

    /// <summary>
    /// 玩家离开砖块时，砖块可以被捡起
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("T_playerBody") && !hasFlag && !isBase) {
            CanCatch = true;
        }
    }

    public void SetXY(int x, int y)
    {
        this.x = x;
        this.y = y;

        //顺便设置一下砖块名称了
        if(isBase)
            transform.name = "BaseBrick" + x + "_" + y;
        else
            transform.name = "PuzzleBrick" + x + "_" + y;
    }
}
