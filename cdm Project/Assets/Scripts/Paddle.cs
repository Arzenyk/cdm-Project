using UnityEngine;

public class Paddle : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }

    public Vector2 direction { get; private set; }

    public float speed = 30f;
    public float maxbounceAngle = 75f;

    private GameManager gm;
    public Ball ball;

    private bool Pausado = false;

    private void Awake()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        gm = GameManager.Instance;
        if (gm == null)
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

    public void ResetPaddle()
    {
        this.transform.position = new Vector2(0f, this.transform.position.y);
        this.rigidbody.velocity = Vector2.zero;
    }

    private void Update()
    {
        if (gm != null && gm.gameOver)
        {
            return;
        }
        if (gm != null && gm.levelWon)
        {
            return;
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.direction = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.direction = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausarJuego();
        }
        else
        {
            this.direction = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (gm != null && gm.gameOver)
        {
            return;
        }

        if (this.direction != Vector2.zero)
        {
            this.rigidbody.AddForce(this.direction * this.speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            Vector3 paddlePosition = this.transform.position;
            Vector2 contactPoint = collision.GetContact(0).point;

            float offset = paddlePosition.x - contactPoint.x;
            float width = collision.otherCollider.bounds.size.x / 2;

            float currentAngle = Vector2.SignedAngle(Vector2.up, ball.rigidbody.velocity);
            float bounceAngle = (offset / width) * this.maxbounceAngle;
            float newAngle = Mathf.Clamp(currentAngle + bounceAngle, -this.maxbounceAngle, this.maxbounceAngle);

            Quaternion rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);

            ball.rigidbody.velocity = rotation * Vector2.up * ball.rigidbody.velocity.magnitude;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Duplicate PowerUp(Clone)")
        {
            ball.DuplicateBall();
            Destroy(other.gameObject);
        }
        if (other.gameObject.name == "Life Powerup(Clone)")
        {
            gm.AddLife();
            Destroy(other.gameObject);
        }
    }

    public void PausarJuego()
    {
        if (Pausado)
        {
            ResumirJuego();
        }
        else
        {
            PausarPantalla();
        }
    }

    public void ResumirJuego()
    {
        Time.timeScale = 1;
        Pausado = false;
    }

    public void PausarPantalla()
    {
        Time.timeScale = 0;
        Pausado = true;
    }
}
