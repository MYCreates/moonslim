using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    // Déclaration des constantes
    private static readonly Vector3 FlipRotation = new Vector3(0, 180, 0);
    private static readonly Vector3 CameraPosition = new Vector3(10, 1, 0);
    private static readonly Vector3 InverseCameraPosition = new Vector3(-10, 1, 0);

    // Déclaration des variables
    bool _Grounded { get; set; }
    bool _Flipped { get; set; }
    Animator _Anim { get; set; }
    Rigidbody _Rb { get; set; }
    Camera _MainCamera { get; set; }
    bool _ArrierePlan { get; set; }
    bool _AnimChangementPlan { get; set; }
    bool _HasControl { get; set; }

    Time _ChangementPlanStartTime;
    Time _ChangementPlanTime;

    // Valeurs exposées
    [SerializeField]
    float MoveSpeed = 5.0f;

    [SerializeField]
    float JumpForce = 10f;

    [SerializeField]
    LayerMask WhatIsGround;

    [SerializeField]
    float ChangementPlanDuration = 1.0f;

    // Awake se produit avait le Start. Il peut être bien de régler les références dans cette section.
    void Awake()
    {
        _Anim = GetComponent<Animator>();
        _Rb = GetComponent<Rigidbody>();
        _MainCamera = Camera.main;
    }

    // Utile pour régler des valeurs aux objets
    void Start()
    {
        _Grounded = false;
        _Flipped = false;
        _ArrierePlan = false;
        _HasControl = true;
    }

    // Vérifie les entrées de commandes du joueur
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal") * MoveSpeed;
        if (_HasControl)
        {
            if (_ArrierePlan)
            {

            }
            else
            {
                HorizontalMove(horizontal);
            }
        } 

        FlipCharacter(horizontal);
        CheckJump();
        CheckPlan();
    }

    // Gère le mouvement horizontal
    void HorizontalMove(float horizontal)
    {
        _Rb.velocity = new Vector3(_Rb.velocity.x, _Rb.velocity.y, horizontal);
        _Anim.SetFloat("MoveSpeed", Mathf.Abs(horizontal));
    }

    // Gère le saut du personnage, ainsi que son animation de saut
    void CheckJump()
    {
        if (_Grounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _Rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
                _Grounded = false;
                _Anim.SetBool("Grounded", false);
                _Anim.SetBool("Jump", true);
            }
        }
    }

    void CheckPlan()
    {
        if (!_AnimChangementPlan)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_ArrierePlan)
                {
                    //_Rb.AddForce(new Vector3(0, 0, JumpForce / 5), ForceMode.Impulse);
                    _Grounded = false;
                    _Anim.SetBool("Grounded", false);
                    _Anim.SetBool("Jump", true);
                }
                else
                {
                    //_Rb.AddForce(new Vector3(0, 0, JumpForce / 5), ForceMode.Impulse);
                    _Grounded = false;

                    _Anim.SetBool("Grounded", true);
                    _Anim.SetBool("Jump", false);
                }
                _ArrierePlan = !_ArrierePlan;
            }
        } else
        {
            _Rb.velocity = new Vector3(0, 0, 0);
            Time.deltaTime
        }
    }

    // Gère l'orientation du joueur et les ajustements de la camera
    void FlipCharacter(float horizontal)
    {
        if (horizontal < 0 && !_Flipped)
        {
            _Flipped = true;
            transform.Rotate(FlipRotation);
            _MainCamera.transform.Rotate(-FlipRotation);
            _MainCamera.transform.localPosition = InverseCameraPosition;
        }
        else if (horizontal > 0 && _Flipped)
        {
            _Flipped = false;
            transform.Rotate(-FlipRotation);
            _MainCamera.transform.Rotate(FlipRotation);
            _MainCamera.transform.localPosition = CameraPosition;
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
            _Anim.SetBool("Grounded", _Grounded);
        }
    }
}
