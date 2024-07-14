using UnityEngine;

public class MissZone : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ball")
        {
            FindObjectOfType<GameManager>().Miss();
        }

        if (collision.gameObject.name == "Ball(Clone)")
        {
            Destroy(collision.gameObject);
        }
    }
}
