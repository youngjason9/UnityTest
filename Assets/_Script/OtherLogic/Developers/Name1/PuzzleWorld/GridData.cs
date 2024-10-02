using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridData 
{
    public int width;
    public int height;
    public int[] gridArray;
    public float cellSize;
    public Vector3 originPosition;
    public Vector3[] gridCenter;
    public PuzzleType[] puzzles;   //拼图
    public float[] puzzleZAngle;   //拼图的角度
    public BaseType[] bases;    //基底    
    public float[] baseZAngle;    //基底的角度

    public void PrintInfo()
    {
        Debug.Log("GridData Info: width=" + width + " height=" + height + " cellSize=" + cellSize + " originPosition=" + originPosition);
        //Debug.Log("gridArray.GetLength(0)=" + gridArray.GetLength(0) + " gridArray.GetLength(1)=" + gridArray.GetLength(1));
        //Debug.Log("gridCenter.GetLength(0)=" + gridCenter.GetLength(0) + " gridCenter.GetLength(1)=" + gridCenter.GetLength(1));
        //Debug.Log("puzzles[0, 0] = " +puzzles[0, 0] + " bases[0, 0] = " + bases[0, 0]);
        foreach (var puzzle in puzzles)
        {
            Debug.Log("puzzle = " + puzzle);
        }
        foreach (var baseType in bases)
        {
            Debug.Log("baseType = " + baseType);
        }
    }
}
