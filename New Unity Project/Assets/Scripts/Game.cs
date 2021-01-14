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

    public GameObject   MainTitlePanel;
    public GameObject   TimePanel;

    public MapHelper    MapHelper;
    public GameObject   DogPrefab;
    public GameObject   PlayerPrefab;

    private Text TimeText;
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
        TimeText = TimePanel.GetComponentInChildren<Text>();

        StartGame();
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
                break;
            case GameState.GS_SCORE:
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
                    TimePanel.SetActive(false);
                    Time.timeScale = 1;
                }
                break;

            case GameState.GS_SCORE:
                break;
            case GameState.GS_TIMER:
                {
                    if (_gameState == GameState.GS_MENU)
                    {
                        MainTitlePanel.SetActive(false);
                    }
                    StartCoroutine("Timer");
                    TimePanel.SetActive(true);

                    //Time.timeScale = 0;
                }
                break;
        }

        _gameState = newState;
    }

    IEnumerator Timer()
    {
        TimeText.text = "Ready ? ";
             yield return new WaitForSeconds(1.5f);
        TimeText.text = "Let's Gooooo ! ";
        yield return new WaitForSeconds(1.5f);
        TimeText.text = "3";
        yield return new WaitForSeconds(1f);

        TimeText.text = "2";
        yield return new WaitForSeconds(1f);

        TimeText.text = "1";
        yield return new WaitForSeconds(1f);

        SetState(GameState.GS_GAME);
    }

    void StartGame()
    {
        SpawnPlayer();
        SpawnDogs();
    }

    void SpawnPlayer()
    {
        GameObject.Instantiate(PlayerPrefab, MapHelper.Singleton.spawnPlayerPosition, Quaternion.identity);
    }

    void SpawnDogs()
    {
        foreach(Vector2 position in MapHelper.Singleton.spawnDogsPosition)
        {
            if (position == Vector2.zero) continue;

            GameObject.Instantiate(DogPrefab, position, Quaternion.identity);
        }
    }
}
