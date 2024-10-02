using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoseTools.Utlis;
using static UnityEngine.RuleTile.TilingRuleOutput;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.Diagnostics;


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

    private GridData gridData;

    

    public int Width { get { return width; } }
    public int Height { get { return height; } }
    public int[,] GridArray { get { return gridArray; } }
    public Vector3 OriginPosition { get { return originPosition; } }
    public float CellSize { get { return cellSize; } }
    public Vector3[,] GridCenter { get { return gridCenter; } }
    public GameObject[,] Puzzles { get { return puzzles; } private set { puzzles = value;}}
    public GameObject[,] Bases { get { return bases; }  private set { bases = value; } }



    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        gridArray = new int[width, height];
        gridCenter = new Vector3[width, height];
        Puzzles = new GameObject[width, height];
        Bases = new GameObject[width, height];
        this.originPosition = originPosition;

        for(int x = 0; x<gridArray.GetLength(0); x++) {
            for(int y = 0; y<gridArray.GetLength(1); y++) {
                gridCenter[x,y] = GetWorldPosition(x,y) + new Vector3(cellSize,cellSize)*0.5f;
                //UtlisClass.CreateWorldText(gridArray[x, y].ToString(),null, gridCenter[x,y],5,Color.white,TextAnchor.MiddleCenter);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x+1,y), Color.white,999f);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y+1), Color.white,999f);
            }
        }

        // Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white,999f);
        // Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white,999f);
    }

    public void UpdateGrid(int width, int height, float cellSize, Vector3 originPosition)
    {
         this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        gridArray = new int[width, height];

        //清除旧的网格上的基底和拼图
        for(int x = 0; x<Puzzles.GetLength(0); x++){
            for(int y = 0; y<Puzzles.GetLength(1); y++){
                if(Puzzles[x,y]!= null){
                    GameObject.Destroy(Puzzles[x,y]);
                }
                if(Bases[x,y]!= null){
                    GameObject.Destroy(Bases[x,y]);
                }
            }
        }
        gridCenter = new Vector3[width, height];
        Puzzles = new GameObject[width, height];
        Bases = new GameObject[width, height];
        this.originPosition = originPosition;


        //更新中心网格的数组的位置
        for(int x = 0; x<gridArray.GetLength(0); x++) {
            for(int y = 0; y<gridArray.GetLength(1); y++) {
                gridCenter[x,y] = GetWorldPosition(x,y) + new Vector3(cellSize,cellSize)*0.5f;
                //UtlisClass.CreateWorldText(gridArray[x, y].ToString(),null, gridCenter[x,y],5,Color.white,TextAnchor.MiddleCenter);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x+1,y), Color.white,999f);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y+1), Color.white,999f);
            }
        }
    }

    public void DrawGrid()
    {
         for(int x = 0; x<width; x++) {
             for(int y = 0; y<height; y++) {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x+1,y), Color.white);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y+1), Color.white);
            }
        }

         Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white);
         Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white);
    }

    /// <summary>
    /// 根据传入一个包含相应信息的实例，从而进行初始化的构造方法
    /// </summary>
    public Grid(GridData gridData){
        this.gridData = gridData;
        width = gridData.width;
        height = gridData.height;
        cellSize = gridData.cellSize;
        originPosition = gridData.originPosition;
        gridArray = UtlisClass.OneDimToTwoDim<int>(gridData.gridArray, width, height);
        gridCenter = UtlisClass.OneDimToTwoDim<Vector3>(gridData.gridCenter, width, height);
        float [,] puzzleZAngle = UtlisClass.OneDimToTwoDim<float>(gridData.puzzleZAngle, width, height);
        float [,] baseZAngle = UtlisClass.OneDimToTwoDim<float>(gridData.baseZAngle, width, height);

        Puzzles = new GameObject[width, height];
        Bases = new GameObject[width, height];

        for(int x = 0; x<gridArray.GetLength(0); x++) {
            for(int y = 0; y<gridArray.GetLength(1); y++) {
                gridCenter[x,y] = GetWorldPosition(x,y) + new Vector3(cellSize,cellSize)*0.5f;
                //UtlisClass.CreateWorldText(gridArray[x, y].ToString(),null, gridCenter[x,y],5,Color.white,TextAnchor.MiddleCenter);
            }
        }

        
        //TODO：根据读入的枚举类型，实例化相应的拼图和基底
        PuzzleType[,] puzzleType= UtlisClass.OneDimToTwoDim<PuzzleType>(gridData.puzzles, width, height);
        BaseType[,] baseType = UtlisClass.OneDimToTwoDim<BaseType>(gridData.bases, width, height);

        Debug.Log("----------------------开始实例化拼图和基底-------------------");
        for(int x = 0; x<gridArray.GetLength(0); x++){
            for(int y = 0; y<gridArray.GetLength(1); y++){
                Debug.Log($"[{x},{y}] puzzleType={puzzleType[x,y]}, baseType={baseType[x,y]}");
            }
        }
        BaseAndPuzzle baseAndPuzzle = _GameManager.Instance.BaseAndPuzzle;

         for(int x = 0; x<gridArray.GetLength(0); x++) {
             for(int y = 0; y<gridArray.GetLength(1); y++) {
                //根据枚举类型，获取相应的拼图和基底预制体
                BaseBrick baseBrick = baseAndPuzzle.GetBase(baseType[x,y]);
                PuzzleBrick puzzleBrick = baseAndPuzzle.GetPuzzle(puzzleType[x,y]);

                //Debug.Log("baseBrick :" + baseBrick.gameObject.name);
                //Debug.Log("puzzleBrick :"+  puzzleBrick.gameObject.name);
                if(baseBrick!= null){
                    GameObject baseObj = GameObject.Instantiate(baseBrick.gameObject);
                    SetBase(x,y,baseObj,baseZAngle[x,y]);
                    
                }
                if(puzzleBrick!= null){
                    GameObject puzzleObj = GameObject.Instantiate(puzzleBrick.gameObject);
                    SetPuzzle(x,y,puzzleObj,puzzleZAngle[x,y]);
                }                              
             }
         }               
    }

    public void PrintInfo()
    {
        Debug.Log("Grid Info: width=" + width + ", height=" + height + ", cellSize=" + cellSize + ", originPosition=" + originPosition);
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
            flag.transform.SetParent(Puzzles[x, y].transform);
            flag.transform.localPosition = new Vector3(0, -0.5f, 0);
            Brick brick = Puzzles[x, y].GetComponent<Brick>();
            if (brick != null) {
                brick.CanCatch = false;
                brick.HasFlag = true;
            }
        }
    }

