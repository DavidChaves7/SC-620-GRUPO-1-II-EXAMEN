using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    public static LevelManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void FirstLevel()
    {
        int level = 0;
        LoadLevel(level);
    }

    public void NextLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex + 1;
        if (level > SceneManager.sceneCountInBuildSettings - 1)
        {
            level = SceneManager.sceneCountInBuildSettings - 1;
        }
        LoadLevel(level);
    }

    public void YouWin()
    {

    }

    public void LastLevel()
    {
        int level = SceneManager.sceneCountInBuildSettings - 1;
        LoadLevel(level);
    }
    private void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
