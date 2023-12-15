using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitApp()
    {
        Application.Quit();
    }


}
