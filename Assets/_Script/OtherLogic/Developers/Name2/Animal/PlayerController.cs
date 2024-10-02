using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Animal,ITimeStopAndContinue
{
    [SerializeField] private bool canMove = true;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float gravityScale = 1f;

    private Vector2 velocity;

    Vector3 originScale;

    private void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        originScale = transform.localScale;
       
    }

    private void OnEnable() 
    {
        _GameManager.Instance.OnPuzzleDrug += TimeStop;
        _GameManager.Instance.OnPuzzleSetDown += TimeContinue;
    }

    private void Update()
    {
        PlayerMove();
        PlayerJump();
    }

    private void OnDisable()
    {
        if(_GameManager.Instance!= null){
            _GameManager.Instance.OnPuzzleDrug -= TimeStop;
            _GameManager.Instance.OnPuzzleSetDown -= TimeContinue;
        }
    }

    private void PlayerMove()
    {
        if (canMove) {
            float inputX = Input.GetAxisRaw("Horizontal");
            Vector2 moveDir  = new Vector2(inputX, 0f).normalized;
            velocity = new Vector2(moveDir.x * speed, rb.velocity.y);

            rb.velocity = velocity;
            if (rb.velocity.x > 0) {
                transform.localScale = new Vector3(originScale.x, originScale.y, originScale.z);
            }
            else if (rb.velocity.x < 0) {
                transform.localScale = new Vector3(-originScale.x, originScale.y, originScale.z);
            }
        }
        else {
            //执行一些暂停小人动作的逻辑
        }
    }

    private void PlayerJump()
    {
        if (canMove) {
            if (Input.GetButtonDown("Jump")) {
                rb.gravityScale = gravityScale;
                float jumpForce = Mathf.Sqrt(jumpHeight * (Physics2D.gravity.y * rb.gravityScale) * -2f) * rb.mass;
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EndFlag")) {
            Debug.Log("You Win!");
        }
        else if (collision.CompareTag("Area_DeadZone")) {
            _GameManager.Instance.GameOver();
        }
    }

    public void SetPlayerPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void TimeStop()
    {
        canMove = false;
        velocity = rb.velocity;     //保存时间停止前的速度
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
    }

    public void TimeContinue()
    {
        rb.velocity = velocity;
        rb.gravityScale = gravityScale;
        canMove = true;
    }

    


    //还有两个待解决的问题：
    //1. 当小人跳跃起来贴到另一个墙壁的时候，应该是要顺着墙壁滑落下来，而目前是停滞在空中(KO)
    //2. 小人所处的方格位置处的砖块应该是不能被鼠标选中移动的，目前是可以被选中的(KO)
}
