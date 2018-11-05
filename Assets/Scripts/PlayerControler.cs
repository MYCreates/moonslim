using UnityEngine;
using System;

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
    bool _ArrierePlan { get; set; }
    bool _AnimChangementPlan { get; set; }
    bool _HasControl { get; set; }
    
    float _ChangementPlanTime;

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
    float ChangementPlanDuration = 1.0f;

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
        _ArrierePlan = false;
        _HasControl = true;
    }

    // Vérifie les entrées de commandes du joueur
    void Update()
    {
        CheckBoost();
        var horizontal = Input.GetAxis("Horizontal") * MoveSpeed;
        var vertical = Input.GetAxis("Vertical") * MoveSpeed;
        if (_HasControl)
        {
            if (_ArrierePlan)
            {
                CheeseMove(horizontal, vertical);
            }
            else
            {
                HorizontalMove(horizontal);
            }

            FlipCharacter(horizontal);
            CheckJump();
        }

        CheckPlan();
        CheckGround();
        CheckGlide();
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
        if (_Grounded)
        {
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
    void CheeseMove(float horizontal, float vertical)
    {
        _Rb.velocity = new Vector3(_Rb.velocity.x, vertical * _SpeedBoost, horizontal * _SpeedBoost);
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
        if (_Grounded && _HasControl)
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

    void CheckPlan()
    {
        if (!_AnimChangementPlan)
        {
            if (Input.GetKeyDown(KeyCode.E) && _ChangementPlanTime <= 0 && _HasControl)
            {
                if (_ArrierePlan) //On passe au premier plan
                {
                    _ChangementPlanTime = ChangementPlanDuration;
                    _Grounded = false;
                    _Anim.SetBool("Back", false);
                    _Rb.useGravity = true;
                }
                else //on passe à l'arriere plan
                {
                    _ChangementPlanTime = ChangementPlanDuration;
                    _Grounded = false;
                    _Anim.SetBool("Back", true);
                    _Rb.useGravity = false;
                }
                _ArrierePlan = !_ArrierePlan;
                _AnimChangementPlan = true;
                _HasControl = false;
            }
        } else
        {
            _Rb.velocity = new Vector3(0, 0, 0);

            if( _ChangementPlanTime > 0)
            {
                float arrierePlanDistance = 1.5f;
                if(_ArrierePlan) //Va vers l'arriere plan
                {
                    _Rb.MovePosition(_Rb.position + new Vector3(-arrierePlanDistance * Time.deltaTime, 0, 0));
                } else //Va vers le premier plan
                {
                    _Rb.MovePosition(_Rb.position + new Vector3(arrierePlanDistance * Time.deltaTime, 0, 0));
                }
                _ChangementPlanTime -= Time.deltaTime;
            } else
            {
                _HasControl = true;
                _AnimChangementPlan = false;
                _Grounded = true;//hum
                _ChangementPlanTime = 0;
            }
        }
    }

    // Gere la glissade sur les murs
    void CheckGlide()
    {
        if (_ArrierePlan) return;

        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * 1.0f, Color.green);

        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.left));
        if (Physics.Raycast(ray, out hit, 1.0f, WhatIsWall) && Input.GetKey("left"))
        {
            _Rb.velocity = new Vector3(_Rb.velocity.x, Math.Max(_Rb.velocity.y, -1), _Rb.velocity.z);
            //Debug.Log("Did hit left : " + hit.collider.gameObject.layer);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * 1.0f, Color.green);
        }
        else
        {
            ray = new Ray(transform.position, transform.TransformDirection(Vector3.right));
            if (Physics.Raycast(ray, out hit, 1.0f, WhatIsWall) && Input.GetKey("right"))
            {
                _Rb.velocity = new Vector3(_Rb.velocity.x, Math.Max(_Rb.velocity.y, -1), _Rb.velocity.z);
                //Debug.Log("Did hit right : " + hit.collider.gameObject.layer);
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.green);
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

    public void Kill()
    {
        //Anim
        //Text Canvas
        _HasControl = false;

    }
}
