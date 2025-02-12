using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GhostFX : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;

    public float newGhostDelay;     // delay para criar novo ghost
    private float newGhostTime;
    public GameObject ghost;

    // Start is called before the first frame update
    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        newGhostTime = newGhostDelay;
    }

    public void ghostEffect(){

        if(newGhostTime>0f){
            newGhostTime -= Time.deltaTime;     // subtrai tempo da variavel
        }else{
            #region cria ghost
                GameObject currentGhost = Instantiate(ghost, transform.position, quaternion.identity);

                currentGhost.transform.localScale = transform.parent.localScale;    // pega localScale do player

                Sprite currentSprite = spriteRenderer.sprite;                       // pega o sprite atual do player
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite; // add sprite do player no spriteRenderer do ghost

                Destroy(currentGhost, 0.5f);
            #endregion

            newGhostTime = newGhostDelay;
        }

        
    }
}
