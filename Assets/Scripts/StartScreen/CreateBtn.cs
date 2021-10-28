using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateBtn : MonoBehaviour
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

        set
        {
            GameObject obj = GameObject.Find("RoomInput");
            InputField input = obj.GetComponent<InputField>();
            input.text = value;
        }
    }

    private void create_session_callback(CreateSessionResponse data)
    {
        if (data.success)
        {
            PaintSplatManager.instance.is_host = true;
            session_id = data.session_id;
        }
    }

    private void get_session_info_callback(GetSessioInfoResponse data)
    {
        if (data.success && data.available_seats == 0)
        {
            PaintSplatManager.instance.goto_battleground(session_id, player_id);
        }
    }

    private void create_session()
    {
        if (player_id != string.Empty)
        {
            CreateSessionRequest request = new CreateSessionRequest();
            request.player_id = player_id;
            PaintSplatManager.instance.create_session(request, create_session_callback);
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
        create_session();
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
