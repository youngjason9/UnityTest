using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    [SerializeField]private Rigidbody2D rb;
    private Vector2 velocity;
    public PuzzleArrow shootFrom;

    private void Start()
    {
        _GameManager.Instance.OnPuzzleDrug += TimeStop;
        _GameManager.Instance.OnPuzzleSetDown += TimeContinue;
    }

    private void OnDisable()
    {
        if(_GameManager.Instance!= null){
            _GameManager.Instance.OnPuzzleDrug -= TimeStop;
            _GameManager.Instance.OnPuzzleSetDown -= TimeContinue;
        }
    }

    public void InitArrow(Vector2 direction, float speed,Vector2 position,PuzzleArrow shootFrom)
    {
        transform.parent = null;
        transform.position = position;
        //根据射击的方向，确定箭的朝向
        float zAngle = Vector2.SignedAngle(Vector2.right, direction)-90;
        Debug.Log("箭的角度为"+zAngle);
        transform.Rotate(Vector3.forward * zAngle);
        
        
        this.shootFrom = shootFrom;
        
        velocity = rb.velocity = direction * speed;
       
    }

    private void KillAnimal(Animal animal)
    {
        if (animal == null){
            Debug.LogError("传入的Animal为空");
            return;
        }

        animal.Died();
        Destroy(gameObject);
    }

    private void TimeStop()
    {
        rb.velocity = Vector2.zero;
    }

    private void TimeContinue()
    {
        rb.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Animal animal = collision.gameObject.GetComponent<Animal>();
        if (animal != null)
        {
            KillAnimal(animal);
            Destroy(gameObject);
            return;
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Button")){
            Debug.Log("箭击中了按钮");
            Destroy(gameObject,3f);
            return;
        }

        if(collision.gameObject != gameObject || collision.gameObject != shootFrom.gameObject){
            Destroy(gameObject);
        }
        else{
            Debug.LogWarning("箭自己碰到了自己，但是不用销毁");
        }
    }


}
