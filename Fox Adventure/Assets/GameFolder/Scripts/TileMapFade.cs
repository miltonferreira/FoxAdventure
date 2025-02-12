using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileMapFade : MonoBehaviour
{

    private Animator anim;

    // Start is called before the first frame update
    void Start(){
        anim = transform.parent.GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.transform.tag == "Player"){
            anim.Play("FadeOut");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.transform.tag == "Player"){
            anim.Play("FadeIn");
        }
    }

    
}
