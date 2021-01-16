using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    PS_CarryBone,
    PS_Scared,
    PS_Dig,
    PS_Pee,
    PS_Bark,
    PS_GoToBone,
    PS_NONE
}

public class PlayerInteraction : MonoBehaviour
{

    public float timeForDigging = 3.0f;
    public float timeForPeeing = 2.0f;
    public Color colorPlayer;

    protected float timeCoolDownInput;
    private Animator animatorComponent;
    protected SoundManager SoundManagerComponent;

    public Bone CurrentBoneDistance;
    public Bone BoneCarried;
    public PlayerState playerState = PlayerState.PS_NONE;

    public float Score;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        playerState = PlayerState.PS_NONE;
        SetColor();
    }
    protected virtual void Awake()
    {
        animatorComponent = GetComponent<Animator>();
        SoundManagerComponent = GetComponent<SoundManager>();
    }

    public void SetColor()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color color = new Color();
        color.r = Random.Range(0, 255);
        color.g = Random.Range(0, 255);
        color.b = Random.Range(0, 255);
        color.a = 255;

        colorPlayer = new Color(color.r/255,  color.g / 255, color.b / 255,1);
        renderer.color = new Color(color.r / 255, color.g / 255, color.b / 255, 1);
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerState)
        {
            case PlayerState.PS_CarryBone:
                {
                    if (Input.GetButtonDown("Carry"))
                        LetGoTheBone();
                    if ( Input.GetButtonDown("Dig") )
                         DigTheBone();
                }
                break;
            case PlayerState.PS_Dig:
                
                if (Input.GetButtonUp("Dig"))
                    SetState(PlayerState.PS_NONE);
                if (timeCoolDownInput < Time.time)
                {
                    MapHelper.Singleton.SetTileDigged(transform.position);
                    SetState(PlayerState.PS_NONE);
                }
                break;
            case PlayerState.PS_Pee:
                {
                    if (timeCoolDownInput < Time.time)
                    {
                        if (CurrentBoneDistance != null)
                            CurrentBoneDistance.Mark(this);
                        SetState(PlayerState.PS_NONE);
                    }
                }
                break;
            case PlayerState.PS_Scared:
            case PlayerState.PS_Bark:
            case PlayerState.PS_NONE:

                if (Input.GetButtonDown("Bark"))
                    Bark();
                if (Input.GetButtonDown("Pee"))
                    Pee();
                if (Input.GetButtonDown("Dig") )
                    Dig();
                if( Input.GetButtonDown("Carry"))
                    Carry();
                break;
        }


    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.tag == "Bone")
        {
            CurrentBoneDistance = other.GetComponent<Bone>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Bone")
        {
            CurrentBoneDistance = null;
        }
    }

    protected void SetState(PlayerState newState)
    {
        if (newState == playerState) return;

        switch (playerState)
        {
            case PlayerState.PS_CarryBone:
                break;
            case PlayerState.PS_Scared:
                break;
            case PlayerState.PS_Dig:
                animatorComponent.SetBool("Dig", false);
                break;
            case PlayerState.PS_Pee:
                animatorComponent.SetBool("Pee", false);

                break;
            case PlayerState.PS_Bark:

                break;
            case PlayerState.PS_NONE:
                break;
        }
        switch (newState)
        {
            case PlayerState.PS_CarryBone:
                break;
            case PlayerState.PS_Scared:
                break;
            case PlayerState.PS_Dig:
                animatorComponent.SetBool("Dig", true);
                timeCoolDownInput = Time.time + timeForDigging;
                break;
            case PlayerState.PS_Pee:
                animatorComponent.SetBool("Pee", true);

                timeCoolDownInput = Time.time + timeForPeeing;
                break;
            case PlayerState.PS_Bark:
              
                break;
            case PlayerState.PS_NONE:
                SetStateNone();
                break;
        }

        if ( SoundManagerComponent != null )
         SoundManagerComponent.PlaySound(newState);
        playerState = newState;
    }

    protected virtual void SetStateNone()
    {

    }
    protected void DigTheBone()
    {
        if (!MapHelper.Singleton.SetTileWithBone(transform.position, BoneCarried)) return;

        BoneCarried.gameObject.transform.SetParent(null, true);
        //Destroy(BoneCarried.gameObject);
        SetState(PlayerState.PS_NONE);
        BoneCarried = null;
    }

    protected void LetGoTheBone()
    {
        BoneCarried.gameObject.transform.SetParent(null, true);
        SetState(PlayerState.PS_NONE);
        BoneCarried = null;
    }

    protected void Carry()
    {
        if (CurrentBoneDistance == null) return;
        
        BoneCarried = CurrentBoneDistance;

        BoneCarried.gameObject.transform.SetParent(gameObject.transform, true);
        BoneCarried.gameObject.transform.localPosition = Vector3.zero;

        SetState(PlayerState.PS_CarryBone);
    }

    void Dig()
    {
        if ( MapHelper.Singleton.GetTileWithPosition(transform.position))
            SetState(PlayerState.PS_Dig);
    }

    void Pee()
    {
        SetState(PlayerState.PS_Pee);
    }

    void Bark()
    {
        SoundManagerComponent.PlaySound(PlayerState.PS_Bark);
        animatorComponent.SetTrigger("Bark");
    }
}
