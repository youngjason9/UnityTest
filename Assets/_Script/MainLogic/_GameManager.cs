using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct curCatchPuzzle
{
    public GameObject puzzle;
    public int oldx;
    public int oldy;
}

public class _GameManager : SingletonMono< _GameManager>
{

    [SerializeField]private int canMoveCount = 3;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private float timer = 0f;

    public int CanMoveCount{get => canMoveCount;}
    public int CurrentLevel{get => currentLevel;}
    public float Timer{get => timer;}
    private BaseModel baseModel;
    /// 与游戏逻辑有关的变量
    private Grid grid;
    [SerializeField] private int x,y;   //鼠标当前所处的格子坐标
    private curCatchPuzzle curCatchBrick;   //鼠标当前抓住的方块
    public event Action  OnMouseDown;
    public event Action  OnMouseMove;
    public event Action  OnMouseUp;

    //实验性UI架构的测试变量
    public event Action OnPuzzleDrug;
    public event Action OnPuzzleSetDown;

    public Grid Grid
    {
        get {
            if(grid == null){
                Debug.LogError("Grid实例不存在！");
            }
            return grid; 
        }

        set{
            grid = value;
        }
    }


    //---------------------------------新加入的用于管理的类----------------------------------
    [Header("存放基底和拼图的仓库")]
    [SerializeField] private BaseAndPuzzle baseAndPuzzle;
    public BaseAndPuzzle BaseAndPuzzle{get => baseAndPuzzle;}


    [Header("存放游戏数据,Json格式")]
    [SerializeField] private TextAsset[] levelData;

    [Space(1)]
    [Header("是否进入编辑器模式")]
    public bool IsEditorMode;   //是否是编辑器模式，如果是的话只生成基础的网格，不生成关卡数据，同时允许编辑网格
    [SerializeField] private LevelEditor levelEditor;

    

    private void Awake()
    {
        Debug.Log("GameManager脚本已加载");
        if(BaseModel.Instance != null)
        {
            baseModel = BaseModel.Instance;
            baseModel.InitData(CurrentLevel, timer,canMoveCount);
        }

        if(!IsEditorMode){
            GameStart();
        }
        else{
            Debug.LogWarning("你正在使用编辑器模式，网格将不会生成，请在编辑器中编辑网格,与加入基底和拼图");
            levelEditor.SpawnTheGrid();
        }

    }

    private void OnEnable() 
    {
        OnPuzzleSetDown += UpdateGameData;
        OnMouseDown += CatchPuzzle;
        OnMouseMove += PuzzleMove;
        OnMouseMove += GetXYFromMousePos;
        OnMouseUp += SetPuzzleDown;
    }

    private void Start()
    {
        
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
            OnPuzzleDrug?.Invoke();
        }
        if(Input.GetMouseButtonUp(0)){
            OnMouseUp?.Invoke();
        }

        //持续绘制网格
        if(grid != null){
            grid.DrawGrid();
        }
        if(IsEditorMode){
            levelEditor.DrugTheBrick();
            levelEditor.PlaceTheBrick();
        }
    }

    private void OnDisable()
    {
        OnPuzzleSetDown -= UpdateGameData;
        OnMouseDown -= CatchPuzzle;
        OnMouseMove -= PuzzleMove;
        OnMouseUp -= SetPuzzleDown;
    }

    private void OnDestroy()
    {
        Debug.LogWarning("GameManager已销毁");
    }
    

    private void UpdateGameData()
    {
        baseModel.UpdateData(CurrentLevel, canMoveCount);
    }

    private void UpdateTimeDate()
    {
        timer += Time.deltaTime;
        baseModel.UpdateTime(timer);
    }


//----管理游戏进程的函数----
    /// <summary>
    /// 游戏开始时调用的函数，他会进行网格和他其中信息的初始化
    /// </summary>
    /// <param name="Count">可移动步数</param>
    /// <param name="Level">当前关卡</param>
    public void GameStart()
    {
         Debug.Log("游戏开始");
         //TODO：读取文件数据初始化网格，基底，拼图
         //OnLoadGrid();             
    }

    /// <summary>
    /// 编辑器模式下，开始游戏测试
    /// </summary>
    GameObject[,] baseBricks;
    GameObject[,] puzzleBricks;
    public void OnTestStart()
    {
        if(IsEditorMode){
             Debug.LogWarning("编辑器界面开始游戏关卡测试");
             //1.找到开始拼图
             PuzzleStartOrEnd puzzleStartOrEnd = grid.GetStartPuzzle();
             //2.生成玩家
             if(puzzleStartOrEnd != null){
                 Debug.Log("找到起始拼图");
                 puzzleStartOrEnd.SpawnPlayer();
             }
             else{
                 Debug.LogError("因为没有找到起始拼图，所以无法加载进玩家");
             }
             //3.生成其他生物
             //4.保存当前砖块位置和状态
             for(x = 0; x < Grid.Width; x++){
                for(y = 0; y < Grid.Height; y++){
                       baseBricks[x,y] = grid.GetBase(x,y);
                       puzzleBricks[x,y] = grid.GetPuzzle(x,y);
                }
             }
             //5.开始测试
             IsEditorMode = false;
         }    
    }
    /// <summary>
    /// 停止测试，清除所有生物
    /// </summary>
    public void OnTestStop()
    {
        if(IsEditorMode){
            Debug.LogWarning("编辑器界面停止游戏关卡测试");
            //1.清除所有生物
            GameObject animals = GameObject.FindAnyObjectByType<Animal>().gameObject;
            while(animals != null){
                Destroy(animals);
                animals = GameObject.FindAnyObjectByType<Animal>().gameObject;
            }

            //2.重置基底和拼图
            for(x = 0; x < Grid.Width; x++){
                for(y = 0; y < Grid.Height; y++){
                    Grid.SetBase(x, y, baseBricks[x,y]);
                    Grid.SetPuzzle(x, y, puzzleBricks[x,y]);
                }
            }
            //3.重置可移动步数
            canMoveCount = levelEditor.steps;
            //4.重置时间
            timer = 0f;
            //5.重置编辑器模式
            IsEditorMode = true;
        }
    }
    public void LoadNewLevel()
    {
        currentLevel++;
        Debug.Log("加载新关卡：" + CurrentLevel);
        GameOver();
        GameStart();
    }
    public void GameOver()
    {
        Debug.Log("Game Over");
        //TODO：销毁当前场景的所有物体
        grid = null;
        
        SceneManager.LoadScene(0);
    }

