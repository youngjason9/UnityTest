using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFixed : BaseBrick
{
    public override void BaseFuncRun(GameObject puzzle)
    {
        base.BaseFuncRun(puzzle);
        //Debug.Log($"{puzzle.name}在其上，有需要实现的逻辑可以对他进行操作");
        puzzle.GetComponent<PuzzleBrick>().IsFixed = true;
        Debug.Log($"这是固定基底，其上的{puzzle.name}不能再被抓取移动");
    }

    public override void BaseFuncStop()
    {
        base.BaseFuncStop();
    }
}
