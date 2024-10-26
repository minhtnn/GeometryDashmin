using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum Speeds { Slow, Normal, Fast, Faster, Fastest };
public enum Gamemodes { Cube, Ship, Ball, UFO, Wave, Robot, Spider };
public class PlayerMove : MonoBehaviour
{
    public Speeds CurrentSpeed;
    public Gamemodes CurrentGameMode;
    float[] SpeedValues = { 8.6f, 10.4f, 12.96f, 15.6f, 19.27f };
    float JumpPower = 21.6451f;
    private bool isDead = false;
    public Sprite CubeSprite;
    public Sprite ShipSprite;
    public Sprite BallSprite;
    public Sprite UFOSprite;
    public Sprite WaveSprite;
    public Sprite RobotSprite;
    public Sprite SpiderSprite;

    public Animator animator;
    private AudioManager audioManager;

    public LayerMask GroundMask;
    public LayerMask KillerMask;
    public float GroundCheckRadius;

    
    [System.NonSerialized] public int Gravity = 1;
    [System.NonSerialized] public bool clickProcessed = false;
    public Transform Sprite;

    Rigidbody2D rb;
    [SerializeField] public SpriteRenderer CubeSpriteRenderrer;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (CubeSpriteRenderrer == null)
        {
            Debug.LogError("No SpriteRenderer found on " + gameObject.name + " or its children.");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GamePlay();
    }

    public void GamePlay()
    {
        ChangeGameMode();
        if (isDead)
            return;
        animator.enabled = false;
        Vector3 newPosition = transform.position;
        newPosition.x += SpeedValues[(int)CurrentSpeed] * Time.deltaTime;
        transform.position = newPosition;
        Invoke(CurrentGameMode.ToString(), 0);

        // Nếu nhân vật chạm tường hoặc kẻ địch, gọi Coroutine xử lý cái chết
        if (TouchingWall() || TouchingKiller())
            StartCoroutine(HandleDeath()); // Gọi Coroutine để xử lý
    }

    IEnumerator HandleDeath()
    {
        isDead = true;
        animator.SetTrigger("isDeath");
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        audioManager.PlaySFX(audioManager.death, audioManager.SFXVolume * 10);
        audioManager.StopBackgroundMusic();
        yield return new WaitForSeconds(audioManager.death.length);
        ResetPlayer();
    }

