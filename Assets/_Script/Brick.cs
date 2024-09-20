using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public bool canCatch;
    public bool canExChange;
    public bool hasFlag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("T_playerBody") && !hasFlag) {
            canCatch = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("T_playerBody") && !hasFlag) {
            canCatch = true;
        }
    }
}
