using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoseTools.Utlis;
using UnityEngine.Diagnostics;
using Unity.VisualScripting;


public class PressButton : MonoBehaviour
{
    private Rigidbody2D rb;
    private PuzzleBrick puzzleBrick;
    private Vector3 originPosition;
    private Vector3 canDownDir;

    private void Start()
    {
        puzzleBrick = GetComponentInParent<PuzzleBrick>();
        if(UtlisClass.IsNull(puzzleBrick)){
            Debug.LogError("没有找到父物体下的PuzzleBrick!");
        }
        rb = GetComponent<Rigidbody2D>();
        if(UtlisClass.IsNull(rb)){
            Debug.LogError("没有找到Rigidbody2D!");
        }

        originPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // 获取与之碰撞的物体
        Rigidbody2D otherRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

        if (otherRigidbody != null)
        {
            // 计算受力方向，根据相对速度来做粗略估算
            Vector3 forceDirection = collision.relativeVelocity.normalized;

            // 计算简单的受力大小
            float forceMagnitude = collision.relativeVelocity.magnitude * otherRigidbody.mass; // 假设用动量来表示受力大小

            //Debug.Log("受力方向: " + forceDirection);
            //Debug.Log("受力大小: " + forceMagnitude);

            if (JudgeButtonPressed(forceDirection))
            {
                Debug.Log("按钮触发");
                ActivateNearbyBaseBricks();
            }

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Rigidbody2D otherRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

        if (otherRigidbody != null)
        {
            Debug.Log("按钮释放");
            StopNearbyBaseBricks();
        }
    }

    /// <summary>
    /// 碰撞按钮后，根据碰撞方向，判断按钮有没有被成功按下
    /// </summary>
    private bool JudgeButtonPressed(Vector3 forceDirection)
    {
        //1.判断拼图当前旋转位置
        puzzleBrick.transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
        //2.如果施加的力的方向和按钮父物体方向的夹角在-90度到90度之间，则认为按钮被按下
        float angle = Vector3.Angle(ButtonCanDownDir(rotation.eulerAngles), forceDirection);

        
        if (angle >= 0 && angle < 90)
        {
            Debug.Log("按钮被按下,角度是" + angle);
            return true;
        }
        else
        {
            Debug.Log("按钮未被按下,角度是" + angle);
            return false;
        }      
    }

    private Vector3 ButtonCanDownDir(Vector3 eulerAngles)
    {
        // 计算按钮可以下移的方向
        float zAngle = eulerAngles.z;
        Vector3 canDownDir;

        if (zAngle == 0)
        {
            // Z轴旋转为0，按钮可以下移的方向为Y轴负方向
            canDownDir = new Vector3(0, -1, 0);
        }
        else
        {
            // 将角度转换为弧度
            float radians = zAngle * Mathf.Deg2Rad;
            
            // 计算 X 和 Y 方向的位移分解
            float xOffset = Mathf.Sin(radians);
            float yOffset = Mathf.Cos(radians);
            
            // 更新按钮可以下移的方向
            canDownDir = new Vector3(xOffset, yOffset, 0).normalized;
        }

        this.canDownDir = canDownDir;
        return canDownDir;

    }

   

    /// <summary>
    /// 按钮被按下后，把它XY临近三格内的基底变为激活状态+1
    /// </summary>
    private void ActivateBaseBricks()
    {
        BaseBrick baseBrick = puzzleBrick.BaseBrick;
        if (baseBrick == null)
        {
            Debug.LogError("没有找到相应的基底!");
            return;
        }

        baseBrick.ActivateSignal();
    }

    /// <summary>
    /// 按钮松开的时候，把它XY临近三格内的基底变为未激活状态-1
    /// </summary>
    private void StopBaseBricks()
    {
        BaseBrick baseBrick = puzzleBrick.BaseBrick;
        if (baseBrick == null)
        {
            Debug.LogError("没有找到相应的基底!");
            return;
        }

        baseBrick.StopSignal();
    }

    ///<summary>
    ///获取上下左右至多九个方向的基底并激活
    ///</summary>
    private void ActivateNearbyBaseBricks()
    {
        int x = puzzleBrick.X;
        int y = puzzleBrick.Y;
        
        //左边
        for(int i = x-3; i<=x-1; i++){
            BaseBrick baseBrick = _GameManager.Instance.Grid.GetBaseScript(i, y);
            if(baseBrick!= null){
                baseBrick.ActivateSignal();
            }
        }

        //右边
        for (int i = x + 1; i <= x + 3; i++)
        {
            BaseBrick baseBrick = _GameManager.Instance.Grid.GetBaseScript(i, y);
            if (baseBrick!= null)
            {
                baseBrick.ActivateSignal();
            }
        }

        //下边
        for (int i = y - 3; i <= y - 1; i++)
        {
            BaseBrick baseBrick = _GameManager.Instance.Grid.GetBaseScript(x, i);
            if (baseBrick!= null)
            {
                baseBrick.ActivateSignal();
            }
        }
        //上边
        for (int i = y + 1; i <= y + 3; i++)
        {
            BaseBrick baseBrick = _GameManager.Instance.Grid.GetBaseScript(x, i);
            if (baseBrick!= null)
            {
                baseBrick.ActivateSignal();
            }
        }

    }

    /// <summary>
    /// 获取上下左右至多九个方向的基底并停止激活
    /// </summary>
    private void StopNearbyBaseBricks()
    {
        int x = puzzleBrick.X;
        int y = puzzleBrick.Y;

        //左边
        for (int i = x - 3; i <= x - 1; i++)
        {
            BaseBrick baseBrick = _GameManager.Instance.Grid.GetBaseScript(i, y);
            if (baseBrick != null)
            {
                baseBrick.StopSignal();
            }
        }

        //右边
        for (int i = x + 1; i <= x + 3; i++)
        {
            BaseBrick baseBrick = _GameManager.Instance.Grid.GetBaseScript(i, y);
            if (baseBrick != null)
            {
                baseBrick.StopSignal();
            }
        }

        //下边
        for (int i = y - 3; i <= y - 1; i++)
        {
            BaseBrick baseBrick = _GameManager.Instance.Grid.GetBaseScript(x, i);
            if (baseBrick != null)
            {
                baseBrick.StopSignal();
            }
        }
        //上边
        for (int i = y + 1; i <= y + 3; i++)
        {
            BaseBrick baseBrick = _GameManager.Instance.Grid.GetBaseScript(x, i);
            if (baseBrick != null)
            {
                baseBrick.StopSignal();
            }
        }
        
    }


}
