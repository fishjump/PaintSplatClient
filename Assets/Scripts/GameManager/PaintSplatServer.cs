
using Proyecto26;
using UnityEditor;
using UnityEngine;

public class PaintSplatServer : MonoBehaviour
{
    private const string domain = "localhost";
    private const int port = 8000;

    private readonly string session_url = string.Format("{0}:{1}/{2}", domain, port, "battleground/");
    private readonly string create_session_url = string.Format("{0}:{1}/{2}", domain, port, "battleground/create");
    private readonly string join_session_url = string.Format("{0}:{1}/{2}", domain, port, "battleground/join");
    private readonly string session_log_url = string.Format("{0}:{1}/{2}", domain, port, "battleground/battle_log");
    private readonly string session_info_url = string.Format("{0}:{1}/{2}", domain, port, "battleground/info");

    public delegate void CreateSessionCallback(CreateSessionResponse data);
    public delegate void JoinSessionCallback(JoinSessionResponse data);
    public delegate void GetSessionInfoCallback(GetSessioInfonResponse data);
    public delegate void ActInSessionCallback(ActInSessionResponse data);
    public delegate void GetSessionLogRCallback(GetSessionLogResponse data);

    public void create_session(CreateSessionRequest request, CreateSessionCallback cbk)
    {
        string json = JsonUtility.ToJson(request);

        RestClient.Post(create_session_url, json).Then(response =>
         {
             cbk(JsonUtility.FromJson<CreateSessionResponse>(response.Text));
         });
    }

    public void join_session(JoinSessionRequest request, JoinSessionCallback cbk)
    {
        string json = JsonUtility.ToJson(request);

        RestClient.Post(join_session_url, json).Then(response =>
        {
            cbk(JsonUtility.FromJson<JoinSessionResponse>(response.Text));
        });
    }

    public void act_in_session(ActInSessionRequest request, ActInSessionCallback cbk)
    {
        string json = JsonUtility.ToJson(request);

        RestClient.Post(session_url, json).Then(response =>
        {
            cbk(JsonUtility.FromJson<ActInSessionResponse>(response.Text));
        });
    }

    public void get_session_info(GetSessionInfoRequest request, GetSessionInfoCallback cbk)
    {
        string json = JsonUtility.ToJson(request);

        RestClient.Post(session_info_url, json).Then(response =>
        {
            cbk(JsonUtility.FromJson<GetSessioInfonResponse>(response.Text));
        });
    }

    public void get_session_log(GetSessionLogRequest request, GetSessionLogRCallback cbk)
    {
        string json = JsonUtility.ToJson(request);

        RestClient.Post(session_log_url, json).Then(response =>
        {
            cbk(JsonUtility.FromJson<GetSessionLogResponse>(response.Text));
        });
    }

    public static PaintSplatServer Instance { get; private set; }

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}