using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelbtn : MonoBehaviour
{
    private GameManager gm;
    public void GoNextLevel()
    {
        gm = GameManager.Instance;
        gm.LoadNextLevel();
    }
}