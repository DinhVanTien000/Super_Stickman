using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance = null;

    [SerializeField] private float speed;
    [SerializeField] private float speedAttack;
    [SerializeField] private float superSpeed;
    [SerializeField] private float speedHit;

    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private Joystick joystick;
    [SerializeField] private GameObject effectSuperSpeed, effectDieGround, effectSkill1, effectSkill2, _effectSkill2, effectSkill3
                                        , effectHitNomal, effectHitSkill1, effectHitSkill2, effectHitSkill3;

    [HideInInspector] public Transform _player;
    [HideInInspector] public bool NotMove;
    [HideInInspector] public bool NotMoveTop, NotMoveBelow, NotMoveRight, NotMoveleft;
    [HideInInspector] public bool superFast;
    [HideInInspector] public bool touchEnemy;
    [HideInInspector] public bool blownAway;

    [SerializeField] private Transform enemyFocus;
    private Transform firePoint;
    private Transform _transform;
    private GameObject tam;

    private float scaleX, scaleY;
    private string currentAnimaton;

    private float moveH, moveV;
    public bool attacking;
    private bool hitting;
    [SerializeField] private bool activateButton = true;
    private Vector2 moveDirection;

    public int noOfClick = 0, noOfHit = 0;
    private float lastClickedTime = 0, lastHitTime = 0;
    public float maxComboDelay = 0.9f;
    public float maxComboHitDelay = 0.9f;


    public void BtRestart()
    {
        SceneManager.LoadScene(0);
    }

    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        _transform = _player = transform;
        scaleX = _transform.localScale.x;
        scaleY = _transform.localScale.y;
        firePoint = _transform.GetChild(0).transform;
    }
    private void FixedUpdate()
    {
        MovePlayer();
        MovePlayerWhenAttack();
        RotationPlayer();
        UpdateAnimation();
    }
    private void MovePlayer()
    {
        if (!NotMove)
        {
            moveH = joystick.Horizontal;
            moveV = joystick.Vertical;

            //if(Input.GetKey(KeyCode.A))
            //{
            //    moveH = -1;
            //}
            //else if (Input.GetKey(KeyCode.D))
            //{
            //    moveH = 1;
            //}
            //else
            //{
            //    moveH = 0;
            //}

            //if (Input.GetKey(KeyCode.W))
            //{
            //    moveV = 1;
            //}
            //else if (Input.GetKey(KeyCode.S))
            //{
            //    moveV = -1;
            //}
            //else
            //{
            //    moveV = 0;
            //}


            if (NotMoveTop && moveV > 0)
            {
                moveV = 0;
            }
            else if (NotMoveBelow && moveV < 0)
            {
                moveV = 0;
            }

            if (NotMoveleft && moveH < 0)
            {
                moveH = 0;
            }
            else if (NotMoveRight && moveH > 0)
            {
                moveH = 0;
            }

            moveDirection = new Vector2(moveH, moveV);
            rb.velocity = moveDirection * speed;
        }
    }
    private void MovePlayerWhenAttack()
    {
        if (attacking && !touchEnemy)
        {
            Vector3 directionAttack = (enemyFocus.position - _transform.position).normalized;
            rb.velocity = directionAttack * speedAttack;
        }
    }
    public void StopMoveAttack()
    {
        touchEnemy = true;
        rb.velocity = new Vector2(0, 0);
    }
    private void RotationPlayer()
    {
        if(moveH == 0 && moveV == 0)
        {
            if (_transform.position.x > enemyFocus.transform.position.x)
            {
                SetScaleY(scaleY);

                Vector3 direction = _transform.position - enemyFocus.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                rb.rotation = angle;
            }
            else if (_transform.position.x < enemyFocus.transform.position.x)
            {
                SetScaleY(-scaleY);

                Vector3 direction = enemyFocus.position - _transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                rb.rotation = angle;
            }
        }


        if (_transform.position.x > enemyFocus.transform.position.x)
        {
            SetScaleX(scaleX);
        }
        else if (_transform.position.x < enemyFocus.transform.position.x)
        {
            SetScaleX(-scaleX);
        }
    }
    private float GetScaleY()
    {
        return _transform.localScale.y;
    }
    private void SetScaleY(float y)
    {
        _transform.localScale = new Vector2(scaleX, -y);
    }
    private void SetScaleX(float x)
    {
        _transform.localScale = new Vector2(-x, scaleY);
    }
    public void BtFightNormal()
    {
        if(activateButton)
        {
            if (Time.time - lastClickedTime > maxComboDelay)
            {
                noOfClick = 0;
            }

            SetNotMove();

            lastClickedTime = Time.time;
            noOfClick++;
            if (noOfClick == 1)
            {
                ChangeAnimationState(_AnimationState.Attack1);
            }

            noOfClick = Mathf.Clamp(noOfClick, 0, 4);
        }
    }
    public void Return1()
    {
        if (noOfClick >= 2)
        {
            attacking = true;
            ChangeAnimationState(_AnimationState.Attack2);
        }
        else
        {
            ChangeAnimationState(_AnimationState.Idle1);
            noOfClick = 0;
            NotMove = false;
            attacking = false;
        }
    }
    public void Return2()
    {
        if (noOfClick >= 3)
        {
            ChangeAnimationState(_AnimationState.Attack3);
        }
        else
        {
            ChangeAnimationState(_AnimationState.Idle1);
            noOfClick = 0;
            NotMove = false;
            attacking = false;
        }
    }
    //public void Return3()
    //{
    //    if (noOfClick >= 4)
    //    {
    //        ChangeAnimationState(_AnimationState.Attack4);
    //    }
    //    else
    //    {
    //        ChangeAnimationState(_AnimationState.Idle1);
    //        noOfClick = 0;
    //        NotMove = false;
    //        attacking = false;
    //    }
    //}
    public void Return4()
    {
        if (noOfClick >= 4)
        {
            ChangeAnimationState(_AnimationState.Attack5);
        }
        else
        {
            ChangeAnimationState(_AnimationState.Idle1);
            noOfClick = 0;
            NotMove = false;
            attacking = false;
        }
    }
    public void Return5()
    {
        var enemy = EnemyControoller.instance;
        if (enemy.noOfHit >= 4)
        {
            Invoke(nameof(DelayIdle), 0.7f);
            attacking = false;
            rb.velocity = Vector3.zero;

            enemy.BlownAway(0.8f);
        }
        else
        {
            DelayIdle();
        }
    }
    public void DelayIdle()
    {
        attacking = false;
        rb.velocity = Vector3.zero;
        noOfClick = 0;
        NotMove = false;
        ChangeAnimationState(_AnimationState.Idle1);
    }
    public void BtDefenseDown()
    {
        if (activateButton)
        {
            SetNotMove();
            ChangeAnimationState(_AnimationState.Defense);
        }
    }
    public void BtDefenseUp()
    {
        NotMove = false;
        ChangeAnimationState(_AnimationState.Idle1);
    }
    public void BtSkill1()
    {
        if (activateButton)
        {
            SetNotMove();
            ChangeAnimationState(_AnimationState.Skill1);
        }
    }
    public void ReturnSkill1()
    {
        NotMove = false;
        ChangeAnimationState(_AnimationState.Idle1);
    }
    public void BtSkill2()
    {
        if (activateButton)
        {
            activateButton = false;
            EnemyControoller.instance.LockPlayer();
            SetNotMove();
            ChangeAnimationState(_AnimationState.Skill2_start);
        }
    }
    public void EndSkill2()
    {
        ChangeAnimationState(_AnimationState.Skill2);
    }
    public void ReturnSkill2()
    {
        activateButton = true;
        NotMove = false;
        ChangeAnimationState(_AnimationState.Idle1);
    }
    public void BtSkill3()
    {
        if (activateButton)
        {
            activateButton = false;
            EnemyControoller.instance.LockPlayer();
            SetNotMove();
            ChangeAnimationState(_AnimationState.Skill3_start);
        }
    }
    public void EndSkill3()
    {
        ChangeAnimationState(_AnimationState.Skill3);
    }
    public void ReturnSkill3()
    {
        activateButton = true;
        NotMove = false;
        ChangeAnimationState(_AnimationState.Idle1);
    }
    public void BtSuperSpeed()
    {
        if (activateButton)
        {
            superFast = true;
            if (!NotMove)
            {
                SetNotMove();
            }
            if (attacking)
            {
                attacking = false;
            }

            ChangeAnimationState(_AnimationState.MoveFont);

            Vector3 directionSuperSpeed = (enemyFocus.position - _transform.position).normalized;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(directionSuperSpeed * superSpeed, ForceMode2D.Impulse);

            CreateEffectSuperSpeed();
            Invoke(nameof(EndSuperSpeed), 0.2f);
        } 
    }
    private void EndSuperSpeed()
    {
        Destroy(tam);
        rb.bodyType = RigidbodyType2D.Kinematic;

        if (NotMove)
        {
            rb.velocity = Vector3.zero;
            NotMove = false;
            ChangeAnimationState(_AnimationState.Idle1);
        }
    }
    public void SuperSpeedPlus_H()
    {
        superFast = false;
        Vector2 Vt = new Vector2(-rb.velocity.x, rb.velocity.y).normalized;
        rb.velocity = Vt * superSpeed;
    }
    public void SuperSpeedPlus_V()
    {
        superFast = false;
        Vector2 Vt = new Vector2(rb.velocity.x, -rb.velocity.y).normalized;
        rb.velocity = Vt * superSpeed;
    }
    public void BtTranform()
    {
        if (activateButton)
        {
            SetNotMove();
            ChangeAnimationState(_AnimationState.Tranform);
        } 
    }
    public void ReturnTranform()
    {
        if (activateButton)
        {
            NotMove = false;
            ChangeAnimationState(_AnimationState.Idle1);
        } 
    }
    public void BtBuffDown()
    {
        if (activateButton)
        {
            SetNotMove();
            ChangeAnimationState(_AnimationState.Buff);
        }
    }
    public void BtBuffUP()
    {
        NotMove = false;
        ChangeAnimationState(_AnimationState.Idle1);
    }
    public void ActionHit1()
    {
        SetNotMove();

        if (Time.time - lastHitTime > maxComboHitDelay)
        {
            noOfHit = 0;
        }

        lastHitTime = Time.time;
        noOfHit++;
        hitting = true;

        if (noOfHit == 1)
        {
            ChangeAnimationState(_AnimationState.Hit1);
        }

        noOfHit = Mathf.Clamp(noOfHit, 0, 4);
    }
    public void ReturnHit1()
    {
        if (noOfHit >= 2)
        {
            ChangeAnimationState(_AnimationState.Hit2);
        }
        else
        {
            ChangeAnimationState(_AnimationState.Idle1);
            noOfHit = 0;
            hitting = false;
            NotMove = false;
        }
    }
    public void ReturnHit2()
    {
        if (noOfHit >= 3)
        {
            ChangeAnimationState(_AnimationState.Hit3);
        }
        else
        {
            ChangeAnimationState(_AnimationState.Idle1);
            noOfHit = 0;
            hitting = false;
            NotMove = false;
        }
    }
    public void ReturnHit3()
    {
        if (noOfHit >= 4)
        {
            ChangeAnimationState(_AnimationState.Hit4);
        }
        else
        {
            ChangeAnimationState(_AnimationState.Idle1);
            noOfHit = 0;
            hitting = false;
            NotMove = false;
        }
    }
    public void ReturnHit4()
    {
        noOfHit = 0;
        NotMove = false;
        if (!blownAway)
        {
            ChangeAnimationState(_AnimationState.Idle1);
        }
    }
    public void Fall()
    {
        activateButton = false;
        SetNotMove();
        if (attacking)
        {
            attacking = false;
        }
        ChangeAnimationState(_AnimationState.Fall);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    public void ActionWin()
    {
        SetNotMove();
        attacking = false;
        activateButton = false;
        ChangeAnimationState(_AnimationState.Win);
    }
    private void CreateEffectSuperSpeed()
    {
        tam = Instantiate(effectSuperSpeed, _transform.position, _transform.rotation, _transform);
    }
    public void CreateEffectSkill1()
    {
        GameObject skill1 = Instantiate(effectSkill1, firePoint.position, _transform.rotation);
        skill1.GetComponent<Skill1Controller>().Fire(enemyFocus);
    }
    public void CreateEffectSkill2()
    {
        if(_transform.localScale.x > 0)
        {
            GameObject skill2 = Instantiate(effectSkill2, firePoint.position, _transform.rotation);
            skill2.GetComponent<Skill2Controller>().OnColllier(false);
        }
        else
        {
            GameObject skill2 = Instantiate(_effectSkill2, firePoint.position, _transform.rotation);
            skill2.GetComponent<Skill2Controller>().OnColllier(false);
        }
    }
    private void CreateEffectSkill3()
    {
        tam = Instantiate(effectSkill3, firePoint.position, _transform.rotation);
    }
    public void MoveSkill3()
    {
        tam.GetComponent<Skill3Controller>().Fire(enemyFocus);
    }
    public void DelayBlownAway(float timeDelay, float timeEnd)
    {
        StartCoroutine(waitBlownAway(timeDelay, timeEnd));
    }
    IEnumerator waitBlownAway(float timeDelay, float timeEnd)
    {
        yield return new WaitForSeconds(timeDelay);
        BlownAway(timeEnd);
        ChangeAnimationState(_AnimationState.Hit2);
    }
    public void BlownAway(float timeEnd)
    {
        SetNotMove();
        blownAway = true;
        Invoke(nameof(EndBlowAway), timeEnd);
        if (NotMoveTop || NotMoveBelow || NotMoveleft || NotMoveRight)
        {
            Vector3 directionHit = (enemyFocus.position - _transform.position).normalized;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(directionHit * speedHit, ForceMode2D.Impulse);
        }
        else
        {
            Vector3 directionHit = (_transform.position - enemyFocus.position).normalized;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(directionHit * speedHit, ForceMode2D.Impulse);
        }
    }
    private void EndBlowAway()
    {
        if (blownAway)
        {
            blownAway = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector3.zero;
            ChangeAnimationState(_AnimationState.Idle1);
        }
        noOfHit = 0;
        NotMove = false;
        UnlockPlayer();
    }
    public void BounceWall_H()
    {
        if (blownAway)
        {
            Vector2 Vt = new Vector2(-rb.velocity.x, rb.velocity.y).normalized;
            rb.velocity = Vt * speedHit;
        }
    }
    public void BounceWall_V()
    {
        Vector2 Vt = new Vector2(rb.velocity.x, -rb.velocity.y).normalized;
        rb.velocity = Vt * speedHit;
    }
    public void CreateEffectHitNomal()
    {
        Instantiate(effectHitNomal, _transform.position, _transform.rotation, _transform);
    }
    public void CreateEffectHitSkill1()
    {
        SetNotMove();
        Instantiate(effectHitSkill1, _transform.position, _transform.rotation, _transform);
        DelayBlownAway(0.01f, 0.15f);
    }
    public void CreateEffectHitSkill2()
    {
        CreateEHSkill2Plus();
        Invoke(nameof(CreateEHSkill2Plus), 0.2f);
        DelayBlownAway(0.2f, 0.8f);
    }
    private void CreateEHSkill2Plus()
    {
        Instantiate(effectHitSkill2, _transform.position, _transform.rotation, _transform);
    }
    public void CreateEffectHitSkill3()
    {

        Instantiate(effectHitSkill3, _transform.position, _transform.rotation, _transform);
        DelayBlownAway(0.3f, 0.8f);
    }
    public void LockPlayer()
    {
        SetNotMove();
        activateButton = false;
        rb.velocity = Vector3.zero;
    }
    public void UnlockPlayer()
    {
        if(NotMove)
        {
            NotMove = false;
        }
        if(!activateButton)
        {
            activateButton = true;
        }
    }
    public void SetNotMove()
    {
        moveH = moveV = 0;
        NotMove = true;
        rb.velocity = Vector3.zero;
    }
    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;
        anim.Play(newAnimation);
        currentAnimaton = newAnimation;
    }
    private void UpdateAnimation()
    {
        if(!NotMove)
        {
            if (_transform.position.x < enemyFocus.position.x)
            {
                if (moveH > 0)
                {
                    ChangeAnimationState(_AnimationState.MoveFont);
                }
                else if (moveH < 0)
                {
                    ChangeAnimationState(_AnimationState.MoveBack);
                }
                else
                {
                    ChangeAnimationState(_AnimationState.Idle1);
                }
            }
            else
            {
                if (moveH > 0)
                {
                    ChangeAnimationState(_AnimationState.MoveBack);
                }
                else if (moveH < 0)
                {
                    ChangeAnimationState(_AnimationState.MoveFont);
                }
                else
                {
                    ChangeAnimationState(_AnimationState.Idle1);
                }
            }
        }
    }
}

public class _AnimationState
{
    public const string Idle1 = "Idle1";
    public const string Idle2 = "Idle2";
    public const string MoveBack = "MoveBack";
    public const string MoveFont = "MoveFont";
    public const string Attack1 = "Attack1";
    public const string Attack2 = "Attack2";
    public const string Attack3 = "Attack3";
    public const string Attack4 = "Attack4";
    public const string Attack5 = "Attack5";
    public const string Hit1 = "Hit1";
    public const string Hit2 = "Hit2";
    public const string Hit3 = "Hit3";
    public const string Hit4 = "Hit4";
    public const string Skill1 = "Skill1";
    public const string Skill2_start = "Skill2_start";
    public const string Skill2 = "Skill2";
    public const string Skill3_start = "Skill3_start";
    public const string Skill3 = "Skill3";
    public const string Buff = "Buff";
    public const string Defense = "Defense";
    public const string Fall = "Fall";
    public const string Tranform = "Tranform";
    public const string Win = "Win";
}
