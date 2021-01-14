using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{
    // Start is called before the first frame update
    public int points = 0;
    public PlayerInteraction owner;
    public SpriteRenderer spriteRenderer;
    public bool bIsBurried = false;
    public Color Color
    {
        get { return _color; }
        set
        {
            _color = value;
            spriteRenderer.color = value;
        }
    }
    private Color _color;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

   
    public void Mark(PlayerInteraction newOwner)
    {
        owner = newOwner;
        Color = newOwner.colorPlayer;
    }

    public void Burried(bool burried)
    {
        gameObject.SetActive(!burried);
        bIsBurried = burried;
    }
}
