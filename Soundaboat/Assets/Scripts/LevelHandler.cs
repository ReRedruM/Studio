using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour {

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
