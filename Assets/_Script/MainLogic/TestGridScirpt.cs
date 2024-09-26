using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class TestGridScirpt : MonoBehaviour
{
    [SerializeField] int width = 7;
    [SerializeField] int height = 3;
    [SerializeField] float tileSize = 10f;
    private Vector3 originPosition = new Vector3(-30, 0, 0);
    [SerializeField] private GameObject fullBrick;
    [SerializeField] private GameObject puzzelBrick;
    [SerializeField] private GameObject airBirck;
    [SerializeField] private PlayerController player;

    [SerializeField] private GameObject[] flags;

    private GameObject curCatchBrick;


    private Grid grid;
    private int x = 0;
    private int y = 0;

    [SerializeField]private int[] catchIndex = new int[2];


    private void Start()
    {
        grid = new Grid(width, height, tileSize,originPosition);
        for(int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if(y > 0 && y < height-1)
                    grid.SetTile(x, y, Instantiate(airBirck));
                else
                    grid.SetTile(x, y, Instantiate(fullBrick));
            }
        }
        grid.SetTile(3, 2, puzzelBrick);
        grid.SetFlag(1, 1, flags[0]);
        grid.SetFlag(5, 1, flags[1]);

        grid.SetTile(0, 1, Instantiate(fullBrick));
        grid.SetTile(6, 1, Instantiate(fullBrick));
        grid.SetTile(4, 1, Instantiate(fullBrick));

        player.SetPlayerPosition(grid.GetGridCenter(1, 1));
    }

    private void Update()
    {
        if (curCatchBrick == null && Input.GetMouseButtonDown(0) && _GameManager.Instance.CanMovePuzzle()) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.GetXY(mousePos, out x, out y);
            if(x != -1 && y != -1) {
                catchIndex[0] = x;
                catchIndex[1] = y;
            }
            curCatchBrick = grid.CatchTile(x, y);
            if (curCatchBrick != null) {
                BoxCollider2D collider = curCatchBrick.GetComponent<BoxCollider2D>();
                if (collider != null) {
                    collider.enabled = false;
                }
                _GameManager.Instance.OnPuzzleDrug?.Invoke();
            }
        }

        if (curCatchBrick != null && Input.GetMouseButton(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            curCatchBrick.transform.position = new Vector3(mousePos.x, mousePos.y, -1);
        }
        else if (curCatchBrick != null && Input.GetMouseButtonUp(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.GetXY(mousePos, out x, out y);
            grid.DropTile(catchIndex[0], catchIndex[1], x, y, curCatchBrick);
            BoxCollider2D collider = curCatchBrick.GetComponent<BoxCollider2D>();
            if (collider != null) {
                collider.enabled = true;
            }
            curCatchBrick = null;

            _GameManager.Instance.OnPuzzleSetDown?.Invoke();
        }
    }

    //拖拽拼图块是一个事件，场景中的人物，机关等活动对象都需要监听这个事件，并绑定相应的处理函数。
}
*/