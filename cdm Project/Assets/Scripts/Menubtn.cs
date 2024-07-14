using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menubtn : MonoBehaviour
{
    public void Menu()
    {
        SceneManager.LoadScene("AMenu");
    }
}
