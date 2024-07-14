using UnityEngine;

public class Brick : MonoBehaviour
{
    public SpriteRenderer spriteRenderer {  get; private set; }

    public Sprite[] states;
    
    public int health {  get; private set; }

    public int points = 100;

    public bool unbreakable;

    public Transform explosionBlue;
    public Transform explosionGreen;
    public Transform explosionYellow;
    public Transform explosionOrange;
    public Transform explosionRed;

    public Transform powerup;

    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ResetBricks();
    }

    public void ResetBricks()
    {
        this.gameObject.SetActive(true);

        if (!unbreakable)
        {
            this.health = this.states.Length;
            this.spriteRenderer.sprite = this.states[this.health - 1];
        }
    }

    private void Hit()
    {
        if (this.unbreakable)
        {
            return;
        }

        if (this.health == 5)
        {
            //Red
            this.health--;
            this.spriteRenderer.sprite = this.states[this.health - 1];
            Transform newExplosionRed = Instantiate(explosionRed, transform.position, transform.rotation);
            Destroy (newExplosionRed.gameObject, 2.5f);
        }
        else if (this.health == 4)
        {
            //Orange
            this.health--;
            this.spriteRenderer.sprite = this.states[this.health - 1];
            Transform newExplosionOrange = Instantiate(explosionOrange, transform.position, transform.rotation);
            Destroy (newExplosionOrange.gameObject, 2.5f);
        }
        else if (this.health == 3)
        {
            //Yellow
            this.health--;
            this.spriteRenderer.sprite = this.states[this.health - 1];
            Transform newExplosionYellow = Instantiate(explosionYellow, transform.position, transform.rotation);
            Destroy (newExplosionYellow.gameObject, 2.5f);
        }
        else if (this.health == 2)
        {
            //Green
            this.health--;
            this.spriteRenderer.sprite = this.states[this.health - 1];
            Transform newExplosionGreen = Instantiate(explosionGreen, transform.position, transform.rotation);
            Destroy (newExplosionGreen.gameObject, 2.5f);
        }
        else if (this.health == 1)
        {
            //Blue
            this.gameObject.SetActive(false);
            Transform newExplosionBlue = Instantiate(explosionBlue, transform.position, transform.rotation);
            Destroy (newExplosionBlue.gameObject, 2.5f);
        }

        FindObjectOfType<GameManager>().Hit(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PowerupChance();

        if (collision.gameObject.name == "Ball")
        {
            Hit();
        }

        if (collision.gameObject.name == "Ball(Clone)")
        {
            Hit();
        }
    }

    public void PowerupChance()
    {
        int randChance = Random.Range(1, 101);

        if (randChance > 90)
        {
            Instantiate(powerup, transform.position, transform.rotation);
        }
    }
}
