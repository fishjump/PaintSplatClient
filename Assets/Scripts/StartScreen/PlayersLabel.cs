using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayersLabel : MonoBehaviour
{
    private float timer = 0;

    private string session_id
    {
        get
        {
            GameObject obj = GameObject.Find("RoomInput");
            InputField input = obj.GetComponent<InputField>();
            return input.text;
        }
    }

    private string text
    {
        set
        {
            TextMeshProUGUI input = gameObject.GetComponent<TextMeshProUGUI>();
            input.text = value;
        }
    }

    private void get_session_info_callback(GetSessioInfoResponse data)
    {
        if (data.success)
        {
            text = string.Format("Players({0}/2)", 2 - data.available_seats);
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
