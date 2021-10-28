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

    private void get_session_info_callback(GetSessioInfoResponse data)
    {
        if (data.success && data.available_seats == 0)
        {
            PaintSplatManager.instance.goto_battleground(session_id, player_id);
        }
    }

    private void join_session()
    {
        if (player_id != string.Empty && session_id != string.Empty)
        {
            JoinSessionRequest request = new JoinSessionRequest();
            request.player_id = player_id;
            request.session_id = session_id;
            PaintSplatManager.instance.join_session(request, null);
        }
    }

    private void get_session_info()
    {
        if (session_id != string.Empty)
        {
            GetSessionInfoRequest request = new GetSessionInfoRequest();
            request.session_id = session_id;
            PaintSplatManager.instance.get_session_info(request, get_session_info_callback);
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
