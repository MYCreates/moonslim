﻿using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    // Déclaration des variables
    bool _Grounded { get; set; }
    bool _Flipped { get; set; }
    float _SpeedBoost { get; set; }
    float _BoostTimer { get; set; }
    float _JumpBoost { get; set; }
    Animator _Anim { get; set; }
    Rigidbody _Rb { get; set; }
    Collider _Collider { get; set; }
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
    LayerMask WhatIsGround;
    [SerializeField]
    LayerMask WhatIsWall;
    [SerializeField]
    LayerMask WhatIsMouse;
    [SerializeField]
    float BackgroundGotoDuration = 1.0f;
    [SerializeField]
    float BackgroundPlayerVelocity = 1.2f;

    void Awake()
    {
        _Anim = GetComponent<Animator>();
        _Rb = GetComponent<Rigidbody>();
        _Collider = GetComponent<Collider>();
    }

    // Utile pour régler des valeurs aux objets
    void Start()
    {
        _Grounded = false;
        _Flipped = false;
        _SpeedBoost = 1.0f;
        _BoostTimer = 0.0f;
        _JumpBoost = 1.0f;
        _Background = false;
        _HasControl = true;

        _Rb.freezeRotation = true;

    }

    void Update()
    {
        CheckBoost();
        if (_HasControl)
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
        }
        CheckMouseGrabbed();
        CheckPlan();
        CheckGlide();
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
            //float h = horizontal * Time.deltaTime * MoveSpeed;
            //transform.Translate(h, 0, 0);
            _Rb.velocity = new Vector3(_Rb.velocity.x, _Rb.velocity.y, horizontal * _SpeedBoost);
        }
        else
        {
            if (horizontal < 0)
            {
                _Rb.velocity = new Vector3(_Rb.velocity.x, _Rb.velocity.y, Math.Max(horizontal * _SpeedBoost, _Rb.velocity.z + horizontal * _SpeedBoost / 20));
            }
            else if (horizontal > 0)
            {
                _Rb.velocity = new Vector3(_Rb.velocity.x, _Rb.velocity.y, Math.Min(horizontal * _SpeedBoost, _Rb.velocity.z + horizontal * _SpeedBoost / 20));
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
            if (Input.GetKeyDown(KeyCode.E) && _BackgroundGotoTime <= 0 && _HasControl)
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
        }
        else
        {
            _Rb.velocity = new Vector3(0, 0, 0);

            if (_BackgroundGotoTime > 0)
            {
                float BackgroundDistance = 1.5f;
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
                _Grounded = true;//hum?
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
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * 1.0f, Color.green);

        // TO DO : ne fonctionne pas (pas de Debug lancé)
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.left));
        if (Physics.Raycast(ray, out hit, 1.0f, WhatIsWall) && Input.GetKey("left"))
        {
            _Rb.velocity = new Vector3(_Rb.velocity.x, Math.Max(_Rb.velocity.y, -1), _Rb.velocity.z);
            Debug.Log("Did hit left : " + hit.collider.gameObject.layer);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * 1.0f, Color.green);
            _Anim.SetBool("OnWall", true);
            return;
        }

        ray = new Ray(transform.position, transform.TransformDirection(Vector3.right));
        if (Physics.Raycast(ray, out hit, 1.0f, WhatIsWall) && Input.GetKey("right"))
        {
            _Rb.velocity = new Vector3(_Rb.velocity.x, Math.Max(_Rb.velocity.y, -1), _Rb.velocity.z);
            Debug.Log("Did hit right : " + hit.collider.gameObject.layer);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.green);
            _Anim.SetBool("OnWall", true);
            return;
        }
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
                _MouseIFrame = 1;
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
    void OnCollisionEnter(Collision coll)
    {
        // Collision avec un sol
        if ((WhatIsGround & (1 << coll.gameObject.layer)) != 0)
        {
            // Évite une collision avec le plafond
            if (coll.contacts[0].normal == transform.up)
            {
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

            // Variables
            _MouseGrabbed = true;
            _Grounded = false;
            _MouseGrabberCenter = coll.gameObject.transform.position;
            _Rb.velocity = _MouseGrabberCenter - _Rb.position;
            _HasControl = false;
            _MouseGrabbedDuration = coll.gameObject.GetComponent<MouseController>().GrabDuration;
            Vector3 contact = coll.contacts[0].normal;
            _MouseThrownDirection = new Vector3(contact.x, contact.y + 1f, contact.z);
        }
    }

    // Collision avec le sol
    void OnCollisionExit(Collision coll)
    {
        // On s'assure de bien être en contact avec le sol
        if ((WhatIsGround & (1 << coll.gameObject.layer)) != 0)
        {
            _Grounded = false;
            _Anim.SetBool("Grounded", false);
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
            case AgiltyBoost.WallJump:

                break;
            default:
                break;
        }
    }

    public void Kill()
    {
        // TODO : Anim
        // TODO : Text Canvas
        _HasControl = false;

    }
}