using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAndPuzzle
{
    [Header("放入你做好了的基底砖块")]
    [SerializeField] private BaseBrick[] baseBricks;
    [Header("放入你做好的拼图砖块")]
    [SerializeField] private PuzzleBrick[] puzzleBricks;

    public BaseBrick[] BaseBricks { get => baseBricks;}

    public void PrintInfo()
    {
        foreach (var baseBrick in baseBricks)
        {
            Debug.Log(baseBrick.name);
        }
        foreach (var puzzleBrick in puzzleBricks)
        {
            Debug.Log(puzzleBrick.name);
        }
        Debug.Log("BaseAndPuzzle Info Printed");
    }

    public BaseBrick GetBase(BaseType baseType)
    {
        foreach(var baseBrick in baseBricks)
        {
            if(baseType == BaseType.None){
                Debug.LogError("这里没有相应的基底！");
                return null;
            }

            if (baseType == baseBrick.BaseType)
            {
                return baseBrick;
            }
        }

        Debug.LogError("初始化失败，没有找到对应的基底" + baseType.ToString());
        return null;
    }

    public PuzzleBrick GetPuzzle(PuzzleType puzzleType)
    {
        foreach (var puzzleBrick in puzzleBricks)
        {
            if(puzzleType == PuzzleType.None){
                Debug.LogWarning("这里没有相应的拼图！请检查是确实没有还是确实没有放在这里");
                return null;
            }

            if (puzzleType == puzzleBrick.PuzzleType)
            {
                return puzzleBrick;
            }
        }

        Debug.LogError("初始化失败，没有找到对应的拼图" + puzzleType.ToString());
        return null;
    }

    public GameObject GetBaseObject(BaseType baseType)
    {
        return GetBase(baseType).gameObject;
    }

    public GameObject GetPuzzleObject(PuzzleType puzzleType)
    {
        return GetPuzzle(puzzleType).gameObject;
    }
}
