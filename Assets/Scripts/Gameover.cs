using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour {
    [Header("Go to the scene")]
    public string goToLevel1;
    public string goToLevel2;
    public Text scoreText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape))
            Quit();
        scoreText.text = "Score: " + Shooter.Instance.GetScoreText().ToString();
	}

    public void Level1(){
        SceneManager.LoadScene(goToLevel1);
        Time.timeScale = 1f;
    }

    public void Level2(){
        SceneManager.LoadScene(goToLevel2);
        Time.timeScale = 1f;
    }

    public void Quit(){
        Application.Quit();
    }
}
