using JetBrains.Annotations;
using UnityEngine;

public class MissZone : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ball")
        {
            FindObjectOfType<GameManager>().Miss();
            DestroyAllClones();

        }

        if (collision.gameObject.name == "Ball(Clone)")
        {
            Destroy(collision.gameObject);
        }
    }

    public void DestroyAllClones()
    {
        GameObject[] ballClones = GameObject.FindGameObjectsWithTag("Ball Clone");
        foreach (GameObject ballClone in ballClones)
        {
            Destroy(ballClone);
        }
    }
}
