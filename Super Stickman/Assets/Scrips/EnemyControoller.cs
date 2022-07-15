using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControoller : MonoBehaviour
{
    public static EnemyControoller instance = null;

    [HideInInspector] public Transform _enemy;

    private Player playerInstance;

    [SerializeField] private float speed;
    [SerializeField] private float speedAttack;
    [SerializeField] private float speedHit;
    [SerializeField] private float superSpeed;
    [SerializeField] private float movementTime;

    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private GameObject effectSuperSpeed, effectDieGround, effectSkill1, effectSkill2, _effectSkill2, effectSkill3
                                        , effectHitNomal, effectHitSkill1, effectHitSkill2, effectHitSkill3;

    [HideInInspector] public bool moving;
    [HideInInspector] public bool NotMoveTop, NotMoveBelow, NotMoveRight, NotMoveleft;
    [HideInInspector] public bool touchPlayer;
    [HideInInspector] public bool hitting;
    [HideInInspector] public bool attacking;
    [HideInInspector] public bool buffing;
    [HideInInspector] public bool blownAway;
    [HideInInspector] public bool useSkill;
    [HideInInspector] public bool superFast;

    [SerializeField] private Transform playerFocus;

    private Transform firePoint;
    private Transform _transform;
    private GameObject tam;

    private float scaleX, scaleY;
    private string currentAnimaton;

    private float moveH, moveV;
    //private bool activateButton = true;
    private bool stopActionEndMoveHit;
    private Vector2 moveDirection;
    private float lerpValue;

    public int noOfClick = 0, noOfHit = 0;
    private float lastClickedTime = 0, lastHitTime = 0;
    public float maxComboDelay = 0.9f;
    public float maxComboHitDelay = 0.9f;

    private void Awake()
    {
        instance = this;

        playerInstance = Player.instance;

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
        MoveEnemy();
    }
    private void MoveEnemy()
    {
        if (moving)
        {
            lerpValue += Time.deltaTime / movementTime;
            transform.position = Vector3.Lerp(_transform.position, playerFocus.position, lerpValue);
        }
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
    public void FightNormal()
    {
        Player.instance.LockPlayer();
        attacking = true;
        ChangeAnimationState(_AnimationState.Attack1);
    }
    public void Return1()
    {
        //if(playerInstance.noOfHit >= 1)
        //{

        //}
        ChangeAnimationState(_AnimationState.Attack2);
    }
    public void Return2()
    {
        ChangeAnimationState(_AnimationState.Attack3);
    }
    public void Return4()
    {
        ChangeAnimationState(_AnimationState.Attack5);
    }
    public void Return5()
    {
        var player = Player.instance;
        if (player.noOfHit >= 4)
        {
            Invoke(nameof(DelayIdle), 0.9f);
            rb.velocity = Vector3.zero;
            attacking = false;
            player.BlownAway(1f);
        }
        else
        {
            FightNormal();
        }
    }
    public void DelayIdle()
    {
        rb.velocity = Vector3.zero;
        ChangeAnimationState(_AnimationState.Idle1);
    }
    public void BtDefenseDown()
    {
        ChangeAnimationState(_AnimationState.Defense);
    }
    public void BtDefenseUp()
    {
        ChangeAnimationState(_AnimationState.Idle1);
    }
    public void UseSkill1()
    {
        useSkill = true;
        ChangeAnimationState(_AnimationState.Skill1);
    }
    public void ReturnSkill1()
    {
        ChangeAnimationState(_AnimationState.Idle1);
        useSkill = false;
    }
    public void UseSkill2()
    {
        Player.instance.LockPlayer();
        useSkill = true;
        ChangeAnimationState(_AnimationState.Skill2_start);
    }
    public void EndSkill2()
    {
        ChangeAnimationState(_AnimationState.Skill2);
    }
    public void ReturnSkill2()
    {
        Destroy(tam);
        useSkill = false;
        ChangeAnimationState(_AnimationState.Idle1);
    }
    public void UseSkill3()
    {
        Player.instance.LockPlayer();
        ChangeAnimationState(_AnimationState.Skill3_start);
    }
    public void EndSkill3()
    {
        ChangeAnimationState(_AnimationState.Skill3);
    }
    public void MoveSkill3()
    {
        tam.GetComponent<Skill3Controller>().Fire(playerFocus);
    }
    public void ReturnSkill3()
    {
        ChangeAnimationState(_AnimationState.Idle1);
    }
    public void SuperSpeed()
    {
        superFast = true;

        ChangeAnimationState(_AnimationState.MoveFont);

        Vector3 directionSuperSpeed = (playerFocus.position - _transform.position).normalized;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(directionSuperSpeed * superSpeed, ForceMode2D.Impulse);

        CreateEffectSuperSpeed();
        Invoke(nameof(EndSuperSpeed), 0.2f);
    }
    public void MoveToPlayer()
    {
        moving = true;
        CreateEffectSuperSpeed();
        ChangeAnimationState(_AnimationState.MoveFont);
    }
    public void SetMove()
    {
        moving = false;
        ChangeAnimationState(_AnimationState.Idle1);
        Destroy(tam);
    }
    private void EndSuperSpeed()
    {
        Destroy(tam);
        rb.bodyType = RigidbodyType2D.Kinematic;
        superFast = false;
        rb.velocity = Vector3.zero;
        ChangeAnimationState(_AnimationState.Idle1);
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
        ChangeAnimationState(_AnimationState.Tranform);
    }
    public void ReturnTranform()
    {
        ChangeAnimationState(_AnimationState.Idle1);
    }
    public void Buff()
    {
        buffing = true;
        ChangeAnimationState(_AnimationState.Buff);
        var timeStop = Random.Range(2f, 3.5f);
        Invoke(nameof(StopBuff), timeStop);
    }
    public void StopBuff()
    {
        buffing = false;
        ChangeAnimationState(_AnimationState.Idle1);
    }
    public void ActionHit1()
    {
        if (Time.time - lastHitTime > maxComboHitDelay)
        {
            noOfHit = 0;
        }

        ResetStatusAttack();

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
        Destroy(tam);
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
        Destroy(tam);
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
        Destroy(tam);
        if (noOfHit >= 4)
        {
            ChangeAnimationState(_AnimationState.Hit4);
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
        Destroy(tam);
        noOfHit = 0;
        if (!blownAway)
        {
            hitting = false;
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
            Destroy(tam);
        }
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
    public void Fall()
    {
        ChangeAnimationState(_AnimationState.Fall);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    public void ActionWin()
    {
        ChangeAnimationState(_AnimationState.Win);
    }
    private void CreateEffectSuperSpeed()
    {
        tam = Instantiate(effectSuperSpeed, _transform.position, _transform.rotation, _transform);
    }
    public void CreateEffectHitNomal()
    {
        Instantiate(effectHitNomal, _transform.position, _transform.rotation, _transform);
    }
    public void CreateEffectHitSkill1()
    {
        hitting = true;
        ResetStatusAttack();
        Instantiate(effectHitSkill1, _transform.position, _transform.rotation, _transform);
        DelayBlownAway(0.01f, 0.15f);
    }
    public void CreateEffectHitSkill2()
    {
        hitting = true;
        ResetStatusAttack();
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
        hitting = true;
        ResetStatusAttack();
        Instantiate(effectHitSkill3, _transform.position, _transform.rotation, _transform);
        DelayBlownAway(0.3f, 0.8f);
    }
    public void CreateEffectSkill1()
    {
        tam = Instantiate(effectSkill1, firePoint.position, _transform.rotation);
        tam.GetComponent<Skill1Controller>().Fire(playerFocus);
    }
    public void CreateEffectSkill2()
    {
        if (_transform.localScale.x > 0)
        {
            tam = Instantiate(effectSkill2, firePoint.position, _transform.rotation);
            tam.GetComponent<Skill2Controller>().OnColllier(true);
        }
        else
        {
            tam = Instantiate(_effectSkill2, firePoint.position, _transform.rotation);
            tam.GetComponent<Skill2Controller>().OnColllier(true);
        }
    }
    private void CreateEffectSkill3()
    {
        tam = Instantiate(effectSkill3, firePoint.position, _transform.rotation);
    }
    public void LockPlayer()
    {
        hitting = true;
        rb.velocity = Vector3.zero;
    }
    public void UnlockPlayer()
    {
        if (hitting)
        {
            hitting = false;
        }
    }
    public void ResetStatusAttack()
    {
        if (useSkill)
        {
            useSkill = false;
        }
        if (buffing)
        {
            buffing = false;
        }
        if(attacking)
        {
            attacking = false;
        }
    }
    void ChangeAnimationState(string newAnimation)
    {

        if (currentAnimaton == newAnimation) return;
        anim.Play(newAnimation);
        currentAnimaton = newAnimation;
    }
    private void UpdateAnimation()
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
