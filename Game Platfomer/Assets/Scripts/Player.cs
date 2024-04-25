using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("----------Infor----------")]
    public bool facingRight;

    [Header("----------Move infor----------")]
    public float moveSpeed = 12f;

    [Header("----------Jump infor----------")]
    public float jumpFoce = 20f;

    [Header("----------Dash infor----------")]
    [SerializeField] float dashDelay = 2f;
    float _dashDelay = 0;
    public float speedDash = 25f;
    public float dashDuration = 0.2f;

    [Header("----------Wall infor----------")]
    public float wallDelay = 0.2f;

    [Header("----------Attack infor----------")]
    public bool isAttack;
    public Vector2[] speedAttack;

    [Header("----------Collider----------")]
    public Transform groundCheck;
    public float groundCheckDistance;
    public LayerMask groundCheckLm;

    public Transform wallCheck;
    public float wallCheckDistance;
    public Transform wallCheck2;
    public float wallCheckDistance2;
    public LayerMask wallCheckLm;

    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallState wallState { get; private set; }
    public PlayerAttackState attackState { get; private set; }

    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallState = new PlayerWallState(this, stateMachine, "Wall");
        attackState = new PlayerAttackState(this, stateMachine, "Attack");
    }
    // Start is called before the first frame update
    void Start()
    {
        stateMachine.Initalize(idleState);
        facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.currentState.Update();
        CheckDashInput();
        CheckAttack();
    }
    void CheckAttack()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isAttack && !IsWallCheck())
        {
            stateMachine.ChangeState(attackState);
        }
    }
    public void AnimationTrigger()
    {
        isAttack = false;
    }
    void CheckDashInput()
    {
        _dashDelay -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.S) && _dashDelay <= 0)
        {
            _dashDelay = dashDelay;
            stateMachine.ChangeState(dashState);
        }
    }
    public void FlipX()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    public void FlipController(float _x)
    {
        if ((_x < 0 && facingRight) || (_x > 0 && !facingRight))
            FlipX();
    }
    public void SetVelocity(float x, float y)
    {
        rb.velocity = new Vector2(x, y);
        FlipController(rb.velocity.x);
    }
    #region Colision
    public bool IsGroundCheck() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundCheckLm);
    public bool IsWallCheck() => Physics2D.Raycast(wallCheck.position, Vector2.right*(facingRight ? 1 : -1), wallCheckDistance, wallCheckLm);
    public bool IsWallCheck2() => Physics2D.Raycast(wallCheck2.position, Vector2.right * (facingRight ? -1 : 1), wallCheckDistance2, wallCheckLm);
    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position - Vector3.up * groundCheckDistance);
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance);
        Gizmos.DrawLine(wallCheck2.position, wallCheck2.position + Vector3.right * wallCheckDistance2 * (facingRight ? -1 : 1));
    }
    #endregion
}
