using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public bool isShowPanelGems;

    [HideInInspector]public string lastScene;
    
    // Start is called before the first frame update
    void Awake(){
        
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        isShowPanelGems = true;

    }

    void Start() {
        lastScene = "Scene_1";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
