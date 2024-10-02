using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// 最基础的拼图砖块类，其他要扩展的功能都可以继承这个类
/// </summary>
public class PuzzleBrick : Brick
{
    [Header("砖块的类型")]
    [SerializeField] private PuzzleType puzzleType ;
    [SerializeField] protected BaseBrick baseBrick;  //这个拼图块所安装在的基底块

    public BaseBrick BaseBrick { 
        get { 
            if (baseBrick == null) {
                Debug.LogError($"{gameObject.name}所处的基底块为空！");
                return null;
            }
            return baseBrick; 
            }
        set { baseBrick = value; }
    }

    public PuzzleType PuzzleType { get { return puzzleType; } }
    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        
    }


   


}