    public bool OnGround()
    {
        return Physics2D.OverlapBox(Sprite.position + Vector3.down * Gravity * 0.5f,
            Vector2.right * 1.1f + Vector2.up * GroundCheckRadius, 0, GroundMask);
    }
    bool TouchingWall()
    {
        Vector2 wallCheckPosition = new Vector2(Sprite.position.x + 0.5f, Sprite.position.y);
        Vector2 boxSize = (Vector2.up * 0.7f) + (Vector2.right * GroundCheckRadius);

        return Physics2D.OverlapBox(wallCheckPosition, boxSize, 0, GroundMask);
    }
    bool TouchingKiller()
    {
        Vector2 wallCheckPosition = new Vector2(Sprite.position.x, Sprite.position.y);
        Vector2 boxSize = (Vector2.up) + (Vector2.right);

        // Sử dụng OverlapBox để kiểm tra va chạm
        Collider2D hitCollider = Physics2D.OverlapBox(wallCheckPosition, boxSize, 0, KillerMask);

        if (hitCollider != null && hitCollider.CompareTag("Killer"))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    void OnDrawGizmos()
    {
        Vector2 wallCheckPosition = new Vector2(Sprite.position.x, Sprite.position.y);
        Vector2 boxSize = (Vector2.up * 1.1f) + (Vector2.right * 1.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(wallCheckPosition, boxSize);
    }
    public void ChangeGameMode()
    {
        CubeSpriteRenderrer.transform.localScale = Vector3.one;
        switch (CurrentGameMode) 
        { 
            case Gamemodes.Cube:
                CubeSpriteRenderrer.sprite = CubeSprite;
                
                break;
            case Gamemodes.Ball:
                CubeSpriteRenderrer.sprite = BallSprite;
                break;
            case Gamemodes.Ship:
                CubeSpriteRenderrer.sprite = ShipSprite;
                
                break;
            case Gamemodes.Wave:
                CubeSpriteRenderrer.sprite = WaveSprite;
                
                break;
            case Gamemodes.Robot:
                CubeSpriteRenderrer.sprite = RobotSprite;
                
                break;
            case Gamemodes.UFO:
                CubeSpriteRenderrer.sprite = UFOSprite;
                
                break;
            case Gamemodes.Spider:
                CubeSpriteRenderrer.sprite = SpiderSprite;
                
                break;
        }
        // Optional: Adjust the scale based on the new sprite's size if needed
        AdjustSpriteScale(CubeSpriteRenderrer);
        if (isDead) animator.enabled = true;
    }

    void AdjustSpriteScale(SpriteRenderer renderer)
    {
        if (renderer.sprite == null) return;

        // Lấy tỷ lệ của sprite mới
        Vector2 spriteSize = renderer.sprite.bounds.size;

        // Thiết lập tỷ lệ chuẩn để sprite được hiển thị đúng kích thước
        Vector3 newScale = new Vector3(spriteSize.x, spriteSize.y, 1);

        // Cập nhật lại scale cho transform của SpriteRenderer
        renderer.transform.localScale = newScale;
    }

    void Cube()
    {
        Generic.CreateGamemode(rb, this, true, JumpPower, 9.24f, true, false, 409.1f);
    }
    void Ship()
    {
        rb.gravityScale = 3.40484309302f * (Input.GetKey(KeyCode.Space) ? 1 : -1) * -Gravity;
        Generic.LimitYVelocity(9.95f, rb);
        Sprite.rotation = Quaternion.Euler(0, 0, rb.velocity.y * 2);
    }
    void Ball()
    {
        Generic.CreateGamemode(rb, this, true, 0, 6.2f, false, true);
    }
    void UFO()
    {
        Generic.CreateGamemode(rb, this, false, 10.841f, 4.1483f, false, false, 0, 10.841f);
    }
    void Wave()
    {
        rb.gravityScale = 0;
        rb.velocity = new Vector2(0, SpeedValues[(int)CurrentSpeed] * (Input.GetKey(KeyCode.Space) ? 1 : -1) * Gravity);
    }

    float robotXstart = -100;
    bool onGroundProcessed;
    bool gravityFlipped;
    void Robot()
    {
        if (!Input.GetKey(KeyCode.Space))
        if (!Input.GetKey(KeyCode.Space))
            clickProcessed = false;

        if (OnGround() && !clickProcessed && Input.GetKey(KeyCode.Space))
        {
            gravityFlipped = false;
            clickProcessed = true;
            robotXstart = transform.position.x;
            onGroundProcessed = true;
        }

        if (Mathf.Abs(robotXstart - transform.position.x) <= 3)
        {
            if (Input.GetKey(KeyCode.Space) && onGroundProcessed && !gravityFlipped)
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.up * 10.4f * Gravity;
                return;
            }
        }
        else if (Input.GetKey(KeyCode.Space))
            onGroundProcessed = false;

        rb.gravityScale = 8.62f * Gravity;
        Generic.LimitYVelocity(23.66f, rb);
    }
    void Spider()
    {
        Generic.CreateGamemode(rb, this, true, 2388.29f, 6.2f, false, true, 0, 2388.29f);
    }

    public void ResetPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        audioManager.PlayBackgroundMusic();
        Time.timeScale = 1.0f;
    }
    public void ChangeThroughPortal(Gamemodes Gamemode, Speeds Speed, int gravity, int State)
    {
        switch (State)
        {
            case 0:
                CurrentSpeed = Speed;
                break;
            case 1:
                CurrentGameMode = Gamemode;
                Sprite.rotation = Quaternion.identity;
                break;
            case 2:
                Gravity = gravity;
                rb.gravityScale = Mathf.Abs(rb.gravityScale) * gravity;
                gravityFlipped = true;
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PortalScript portal = collision.gameObject.GetComponent<PortalScript>();

        if (portal)
            portal.initiatePortal(this);
    }
}