//----管理游戏数据和逻辑的函数----
    private void GetXYFromMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        if(Grid != null){
            Grid.GetXY(worldPos, out x, out y);
        }
        
    }

    private void CatchPuzzle()
    {
        if (curCatchBrick.puzzle != null)
        {return;}
        if(Grid == null){
            Debug.LogError("Grid实例不存在！");
            return;
        }
        if(IsEditorMode){
            //Debug.LogWarning("你正在使用编辑器模式，无法抓取拼图");
            return;
        }

        GetXYFromMousePos();
        curCatchBrick.puzzle = grid.CatchPuzzle(x, y);
        if (curCatchBrick.puzzle != null)
        {
            curCatchBrick.oldx = x;
            curCatchBrick.oldy = y;
            curCatchBrick.puzzle.GetComponent<Collider2D>().enabled = false;
            Collider2D[] colliders = curCatchBrick.puzzle.GetComponents<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = false;
            }
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
            curCatchBrick.puzzle.GetComponent<Collider2D>().enabled = true;
            Collider2D[] colliders = curCatchBrick.puzzle.GetComponents<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                collider.enabled = true;
            }
        }
        else {
            //Debug.Log("放下失败，该位置已有其他拼图");
        }
        OnPuzzleSetDown?.Invoke();
        curCatchBrick.puzzle = null;
    }

    public void UseOnePuzzle()
    {
        if(canMoveCount > 0) {
            canMoveCount--;
            Debug.Log("剩余可移动次数："  + canMoveCount);
        }
    }

    public bool CanMovePuzzle()
    {
        if (canMoveCount > 0) {
            return true;
        }
        else {
            Debug.Log("剩余可移动次数不足，无法移动");
            return false;
        }
    }

    ///-------------用于在按钮处绑定的函数----------------
    ///
    public void OnSaveGrid()
    {
        if(grid != null){
            _SaveLoadManager.Instance.Save(grid);
        }
        else{
            Debug.LogError("当前网格实例不存在或者未初始化完成！");
        }    
    }

    public void OnLoadGrid()
    {
        //TODO：从文件中加载数据初始化网格，基底，拼图
        if(grid != null){
            Debug.LogWarning("当前网格实例已存在，将会覆盖！");
            grid = null;
        }
        if(levelData.Length == 0)
            Debug.LogError("你是不是忘记把关卡数据文件拖进去了呀？");
        if(CurrentLevel-1 > levelData.Length || CurrentLevel-1 < 0)
            Debug.LogError("请输入正确的关卡编号（大于0小于等于"+levelData.Length+"）");


        GridData gridData = _SaveLoadManager.Instance.Load(levelData[CurrentLevel]);    //第0个场景是测试用的
        
        if(gridData != null){
            //gridData.PrintInfo();
            grid = new Grid(gridData);
        }
        else{
            Debug.LogError("没有从文件中加载到数据！");
        }
        
        //TODO：初始化场景中的生物
        PuzzleStartOrEnd puzzleStart = grid.GetStartPuzzle();
        if(puzzleStart != null){
            Debug.Log("找到起始拼图");
            puzzleStart.SpawnPlayer();
        }
        else{
            Debug.LogError("因为没有找到起始拼图，所以无法加载进玩家");
        } 
    }

    public void OnLoadGrid(TextAsset levelData)
    {
        if(grid != null){
            Debug.LogWarning("当前网格实例已存在！,当前版本不要重复操作载入");
            return;
        }

        GridData gridData = _SaveLoadManager.Instance.Load(levelData);  
        if(gridData != null){
            //gridData.PrintInfo();
            grid = new Grid(gridData);
            //TODO：存储初始数据，用来还原
        }
        else{
            Debug.LogError("没有从文件中加载到数据！");
        }

        //TODO：初始化场景中的生物
        PuzzleStartOrEnd puzzleStart = grid.GetStartPuzzle();
        if(puzzleStart != null){
            Debug.Log("找到起始拼图");
            puzzleStart.SpawnPlayer();
        }
        else{
            Debug.LogError("因为没有找到起始拼图，所以无法加载进玩家");
        } 

    }

    /// <summary>
    /// 设置所有基底为普通砖块
    /// </summary>
    public void OnSetAllNormalBase(GameObject brick)
    {
        if(IsEditorMode){
            levelEditor.SetAllNormalBase(brick);
        }
    }

    public void OnRestTheGird()
    {
        levelEditor.SpawnTheGrid();
    }

    public void OnGetBrick(GameObject brick)
    {
        if(brick == null){
            Debug.LogError("你还没有加入点击会获取的砖块");
            return;
        }

        if(IsEditorMode){
            GameObject newBrick = GameObject.Instantiate(brick);
            levelEditor.GetTheBrick(newBrick);
        }
        else{
            Debug.LogWarning("你正在使用游戏模式，无法获取砖块");
        }
    }

    public void OnResetBrick()
    {
        if(!IsEditorMode && Grid != null){
            Grid.ResetAllBrick();
        }
    }
}
