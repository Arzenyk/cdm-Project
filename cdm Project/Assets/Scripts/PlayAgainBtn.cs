using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayAgainBtn : MonoBehaviour
{
    private GameManager gm;

    public void PlayAgain()
    {
        if (GameManager.Instance != null)
        {
            gm = GameManager.Instance;
            gm.NewGame();
        }
        else
        {
            Debug.LogError("GameManager.Instance is null!");
        }
    }
}
