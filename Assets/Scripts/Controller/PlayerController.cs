﻿using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    // Déclaration des variables
    bool _Dead { get; set; }
    bool _Grounded { get; set; }
    int _FloorsOn { get; set; }
    bool _Flipped { get; set; }
    float _SpeedBoost { get; set; }
    float _BoostTimer { get; set; }
    float _JumpBoost { get; set; }
    Animator _Anim { get; set; }
    Rigidbody _Rb { get; set; }
    Collider _Collider { get; set; }
    Score _Score { get; set; }


    bool _Background { get; set; }
    bool _AnimBackgroundGoto { get; set; }

    bool _MouseGrabbed { get; set; }
    float _MouseGrabbedDuration { get; set; }
    float _MouseIFrame { get; set; }
    Vector3 _MouseThrownDirection { get; set; }
    Vector3 _MouseGrabberCenter { get; set; }
    bool _HasControl { get; set; }
    Vector3 PlayerInitialPosition;

    float _BackgroundGotoTime;

    // Valeurs exposées
    [SerializeField]
    float MoveSpeed = 3.0f;
    [SerializeField]
    float JumpForce = 5.0f;
    [SerializeField]
    float BackgroundGotoDuration = 0.2f;
    [SerializeField]
    float BackgroundPlayerVelocity = 1.2f;

    LayerMask WhatIsGround;
    LayerMask WhatIsWall;
    LayerMask WhatIsMouse;

    float BackgroundDistance = 1.5f;

    void Awake()
    {
        _Anim = GetComponent<Animator>();
        _Rb = GetComponent<Rigidbody>();
        _Collider = GetComponent<Collider>();

        WhatIsGround = LayerMask.GetMask("Floor");
        WhatIsWall = LayerMask.GetMask("Walls");
        WhatIsMouse = LayerMask.GetMask("Hostile");

    }

    // Utile pour régler des valeurs aux objets
    void Start()
    {
        _Score = FindObjectOfType<Score>();

        _Dead = false;
        _Grounded = false;
        _Flipped = false;
        _SpeedBoost = 1.0f;
        _BoostTimer = 0.0f;
        _JumpBoost = 1.0f;
        _Background = false;
        _HasControl = true;
    }

    void Update()
    {
        CheckBoost();
        if (_HasControl && !_Dead)
        {          
            float horizontal = Input.GetAxis("Horizontal") * MoveSpeed;
            float vertical = Input.GetAxis("Vertical") * MoveSpeed;
            if (_Background)
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
        CheckMouseGrabbed();
        CheckPlan();
        CheckOrientation();
    }

    // Vérifie que le Boost de Vitesse n'est pas terminé
    private void CheckBoost()
    {
        if (_SpeedBoost == 1) return;
        _BoostTimer -= Time.deltaTime;
        if (_BoostTimer < 0)
        {
            _BoostTimer = 0.0f;
            _SpeedBoost = 1.0f;
        }
    }

    // Gère le mouvement horizontal
    void HorizontalMove(float horizontal)
    {
        if (_Grounded)
        {
            _Rb.velocity = new Vector3(_Rb.velocity.x, _Rb.velocity.y, horizontal * _SpeedBoost);
        }
        else
        {
            
            if (horizontal < 0)
            {
                _Rb.velocity = new Vector3(_Rb.velocity.x, _Rb.velocity.y, Math.Max(-3 * _SpeedBoost, _Rb.velocity.z + horizontal * _SpeedBoost / 5));
            }
            else if (horizontal > 0)
            {
                _Rb.velocity = new Vector3(_Rb.velocity.x, _Rb.velocity.y, Math.Min(3 * _SpeedBoost, _Rb.velocity.z + horizontal * _SpeedBoost / 5));
            } else
            {
                _Rb.velocity = new Vector3(_Rb.velocity.x, _Rb.velocity.y, Math.Min(3 * _SpeedBoost, Math.Max(_Rb.velocity.z, -3 * _SpeedBoost)));
            }
        }
    }

    // Gère le mouvement dans le fond
    void BackgroundMove(float horizontal, float vertical)
    {
        _Rb.velocity = new Vector3(_Rb.velocity.x, vertical * _SpeedBoost * BackgroundPlayerVelocity, horizontal * _SpeedBoost * BackgroundPlayerVelocity);
    }

    // Gère l'orientation du joueur
    private void FlipCharacter(float horizontal)
    {
        if (horizontal > 0 && !_Flipped)
        {
            _Flipped = true;
        }
        else if (horizontal < 0 && _Flipped)
        {
            _Flipped = false;
        }
        GetComponent<SpriteRenderer>().flipX = _Flipped;
    }

    // Gère le saut du personnage, ainsi que son animation de saut
    void CheckJump()
    {
        if (_Grounded && _HasControl)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _Rb.AddForce(new Vector3(0, JumpForce * _JumpBoost, 0), ForceMode.Impulse);
                _Grounded = false;
                _Anim.SetBool("Grounded", false);
                _Anim.SetBool("Jump", true);
            }
        }
    }

    // Vérifie dans quel plan le personnage se situe
    void CheckPlan()
    {
        if (!_AnimBackgroundGoto)
        {
            if ( Input.GetKeyDown(KeyCode.E) &&
                _BackgroundGotoTime <= 0 &&
                _HasControl &&
                !Physics.Raycast(_Rb.position, new Vector3(1 - 2 * Convert.ToInt32(!_Background), 0, 0), BackgroundDistance, 11, QueryTriggerInteraction.Ignore) &&
                !Physics.Raycast(_Rb.position + new Vector3(0, _Collider.bounds.extents.y, _Collider.bounds.extents.z), new Vector3(1 - 2 * Convert.ToInt32(!_Background), 0, 0), BackgroundDistance, 11, QueryTriggerInteraction.Ignore) &&
                !Physics.Raycast(_Rb.position + new Vector3(0, -_Collider.bounds.extents.y, _Collider.bounds.extents.z), new Vector3(1 - 2 * Convert.ToInt32(!_Background), 0, 0), BackgroundDistance, 11, QueryTriggerInteraction.Ignore) &&
                !Physics.Raycast(_Rb.position + new Vector3(0, _Collider.bounds.extents.y, -_Collider.bounds.extents.z), new Vector3(1 - 2 * Convert.ToInt32(!_Background), 0, 0), BackgroundDistance, 11, QueryTriggerInteraction.Ignore) &&
                !Physics.Raycast(_Rb.position + new Vector3(0, -_Collider.bounds.extents.y, -_Collider.bounds.extents.z), new Vector3(1 - 2 * Convert.ToInt32(!_Background), 0, 0), BackgroundDistance, 11, QueryTriggerInteraction.Ignore) && 
                !Physics.Raycast(_Rb.position + new Vector3(0, 0, _Collider.bounds.extents.z), new Vector3(1 - 2 * Convert.ToInt32(!_Background), 0, 0), BackgroundDistance, 11, QueryTriggerInteraction.Ignore) &&
                !Physics.Raycast(_Rb.position + new Vector3(0, _Collider.bounds.extents.y, 0), new Vector3(1 - 2 * Convert.ToInt32(!_Background), 0, 0), BackgroundDistance, 11, QueryTriggerInteraction.Ignore) &&
                !Physics.Raycast(_Rb.position + new Vector3(0, 0, -_Collider.bounds.extents.z), new Vector3(1 - 2 * Convert.ToInt32(!_Background), 0, 0), BackgroundDistance, 11, QueryTriggerInteraction.Ignore) &&
                !Physics.Raycast(_Rb.position + new Vector3(0, -_Collider.bounds.extents.y, 0), new Vector3(1 - 2 * Convert.ToInt32(!_Background), 0, 0), BackgroundDistance, 11, QueryTriggerInteraction.Ignore)
                )
            {
                
                if (_Background) //On passe au premier plan
                {
                    _Grounded = false;
                    _Anim.SetBool("Back", false);
                    _Rb.useGravity = true;
                }
                else //on passe à l'arriere plan
                {
                    _Grounded = false;
                    _Anim.SetBool("Back", true);
                    _Rb.useGravity = false;
                }

                _BackgroundGotoTime = BackgroundGotoDuration;
                _Background = !_Background;
                _AnimBackgroundGoto = true;
                _HasControl = false;
                PlayerInitialPosition = _Rb.position;
            }
            else
            {
                if (_Background)
                    transform.localPosition = new Vector3(-1.5f, transform.localPosition.y, transform.localPosition.z);  
                else
                    transform.localPosition = new Vector3(0.0f, transform.localPosition.y, transform.localPosition.z);
            }
        }
        else
        {
            _Rb.velocity = new Vector3(0, 0, 0);

            if (_BackgroundGotoTime > 0)
            {
                float elapsed = 1 - (_BackgroundGotoTime / BackgroundGotoDuration);
                if (_Background) //Va vers l'arriere plan
                {
                    Vector3 goBackground = new Vector3(-BackgroundDistance, 0, 0);
                    _Rb.MovePosition(Vector3.Lerp(PlayerInitialPosition, PlayerInitialPosition + goBackground, elapsed));
                }
                else //Va vers le premier plan
                {
                    Vector3 goForeground = new Vector3(BackgroundDistance, 0, 0);
                    _Rb.MovePosition(Vector3.Lerp(PlayerInitialPosition, PlayerInitialPosition + goForeground, elapsed));
                }
                _BackgroundGotoTime -= Time.deltaTime;
                _BackgroundGotoTime = Math.Max(_BackgroundGotoTime, 0);
            }
            else
            {
                _HasControl = true;
                _AnimBackgroundGoto = false;
                _BackgroundGotoTime = 0;
            }
        }
    }

    // Gere la glissade sur les murs
    void CheckGlide()
    {
        if (_Background) return;
        if (_Grounded)
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

    // TO DO : Je pense qu'il vaut mieux gérer une bonne partie dans MouseController::OnCollisionEnter()
    void CheckMouseGrabbed()
    {
        if (_MouseIFrame > 0)
        {
            _MouseIFrame -= Time.deltaTime;
            if (_MouseIFrame <= 0)
            {
                // TODO : pour bien faire, prendre le collider DE LA SOURIS plutot que celui d'Ekto --> pas d'embrouille dans les iframes
                _Collider.enabled = true;
            }
        }

        if (_MouseGrabbed)
        {
            if (_MouseGrabbedDuration > 0)
            {
                _MouseGrabbedDuration -= Time.deltaTime;
                _Rb.velocity = _MouseGrabberCenter - _Rb.position;
            }
            else
            {
                // Lancer Ekto
                _MouseGrabbed = false;
                _HasControl = true;
                _Rb.useGravity = true;
                _Rb.velocity = _MouseThrownDirection * 5f;
                _MouseIFrame = 0.5f;
            }

        }
    }

    // Vérifie qu'en l'air le personnage ne vrille pas
    void CheckOrientation()
    {
        if (!_Grounded || _Background)
        {
            _Rb.rotation = Quaternion.Euler(0, -90, 0);
            _Rb.angularVelocity = new Vector3(0, 0, 0);

        }
    }

    // Collision avec le sol
    // TODO : coll --> col
    void OnCollisionEnter(Collision coll)
    {
        // Collision avec un sol
        if ((WhatIsGround & (1 << coll.gameObject.layer)) != 0)
        {
            float dot = Vector3.Dot(coll.contacts[0].normal, Vector3.up);
            if (dot > 0.25)
            {
                _FloorsOn += 1;
                _Grounded = true;
                _Anim.SetBool("Jump", false);
                _Anim.SetBool("Grounded", _Grounded);
            }
        }

        // Mouse grabbed
        if ((WhatIsMouse & (1 << coll.gameObject.layer)) != 0 && _MouseIFrame <= 0)
        {
            // Collider & model
            _Collider.enabled = false;
            _Rb.useGravity = false;
            Hit();

            // Variables
            _MouseGrabbed = true;
            _Grounded = false;
            _MouseGrabberCenter = coll.gameObject.transform.position;
            _Rb.velocity = _MouseGrabberCenter - _Rb.position;
            _HasControl = false;
            _MouseGrabbedDuration = coll.gameObject.GetComponent<MouseController>().GrabDuration;

            // Direction du lancer
            if (coll.collider.bounds.center.z - _Collider.bounds.center.z < 0)
            {
                _MouseThrownDirection = new Vector3(0, 1f, 1f);
            } else
            {
                _MouseThrownDirection = new Vector3(0, 1f, -1f);
            }
        }
    }

    // Collision avec le sol
    void OnCollisionExit(Collision coll)
    {
        // On s'assure de bien être en contact avec le sol
        if ((WhatIsGround & (1 << coll.gameObject.layer)) != 0)
        {
            if (_FloorsOn > 0) _FloorsOn -= 1;
            if (_FloorsOn == 0)
            {
                _Grounded = false;
                _Anim.SetBool("Grounded", false);
            }
        }
    }

    public void EatCheese(float speed, float time)
    {
        // TODO : Anim
        _SpeedBoost = speed;
        _BoostTimer += time;
    }

    public void UseStar(Star star)
    {
        // TODO : Anim
        switch (star.TypeBoost)
        {
            case AgiltyBoost.HigherJump:
                _JumpBoost = star.JumpBoost;
                break;
            //case AgiltyBoost.WallJump:

            //    break;
            default:
                break;
        }
    }

    public void Hit()
    {
        // TODO : Anim
        _Score.EktoHit();
        if (_Score.ScoreVal < 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        _Score.gameObject.SetActive(false);
        _Score.Death.SetActive(true);
        _Anim.SetTrigger("Dead");
        _Dead = true;
        Destroy(gameObject, 2);

    }
}