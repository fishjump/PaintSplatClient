using System.Collections.Generic;
using UnityEngine;

public sealed partial class PaintBoard : BattlegroundObject
{
    public override void on_timer_end()
    {
        is_stop = true;
    }
}


public sealed partial class PaintBoard : BattlegroundObject
{
    private float timer;

    // properties
    private Vector2 delta { get { return speed * direction * Time.deltaTime; } }

    private Vector2 scale { get { return gameObject.transform.lossyScale / 2; } }
    private Vector2 pos { get { return gameObject.transform.position; } }

    // fields
    private bool is_stop = false;
    private uint frame_counter = 0;
    private float speed = 5;
    private Vector2 direction = new Vector2(0, 0);
    private int log_count = 0;

    private List<GameObject> circles = new List<GameObject>();

    private GameObject red_circle { get { return GameObject.Find("Red"); } }
    private GameObject blue_circle { get { return GameObject.Find("Blue"); } }
    private GameObject yellow_circle { get { return GameObject.Find("Yellow"); } }
    private GameObject green_circle { get { return GameObject.Find("Green"); } }

    private void draw(List<BattleLog> logs)
    {
        foreach (var log in logs)
        {
            Vector3 pos = new Vector3(log.pos.x + gameObject.transform.position.x, log.pos.y + gameObject.transform.position.y, -90f);

            GameObject clone;
            if (log.player_id == PaintSplatManager.instance.player_id)
            {
                if (PaintSplatManager.instance.is_host)
                {
                    clone = Instantiate(red_circle, pos, new Quaternion());
                }
                else
                {
                    clone = Instantiate(blue_circle, pos, new Quaternion());
                }
            }
            else
            {
                if (PaintSplatManager.instance.is_host)
                {
                    clone = Instantiate(blue_circle, pos, new Quaternion());
                }
                else
                {
                    clone = Instantiate(red_circle, pos, new Quaternion());
                }
            }


            clone.GetComponent<Renderer>().enabled = true;
            clone.transform.localScale = new Vector2(0.5f, 0.5f);
            clone.transform.SetParent(gameObject.transform, true);

            circles.Add(clone);
        }
    }

    private void pull_logs()
    {
        GetSessionLogRequest request = new GetSessionLogRequest();
        request.session_id = PaintSplatManager.instance.session_id;
        request.from = log_count;
        request.to = -1;
        PaintSplatManager.instance.get_session_log(request, get_session_info_callback);
    }

    private void get_session_info_callback(GetSessionLogResponse data)
    {
        if (data.success)
        {
            draw(data.logs);
            log_count += data.logs.Count;
        }
    }

    private void update_direction()
    {
        do
        {
            direction.x = Random.Range(-1f, 1f);
            direction.y = Random.Range(-1f, 1f);
            direction.Normalize();
        } while (direction.magnitude == 0);
    }

    private void update_speed()
    {
        speed += 0.001;
    }

    private void move()
    {
        if (PaintSplatManager.instance.is_host)
        {
            while (!try_move(delta))
            {
                update_direction();
            }
            UploadBoardRequest request = new UploadBoardRequest();
            request.session_id = PaintSplatManager.instance.session_id;
            request.player_id = PaintSplatManager.instance.player_id;
            request.pos.x = gameObject.transform.position.x;
            request.pos.y = gameObject.transform.position.y;
            PaintSplatManager.instance.upload_session_board(request, null);
        }
        else
        {
            SyncBoardRequest request = new SyncBoardRequest();
            request.session_id = PaintSplatManager.instance.session_id;
            PaintSplatManager.instance.sync_session_board(request, (SyncBoardResponse data) =>
            {
                if (data.success)
                {
                    Vector2 delta = new Vector2(data.pos.x - pos.x, data.pos.y - pos.y);
                    gameObject.transform.Translate(delta, Space.World);
                }
            });
        }
    }

    void Start()
    {
        Random.InitState(0);
        update_direction();
    }

    void Update()
    {
        if (is_stop)
        {
            return;
        }

        update_speed();

        pull_logs();

        move();

        frame_counter++;
        timer += Time.deltaTime;
    }
}
