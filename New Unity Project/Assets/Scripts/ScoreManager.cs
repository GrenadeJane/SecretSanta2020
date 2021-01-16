using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public GameObject ScoreLinePrefab;
    // Start is called before the first frame update

    public void ShowScore()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<ScoreComponent>())
                Destroy(child.gameObject);
        }

        foreach (PlayerInteraction p in Game.Singleton.Players)
        {
            GameObject score =  GameObject.Instantiate(ScoreLinePrefab, this.transform);
            ScoreComponent scoreCompo = score.GetComponent<ScoreComponent>();
            scoreCompo.ColorPlayer.color = p.colorPlayer;
            scoreCompo.ScoreText.text = "Score : "  + p.Score;
        }
    }
}
