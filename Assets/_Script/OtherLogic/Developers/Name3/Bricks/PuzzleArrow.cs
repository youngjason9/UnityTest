using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleArrow : PuzzleBrick,ITimeStopAndContinue
{
    [SerializeField] private GameObject arrowPref;
    [SerializeField] private Transform shootPos;
    [SerializeField] private float shootSpeed;
    [SerializeField] private float shootInterval = 1f;
    private Vector2 shootDirection;
    public Vector2 ShootDirection
    {
        get
        {
            shootDirection = UpdateShootDir();
            return shootDirection;
        }
    }

    private Coroutine KeepShootingCoroutine;
    private bool canShoot = true;

    private void Start()
    {
        _GameManager.Instance.OnPuzzleDrug += TimeStop;
        _GameManager.Instance.OnPuzzleSetDown += TimeContinue;
    }

    private void OnDisable()
    {
        if(_GameManager.Instance != null){
            _GameManager.Instance.OnPuzzleDrug -= TimeStop;
            _GameManager.Instance.OnPuzzleSetDown -= TimeContinue;
        }
    }

    private void Update()
    {
        if(BaseBrick != null && BaseBrick.ReciveSignalNum <= 0){
            if(KeepShootingCoroutine != null){
                StopCoroutine(KeepShootingCoroutine);
                KeepShootingCoroutine = null;
                Debug.Log("停止射箭");
            }           
        }
        else if(BaseBrick == null){
            Debug.LogError($"{name}还没有放在基底上！");
        }
        else{
            if(KeepShootingCoroutine == null){
                KeepShootingCoroutine = StartCoroutine(ShootArrowCoroutine());
                Debug.Log("开始射箭");
            }
        }

    }

    /// <summary>
    /// 射出一个物体
    /// </summary>
    /// <param name="arrow"></param>
    private void Shoot(Arrow arrow)
    {
        if (arrow == null){
            Debug.Log("未设置射出的箭头！");
            return;
        }

        if(shootPos == null){
            Debug.Log("未设置射出的位置！");
            return;
        }
        arrow.InitArrow(ShootDirection, shootSpeed,shootPos.position,this);
        Debug.Log("真的射出箭了");
    }

    /// <summary>
    /// 更新射出的方向
    /// </summary>
    /// <returns></returns>
    private Vector2 UpdateShootDir()
    {
        transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
        float shootAngle = rotation.eulerAngles.z + 90;
        
        Vector2 dir = new Vector2(Mathf.Cos(shootAngle * Mathf.Deg2Rad), Mathf.Sin(shootAngle * Mathf.Deg2Rad));
        return dir;
    }

    /// <summary>
    /// 射出一根箭
    /// </summary>
    public void ShootArrow()
    {   
        if(canShoot){
            Arrow newArrow = Instantiate(arrowPref).GetComponent<Arrow>();
            Shoot(newArrow);
            //Debug.Log("射出箭" + newArrow.gameObject.name);
        }
        else{
            Debug.Log("时停了，不能射箭");
        }
    }

    private IEnumerator ShootArrowCoroutine()
    {
        while (true)
        {
            ShootArrow();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    public void TimeStop()
    {
        canShoot = false;
    }

    public void TimeContinue()
    {
        canShoot = true;
    }

}
