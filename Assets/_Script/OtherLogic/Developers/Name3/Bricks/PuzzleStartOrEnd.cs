using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PuzzleStartOrEnd : PuzzleBrick
{
    [SerializeField] private GameObject player;     //场上只会有一个可操控玩家

    private bool hasPlayerInScene;

    private GameObject Player{
        get{
            GameObject thisPlayer = GameObject.FindGameObjectWithTag("Player");
           if(thisPlayer == null){
               hasPlayerInScene = false;
               return player;
           }
           else{
               hasPlayerInScene = true;
               return thisPlayer;
           }
        }
    }

    private void Start()
    {
        // if(PuzzleType == PuzzleType.startPuzzle){
        //     SpawnPlayer();
        // }
    }

    // private void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.L)){
    //         SpawnPlayer();
    //     }
    // }

    /// <summary>
    /// 当场上不存在玩家的时候，生成一个玩家
    /// </summary>
    public void SpawnPlayer()
    {
        if(!hasPlayerInScene){
            Instantiate(Player, transform.position, Quaternion.identity);
            hasPlayerInScene = true;
        }
        else{
            Player.transform.position = transform.position;
        }
    }

    /// <summary>
    /// 触碰到这个砖块时，获得胜利
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(PuzzleType != PuzzleType.EndPuzzle)
        {
            return;
        }

        if(other.CompareTag("Player")){
            Debug.Log("胜利！");
            //_GameManager.Instance.GameOver();
        }
    }
}
