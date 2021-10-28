using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartBtn : MonoBehaviour
{
    private float timer = 0;

    private string player_id
    {
        get
        {
            GameObject obj = GameObject.Find("PlayerInput");
            InputField input = obj.GetComponent<InputField>();
            return input.text;
        }
    }

    private string session_id
    {
        get
        {
            GameObject obj = GameObject.Find("RoomInput");
            InputField input = obj.GetComponent<InputField>();
            return input.text;
        }
    }

    private void JoinSessionCallback(JoinSessionResponse data)
    {

    }

    private void GetSessionInfoCallback(GetSessioInfonResponse data)
    {
        if (data.success && data.available_seats == 0)
        {
            SceneManager.LoadScene("Battleground", LoadSceneMode.Single);
        }
    }

    private void join_session()
    {
        if (player_id != string.Empty && session_id != string.Empty)
        {
            JoinSessionRequest request = new JoinSessionRequest();
            request.player_id = player_id;
            request.session_id = session_id;
            PaintSplatServer.Instance.join_session(request, JoinSessionCallback);
        }
    }

    private void get_session_info()
    {
        if (session_id != string.Empty)
        {
            GetSessionInfoRequest request = new GetSessionInfoRequest();
            request.session_id = session_id;
            PaintSplatServer.Instance.get_session_info(request, GetSessionInfoCallback);
        }
    }

    public void on_click()
    {
        join_session();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            get_session_info();
            timer = 0;
        }
    }
}
