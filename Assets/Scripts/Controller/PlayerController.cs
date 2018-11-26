﻿using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    // Variables d'accès rapide à des Components
    Animator _Anim;
    Rigidbody _Rb;
    Collider _Collider;
    Score _Score;

    // Variables de contrôle
    bool Dead = false;
    public bool HasControl { get; set; } = true;
    bool Flipped = false;

    // Variables de gestion des collisions
    bool Grounded = false;
    int FloorsOn = 0;
    LayerMask WhatIsGround;
    LayerMask WhatIsWall;
    LayerMask WhatIsHostile;

    // Variables de gestion des boost
    float SpeedBoost = 1.0f;
    float BoostTimer = 0.0f;
    float JumpBoost = 1.0f;

    // Variables de gestion des passages entre les plans
    Vector3 PlayerInitialPosition;
    bool Background = false;
    bool AnimBackgroundGoto = false;
    float BackgroundGotoTime = 0.0f;
    float BackgroundDistance = 2.0f;
    float BackgroundGotoDuration = 0.4f;

    // Valeurs exposées
    [SerializeField]
    float MoveSpeed = 3.0f;
    [SerializeField]
    float JumpForce = 5.0f;
    [SerializeField]
    float BackgroundMoveSpeed = 1.2f;

    void Awake()
    {
        _Anim = GetComponent<Animator>();
        _Rb = GetComponent<Rigidbody>();
        _Collider = GetComponent<Collider>();

        WhatIsGround = LayerMask.GetMask("Floor");
        WhatIsWall = LayerMask.GetMask("Walls");
        WhatIsHostile = LayerMask.GetMask("Hostile");
    }

    void Start()
    {
        _Score = FindObjectOfType<Score>();
    }

    void Update()
    {
        CheckBoost();
        if (HasControl && !Dead)
        {          
            float horizontal = Input.GetAxis("Horizontal") * MoveSpeed;
            float vertical = Input.GetAxis("Vertical") * MoveSpeed;
            if (Background)
            {
                BackgroundMove(horizontal, vertical);
            }
            else
            {
                HorizontalMove(horizontal);
                CheckJump();
            }
            FlipCharacter(horizontal);
            CheckGlide();
        }
        CheckPlan();
        CheckOrientation();
    }

    // Vérifie que le Boost de Vitesse n'est pas terminé
    private void CheckBoost()
    {
        if (SpeedBoost == 1) return;
        BoostTimer -= Time.deltaTime;
        if (BoostTimer < 0)
        {
            BoostTimer = 0.0f;
            SpeedBoost = 1.0f;
        }
    }

    // Mouvement horizontal en premier plan
    void HorizontalMove(float horizontal)
    {
        if (Grounded)
            _Rb.velocity = new Vector3(_Rb.velocity.x, _Rb.velocity.y, horizontal * SpeedBoost);
        else
        {           
            if (horizontal < 0)
                _Rb.velocity = new Vector3(_Rb.velocity.x, _Rb.velocity.y, Math.Max(-3 * SpeedBoost, _Rb.velocity.z + horizontal * SpeedBoost / 5));
            else if (horizontal > 0)
                _Rb.velocity = new Vector3(_Rb.velocity.x, _Rb.velocity.y, Math.Min(3 * SpeedBoost, _Rb.velocity.z + horizontal * SpeedBoost / 5));
            else
                _Rb.velocity = new Vector3(_Rb.velocity.x, _Rb.velocity.y, Math.Min(3 * SpeedBoost, Math.Max(_Rb.velocity.z, -3 * SpeedBoost)));
        }
    }

    // Mouvement dans le fond
    void BackgroundMove(float horizontal, float vertical)
    {
        _Rb.velocity = new Vector3(_Rb.velocity.x, vertical * SpeedBoost * BackgroundMoveSpeed, horizontal * SpeedBoost * BackgroundMoveSpeed);
    }

    // Orientation du joueur
    private void FlipCharacter(float horizontal)
    {
        if (horizontal > 0 && !Flipped)
            Flipped = true;
        else if (horizontal < 0 && Flipped)
            Flipped = false;
        GetComponent<SpriteRenderer>().flipX = Flipped;
    }

    // Saut du personnage
    void CheckJump()
    {
        if (Grounded && HasControl)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _Rb.AddForce(new Vector3(0, JumpForce * JumpBoost, 0), ForceMode.Impulse);
                Grounded = false;
                _Anim.SetBool("Grounded", false);
                _Anim.SetBool("Jump", true);
            }
        }
    }

    // Gestion des différents plans
    void CheckPlan()
    {
        if (!AnimBackgroundGoto)
        {
            int layerMask = 1 << 11;
            layerMask = ~layerMask;

            // Si le joueur appuie sur E et qu'il a le droit de changer de plan
            if ( Input.GetKeyDown(KeyCode.E) &&
                BackgroundGotoTime <= 0 &&
                HasControl &&
                !Physics.Raycast(_Rb.position, new Vector3(1 - 2 * Convert.ToInt32(!Background), 0, 0), BackgroundDistance, 11, QueryTriggerInteraction.Ignore) &&
                !Physics.Raycast(_Rb.position + new Vector3(0, _Collider.bounds.extents.y, _Collider.bounds.extents.z), new Vector3(1 - 2 * Convert.ToInt32(!Background), 0, 0), BackgroundDistance, layerMask, QueryTriggerInteraction.Ignore) &&
                !Physics.Raycast(_Rb.position + new Vector3(0, -_Collider.bounds.extents.y, _Collider.bounds.extents.z), new Vector3(1 - 2 * Convert.ToInt32(!Background), 0, 0), BackgroundDistance, 11, QueryTriggerInteraction.Ignore) &&
                !Physics.Raycast(_Rb.position + new Vector3(0, _Collider.bounds.extents.y, -_Collider.bounds.extents.z), new Vector3(1 - 2 * Convert.ToInt32(!Background), 0, 0), BackgroundDistance, 11, QueryTriggerInteraction.Ignore) &&
                !Physics.Raycast(_Rb.position + new Vector3(0, -_Collider.bounds.extents.y, -_Collider.bounds.extents.z), new Vector3(1 - 2 * Convert.ToInt32(!Background), 0, 0), BackgroundDistance, 11, QueryTriggerInteraction.Ignore) && 
                !Physics.Raycast(_Rb.position + new Vector3(0, 0, _Collider.bounds.extents.z), new Vector3(1 - 2 * Convert.ToInt32(!Background), 0, 0), BackgroundDistance, 11, QueryTriggerInteraction.Ignore) &&
                !Physics.Raycast(_Rb.position + new Vector3(0, _Collider.bounds.extents.y, 0), new Vector3(1 - 2 * Convert.ToInt32(!Background), 0, 0), BackgroundDistance, 11, QueryTriggerInteraction.Ignore) &&
                !Physics.Raycast(_Rb.position + new Vector3(0, 0, -_Collider.bounds.extents.z), new Vector3(1 - 2 * Convert.ToInt32(!Background), 0, 0), BackgroundDistance, 11, QueryTriggerInteraction.Ignore) &&
                !Physics.Raycast(_Rb.position + new Vector3(0, -_Collider.bounds.extents.y, 0), new Vector3(1 - 2 * Convert.ToInt32(!Background), 0, 0), BackgroundDistance, 11, QueryTriggerInteraction.Ignore)
                )
            {
                //DECLANCHE :

                if (Background) // ARRIERE PLAN -> PREMIER PLAN
                {
                    Grounded = false;
                    _Anim.SetBool("Back", false);
                    _Rb.useGravity = true;
                    GetComponent<BoxCollider>().enabled = true;
                    GetComponent<SphereCollider>().enabled = false;
                }
                else // PREMIER PLAN -> ARRIERE PLAN
                {
                    Grounded = false;
                    _Anim.SetBool("Back", true);
                    _Rb.useGravity = false;
                    GetComponent<SphereCollider>().enabled = true;
                    GetComponent<BoxCollider>().enabled = false;
                }
                
                BackgroundGotoTime = BackgroundGotoDuration;
                Background = !Background;
                AnimBackgroundGoto = true;
                HasControl = false;
                PlayerInitialPosition = transform.position;
            }
            else
            {
                // Si Ekto a une rotation en x car il vient de finir son animation
                if (transform.localRotation.x != 0.0f)
                    transform.localRotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
                // Si on est pas en animation, on veut s'assurer que ekto est bien placé
                if (Background)
                    transform.localPosition = new Vector3(-BackgroundDistance, transform.localPosition.y, transform.localPosition.z);  
                else
                    transform.localPosition = new Vector3(0.0f, transform.localPosition.y, transform.localPosition.z);
            }
        }
        else
        {
            _Rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);

            if (BackgroundGotoTime > 0)
            {
                transform.localRotation = Quaternion.Euler(45.0f, -90.0f, 0.0f);
                float elapsed = 1 - (BackgroundGotoTime / BackgroundGotoDuration);
                if (Background) //Va vers l'arriere plan
                {
                    Vector3 goBackground = new Vector3(-BackgroundDistance, 0, 0);
                    _Rb.MovePosition(Vector3.Lerp(PlayerInitialPosition, PlayerInitialPosition + goBackground, elapsed));
                }
                else //Va vers le premier plan
                {
                    Vector3 goForeground = new Vector3(BackgroundDistance, 0.0f, 0.0f);
                    _Rb.MovePosition(Vector3.Lerp(PlayerInitialPosition, PlayerInitialPosition + goForeground, elapsed));
                }
                BackgroundGotoTime -= Time.deltaTime;
                BackgroundGotoTime = Math.Max(BackgroundGotoTime, 0);
            }
            else
            {
                HasControl = true;
                AnimBackgroundGoto = false;
                BackgroundGotoTime = 0;
            }
        }
    }

    // Gere la glissade sur les murs
    void CheckGlide()
    {
        if (Background) return;
        if (Grounded)
        {
            _Anim.SetBool("OnWall", false);
            return;
        }

        RaycastHit hit;

        Ray ray = new Ray(transform.position, Vector3.back);
        if (Physics.Raycast(ray, out hit, _Collider.bounds.extents.z + 0.1f, WhatIsWall)
            && (Input.GetKey("left") || Input.GetKey(KeyCode.Q)))
        {
            _Rb.velocity = new Vector3(_Rb.velocity.x, Math.Max(_Rb.velocity.y, -1), _Rb.velocity.z);
            _Anim.SetBool("OnWall", true);
            return;
        }

        ray = new Ray(transform.position, Vector3.forward);
        if (Physics.Raycast(ray, out hit, _Collider.bounds.extents.z + 0.1f, WhatIsWall)
            && (Input.GetKey("right") || Input.GetKey(KeyCode.D)))
        {
            _Rb.velocity = new Vector3(_Rb.velocity.x, Math.Max(_Rb.velocity.y, -1), _Rb.velocity.z);
            _Anim.SetBool("OnWall", true);
            return;
        }

        _Anim.SetBool("OnWall", false);
    }


    // En l'air le personnage reste droit
    void CheckOrientation()
    {
        if (!Grounded || Background)
        {
            _Rb.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
            _Rb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);

        }
    }

    void OnCollisionEnter(Collision col)
    {
        // Collision avec le sol
        if ((WhatIsGround & (1 << col.gameObject.layer)) != 0)
        {
            float dot = Vector3.Dot(col.contacts[0].normal, Vector3.up);
            if (dot > 0.25)
            {
                FloorsOn += 1;
                Grounded = true;
                _Anim.SetBool("Jump", false);
                _Anim.SetBool("Grounded", Grounded);
            }
        }
        // Collision hostile
        if ((WhatIsHostile & (1 << col.gameObject.layer)) != 0)
            Hit();

        if (col.gameObject.CompareTag("Eatable"))
            col.gameObject.SetActive(false);
    }

    void OnCollisionExit(Collision col)
    {
        // Collision était avec le sol
        if ((WhatIsGround & (1 << col.gameObject.layer)) != 0)
        {
            if (FloorsOn > 0) FloorsOn -= 1;
            if (FloorsOn == 0)
            {
                Grounded = false;
                _Anim.SetBool("Grounded", false);
            }
        }
    }

    public void EatCheese(float speed, float time)
    {
        // TODO : Anim + SON
        SpeedBoost = speed;
        BoostTimer += time;
    }

    public void UseStar(Star star)
    {
        // TODO : Anim + SON
        switch (star.TypeBoost)
        {
            case AgiltyBoost.HigherJump:
                JumpBoost = star.JumpBoost;
                break;
            //case AgiltyBoost.WallJump:

            //    break;
            default:
                break;
        }
    }

    public void Grab(Vector3 mousePosition)
    {
        _Rb.useGravity = false;
        Grounded = false;
        _Rb.velocity = mousePosition - _Rb.position;
        HasControl = false;
    }

    public void Hit()
    {
        // TODO : Anim + SON
        _Score.EktoHit();
        if (_Score.ScoreVal < 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        // TODO : Anim + SON
        _Score.gameObject.SetActive(false);
        _Score.Death.SetActive(true);
        _Anim.SetTrigger("Dead");
        Dead = true;
        Destroy(gameObject, 2);
    }
}