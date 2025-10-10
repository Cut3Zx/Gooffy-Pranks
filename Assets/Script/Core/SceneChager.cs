using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChager : MonoBehaviour
{

    public void loadScene()//LoadSceneでシーンを切り替える
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
    }


    public void exitGame()//ゲームを終了する
    {
        Application.Quit();

        //Unityエディタ上で動作確認するためのコード
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
