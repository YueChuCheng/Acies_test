﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class VitaSoul_particle : MonoBehaviour
{
    [System.NonSerialized]
    public float MoveSpeed = 2.5f; 
    [System.NonSerialized]
    public bool bMoveFinish = false;

    [System.NonSerialized]
    public bool canDriveOut = false;
    
    public  GameObject OuterParticleObj;
    private ParticleSystem OuterParticleSys;

    [SerializeField]
    private GameObject PromptArrow;
    private SpriteRenderer PromptSprite;

    private GameObject VitaSoulPicture;
    private SpriteRenderer VitaSoulPictureRenderer;

    private TrailRenderer ParticleTrail;

    [System.NonSerialized]
    public int SkillNUM = 0;



    public Animator animator;

    //Follow Player
    private Transform target;
    private float stoppingDistance = 1.2f;

    private Rigidbody2D rb;

    [System.NonSerialized]
    public bool bPromptExist = false;

    // Start is called before the first frame update
    void Start()
    {
        OuterParticleSys = OuterParticleObj.GetComponent<ParticleSystem>();


        ParticleTrail = OuterParticleObj.GetComponent<TrailRenderer>();

        //follow player
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();


        //prompt arrow
        PromptSprite = PromptArrow.GetComponent<SpriteRenderer>();


        rb = this.GetComponent<Rigidbody2D>();
    }

    public void LightUpVita(Color lightColor)
    {
        ParticleTrail.startColor = lightColor;
        ParticleTrail.endColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        

        this.GetComponent<SpriteRenderer>().color = lightColor;

        //set Skill
        if (PlayerSkill.CURRENTSKILL == 1)
            canDriveOut = true;

   

    }



    public void VitaSpriteFadeIn()
    {
        VitaSoulPictureRenderer = VitaSoulPicture.GetComponent<SpriteRenderer>();
        
        //stop IEnumerator
       StartCoroutine(VitaSpriteFadeInIEnumerator());
    }

    IEnumerator VitaSpriteFadeInIEnumerator() //Vita core
    {

        for (float a = 0.0f; a < 1.0f; a += 0.4f)
        {

            VitaSoulPictureRenderer.color = new Color(VitaSoulPictureRenderer.color.r, VitaSoulPictureRenderer.color.g, VitaSoulPictureRenderer.color.b, a);
            yield return new WaitForSeconds(0.15f);
        }
        VitaSoulPictureRenderer.color = new Color(VitaSoulPictureRenderer.color.r, VitaSoulPictureRenderer.color.g, VitaSoulPictureRenderer.color.b, 1.0f);

    }

    public void PromptFadeIn()
    {
        bPromptExist = true;
        //stop IEnumerator
        StopCoroutine(PromptFadeInIEnumerator());
        StopCoroutine(PromptFadeOutIEnumerator());

        StartCoroutine(PromptFadeInIEnumerator());

    }

    public void PromptFadeOut()
    {

        bPromptExist = false;
        //stop IEnumerator
        StopCoroutine(PromptFadeInIEnumerator());
        StopCoroutine(PromptFadeOutIEnumerator());

        StartCoroutine(PromptFadeOutIEnumerator());

    }
    IEnumerator PromptFadeInIEnumerator() //Vita core
    {
        
        for (float a = 0.0f; a < 1.0f; a += 0.4f)
        {
            
            PromptSprite.color = new Color(PromptSprite.color.r, PromptSprite.color.g, PromptSprite.color.b, a);
            yield return new WaitForSeconds(0.15f);
        }
        PromptSprite.color = new Color(PromptSprite.color.r, PromptSprite.color.g, PromptSprite.color.b, 1.0f);

    }

    IEnumerator PromptFadeOutIEnumerator()//Vita core
    {
        for (float a = 1.0f; a > 0.0f; a -= 0.4f)
        {
            PromptSprite.color = new Color(PromptSprite.color.r, PromptSprite.color.g, PromptSprite.color.b, a); 
            yield return new WaitForSeconds(0.15f);
        }
        PromptSprite.color = new Color(PromptSprite.color.r, PromptSprite.color.g, PromptSprite.color.b, 0.0f);

    }




    public void StopSkillAfterTime(float time)
    {
       
        StopAllCoroutines();
        StartCoroutine(FadeOutAfterTimeIEnumerator( time));
    }

    IEnumerator FadeOutAfterTimeIEnumerator(float time)
    {
        yield return new WaitForSeconds(time);
       
      

        //reset
        reset();

    }

    public void reset()
    {
        this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        ParticleTrail.startColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        ParticleTrail.endColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        canDriveOut = false;

    }


    public void MoveToward(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.x, target.y), MoveSpeed * Time.deltaTime);
        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            bMoveFinish = true;
        }
        else
        {
            bMoveFinish = false;
        }
    }

    bool bChangeFace = true;

    public void FollowObj()
    {
        float moveInput = Input.GetAxis("Horizontal");


        //change Scale
        if ((moveInput > 0 && transform.localScale.x < 0.0f) || (moveInput < 0 && transform.localScale.x > 0.0f))
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            bChangeFace = true;

        }

        

        //change X
        if (bChangeFace || Mathf.Abs(transform.position.x - target.position.x) > stoppingDistance)
        {
            //change vita soul position
            if (transform.localScale.x > 0) // face left
            {
                if (transform.position.x != target.position.x - stoppingDistance)
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x - stoppingDistance, transform.position.y), 10.0f * Time.deltaTime);
                else
                    bChangeFace = false;
            }
            else // face right
            {
                if (transform.position.x != target.position.x + stoppingDistance) 
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x + stoppingDistance, transform.position.y), 10.0f * Time.deltaTime);
                else
                    bChangeFace = false;
            }
        }
        else if(Mathf.Abs(transform.position.x - target.position.x) > stoppingDistance) // vita move with player
        {
            rb.velocity = new Vector2(moveInput * 5.0f, rb.velocity.y);
        }
       



        //change Y
        if (target.position.y + 0.57 != transform.position.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, target.position.y + 0.57f), 10.0f * Time.deltaTime); //only move x
        }
        


    }


}
