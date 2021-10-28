using System.Collections.Generic;
using UnityEngine;

public sealed partial class PlantBoard : BattlegroundObject
{
    public override void on_timer_end()
    {
        is_stop = true;
    }
}


public sealed partial class PlantBoard : BattlegroundObject
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

    private List<GameObject> circles = new List<GameObject>();

    private GameObject red_circle { get { return GameObject.Find("Red"); } }
    private GameObject blue_circle { get { return GameObject.Find("Blue"); } }
    private GameObject yellow_circle { get { return GameObject.Find("Yellow"); } }
    private GameObject green_circle { get { return GameObject.Find("Green"); } }

    private int logs_count = 0;

    private void draw(List<BattleLog> logs)
    {
        foreach (var log in logs)
        {
            Vector3 pos = new Vector3(log.pos.x, log.pos.y, -90f);

            GameObject clone;
            if (log.player_id == PaintSplatManager.instance.player_id)
            {
                clone = Instantiate(red_circle, pos, new Quaternion());
            }
            else
            {
                clone = Instantiate(blue_circle, pos, new Quaternion());
            }

            if (!gameObject.contains(clone))
            {
                return;
            }

            foreach (var item in circles)
            {
                if (clone.overlays(item))
                {
                    Destroy(clone);
                    return;
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
        request.from = logs_count;
        request.to = -1;
        PaintSplatManager.instance.get_session_log(request, get_session_info_callback);
    }

    private void get_session_info_callback(GetSessionLogResponse data)
    {
        if (data.success)
        {
            draw(data.logs);
            logs_count += data.logs.Count;
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
        speed++;
    }

    private void move()
    {
        while (!try_move(delta))
        {
            update_direction();
        }
    }

    void Start()
    {
        update_direction();
    }

    void Update()
    {
        if (is_stop)
        {
            return;
        }

        if (frame_counter % 2000 == 0)
        {
            update_speed();
        }

        if ((int)(timer * 1000) % 20 == 0)
        {
            pull_logs();
        }

        move();

        frame_counter++;
        timer += Time.deltaTime;
    }
}
