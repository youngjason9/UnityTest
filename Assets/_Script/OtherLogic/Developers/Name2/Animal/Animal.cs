using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Animal : MonoBehaviour
{

    private bool isPlayer;
    private bool isAlive = true;
    public bool IsAlive { get { return isAlive; } }

    protected Rigidbody2D rb;

    public Rigidbody2D Rb{ get { return rb; }set { rb = value; } }

    public void Died()
    {
        isAlive = false;
        
        if (isPlayer)
        {
            Debug.Log("You killed the Player! And resart the game");
            //TODO：执行玩家死亡动画，重启游戏
        }
        else{
            Debug.Log("You killed an Animal!");
            //TODO：执行被杀死动物的死亡动画
        }
    }
}
