using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{

    public GameObject PausePanel;
    private bool isPause;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.Return) && !isPause){
            isPause = true;
            EventSystem.current.SetSelectedGameObject(null);
            PausePanel.SetActive(true);
            Time.timeScale = 0f;
        }else if(Input.GetKeyDown(KeyCode.Return) && isPause){
            Resume();
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }

    public void Resume(){
        isPause = false;
        EventSystem.current.SetSelectedGameObject(null);
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Menu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
