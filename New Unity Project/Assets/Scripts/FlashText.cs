using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashText : MonoBehaviour
{
    public float speedFade = 0.5f;
    Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("FadeIn");   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeIn()
    {
        float opacity = 1;
        while ( true )
        {
            while ( opacity > 0.1f)
            {
                opacity -= speedFade * Time.deltaTime;
                text.color =  new Color( text.color.r, text.color.g, text.color.b, opacity);
                yield return false;
            }

            while (opacity < 1f)
            {
                opacity += speedFade * Time.deltaTime;
                text.color = new Color(text.color.r, text.color.g, text.color.b, opacity);
                yield return false;
            }
        }

    }
}
