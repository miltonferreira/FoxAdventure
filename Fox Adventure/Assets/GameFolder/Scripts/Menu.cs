using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    public void startGame(){
        SceneManager.LoadScene(GameManager.instance.lastScene);
    }

    public void exitGame(){
        Application.Quit();
    }

    public void soundFX(){
        audioSource.Play();
    }
}
