using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public enum GameState
    {
        GS_MENU,
        GS_TIMER,
        GS_GAME,
        GS_SCORE
    }

    public static Game Singleton;
    public ScoreManager ScoreManager;

    public GameObject   MainTitlePanel;
    public GameObject   TimePanel;
    public GameObject   ScorePanel;

    public MapHelper    MapHelper;
    public GameObject   DogPrefab;
    public GameObject   PlayerPrefab;

    public List<PlayerInteraction> Players;

    public  Text TimeText;
    public  Text TimerText;

    public float TimeAGame = 60.0f;

    private float TimeStartGame = 0.0f;
    public GameState gameState {  get { return _gameState;  } }
    GameState _gameState;

    private void Awake()
    {
        Singleton = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _gameState = GameState.GS_MENU;

        TimePanel.SetActive(false);
        ScorePanel.SetActive(false);

        StartGame();
    }

    string GetTime()
    {
        float timer = TimeAGame - (Time.time - TimeStartGame);
        float minutes = Mathf.Floor(timer / 60);
        float seconds = Mathf.RoundToInt(timer % 60);

        string 
            s_minutes = minutes.ToString(),
            s_seconds = seconds.ToString();

        if (minutes < 10)
        {
            s_minutes = "0" + minutes.ToString();
        }
        if (seconds < 10)
        {
            s_seconds = "0" + Mathf.RoundToInt(seconds).ToString();
        }
        return "Time   " + s_minutes + ':' + s_seconds;
    }
    // Update is called once per frame
    void Update()
    {

        switch (_gameState)
        {
            case GameState.GS_MENU:
                if (Input.anyKeyDown)
                {
                    SetState(GameState.GS_TIMER);
                }
                break;
            case GameState.GS_GAME:
                {
                    TimerText.text = GetTime();
                    float timer = TimeAGame - (Time.time - TimeStartGame);
                    if (TimeStartGame != -1 && timer <= 0)
                    {
                       SetState(GameState.GS_SCORE);                    
                    }
                }  
                break;
            case GameState.GS_SCORE:
                if (Input.anyKeyDown)
                {
                    SetState(GameState.GS_TIMER);
                    ScorePanel.SetActive(false);
                }
                    break;
            case GameState.GS_TIMER:
                {

                }
                break;
        }
    }

    void SetState(GameState newState)
    {
        if (newState == _gameState) return;
        switch (newState)
        {
            case GameState.GS_MENU:
                break;
            case GameState.GS_GAME:
                {
                    MainTitlePanel.SetActive(false);
                    TimeText.enabled = false;
                    TimerText.enabled = true;
                    TimeStartGame = Time.time;
                }
                break;

            case GameState.GS_SCORE:
                ShowScore();
                break;
            case GameState.GS_TIMER:
                {
                    TimeStartGame = -1;

                    if (_gameState == GameState.GS_MENU)
                    {
                        MainTitlePanel.SetActive(false);
                    }
                    StartCoroutine("Timer");
                    TimePanel.SetActive(true);
                    TimerText.enabled = false;
                    TimeText.enabled = true;
                    //Time.timeScale = 0;
                }
                break;
        }

        _gameState = newState;
    }

    void ShowScore()
	{
        BoneManager.Singleton.GetPoints();
        ScoreManager.ShowScore();
        ScorePanel.SetActive(true);
        TimePanel.SetActive(false);

        //foreach (PlayerInteraction p in Players)
        //{
        //    DestroyImmediate(p.gameObject);        
        //}
    }

    IEnumerator Timer()
    {
        TimeText.text = "Ready ? ";
        yield return new WaitForSeconds(0.75f);
  
        TimeText.text = "Let's Gooooo ! ";
        yield return new WaitForSeconds(0.5f);
        SetState(GameState.GS_GAME);
    }

    void StartGame()
    {
        SpawnPlayer();
        SpawnDogs();
    }

    void SpawnPlayer()
    {
        GameObject p = GameObject.Instantiate(PlayerPrefab, MapHelper.Singleton.spawnPlayerPosition, Quaternion.identity);
        Players.Add(p.GetComponent<PlayerInteraction>());
    }

    void SpawnDogs()
    {
        foreach(Vector2 position in MapHelper.Singleton.spawnDogsPosition)
        {
            if (position == Vector2.zero) continue;

            GameObject p = GameObject.Instantiate(DogPrefab, position, Quaternion.identity);
            Players.Add(p.GetComponent<PlayerInteraction>());
        }
    }
}