//---------以下为拼图相关-----

    /// <summary>
    /// 初始化设置基底
    /// </summary>
    /// <param name="x">x坐标</param>
    /// <param name="y">y坐标</param>
    /// <param name="baseObj">基底类型对象</param>
    public void SetBase(int x, int y, GameObject baseObj,float zAngle = 0)
    {
        
        Debug.Log("设置基底" + baseObj.name);

        if(baseObj == null) {
            Debug.LogWarning("传入的基底对象为空!使用默认基底！");
            baseObj = _GameManager.Instance.BaseAndPuzzle.BaseBricks[0].gameObject;
        }

        Vector3 basePos = GetGridCenter(x, y);
        if (x >= 0 && x < width && y >= 0 && y < height && Bases[x, y] == null) {
            baseObj.transform.position = basePos + new Vector3(0,0,0);
            baseObj.transform.localScale = new Vector3(cellSize, cellSize);
            baseObj.transform.Rotate(Vector3.forward*zAngle);
            Bases[x, y] = baseObj;
            Brick brick =baseObj.GetComponent<Brick>();
            
            if(brick!= null){
                brick.SetXY(x,y);
            }
            else{
                Debug.LogError($"这个基底{baseObj.name}上没有Brick组件!");
            }
        }
        else{
            Debug.LogWarning($"[{x},{y}]这个地方不能够放置基底!");
        }
    }

    /// <summary>
    /// 初始化设置基底
    /// </summary>
    /// <param name="position">世界坐标</param>
    public void SetBase(Vector3 position, GameObject baseObj){
        int x, y;
        GetXY(position, out x, out y);
        SetBase(x, y, baseObj);
    }

    /// <summary>
    /// 初始化设置拼图
    /// </summary>
    /// <param name="x">x坐标</param>
    /// <param name="y">y坐标</param>
    /// <param name="puzzle">拼图类型对象</param>
    public void SetPuzzle(int x, int y, GameObject puzzle,float zAngle = 0) 
    {
        if(puzzle == null) {
            Debug.LogWarning("传入的拼图对象为空!");
            return;
        }


        Vector3 puzzlePos = GetGridCenter(x, y);
        if (x >= 0 && x < width && y >= 0 && y < height && Puzzles[x, y] == null) {
            //设置拼图的初始数据
            puzzle.transform.position = puzzlePos + new Vector3(0, 0, -0.5f);
            puzzle.transform.localScale = new Vector3(cellSize, cellSize);
            puzzle.transform.Rotate(Vector3.forward*zAngle);
            Puzzles[x, y] = puzzle;

            Brick brick = puzzle.GetComponent<Brick>();
            if (brick != null) {
                brick.SetXY(x, y);
            }
            else {
                Debug.LogError($"这个拼图{puzzle.name}上没有Brick组件!");
            }

            //TODO：放上去之后，需要调用放置处基底的逻辑
            //Bases[x, y].GetComponent<BaseBrick>().PuzzleObj = puzzle;
            Bases[x, y].GetComponent<BaseBrick>().BaseFuncRun(puzzle);
        }
        else{
            Debug.LogWarning("这个地方不能够放置拼图!");
        }
    }

    /// <summary>
    /// 初始化设置拼图
    /// </summary>
    /// <param name="position">世界坐标</param>
    public void SetPuzzle(Vector3 position, GameObject puzzle)
    {
        int x, y;
        GetXY(position, out x,out y);
        SetPuzzle(x, y, puzzle);
    }

    /// <summary>
    /// 抓取拼图
    /// </summary>
    /// <param name="x">x坐标</param>
    /// <param name="y">y坐标</param>
    /// <returns>抓取到的拼图对象，为空则没有抓取到</returns>
    public GameObject CatchPuzzle(int x, int y)
    {
        if(x<0 || x>=width || y<0 || y>=height || Puzzles[x,y] == null)
            return null;
    
        PuzzleBrick puzzleBrick = Puzzles[x,y].GetComponent<PuzzleBrick>();
        if (puzzleBrick.CanCatch)  
            return Puzzles[x, y];
        else if(!puzzleBrick.IsBase){
            Debug.Log("该拼图处于固定基底上，不可被抓取");
            return null;
        }
        else{
            return null;
        }
    }

    /// <summary>
    /// 将指定的拼图在某个基底上
    /// </summary>
    /// <param name="oldx">拼图原来的x坐标</param>
    /// <param name="oldy">拼图原来的y坐标</param>
    /// <param name="x">目标基底的x坐标</param>
    /// <param name="y">目标基底的y坐标</param>
    /// <param name="puzzle">要放置的拼图对象</param>
    /// <returns>是否成功放置</returns>
    public bool SetPuzzleDown(int oldx, int oldy, int x, int y, GameObject puzzle)
    {
        if (x >= 0 && x < width && y >= 0 && y < height && Puzzles[x, y] == null) {
            puzzle.transform.position = GetGridCenter(x, y);
            Puzzles[x, y] = puzzle;      
            puzzle.GetComponent<Brick>().SetXY(x, y);
            //puzzle.GetComponent<PuzzleBrick>().BaseBrick = Bases[x, y].GetComponent<BaseBrick>();
            Puzzles[oldx, oldy] = null;

            //TODO：放上去之后，需要调用放置处基底的逻辑
            Bases[x, y].GetComponent<BaseBrick>().BaseFuncRun(puzzle);
            Bases[oldx, oldy].GetComponent<BaseBrick>().BaseFuncStop();
            return true;
        }
        else{
            //Debug.Log("这个地方不能够放置拼图");
            //回弹回回来的位置
            puzzle.transform.position = GetGridCenter(oldx, oldy);
            return false;
        }
    }

    /// <summary>
    /// 根据XY坐标获取相应的拼图
    /// </summary>
    public GameObject GetPuzzle(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height){
            Debug.LogWarning($"坐标({x},{y})超出范围,无法获取拼图");
            return null;
        }
            
        return Puzzles[x, y];
    }

    /// <summary>
    /// 根据XY坐标获取相应的基底
    /// </summary>
    public GameObject GetBase(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height){
            Debug.LogWarning($"坐标({x},{y})超出范围,无法获取拼图");
            return null;
        }
            
        return Bases[x, y];
    }

    /// <summary>
    /// 根据XY坐标获取相应的拼图的脚本
    /// </summary>
    public PuzzleBrick GetPuzzleScript(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height || Puzzles[x, y] == null){
            Debug.LogWarning($"坐标({x},{y})超出范围,无法获取拼图");
            return null;
        }
            
        return Puzzles[x, y].GetComponent<PuzzleBrick>();
    }

    /// <summary>
    /// 根据XY坐标获取相应的基底的脚本
    /// </summary>
    public BaseBrick GetBaseScript(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height || Bases[x, y] == null){
            Debug.LogWarning($"坐标({x},{y})超出范围,无法获取拼图");
            return null;
        }
            
        return Bases[x, y].GetComponent<BaseBrick>();
    }
    
    public PuzzleStartOrEnd GetStartPuzzle()
    {
        for (int i = 0; i<width; i++)
        {
            for(int j = 0; j < height; j++){
                if(Puzzles[i,j]!= null && Puzzles[i,j].GetComponent<PuzzleBrick>().PuzzleType == PuzzleType.startPuzzle){
                    return Puzzles[i,j].GetComponent<PuzzleStartOrEnd>();
                }
            }
        }

        Debug.LogWarning("场景中没有找到起始拼图，确认你是否放入");
        return null;
    }

