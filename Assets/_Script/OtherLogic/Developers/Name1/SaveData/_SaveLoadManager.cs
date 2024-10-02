using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using RoseTools.Utlis;

public class _SaveLoadManager : SingletonMono<_SaveLoadManager>
{
    [Header("你要保存的文件名")]
    [SerializeField] private string fileName;

    private string filePath;

    private void Start()
    {
       Debug.Log("SaveLoadManager Start");
    }

    /// <summary>
    /// 保存网格数据
    /// </summary>
    /// <param name="grid">需要保存的网格实例对象</param>
    public void Save(Grid grid)
    {
        fileName += ".json";
        filePath = Path.Combine(Application.dataPath, "Data/LevelData/", fileName);
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        //TODO：将游戏数据保存到本地，包括网格数据
        GridData gridData= SaveGridData(grid);
        string json = JsonUtility.ToJson(gridData);

        //TODO：将json数据保存到本地文件
        File.WriteAllText(filePath, json);
        Debug.Log("成功保存网格数据到" + filePath);
    }

    /// <summary>
    /// 读取网格数据
    /// </summary>
    /// <param name="levelData">传入保存了网格数据的json文件</param>
    /// <returns>返回初始化好的网格数据</returns>
    public GridData Load(TextAsset levelData)
    {
        Debug.Log("读取游戏数据");
        //TODO：解析json数据并初始化游戏数据
        if(levelData!= null)
        {
            string json = levelData.text;
            GridData gridData = JsonUtility.FromJson<GridData>(json);
            Debug.Log("成功读取网格数据,form:" + levelData.name);
            return gridData;
        }
        else{
            Debug.LogError("File not found at " + levelData);
            return null;
        }
    }

    /// <summary>
    /// 保存网格数据，将其中二维数组转化为一维数组进行保存
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    private GridData SaveGridData(Grid grid)
    {
        if(grid ==null){
            Debug.LogError("当前场景中已不存在Grid实例！");
            return null;
        }

        GridData gridData = new GridData();
        gridData.width = grid.Width;
        gridData.height = grid.Height;
        gridData.cellSize = grid.CellSize;
        gridData.originPosition = grid.OriginPosition;

        int[] gridArray = UtlisClass.TwoDimToOneDim(grid.GridArray);
        Vector3[] gridCenter = UtlisClass.TwoDimToOneDim(grid.GridCenter);

        PuzzleType[] puzzleTypeArray = new PuzzleType[grid.Width * grid.Height];
        // for(int i = 0; i<grid.Width; i++){
        //     for(int j = 0;j<grid.Height;j++){
        //         //Debug.Log("i = " + i + " j = " + j);
        //         if(grid.GetPuzzle(i,j) == null)
        //             continue;
        //         puzzleTypeArray[i*grid.Height+j] = grid.GetPuzzleScript(i,j).PuzzleType;

        //     }
        // }
        BaseType[] baseTypeArray = new BaseType[grid.Width * grid.Height];
        // for(int i = 0; i<grid.Width; i++){
        //     for(int j = 0;j<grid.Height;j++){
        //         if(grid.GetBase(i,j) == null)
        //             continue;
        //         baseTypeArray[i*grid.Height+j] = grid.GetBaseScript(i,j).BaseType;
        //     }
        // }
        float[] puzzleZAngle = new float[grid.Width * grid.Height];
        float[] baseZAngle = new float[grid.Width * grid.Height];
        for(int i = 0; i<grid.Width; i++){
            for(int j = 0;j<grid.Height;j++){
                if(grid.GetPuzzle(i,j) != null){
                    puzzleZAngle[i*grid.Height+j] = grid.GetPuzzle(i,j).transform.localEulerAngles.z;
                    puzzleTypeArray[i*grid.Height+j] = grid.GetPuzzleScript(i,j).PuzzleType;
                }
                if(grid.GetBase(i,j) != null){
                    baseZAngle[i*grid.Height+j] = grid.GetBase(i,j).transform.localEulerAngles.z;
                    int index = i*grid.Height+j;
                    Debug.Log("baseZAngle[" + index + "] = " + baseZAngle[i*grid.Height+j]);
                    baseTypeArray[i*grid.Height+j] = grid.GetBaseScript(i,j).BaseType;
                }
            }
        }

        gridData.gridArray = gridArray;
        gridData.gridCenter = gridCenter;
        gridData.puzzles = puzzleTypeArray;
        gridData.bases = baseTypeArray;
        gridData.puzzleZAngle = puzzleZAngle;
        gridData.baseZAngle = baseZAngle;


        Debug.Log("创建实例并赋值完成");
        return gridData;
    }
}
