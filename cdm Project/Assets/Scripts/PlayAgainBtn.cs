using UnityEngine;
using UnityEngine.UI;

public class PlayAgainBtn : MonoBehaviour
{
    public void PlayAgain()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetLevel();
        }
        else
        {
            Debug.LogError("GameManager.Instance is null!");
        }
    }
}