//-----------------------编辑器中调用的方法-----------------

    /// <summary>
    /// 编辑器设置基底，可以覆盖掉原来的基底
    /// </summary>
    public bool SetBaseTile(int x, int y, GameObject baseObj)
    {
        if (x >= 0 && x < width && y >= 0 && y < height) {
            baseObj.transform.position = GetGridCenter(x, y);
            baseObj.transform.localScale = new Vector3(cellSize, cellSize);

            if(Bases[x,y]!= null){
                GameObject.Destroy(Bases[x,y]);
                Bases[x,y] = null;
            }
            Bases[x, y] = baseObj;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 编辑器设置拼图，可以覆盖掉原来的拼图
    /// </summary>
    public bool SetPuzzleTile(int x, int y, GameObject puzzle)
    {
        if (x >= 0 && x < width && y >= 0 && y < height) {

            if(Bases[x,y] == null){
                Debug.LogWarning("基底为空，无法放置拼图,请先在该格放置基底");
                return false;
            }

            puzzle.transform.position = GetGridCenter(x, y);
            puzzle.transform.localScale = new Vector3(cellSize, cellSize);

            if(Puzzles[x,y]!= null){
                GameObject.Destroy(Puzzles[x,y]);
                Puzzles[x,y] = null;
            }
            Puzzles[x, y] = puzzle;
            Bases[x, y].GetComponent<BaseBrick>().PuzzleObj = puzzle;

             //TODO：放上去之后，需要调用放置处基底的逻辑
            Bases[x, y].GetComponent<BaseBrick>().BaseFuncRun(puzzle);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 编辑器删除该格子内的拼图和基底
    /// </summary>
    public void ClearCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height) {
            if (Puzzles[x, y] != null) {
                GameObject.Destroy(Puzzles[x, y]);
                Puzzles[x, y] = null;
            }
            if (Bases[x, y] != null) {
                GameObject.Destroy(Bases[x, y]);
                Bases[x, y] = null;
            }
        }
    }

    /// <summary>
    /// 重置所有的砖块到初始位置
    /// </summary>
    public void ResetAllBrick()
    {

        Debug.Log("重置所有的砖块到初始位置(之后在做了)");
        //1.先把所有的砖块都清空

        //2.重新设置所有的砖块

        // //3.销毁存在的动物
        // Animal animal = GameObject.FindObjectOfType<Animal>();
        // if(animal != null)
        //     GameObject.Destroy(animal.gameObject);

        // //4.生成玩家
        // GetStartPuzzle().SpawnPlayer();
    }

    public void OnDestroyThisGrid()
    {
        //1.先把所有的砖块都清空
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                ClearCell(i, j);
            }
        }
    }


    ///这个网格类主要的数据有：
    ///1.格子的宽高，格子的大小，格子的原点坐标
    ///2.格子中心坐标数组，格子数组，基底数组，拼图数组
}
