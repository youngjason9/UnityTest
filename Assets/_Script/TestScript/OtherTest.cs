using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherTest : MonoBehaviour
{
        [Header("存放基底和拼图的仓库")]
    [SerializeField] private BaseAndPuzzle baseAndPuzzle;

    [Header("存放游戏数据,Json格式")]
    [SerializeField] private TextAsset levelData;

    private void Start()
    {
        baseAndPuzzle.PrintInfo();
    }
}
