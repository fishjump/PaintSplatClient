using Proyecto26;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PaintSplatManager : MonoBehaviour
{
    private const string domain = "http://macbook-air.local";
    private const int port = 8000;

    private readonly string session_url = string.Format("{0}:{1}/{2}", domain, port, "battleground");
    private readonly string create_session_url = string.Format("{0}:{1}/{2}", domain, port, "battleground/create");
    private readonly string join_session_url = string.Format("{0}:{1}/{2}", domain, port, "battleground/join");
    private readonly string session_log_url = string.Format("{0}:{1}/{2}", domain, port, "battleground/battle_log");
    private readonly string session_info_url = string.Format("{0}:{1}/{2}", domain, port, "battleground/info");
    private readonly string session_board_upload_url = string.Format("{0}:{1}/{2}", domain, port, "battleground/board/upload");
    private readonly string session_board_sync_url = string.Format("{0}:{1}/{2}", domain, port, "battleground/board/sync");

    public delegate void CreateSessionCallback(CreateSessionResponse data);
    public delegate void JoinSessionCallback(JoinSessionResponse data);
    public delegate void GetSessionInfoCallback(GetSessioInfoResponse data);
    public delegate void ActInSessionCallback(ActInSessionResponse data);
    public delegate void GetSessionLogCallback(GetSessionLogResponse data);
    public delegate void UploadBoardCallback(UploadBoardResponse data);
    public delegate void SyncBoardCallback(SyncBoardResponse data);


    public string player_id { get; private set; }
    public string session_id { get; private set; }
    public bool is_host { get; set; }


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

    public void get_session_log(GetSessionLogRequest request, GetSessionLogCallback cbk)
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

    public void upload_session_board(UploadBoardRequest request, UploadBoardCallback cbk)
    {
        string json = JsonUtility.ToJson(request);

        RestClient.Post(session_board_upload_url, json).Then(response =>
        {
            if (cbk == null)
            {
                return;
            }
            cbk(JsonUtility.FromJson<UploadBoardResponse>(response.Text));
        });
    }

    public void sync_session_board(SyncBoardRequest request, SyncBoardCallback cbk)
    {
        string json = JsonUtility.ToJson(request);

        RestClient.Post(session_board_sync_url, json).Then(response =>
        {
            if (cbk == null)
            {
                return;
            }
            cbk(JsonUtility.FromJson<SyncBoardResponse>(response.Text));
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