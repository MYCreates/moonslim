﻿using UnityEngine;

public class PlayerControler : MonoBehaviour
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

    // Valeurs exposées
    [SerializeField]
    float MoveSpeed = 3.0f;

    [SerializeField]
    float JumpForce = 5.0f;

    [SerializeField]
    LayerMask WhatIsGround;

    // Awake se produit avait le Start. Il peut être bien de régler les références dans cette section.
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
    }

    // Vérifie les entrées de commandes du joueur
    void Update()
    {
        CheckBoost();
        var horizontal = Input.GetAxis("Horizontal") * MoveSpeed;
        HorizontalMove(horizontal);
        FlipCharacter(horizontal);
        CheckGround();
        CheckJump();
    }

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
        _Rb.velocity = new Vector3(_Rb.velocity.x, _Rb.velocity.y, horizontal*_SpeedBoost);
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

    private void CheckGround()
    {
        _Grounded = Physics.Raycast(transform.position, -Vector3.up, _Collider.bounds.extents.y + 0.1f);
        _Anim.SetBool("Grounded", _Grounded);
    }

    // Gère le saut du personnage, ainsi que son animation de saut
    void CheckJump()
    {
        if (_Grounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _Rb.AddForce(new Vector3(0, JumpForce*_JumpBoost, 0), ForceMode.Impulse);
                _Grounded = false;
                _Anim.SetBool("Grounded", false);
                _Anim.SetBool("Jump", true);
            }
        }
    }

    // Collision avec le sol
    void OnCollisionEnter(Collision coll)
    {        
        // On s'assure de bien être en contact avec le sol
        if ((WhatIsGround & (1 << coll.gameObject.layer)) == 0)
            return;

        // Évite une collision avec le plafond
        if (coll.relativeVelocity.y > 0)
        {
            _Grounded = true;
            _Anim.SetBool("Jump", false);
            _Anim.SetBool("Grounded", _Grounded);
        }
    }

    public void EatCheese(float speed, float time)
    {
        //Anim
        _SpeedBoost = speed;
        _BoostTimer += time;
    }

    public void UseStar(Star star)
    {
        //Anim
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
}
