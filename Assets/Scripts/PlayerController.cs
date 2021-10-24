using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed = 10;

    public Text score;
    public Text WinText;
    public Text LivesText;
    public Text LevelText;

    private int scoreValue = 0;
    private int Lives = 3;

    private bool facingRight = true;
    private bool isJumping = false;

    Animator anim;

    public AudioClip NormalMusic;
    public AudioClip Victory;
    public AudioClip GameOver;
    public AudioSource musicSource;

    public bool HaveWon = false;
    public bool HaveLost = false;


    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        score.text = scoreValue.ToString();

        WinText.text = "";
        LivesText.text = "Lives: 3";
        anim.SetInteger("State", 0);
        LevelText.text = "Level 1";

        musicSource.clip = NormalMusic;
        musicSource.Play();
        musicSource.loop = true;

    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }


        if (Input.GetKeyDown("right")||Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp("right")||Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown("left")||Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp("left")||Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }

        if (scoreValue >= 8)
        {
            WinText.text = "You Win!  Game created by Dakota Robinson.";
        }
        if (Lives == 0)
        {
            WinText.text = "You Lose!";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

   

        rd2d.AddForce(new Vector2(hozMovement * speed * 4, vertMovement * speed));


        if (facingRight == false && hozMovement > 0 && HaveLost == false)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0 && HaveLost == false)
        {
            Flip();
        }

        if (isJumping == true)
        {
            anim.SetInteger("State", 2);
        }

        if (isJumping == true && rd2d.velocity.y <= 0)
        {
            isJumping = false;
            anim.SetInteger("State", 0);
            speed = 3;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            if(scoreValue == 4)
            {
                transform.position = new Vector3 (45.0f, -1.0f, 0.0f);
                Lives = 3;
                LivesText.text = "Lives: " + Lives.ToString();
                LevelText.text = "Level 2";
            }

            if(scoreValue == 8 && HaveWon == false)
            {
                HaveWon = true;
                musicSource.clip = Victory;
                musicSource.Play();
                musicSource.loop = false;
            }
        }


        else if(collision.collider.tag == "Enemy")
        {
            Lives -= 1;
            LivesText.text = "Lives: " + Lives.ToString();
            Destroy(collision.collider.gameObject);

            if(Lives == 0 && HaveLost == false)
            {
                HaveLost = true;
                musicSource.clip = GameOver;
                musicSource.Play();
                musicSource.loop = false;

                anim.SetInteger("State", 4);
                speed = 0;
                rd2d.gravityScale = 100;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            speed = 10;
            if (isJumping == true){
                anim.SetInteger("State", 0);
                isJumping = false;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey("up"))
            {
                rd2d.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
                isJumping = true;
                speed = 3;
            }
        }

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
