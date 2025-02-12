using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public Transform a,b;
    private bool goRight;
    [Header("Velocidade Movimento")]
    public float speedMove = 5f;

    // Update is called once per frame
    void Update()
    {
        followPoints();
    }

    private void followPoints(){
        if(goRight){
            if(Vector2.Distance(transform.position, b.position) < 0.1f){
                goRight = false; // inverte para a esquerda
            }
            transform.position = Vector2.MoveTowards(transform.position, b.position, speedMove * Time.deltaTime);
        }else{
            if(Vector2.Distance(transform.position, a.position) < 0.1f){
                goRight = true; // inverte para a direita
            }
            transform.position = Vector2.MoveTowards(transform.position, a.position, speedMove * Time.deltaTime);
        }
    }
}
