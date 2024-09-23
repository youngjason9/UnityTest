using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public bool canCatch;    // 是否可以被捡起
    public bool canExChange;    // 是否可以交换
    public bool hasFlag;    // 是否有旗子
    public bool isBase; // 是否是基地砖块

    [SerializeField]private int x,y;    // 砖块的坐标
    public int X{ get{ return x; } }
    public int Y{ get{ return y; } }

    /// <summary>
    /// 玩家碰撞到砖块时，砖块不能被捡起
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("T_playerBody") && !hasFlag && !isBase) {
            canCatch = false;
        }
    }

    /// <summary>
    /// 玩家离开砖块时，砖块可以被捡起
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("T_playerBody") && !hasFlag && !isBase) {
            canCatch = true;
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
