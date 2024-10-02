using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBase : BaseBrick
{
    public override void BaseFuncRun(GameObject puzzle)
    {
        base.BaseFuncRun(puzzle);
        //Debug.Log($"{puzzle.name}在其上，有需要实现的逻辑可以对他进行操作");
    }

    public override void BaseFuncStop()
    {
        base.BaseFuncStop();
    }
}
