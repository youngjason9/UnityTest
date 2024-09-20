using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Misco.Utlis;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Grid
{
    private int width;
    private int height;
    private int[,] gridArray;
    private float cellSize;
    private Vector3 originPosition;
    private Vector3[,] gridCenter;
    private GameObject[,] bricks;

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        gridArray = new int[width, height];
        gridCenter = new Vector3[width, height];
        bricks = new GameObject[width, height];
        this.originPosition = originPosition;


        for(int x = 0; x<gridArray.GetLength(0); x++) {
            for(int y = 0; y<gridArray.GetLength(1); y++) {
                gridCenter[x,y] = GetWorldPosition(x,y) + new Vector3(cellSize,cellSize)*0.5f;
                UtlisClass.CreateWorldText(gridArray[x, y].ToString(),null, gridCenter[x,y],20,Color.white,TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x+1,y), Color.white,999f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y+1), Color.white,999f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white,999f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white,999f);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y ) * cellSize + originPosition;
    }

    /// <summary>
    /// 根据世界坐标获取格子下标
    /// </summary>
    public void GetXY(Vector3 position, out int x, out int y)
    {
        if (position.x < originPosition.x || position.y < originPosition.y || position.x > width * cellSize+originPosition.x || position.y > height * cellSize+originPosition.y) {
            x = -1;
            y = -1;
        }
        else {
            x = Mathf.FloorToInt((position.x - originPosition.x) / cellSize);
            y = Mathf.FloorToInt((position.y - originPosition.y )/ cellSize);
        }
    }


    /// <summary>
    /// 根据格子下标获取格子中心坐标
    /// </summary>
    public Vector3 GetGridCenter(int x, int y)
    {
        return gridCenter[x, y];
    }

    /// <summary>
    /// 根据世界坐标获取格子中心坐标
    /// </summary>
    public Vector3 GetGridCenter(Vector3 position)
    {
        int x, y;
        GetXY(position, out x, out y);
        return GetGridCenter(x, y);
    }   

    /// <summary>
    /// 设置格子
    /// </summary>
    public bool SetTile(int x, int y, GameObject tile)
    {
        if(x>=0 && x<width && y>=0 && y < height) {
            if (tile != null) {
                UnityEngine.Transform transform = tile.transform;
                transform.position = GetGridCenter(x, y);
                transform.localScale = new Vector3(cellSize, cellSize);
                if(bricks[x,y] != null) {
                    GameObject.Destroy(bricks[x, y]);
                }
                bricks[x, y] = tile;

            }
            return true;
        }
        else {
            Debug.Log("SetTile: out of range");
            return false;
        }
    }

    public bool SetTile(Vector3 position, GameObject tile)
    {
        int x, y;
        GetXY(position, out x, out y);
        return SetTile(x, y, tile);
    }

    public GameObject CatchTile(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height) {
            GameObject tile = bricks[x, y];
            Brick brick = tile.GetComponent<Brick>();
            if (brick != null && brick.canCatch) {
                bricks[x, y] = null;
                return tile;
            }
        }

        return null;
        
    }

    /// <summary>
    /// 放置与交换两个格子内的砖块
    /// </summary>
    /// <param name="beforx">拼图砖块之前的格子x下标</param>
    /// <param name="befory">拼图砖块之前的格子y下标</param>
    /// <param name="nowx">放置处的格子x下标</param>
    /// <param name="nowy">放置处的格子y下标</param>
    /// <param name="tile">交换用的砖块</param>
    public void DropTile(int beforx, int befory, int nowx, int nowy, GameObject tile)
    {
        if(nowx >= 0 && nowx < width && nowy >= 0 && nowy < height) {
            if((beforx == nowx && befory == nowy)) {
                SetTile(nowx, nowy, tile);
                return;
            }
            Brick brick = bricks[nowx, nowy].GetComponent<Brick>();
            if (brick != null && brick.canExChange) {
                SetTile(beforx, befory, bricks[nowx, nowy]);
                bricks[nowx, nowy] = null;
                SetTile(nowx, nowy, tile);
                _GameManager.Instance.UseOnePuzzle();
                return;
            }
        }

        SetTile(beforx, befory, tile);


    }

    public void SetFlag(int x, int y, GameObject flag)
    {
        if (x >= 0 && x < width && y >= 0 && y < height) {
            flag.transform.SetParent(bricks[x, y].transform);
            flag.transform.localPosition = new Vector3(0, -0.5f, 0);
            Brick brick = bricks[x, y].GetComponent<Brick>();
            if (brick != null) {
                brick.canCatch = false;
                brick.hasFlag = true;
            }
        }
    }
}
