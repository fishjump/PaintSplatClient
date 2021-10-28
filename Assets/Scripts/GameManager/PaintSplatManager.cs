using Proyecto26;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PaintSplatManager : MonoBehaviour
{
    private const string domain = "localhost";
    private const int port = 8000;

    private readonly string session_url = string.Format("{0}:{1}/{2}", domain, port, "battleground");
    private readonly string create_session_url = string.Format("{0}:{1}/{2}", domain, port, "battleground/create");
    private readonly string join_session_url = string.Format("{0}:{1}/{2}", domain, port, "battleground/join");
    private readonly string session_log_url = string.Format("{0}:{1}/{2}", domain, port, "battleground/battle_log");
    private readonly string session_info_url = string.Format("{0}:{1}/{2}", domain, port, "battleground/info");

    public delegate void CreateSessionCallback(CreateSessionResponse data);
    public delegate void JoinSessionCallback(JoinSessionResponse data);
    public delegate void GetSessionInfoCallback(GetSessioInfoResponse data);
    public delegate void ActInSessionCallback(ActInSessionResponse data);
    public delegate void GetSessionLogRCallback(GetSessionLogResponse data);


    public string player_id { get; private set; }
    public string session_id { get; private set; }


    public void goto_battleground(string session_id, string player_id)
    {
        this.session_id = session_id;
        this.player_id = player_id;
        SceneManager.LoadScene("Battleground", LoadSceneMode.Single);
    }

    public void create_session(CreateSessionRequest request, CreateSessionCallback cbk)
    {
        string json = JsonUtility.ToJson(request);

        RestClient.Post(create_session_url, json).Then(response =>
         {
             if (cbk == null)
             {
                 return;
             }
             cbk(JsonUtility.FromJson<CreateSessionResponse>(response.Text));
         });
    }

    public void join_session(JoinSessionRequest request, JoinSessionCallback cbk)
    {
        string json = JsonUtility.ToJson(request);

        RestClient.Post(join_session_url, json).Then(response =>
        {
            if (cbk == null)
            {
                return;
            }
            cbk(JsonUtility.FromJson<JoinSessionResponse>(response.Text));
        });
    }

    public void act_in_session(ActInSessionRequest request, ActInSessionCallback cbk)
    {
        string json = JsonUtility.ToJson(request);

        RestClient.Post(session_url, json).Then(response =>
        {
            if (cbk == null)
            {
                return;
            }
            cbk(JsonUtility.FromJson<ActInSessionResponse>(response.Text));
        });
    }

    public void get_session_info(GetSessionInfoRequest request, GetSessionInfoCallback cbk)
    {
        string json = JsonUtility.ToJson(request);

        RestClient.Post(session_info_url, json).Then(response =>
        {
            if (cbk == null)
            {
                return;
            }
            cbk(JsonUtility.FromJson<GetSessioInfoResponse>(response.Text));
        });
    }

    public void get_session_log(GetSessionLogRequest request, GetSessionLogRCallback cbk)
    {
        string json = JsonUtility.ToJson(request);

        RestClient.Post(session_log_url, json).Then(response =>
        {
            if (cbk == null)
            {
                return;
            }
            cbk(JsonUtility.FromJson<GetSessionLogResponse>(response.Text));
        });
    }

    public static PaintSplatManager instance { get; private set; }

    protected void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}