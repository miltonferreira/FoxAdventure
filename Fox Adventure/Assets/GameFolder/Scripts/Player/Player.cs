using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb2d;
    public Animator playerAnim;
    [SerializeField] private LayerMask groundMask;
    [HideInInspector] public bool isPlayerStopped;
    
    private float moveInput;
    public float moveSpeed;

    public float jumpForce;
    private int moreJumps;

    [SerializeField] private bool onGround;
    private bool wasOnGround;
    private bool isJump;

    //ground circle collider --------------
    private Collider2D[] colliders_1, colliders_2;
    private float groundCheckRadius = 0.036f * 2f;   // tamanho do circulo no pé do player
    public Transform[] groundCheck;             // posicao do obj que vai checa colisao com chao

    // slopes system --------------------------------------------
    public PhysicsMaterial2D noFriction, friction;
    public float slopeCheckDistance;
    private float slopeAngle;
    private bool onSlope;

    // slide ------------
    [Header("Slide System-----------------")]
    public Transform wallCheck; // confere colisao com a parede
    private bool isColliderWall;
    public float wallCheckDistance;
    [HideInInspector]public bool isSliding;
    [Header("velocidade queda parede")]
    public float wallSlideSpeed;
    [Header("velocidade pulo parede")]
    public float wallJumpForce;

    private bool onSliding;     // indica que está pulando fora da parede

    private bool isLandGround = true;   // Usando sfx player tocar chão

    private float isLeavingWall; // pega o lado que o player pulou fora da parede

    [Header("Prefab Bullet -----------")]
    public GameObject bullet;
    public float speedBullet = 20f;

    [Header("Velocidade Dash -----------")]
    public float dashTimeLimit;     // tempo limite que pode ficar em dash
    public float dashSpeed;
    private float dashTime = 1f;
    public GhostFX ghostFX;
    
    // Start is called before the first frame update
    void Start(){
        
        rb2d = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update(){
        InputSystem();
        CreateBullet();
        checkGround();
        ghostPlatform();
        Animations();
        Slopes();
        Slide();
        Dash();

        if(onGround){
            resetJumpWall();
        }
    }

    private void FixedUpdate() {
        if(!onSliding)
        Move();
    }

    private void Move(){

        if(dashTime < dashTimeLimit){
            return;
        }

        if(onSlope && !isJump){
            //movimento com slope/rampa
            rb2d.gravityScale = 20f;
            if(rb2d.velocity.y < -2f){
                rb2d.velocity = new Vector2(moveInput * moveSpeed, -9f);
            }else{
                rb2d.velocity = new Vector2(moveInput * moveSpeed, rb2d.velocity.y);
            }
            
        }else{
            //movimento padrao
            rb2d.gravityScale = 3f;
            rb2d.velocity = new Vector2(moveInput * moveSpeed, rb2d.velocity.y);
        }
        
    }

    private void InputSystem(){

        if(isPlayerStopped || Time.timeScale == 0f){
            moveInput = 0f;
            return;
        }
        
        moveInput = Input.GetAxisRaw("Horizontal");

        // vira player na direção que está andando
        if(moveInput != 0f && !onSliding){
            transform.localScale = new Vector3(moveInput, 1f, 1f);
        }

        // pulo normal
        if(Input.GetKeyDown(KeyCode.Space) && (onGround || (moreJumps < 1 && rb2d.velocity.y > 0))){
            moreJumps++;
            Jump();
        }

        // pulo na parede
        if(Input.GetKeyDown(KeyCode.Space) && isSliding && isLeavingWall != moveInput){
            moreJumps = 1000;
            rb2d.velocity = Vector2.zero;
            rb2d.velocity = new Vector2(wallJumpForce * -moveInput, wallJumpForce);
            onSliding = true;
            isLeavingWall = moveInput;
            StartCoroutine(jumpSlide());
        }

    }

    void CreateBullet(){
        if(Input.GetKeyDown(KeyCode.K) && GameController.instance.totalBullets > 0){
            GameObject b = Instantiate(bullet, wallCheck.position, Quaternion.identity);

            if(isSliding){
                b.GetComponent<Bullet>().xVelocity = -transform.localScale.x * speedBullet;
            }else{
                b.GetComponent<Bullet>().xVelocity = transform.localScale.x * speedBullet;
            }
            
            GameController.instance.totalBullets--;
            GameController.instance.UpdateTotalCherries();
        }
    }

    IEnumerator jumpSlide(){
        SFXController.Instance.SFX("PlayerJump", 0.7f);
        if(moveInput !=0){
            transform.localScale = new Vector3(-moveInput, 1f, 1f);
        }

        yield return new WaitForSeconds(0.3f);

        if(isLeavingWall == moveInput && !onGround && isColliderWall && onSliding){
            yield return new WaitForSeconds(0.3f);
        }else{
           resetJumpWall();
        }
    }

    void resetJumpWall(){
        isLeavingWall = -2f;
        onSliding = false;
    } 

    void checkGround(){
        colliders_1 = Physics2D.OverlapCircleAll(groundCheck[0].position, groundCheckRadius, groundMask);
        colliders_2 = Physics2D.OverlapCircleAll(groundCheck[1].position, groundCheckRadius, groundMask);

        if(onGround && !wasOnGround){isJump = false;};

        wasOnGround = onGround;

        if(colliders_1.Length>0 || colliders_2.Length>0){
            onGround = true;
            moreJumps = 0;
        }else{
            onGround = false;
        }
    }

    private void ghostPlatform(){
        if((colliders_1.Length>0 && colliders_1[0].gameObject.layer == 9) || 
            (colliders_2.Length>0 && colliders_2[0].gameObject.layer == 9)){
            if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)){
                    Physics2D.IgnoreLayerCollision(6,9, true);
            }else if(Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S) || onGround){
                Physics2D.IgnoreLayerCollision(6,9, false);
            }
        }
    }

    private void Jump(){
        isJump = true;
        rb2d.gravityScale = 3f;
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        SFXController.Instance.SFX("PlayerJump", 0.7f);
    }

    private void Slopes(){
        RaycastHit2D hitSlope = Physics2D.Raycast(transform.position, Vector2.down, slopeCheckDistance, groundMask);

        Debug.DrawRay(transform.position, Vector3.down * slopeCheckDistance, Color.red);

        if(hitSlope && !isJump){
            slopeAngle = Vector2.Angle(hitSlope.normal, Vector2.up);    // pega o angulo do slope

            //print(slopeAngle);

            onSlope = slopeAngle != 0;

            if(onSlope && moveInput == 0){
                rb2d.sharedMaterial = friction;
            }else{
                rb2d.sharedMaterial = noFriction;
            }

            if(!isLandGround){
                SFXController.Instance.SFX("LandGround", 0.6f);
                isLandGround = true;
            }
        }else{
            rb2d.sharedMaterial = noFriction;
            isLandGround = false;
        }
    }

    private void Slide(){
        isColliderWall = Physics2D.Raycast(wallCheck.position, wallCheck.TransformDirection(Vector3.right), wallCheckDistance, groundMask);
        //Debug.DrawRay(wallCheck.position, wallCheck.TransformDirection(Vector3.right) * wallCheckDistance, Color.red);
        if(isColliderWall && !onGround && rb2d.velocity.y < 0 && moveInput != 0){
            isSliding = true;
        }else{
            isSliding = false;
        }

        // faz slide na parede caindo
        if(isSliding && rb2d.velocity.y < -wallSlideSpeed){
            rb2d.velocity = new Vector2(rb2d.velocity.x, -wallSlideSpeed);
        }
    }

    private void Dash(){
        dashTime += Time.deltaTime;

        if(dashTime < dashTimeLimit && !isColliderWall){
            rb2d.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
            playerAnim.SetBool("isDash", true);
            ghostFX.ghostEffect();
        }else{
            playerAnim.SetBool("isDash", false);
        }

        if(dashTime < 1f){
            return;
        }

        if(Input.GetKeyDown(KeyCode.L) && dashTime > dashTimeLimit && !isColliderWall){
            dashTime = 0f;
            SFXController.Instance.SFX("Door", 0.6f);
        }
    }
    
    private void Animations(){
        playerAnim.SetFloat("SpeedX", Mathf.Abs(moveInput));
        playerAnim.SetFloat("SpeedY", rb2d.velocity.y);
        playerAnim.SetBool("onGround", onGround);
        playerAnim.SetBool("isSliding", isSliding);
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if(other.gameObject.layer == 9 && other.transform.position.y < transform.position.y){
            transform.parent = other.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.layer == 9){
            transform.parent = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
    }
    
    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheck[0].position, groundCheckRadius);
        Gizmos.DrawSphere(groundCheck[1].position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }

}
