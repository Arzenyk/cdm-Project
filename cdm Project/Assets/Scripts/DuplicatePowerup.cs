using UnityEngine;

public class DuplicatePowerup : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        transform.Translate (new Vector2(0f, -1f) * Time.deltaTime * speed);
    }
}