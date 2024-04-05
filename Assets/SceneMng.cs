using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMng : MonoBehaviour
{

    public static SceneMng instance = null;
    
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
            return;
        }

        Destroy(gameObject);
    }

    public void quit() {
        Application.Quit();
    }
    
    public void goToTitle() {}

    public void startGame() {}

    public void loadE1_0() {
        SceneManager.LoadScene("E1_0");
    }

    public void loadHouse_Livingroom() {
        SceneManager.LoadScene("House_Livingroom");
    }
}
