using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LoadScene : MonoBehaviour
{

    public string nameScene;
    public int indexScene;

    [Header("Ultima cena do game?")]
    public bool isFinalScene;
    
    public void nextScene(string name){
        // mostra panel de aviso de coletar as esmeraldas
        GameManager.instance.isShowPanelGems = true;
        SceneManager.LoadScene(name);
    }

    public void nextScene(int value){
        // mostra panel de aviso de coletar as esmeraldas
        GameManager.instance.isShowPanelGems = true;
        SceneManager.LoadScene(value);
    }

    private void lastSceneInGame(){
        if(isFinalScene){
            GameManager.instance.lastScene = "Scene_1";
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == 6 && !String.IsNullOrEmpty(nameScene)){
            lastSceneInGame();
            nextScene(nameScene);
        }else if(other.gameObject.layer == 6){
            lastSceneInGame();
            nextScene(indexScene);
        }
    }
}
