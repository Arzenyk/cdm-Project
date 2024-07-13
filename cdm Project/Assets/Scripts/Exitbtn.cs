using UnityEngine;
using UnityEngine.UI;

public class Exitbtn : MonoBehaviour
{
    public void Exit()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Exit();
        }
        else
        {
            Debug.LogError("GameManager.Instance is null!");
        }
    }
}
