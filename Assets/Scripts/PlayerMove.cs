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
    public LayerMask GroundMask;
    public Transform GroundCheckTransform;
    public float GroundCheckRadius;

    float[] SpeedValues = { 8.6f, 10.4f, 12.96f, 15.6f, 19.27f };
    float JumpPower = 26.6581f;
    [System.NonSerialized] public int Gravity = 1;
    [System.NonSerialized] public bool clickProcessed = false;
    public Transform Sprite;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * SpeedValues[(int)CurrentSpeed] * Time.deltaTime;
        Invoke(CurrentGameMode.ToString(), 0);

        Debug.Log("Cham tuong: " + TouchingWall());
    }
    public bool OnGround()
    {
        return Physics2D.OverlapBox(GroundCheckTransform.position + Vector3.up - Vector3.up * (Gravity -1 /-2), Vector2.right * 1.1f
            + Vector2.up * GroundCheckRadius, 0, GroundMask);
    }
    bool TouchingWall()
    {
        return Physics2D.OverlapBox(new Vector2(transform.position.x + 0.4f, transform.position.y), (Vector2.up * 0.7f) + (Vector2.right * GroundCheckRadius), 0, GroundMask);
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
        Generic.CreateGamemode(rb, this, true, 238.29f, 6.2f, false, true, 0, 238.29f);
    }

    void ResetPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
