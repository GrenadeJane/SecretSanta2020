using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBehaviour : PlayerInteraction
{
    public float speed = 50.0f;                //Floating point variable to store the player's movement speed.

   public Vector2 Direction;

    private Rigidbody2D rb2d;
    float nextDigTime;

    Bone TargetBone = null;
    Vector3 TargetHole = Vector2.one;
    protected override void Awake()
    {
        base.Awake();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        StartCoroutine("ChangeDirection");
        SetNextTimeDig();
        playerState = PlayerState.PS_NONE;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if ( Game.Singleton.gameState != Game.GameState.GS_GAME) return;

        switch (playerState)
        {
            case PlayerState.PS_CarryBone:
                {
                    rb2d.AddForce(Direction * speed, ForceMode2D.Force);
                    if (rb2d.velocity.sqrMagnitude > 0.2f)
                        SoundManagerComponent.PlayWalkSound();
                    else
                        SoundManagerComponent.StopSound();
                    if (!MapHelper.Singleton.IsStillDigged(TargetHole))
                        LetGoTheBone();
                    if ((  TargetHole - transform.position ).sqrMagnitude < 100f)
                    {
                        DigTheBone();
                    }
                }
                break;
            case PlayerState.PS_Scared:
                break;
            case PlayerState.PS_Dig:
                {
                    if (timeCoolDownInput < Time.time)
                    {
                        if ( MapHelper.Singleton.SetTileDigged(transform.position))
                        {
                            StopCoroutine("ChangeDirection");
                            TargetBone = BoneManager.Singleton.GetClosestBone(transform.position);

                            if (TargetBone != null)
                            {
                                Direction = TargetBone.transform.position -transform.position;
                                Direction.Normalize();

                                SetState(PlayerState.PS_GoToBone);
                                return;
                            }
                        }
                        SetState(PlayerState.PS_NONE);
                    }  
                }
                break;
            case PlayerState.PS_GoToBone:
                {
                    rb2d.AddForce(Direction * speed, ForceMode2D.Force);
                    if (rb2d.velocity.sqrMagnitude > 0.2f)
                        SoundManagerComponent.PlayWalkSound();
                    else
                        SoundManagerComponent.StopSound();
                    if (CurrentBoneDistance != null)
                    {
                        SetState(PlayerState.PS_Pee);
                    }
                    else
                    {
                        SetState(PlayerState.PS_NONE);
                    }

                }

                break;

            case PlayerState.PS_Pee:
                if (timeCoolDownInput < Time.time)
                {
                    if (CurrentBoneDistance != null)
                    {
                        CurrentBoneDistance.Mark(this);
                        Carry();
                        Vector3 loc = MapHelper.Singleton.GetClosestDigged(transform.position);
                        if (loc != Vector3.zero)
                        {
                            Direction = transform.position - loc;
                            TargetHole = loc;
                        }
                        else
                        {
                            SetState(PlayerState.PS_NONE);
                        }
                    }
                    else
                        SetState(PlayerState.PS_NONE);
                }
                    break;
            case PlayerState.PS_Bark:
                break;
            case PlayerState.PS_NONE:
                if (nextDigTime < Time.time)
                {
                    SetState(PlayerState.PS_Dig);
                    SetNextTimeDig();
                }
                rb2d.AddForce(Direction * speed, ForceMode2D.Force);
                if (rb2d.velocity.sqrMagnitude > 0.2f)
                    SoundManagerComponent.PlayWalkSound();
                else
                    SoundManagerComponent.StopSound();
                break;
        }
    }

    protected override void SetStateNone()
    {
        StartCoroutine("ChangeDirection");
        nextDigTime = Time.time + 5;
    }
    void SetNextTimeDig()
    {
        nextDigTime = Time.time + 5f;
    }
     
    void SetRandomDir()
    {
        Direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    IEnumerator ChangeDirection()
    {
        while (true)
        {
            SetRandomDir();
            yield return new WaitForSeconds(Random.Range(3, 5));
        }
    }
}
