using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBrick : Brick      //最基础的基底类
{
    

    [Header("基底类型")]
    [SerializeField] private BaseType baseType;
    
    [SerializeField] private int reciveSignalNum; //接收到的信号数量
    private BaseBrick[] nighborBaseBricks;
    private Collider2D col2D;

    /// <summary>
    /// 在这个基底上的拼图，为null则表示上方没有任何拼图
    /// </summary>
    protected GameObject puzzle;  
    public GameObject PuzzleObj{ get => puzzle; set => puzzle = value; }
    public BaseType BaseType { get => baseType; set => baseType = value; }   
    public BaseBrick[] NighborBaseBricks { get => nighborBaseBricks; }
    public int ReciveSignalNum { 
        get{
            if(reciveSignalNum < 0){
                reciveSignalNum = 0;
            }
            return reciveSignalNum;
        } 
        set => reciveSignalNum = value; 
    }
    private PuzzleBrick Puzzle { 
        get{
            return puzzle.GetComponent<PuzzleBrick>();
        }
    }
        

    public event Action OnActived;   //激活信号的事件
    public event Action OnDeactived; //停止信号的事件



    private void Awake()
    {
        col2D = GetComponent<Collider2D>();       
        IsBase = true;
    }

    private void OnDestroy()
    {
       
    }

    public virtual void BaseFuncRun(GameObject puzzle){
        //Debug.Log("基础基底功能运行，碰撞箱关闭");
        col2D.enabled = false;

        if(puzzle == null){
            Debug.LogWarning($"{gameObject.name}基底上没有拼图!,无法激活基底功能");
        }
        else{
            this.puzzle = puzzle;
            this.puzzle.GetComponent<PuzzleBrick>().BaseBrick = this;    //设置拼图的基底为当前基底
        }
        

        switch (BaseType)
        {
            case BaseType.normalBase:
                //Debug.Log("普通基底");
                break;
            case BaseType.FixedBase:
                //Debug.Log("固定基底");
                FixedPuzzleOnIt(Puzzle);
                break;
            case BaseType.RotaBase:
                //Debug.Log("旋转基底");
                RotatePuzzle(ReciveSignalNum);
                break;
            case BaseType.FiexedRotaBase:
                //Debug.Log("固定且旋转基底");
                FixedPuzzleOnIt(Puzzle);
                RotatePuzzle(ReciveSignalNum);
                break;
            default:
                Debug.LogError("没有这个基底类型!"+BaseType);
                break;
        }

        GetComponent<SpriteRenderer>().enabled = false;
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.enabled = false;
        }
    }

    public virtual void BaseFuncStop(){
        //Debug.Log("基础基底功能停止，碰撞箱开启");
        col2D.enabled = true;
        puzzle = null;

        GetComponent<SpriteRenderer>().enabled = true;
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.enabled = true;
        }
    }

    /// <summary>
    /// 固定在这个基底上的拼图
    /// </summary>
    /// <param name="puzzleBrick">这个基底上的拼图</param>
    private void FixedPuzzleOnIt(PuzzleBrick puzzleBrick)
    {
        puzzleBrick.IsFixed = true;
    }

    /// <summary>
    /// 旋转拼图,当接收到信号的时候，按一定间隔旋转拼图
    /// </summary>
    private void RotatePuzzle(int signalNum)
    {
        //Debug.Log("接收到信号，开始旋转拼图");
        if(puzzle != null){
            puzzle.transform.Rotate(Vector3.forward * 90 * signalNum); 
        }     
    }

    /// <summary>
    /// 激活信号并触发一次BaseFuncRun
    /// </summary>
    public void ActivateSignal()
    {
        Debug.Log("激活信号");
        ReciveSignalNum ++;
        if(baseType == BaseType.RotaBase || baseType == BaseType.FiexedRotaBase){
            RotatePuzzle(1);
        }
        OnActived?.Invoke();
    }

    /// <summary>
    /// 停止信号并触发一次BaseFuncRun
    /// </summary>
    public void StopSignal()
    {
        ReciveSignalNum --;
        if(baseType == BaseType.RotaBase || baseType == BaseType.FiexedRotaBase){
            RotatePuzzle(-1);
            //Debug.Log($"{gameObject.name}信号激活，复原拼图旋转");
        }
        //Debug.Log($"{gameObject.name}停止信号，停止基底功能");
        OnDeactived?.Invoke();
    }

    /// <summary>
    /// 初始化时，更新其上下左右三格内至多9个基底
    /// </summary>
    public void UpdateNighborBaseBricks()
    {
       nighborBaseBricks = new BaseBrick[9];
        //左边
        for(int i = X-3;i<=X-1;i++){
            nighborBaseBricks[i] = _GameManager.Instance.Grid.GetBase(i,Y).GetComponent<BaseBrick>();
        }
        //右边
        for (int i = X+1; i <= X+3; i++)
        {
            nighborBaseBricks[i] = _GameManager.Instance.Grid.GetBase(i, Y).GetComponent<BaseBrick>();
        }
        //下边
        for (int i = Y-3; i <= Y-1; i++)
        {
            nighborBaseBricks[i+3] = _GameManager.Instance.Grid.GetBase(X, i).GetComponent<BaseBrick>();
        }
        //上边
        for (int i = Y+1; i <= Y+3; i++)
        {
            nighborBaseBricks[i+6] = _GameManager.Instance.Grid.GetBase(X, i).GetComponent<BaseBrick>();
        }
    }
}
