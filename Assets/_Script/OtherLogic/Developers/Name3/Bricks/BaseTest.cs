using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBase : BaseBrick
{
    public override void BaseFuncRun(GameObject puzzle)
    {
        Debug.Log("TestBase is running");
    }

    public override void BaseFuncStop()
    {
        Debug.Log("TestBase is stopping");
    }
}
