using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject continueButton;
    public GameObject loadingScreen;
    public GameObject loadingProgressBar;
    void Start()
    {
        if (SaveManager.isAnySaves())
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnContinueGameButtonClick()
    {
        
        SaveManager.LoadGame();
        Load();
    }

    public void OnStartNewGameButtonCkick()
    {
        SaveManager.DeleteSaves();
        SaveManager.LoadGame();
        Load();
        
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
        Debug.Log("Exit pressed!");
    }

    private void Load()
    {
        //SceneManager.LoadScene("Game");
        loadingScreen.SetActive(true);
        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");
        //asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            loadingProgressBar.GetComponent<Slider>().value = asyncLoad.progress;
            /*if(asyncLoad.progress > 0.9f && !asyncLoad.allowSceneActivation)
            {
                if (Input.anyKeyDown)
                {
                    asyncLoad.allowSceneActivation = true;
                }
                asyncLoad.allowSceneActivation = true;
            }*/
            yield return null;
        }

        loadingScreen.SetActive(false);
    }

}
