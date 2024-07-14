using UnityEngine;

public class LifePowerup : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        transform.Translate(new Vector2(0f, -1f) * Time.deltaTime * speed);

        if (transform.position.y < -15f)
        {
            Destroy(gameObject);
        }
    }
}