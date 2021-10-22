using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBtn : MonoBehaviour
{
    private void goto_playground()
    {
        SceneManager.LoadScene("Battleground", LoadSceneMode.Single);
    }

    public void on_click()
    {
        goto_playground();
    }
}
