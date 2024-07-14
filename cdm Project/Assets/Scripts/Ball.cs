using System.Runtime.CompilerServices;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public new Rigidbody2D rigidbody {  get; private set; }

    public float speed = 10f;

    private GameManager gm;

    public GameObject BallDuplicate;

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

        ResetBall();
    }

    public void ResetBall()
    {
        this.transform.position = Vector2.zero;
        this.rigidbody.velocity = Vector2.zero;

        if (gm != null && gm.gameOver)
        {
            return;
        }

        Invoke(nameof(SetRandomTrajectory), 1f);
    }

    private void SetRandomTrajectory()
    {
        if (gm != null && gm.gameOver)
        {
            return;
        }

        Vector2 force = Vector2.zero;
        force.x = Random.Range(-1f, 1f);
        force.y = -1f;

        this.rigidbody.AddForce(force.normalized * this.speed);
    }
    private void FixedUpdate()
    {
        if (gm != null && gm.gameOver)
        {
            ResetBall();
            rigidbody.velocity = Vector2.zero;  // Stop the ball's movement
            return;
        }

        rigidbody.velocity = rigidbody.velocity.normalized * speed;
    }

    public void DuplicateBall()
    {
        GameObject newBall = Instantiate(BallDuplicate, this.transform.position, this.transform.rotation);

        newBall.tag = "Ball Clone";

        Ball ballComponent = newBall.GetComponent<Ball>();
        if (ballComponent != null)
        {
            ballComponent.SetRandomTrajectory();
        }
        else
        {
            Debug.LogError("The duplicated ball does not have a Ball component!");
        }
    }
}