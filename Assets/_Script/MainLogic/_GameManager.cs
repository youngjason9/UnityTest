using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct curCatchPuzzle
{
    public GameObject puzzle;
    public int oldx;
    public int oldy;
}

public class _GameManager : MonoBehaviour
{
    private static _GameManager instance;
    public static _GameManager Instance
    {
        get {
            if (instance == null) {
                instance = new _GameManager();
            }
            return instance;
        }
    }

    //实验性UI架构的测试变量
    public Action OnPuzzleDrug;
    public Action OnPuzzleSetDown;

    [SerializeField] private int CanMoveCount = 3;
    [SerializeField] private int CurrentLevel = 1;
    [SerializeField] private float time = 0f;

    private BaseModel baseModel;

    /// 与游戏逻辑有关的变量
    private Grid grid;
    [SerializeField] private LevelGenerator levelGenerator;   //关卡生成器
    [SerializeField] private int width = 7;
    [SerializeField] private int height = 3;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Vector3 originPos;
    [SerializeField] private int x,y;   //鼠标当前所处的格子坐标
    [SerializeField] private GameObject baseBrick;
    [SerializeField] private GameObject puzzleBrick;

    private curCatchPuzzle curCatchBrick;   //鼠标当前抓住的方块

    public event Action  OnMouseDown;
    public event Action  OnMouseMove;
    public event Action  OnMouseUp;

    public Grid Grid
    {
        get { return grid; }
    }

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        baseModel = BaseModel.Instance;
        baseModel.InitData(CurrentLevel, time,CanMoveCount);

        OnPuzzleSetDown += UpdateGameData;
        OnMouseDown += CatchPuzzle;
        OnMouseMove += PuzzleMove;
        OnMouseMove += GetXYFromMousePos;
        OnMouseUp += SetPuzzleDown;
    }

    private void Start()
    {
        GameStart(CanMoveCount, CurrentLevel);
    }

    private void Update()
    {
        UpdateTimeDate();

        //鼠标控制拼图移动
        if(Input.GetMouseButtonDown(0)){
            OnMouseDown?.Invoke();
        }
        if(Input.GetMouseButton(0) && curCatchBrick.puzzle!= null){
            OnMouseMove?.Invoke();
        }
        if(Input.GetMouseButtonUp(0)){
            OnMouseUp?.Invoke();
        }

    }

    private void OnDestroy()
    {
        OnPuzzleSetDown -= UpdateGameData;
        OnMouseDown -= CatchPuzzle;
        OnMouseMove -= PuzzleMove;
        OnMouseUp -= SetPuzzleDown;
    }

    private void UpdateGameData()
    {
        baseModel.UpdateData(CurrentLevel, CanMoveCount);
    }

    private void UpdateTimeDate()
    {
        time += Time.deltaTime;
        baseModel.UpdateTime(time);
    }


//----管理游戏进程的函数----
    /// <summary>
    /// 游戏开始时调用的函数，他会进行网格和他其中信息的初始化
    /// </summary>
    /// <param name="Count">可移动步数</param>
    /// <param name="Level">当前关卡</param>
    public void GameStart(int Count, int Level)
    {
        CanMoveCount = Count;
        CurrentLevel = Level;

        grid = new Grid(width, height, cellSize, originPos);

        //临时初始化一些测试用的砖块
        //TODO：初始化基底砖块
        // for(x = 0; x < width; x++){
        //     for(y = 0; y < height; y++){
        //         grid.SetBase(x, y, Instantiate(baseBrick));
        //     }
        // }

        // for(x = 0; x < width; x++){
        //     for(y = 0; y < 1; y++){
        //         grid.SetPuzzle(x, y, Instantiate(puzzleBrick));
        //     }
        // }

         levelGenerator.LevelInit(Level,Count,grid);
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        SceneManager.LoadScene(0);
    }

//----管理游戏数据和逻辑的函数----
    private void GetXYFromMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        grid.GetXY(worldPos, out x, out y);
    }

    private void CatchPuzzle()
    {
        if (curCatchBrick.puzzle != null)
        {return;}

        GetXYFromMousePos();
        curCatchBrick.puzzle = grid.CatchPuzzle(x, y);
        if (curCatchBrick.puzzle != null)
        {
            curCatchBrick.oldx = x;
            curCatchBrick.oldy = y;
            //Debug.Log($"抓住了拼图{curCatchBrick.puzzle.name},它得老位置是{curCatchBrick.oldx},{curCatchBrick.oldy}");
        }
        else {
            //Debug.Log("该处没有可抓取的拼图");
        }
    }

    private void PuzzleMove()
    {
        if (curCatchBrick.puzzle == null)
        {return;}

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        curCatchBrick.puzzle.transform.position = new Vector3(mousePos.x, mousePos.y, -1);
    }

    private void SetPuzzleDown()
    {
        if (curCatchBrick.puzzle == null)
        {return;}
        
        if(grid.SetPuzzleDown(curCatchBrick.oldx, curCatchBrick.oldy, x, y, curCatchBrick.puzzle)){
            //Debug.Log("拼图放下了");
        }
        else {
            //Debug.Log("放下失败，该位置已有其他拼图");
        }
        curCatchBrick.puzzle = null;
    }



    public void UseOnePuzzle()
    {
        if(CanMoveCount > 0) {
            CanMoveCount--;
            Debug.Log("剩余可移动次数："  + CanMoveCount);
        }
    }

    public bool CanMovePuzzle()
    {
        if (CanMoveCount > 0) {
            return true;
        }
        else {
            Debug.Log("剩余可移动次数不足，无法移动");
            return false;
        }
    }
}
