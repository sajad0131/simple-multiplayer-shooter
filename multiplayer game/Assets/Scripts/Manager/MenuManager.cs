using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Timers;

public class MenuManager : MonoBehaviour
{

    [SerializeField]
    private string map;
    [SerializeField]
    private string mode;
    [SerializeField]
    private GameObject networkManager;
    [SerializeField]
    private GameObject multiplayerUI;
    [SerializeField]
    private GameObject mainMenuUI;
    [SerializeField]
    private GameObject mapUI;
    [SerializeField]
    private GameObject modeUI;
    private InitiateGame ig;
    public string id;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    float progress;
    public bool isLoaded { get; private set; } = false;

    private Timer timer1;

    private Settings setting;

    void Start()
    {
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager");
        ig = networkManager.GetComponent<InitiateGame>();
        setting = new Settings();


        // DontDestroyOnLoad(networkManager);

    }

    public void OnMultiplayerButton()
    {

        mainMenuUI.SetActive(false);
        multiplayerUI.SetActive(true);

    }
    public void OnMapButton()
    {
        multiplayerUI.SetActive(false);
        mapUI.SetActive(true);
    }

    public void OnModeButton()
    {
        multiplayerUI.SetActive(false);
        modeUI.SetActive(true);
    }
    public void OnCrossButton(GameObject disableObject)
    {
        disableObject.SetActive(false);
        multiplayerUI.SetActive(true);
    }
    public void OnMapSelect(string name)
    {
        map = name;
    }
    public void OnModeSelect(string name)
    {
        mode = name;
    }

    public void OnStartButton()
    {
        ig.Initiate(map, mode,id);
    }

    public void loadLevel()
    {

        StartCoroutine(Loading());
        /*timer1 = new Timer();
        timer1.Elapsed += (sender, e) =>
        {
            if(progress == 1)
            {
                ig.mapLoaded(id);
                timer1.Stop();
            }
        };
        timer1.Interval = 500;//miliseconds

        timer1.Start();*/

    }
    IEnumerator Loading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        
        while (!operation.isDone)
        {
            progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;
            yield return null;

        }
        SceneManager.MoveGameObjectToScene(networkManager, SceneManager.GetSceneByBuildIndex(1));
        SceneManager.UnloadSceneAsync(0);
        ig.mapLoaded(id);
        setting.GetSettings();

    }


    public void Disable(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void Enable(GameObject obj)
    {
        obj.SetActive(true);
    }




}
