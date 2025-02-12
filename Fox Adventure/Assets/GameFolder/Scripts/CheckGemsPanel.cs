using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGemsPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance.isShowPanelGems){
            // nao mostra panel de aviso de coletar as esmeraldas
            GameManager.instance.isShowPanelGems = false;
        }else{
            gameObject.SetActive(false);    // desativa o panel
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
