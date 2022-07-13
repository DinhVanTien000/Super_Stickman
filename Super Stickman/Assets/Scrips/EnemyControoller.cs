using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControoller : MonoBehaviour
{
    public static EnemyControoller instance = null;

    [HideInInspector] public Transform _enemy;

    [SerializeField] private float speed;
    [SerializeField] private float speedAttack;
    [SerializeField] private float speedHit;
    [SerializeField] private float superSpeed;

    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private GameObject effectSuperSpeed, effectDieGround, effectSkill1, effectSkill2, effectSkill3
                                        , effectHitNomal, effectHitSkill1, effectHitSkill2, effectHitSkill3;

    [HideInInspector] public bool NotMove;
    [HideInInspector] public bool NotMoveTop, NotMoveBelow, NotMoveRight, NotMoveleft;
    [HideInInspector] public bool superFast;
    [HideInInspector] public bool hitting;
    [HideInInspector] public bool blownAway;

    [SerializeField] private Transform playerFocus;

    private Transform firePoint;
    private Transform _transform;
    private GameObject tam;

    private float scaleX, scaleY;
    private string currentAnimaton;

    private float moveH, moveV;
    private bool attacking;
    private bool activateButton = true;
    private bool stopActionEndMoveHit;
    private Vector2 moveDirection;

    public int noOfClick = 0, noOfHit = 0;
    private float lastClickedTime = 0, lastHitTime = 0;
    public float maxComboDelay = 0.9f;
    public float maxComboHitDelay = 0.9f;

    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        _transform = _enemy = transform;
        scaleX = _transform.localScale.x;
        scaleY = _transform.localScale.y;
        firePoint = _transform.GetChild(0).transform;
    }

    private void FixedUpdate()
    {
        

        RotationEnemy();
        //MovePlayer();
        //MovePlayerWhenAttack();
        //UpdateAnimation();
    }
    private void MovePlayer()
    {
        //if (!NotMove)
        //{
        //    moveH = joystick.Horizontal;
        //    moveV = joystick.Vertical;

        //    if (NotMoveTop && moveV > 0)
        //    {
        //        moveV = 0;
        //    }
        //    else if (NotMoveBelow && moveV < 0)
        //    {
        //        moveV = 0;
        //    }

        //    if (NotMoveleft && moveH < 0)
        //    {
        //        moveH = 0;
        //    }
        //    else if (NotMoveRight && moveH > 0)
        //    {
        //        moveH = 0;
        //    }

        //    moveDirection = new Vector2(moveH, moveV);
        //    rb.velocity = moveDirection * speed;
        //}
    }
    private void MovePlayerWhenAttack()
    {
        if (attacking)
        {
            Vector3 directionAttack = (playerFocus.position - _transform.position).normalized;
            rb.velocity = directionAttack * speedAttack;
        }
    }
    private void RotationEnemy()
    {
        Vector3 direction = playerFocus.position - _transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;

        if (_transform.position.x > playerFocus.transform.position.x && GetScaleY() > 0)
        {
            SetScale(scaleY);
        }
        else if (_transform.position.x < playerFocus.transform.position.x && GetScaleY() < 0)
        {
            SetScale(-scaleY);
        }
    }
    private float GetScaleY()
    {
        return _transform.localScale.y;
    }
    private void SetScale(float y)
    {
        _transform.localScale = new Vector2(scaleX, -y);
    }
    public void BtFightNormal()
    {
        if (activateButton)
        {
            NotMove = true;
            attacking = true;

            lastClickedTime = Time.time;
            noOfClick++;
            if (noOfClick == 1)
            {
                ChangeAnimationState(_AnimationState.Attack1);
            }

            noOfClick = Mathf.Clamp(noOfClick, 0, 5);
        }
    }
    public void Return1()
    {
        if (noOfClick >= 2)
        {
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
    public void Return3()
    {
        if (noOfClick >= 4)
        {
            ChangeAnimationState(_AnimationState.Attack4);
        }
        else
        {
            ChangeAnimationState(_AnimationState.Idle1);
            noOfClick = 0;
            NotMove = false;
            attacking = false;
        }
    }
    public void Return4()
    {
        if (noOfClick >= 5)
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
        ChangeAnimationState(_AnimationState.Idle1);
        noOfClick = 0;
        NotMove = false;
        attacking = false;
    }
    public void BtDefenseDown()
    {
        if (activateButton)
        {
            NotMove = true;
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
            NotMove = true;
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
            NotMove = true;
            ChangeAnimationState(_AnimationState.Skill2_start);
        }
        //tao ra kamejoko
    }
    public void EndSkill2()
    {
        ChangeAnimationState(_AnimationState.Skill2);
    }
    public void ReturnSkill2()
    {
        NotMove = false;
        ChangeAnimationState(_AnimationState.Idle1);
    }
    public void BtSkill3()
    {
        if (activateButton)
        {
            NotMove = true;
            ChangeAnimationState(_AnimationState.Skill3_start);
        }
    }
    public void EndSkill3()
    {
        ChangeAnimationState(_AnimationState.Skill3);
    }
    public void ReturnSkill3()
    {
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
                NotMove = true;
            }
            if (attacking)
            {
                attacking = false;
            }
            Vector3 directionSuperSpeed = (playerFocus.position - _transform.position).normalized;
            rb.velocity = directionSuperSpeed * superSpeed;
            CreateEffectSuperSpeed();
            Invoke(nameof(EndSuperSpeed), 0.15f);
        }
    }
    private void EndSuperSpeed()
    {
        if (NotMove)
        {
            rb.velocity = Vector3.zero;
            NotMove = false;
            superFast = false;
        }
    }
    public void SuperSpeedPlus_H()
    {
        superFast = false;
        Vector2 Vt = new Vector2(-rb.velocity.x, rb.velocity.y).normalized;
        rb.velocity = Vt * superSpeed / 1.5f;
    }
    public void SuperSpeedPlus_V()
    {
        superFast = false;
        Vector2 Vt = new Vector2(rb.velocity.x, -rb.velocity.y).normalized;
        rb.velocity = Vt * superSpeed / 1.5f;
    }
    public void BtTranform()
    {
        if (activateButton)
        {
            NotMove = true;
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
            NotMove = true;
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
        }
    }
    public void ReturnHit3()
    {
        if (noOfHit >= 4)
        {
            ChangeAnimationState(_AnimationState.Hit4);
            //blownAway = true;
            //BlownAway();
            //Invoke(nameof(EndBlowAway), 0.8f);
        }
        else
        {
            ChangeAnimationState(_AnimationState.Idle1);
            noOfHit = 0;
            hitting = false;
        }
    }
    public void ReturnHit4()
    {
        noOfHit = 0;
        if(!blownAway)
        {
            ChangeAnimationState(_AnimationState.Idle1);
        }
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
        blownAway = true;
        Invoke(nameof(EndBlowAway), timeEnd);
        if(NotMoveTop || NotMoveBelow || NotMoveleft || NotMoveRight)
        {
            Vector3 directionHit = (playerFocus.position - _transform.position).normalized;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(directionHit * speedHit, ForceMode2D.Impulse);
        }
        else
        {
            Vector3 directionHit = (_transform.position - playerFocus.position).normalized;
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
    public void Fall()
    {
        activateButton = false;
        NotMove = true;
        if (attacking)
        {
            attacking = false;
        }
        ChangeAnimationState(_AnimationState.Fall);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    public void ActionWin()
    {
        NotMove = true;
        attacking = false;
        activateButton = false;
        ChangeAnimationState(_AnimationState.Win);
    }
    private void CreateEffectSuperSpeed()
    {
        Instantiate(effectSuperSpeed, _transform.position, _transform.rotation, _transform);
    }
    public void CreateEffectHitNomal()
    {
        Instantiate(effectHitNomal, _transform.position, _transform.rotation, _transform);
    }
    public void CreateEffectHitSkill1()
    {
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
    public void CreateEffectSkill1()
    {
        GameObject skill1 = Instantiate(effectSkill1, firePoint.position, _transform.rotation);
        skill1.GetComponent<Skill1Controller>().Fire(playerFocus);
    }
    public void CreateEffectSkill2()
    {
        GameObject skill2 = Instantiate(effectSkill2, firePoint.position, _transform.rotation);
        skill2.GetComponent<Skill2Controller>().OnColllier(true);
    }
    private void CreateEffectSkill3()
    {
        tam = Instantiate(effectSkill3, firePoint.position, _transform.rotation);
    }
    public void MoveSkill3()
    {
        tam.GetComponent<Skill3Controller>().Fire(playerFocus);
    }
    void ChangeAnimationState(string newAnimation)
    {

        if (currentAnimaton == newAnimation) return;
        anim.Play(newAnimation);
        currentAnimaton = newAnimation;
    }
    private void UpdateAnimation()
    {
        if (!NotMove)
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
    }
}
