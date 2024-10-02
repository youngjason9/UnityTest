using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 用来编辑关卡用的类
/// </summary>
[System.Serializable]
public class LevelEditor
{
    public int levelIndex;
    public int steps;
    private float cellSize = 1f;
    private int x,y;   //鼠标当前所处的格子坐标
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Vector3 originPos;


    [Header("如果要清除格子内的砖块，请勾选此项，然后把鼠标移动到相应的格子上，点击鼠标左键即可")]
    [SerializeField] private bool clearMode = false;   //是否是清除模式
    private Grid grid;   //格子网格
    private GameObject brick;   //砖块物体

    //应该有的的功能
    //1.绘制出相应的格子网格
    //2.点击获取相应的砖块物体
    //3.可以拖拽把砖块移动到相应的格子当中

    /// <summary>
    /// 初始化或者更新网格
    /// </summary>
    public void SpawnTheGrid()
    {
        if(grid == null){
            grid = new Grid(width, height, cellSize, originPos);
            _GameManager.Instance.Grid = grid;
            Debug.Log("已转载新的网格实例");
        }
        else{
            grid.UpdateGrid(width, height, cellSize, originPos);
            _GameManager.Instance.Grid = grid;
            Debug.Log("已更新网格实例");
        }
    }

    ///<summary>
    /// 点击获取相应的砖块物体
    ///</summary>
    public void GetTheBrick(GameObject brick)
    {
        if(this.brick == null){
            this.brick = brick;
        }
        else{
            GameObject.Destroy(this.brick);
            this.brick = brick;
        }
    }

    ///<summary>
    /// 拖拽把砖块移动到相应的格子当中
    ///</summary>
    public void DrugTheBrick()
    {
        if(brick != null){
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mousePos3D = new Vector3(mousePos.x, mousePos.y, -1f);
            brick.transform.position = mousePos3D;     


            //按Q和E可以旋转砖块
            if(Input.GetKeyDown(KeyCode.Q)){
                brick.transform.Rotate(0f, 0f, 90f);
            }     
            else if(Input.GetKeyDown(KeyCode.E)){
                brick.transform.Rotate(0f, 0f, -90f);
            }
        }
    }

    /// <summary>
    /// 把砖块放入相应的格子当中
    /// </summary>
    public void PlaceTheBrick()
    {                
        //1.获取网格XY坐标
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        grid.GetXY(mousePos3D, out x, out y); 
        if(Input.GetMouseButtonDown(0)){
            if(brick != null){ 
                //2.判断当前砖块类型
                BaseBrick baseBrick;
                PuzzleBrick puzzleBrick;
                bool isBase;
                if(brick.TryGetComponent<BaseBrick>(out baseBrick)){
                    Debug.Log("当前砖块类型为：基底砖块" );  
                    isBase = true;

                }
                else if(brick.TryGetComponent<PuzzleBrick>(out puzzleBrick)){
                    Debug.Log("当前砖块类型为：拼图砖块" );
                    isBase = false;
                }
                else{
                    Debug.LogWarning("这个砖块上没有挂载基底或者拼图组件！" );   
                    return;            
                }

                //3.根据XY坐标把砖块放入相应的格子当中
                if(isBase && grid.SetBaseTile(x, y, brick)){
                    this.brick = null;
                }
                else if(!isBase && grid.SetPuzzleTile(x, y, brick)){
                    this.brick = null;
                }
                
            }
            else if(clearMode){
                //如果鼠标什么都没有拿，默认是点击到相应格子内，则清空格子内的物体
                Debug.Log("清空格子内的物体(当鼠标上什么都没拿的时候，默认是点击到相应格子内，则清空格子内的物体)");
                grid.ClearCell(x, y);
            }
        }
        
    }

    public void SetAllNormalBase(GameObject baseBrick)
    {
        for(int i = 0; i < grid.Width; i++){
            for(int j = 0; j < grid.Height; j++){
                grid.SetBaseTile(i, j, GameObject.Instantiate(baseBrick));
            }
        }
    }
    
}
