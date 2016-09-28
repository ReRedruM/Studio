using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    private GameObject _menuRoot;
    private bool _inMenu = false;

	// Use this for initialization
	void Start ()
	{
	    _menuRoot = transform.GetChild(0).gameObject;
        _menuRoot.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	    if (!_inMenu && Input.GetKeyDown(KeyCode.Escape))
	    {
            inMenu(true);
	    }
    }

    void inMenu(bool value)
    {
        if (value == true)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
        _inMenu = value;
        _menuRoot.SetActive(value);
    }

    public void Continue()
    {
        inMenu(false);
        print("CONTINUE!!!!");
    }

    public void Options()
    {
        //TODO options
        print("Nuthing hre");
    }

    public void Exit()
    {
        inMenu(false);
        SceneManager.LoadScene(0);
    }
}
