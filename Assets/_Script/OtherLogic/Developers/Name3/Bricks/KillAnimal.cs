using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAnimal : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        Animal animal = other.gameObject.GetComponent<Animal>();
        if (animal!= null && animal.IsAlive)
        {
            Debug.Log($"{animal.name} has been killed by {gameObject.name}");
            animal.Died();
        }       
    }
}
