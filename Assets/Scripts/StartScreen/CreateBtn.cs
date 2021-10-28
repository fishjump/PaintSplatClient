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

    private void CreateSessionCallback(CreateSessionResponse data)
    {
        if (data.success)
        {
            session_id = data.session_id;
        }
    }

    private void GetSessionInfoCallback(GetSessioInfonResponse data)
    {
        if (data.success && data.available_seats == 0)
        {
            SceneManager.LoadScene("Battleground", LoadSceneMode.Single);
        }
    }

    private void create_session()
    {
        if (player_id != string.Empty)
        {
            CreateSessionRequest request = new CreateSessionRequest();
            request.player_id = player_id;
            PaintSplatServer.Instance.create_session(request, CreateSessionCallback);
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
