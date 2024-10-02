using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPhyisc : MonoBehaviour
{
    [Header("用来在拖拽砖块的时候停止物理效果，防止按钮误触等事情")]

    [SerializeField]private Rigidbody2D rb;
    [SerializeField]private Collider2D col2D;


    private void OnEnable()
    {
        _GameManager.Instance.OnPuzzleDrug += MakeRigidbodyStop;
        _GameManager.Instance.OnPuzzleSetDown += MakeRigidbodyRun;
    }

    private void OnDisable()
    {
        //Debug.LogError($"StopPhyisc OnDisable{_GameManager.Instance == null}");
        if(_GameManager.Instance != null){
            _GameManager.Instance.OnPuzzleDrug -= MakeRigidbodyStop;
            _GameManager.Instance.OnPuzzleSetDown -= MakeRigidbodyRun;
        }
    }
    
    private void MakeRigidbodyStop()
    {
        if(rb != null){
            rb.bodyType = RigidbodyType2D.Kinematic;
            col2D.isTrigger = true;
        }
        
    }

    private void MakeRigidbodyRun()
    {
        if(rb != null){
            rb.bodyType = RigidbodyType2D.Dynamic;
            col2D.isTrigger = false;
        }
    }
}
