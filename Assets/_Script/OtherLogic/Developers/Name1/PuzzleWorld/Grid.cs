using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoseTools.Utlis;
using static UnityEngine.RuleTile.TilingRuleOutput;
using System;


public class Grid
{
    private int width;
    private int height;
    private int[,] gridArray;
    private float cellSize;
    private Vector3 originPosition;
    private Vector3[,] gridCenter;
    private GameObject[,] puzzles;   //拼图
    private GameObject[,] bases;    //基底

    public int Width { get { return width; } }
    public int Height { get { return height; } }
    public float CellSize { get { return cellSize; } }

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        gridArray = new int[width, height];
        gridCenter = new Vector3[width, height];
        puzzles = new GameObject[width, height];
        bases = new GameObject[width, height];
        this.originPosition = originPosition;


        for(int x = 0; x<gridArray.GetLength(0); x++) {
            for(int y = 0; y<gridArray.GetLength(1); y++) {
                gridCenter[x,y] = GetWorldPosition(x,y) + new Vector3(cellSize,cellSize)*0.5f;
                UtlisClass.CreateWorldText(gridArray[x, y].ToString(),null, gridCenter[x,y],10,Color.white,TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x+1,y), Color.white,999f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y+1), Color.white,999f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white,999f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white,999f);
    }


    /// <summary>
    /// 根据格子下标获取世界坐标
    /// </summary>
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
    public void SetFlag(int x, int y, GameObject flag)
    {
        if (x >= 0 && x < width && y >= 0 && y < height) {
            flag.transform.SetParent(puzzles[x, y].transform);
            flag.transform.localPosition = new Vector3(0, -0.5f, 0);
            Brick brick = puzzles[x, y].GetComponent<Brick>();
            if (brick != null) {
                brick.canCatch = false;
                brick.hasFlag = true;
            }
        }
    }

//---------以下为拼图相关-----
    public void SetBase(int x, int y, GameObject baseObj)
    {
        Vector3 basePos = GetGridCenter(x, y);
        if (x >= 0 && x < width && y >= 0 && y < height && bases[x, y] == null) {
            baseObj.transform.position = basePos + new Vector3(0,0,0);
            baseObj.transform.localScale = new Vector3(cellSize, cellSize);
            bases[x, y] = baseObj;
            baseObj.GetComponent<Brick>().SetXY(x, y);
        }
        else{
            Debug.LogWarning($"[{x},{y}]这个地方不能够放置基底!");
        }
    }

    public void SetBase(Vector3 position, GameObject baseObj){
        int x, y;
        GetXY(position, out x, out y);
        SetBase(x, y, baseObj);
    }

    public void SetPuzzle(int x, int y, GameObject puzzle)
    {
        Vector3 puzzlePos = GetGridCenter(x, y);
        if (x >= 0 && x < width && y >= 0 && y < height && puzzles[x, y] == null) {
            puzzle.transform.position = puzzlePos + new Vector3(0, 0, -0.5f);
            puzzle.transform.localScale = new Vector3(cellSize, cellSize);
            puzzles[x, y] = puzzle;
            puzzle.GetComponent<Brick>().SetXY(x,y);

            //TODO：放上去之后，需要调用放置处基底的逻辑
            bases[x, y].GetComponent<BaseBrick>().BaseFuncRun();
            Debug.Log($"bases[{x},{y}]被调用了");
        }
        else{
            Debug.LogWarning("这个地方不能够放置拼图!");
        }
    }

    public void SetPuzzle(Vector3 position, GameObject puzzle)
    {
        int x, y;
        GetXY(position, out x,out y);
        SetPuzzle(x, y, puzzle);
    }
    public GameObject CatchPuzzle(int x, int y)
    {
        if(x<0 || x>=width || y<0 || y>=height || puzzles[x,y] == null)
            return null;
    
        PuzzleBrick puzzleBrick = puzzles[x,y].GetComponent<PuzzleBrick>();
        if (puzzleBrick.canCatch)  
            return puzzles[x, y];
        else
            return null;
    }

    public bool SetPuzzleDown(int oldx, int oldy, int x, int y, GameObject puzzle)
    {
        if (x >= 0 && x < width && y >= 0 && y < height && puzzles[x, y] == null) {
            puzzle.transform.position = GetGridCenter(x, y);
            puzzles[x, y] = puzzle;      
            puzzle.GetComponent<Brick>().SetXY(x, y);
            puzzles[oldx, oldy] = null;

            //TODO：放上去之后，需要调用放置处基底的逻辑
            bases[x, y].GetComponent<BaseBrick>().BaseFuncRun();
            bases[oldx, oldy].GetComponent<BaseBrick>().BaseFuncStop();
            return true;
        }
        else{
            //Debug.Log("这个地方不能够放置拼图");
            //回弹回回来的位置
            puzzle.transform.position = GetGridCenter(oldx, oldy);
            return false;
        }
    }

    public GameObject GetPuzzle(int x, int y)
    {
        return puzzles[x, y];
    }
}
