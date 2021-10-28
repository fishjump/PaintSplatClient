using UnityEngine;

public sealed partial class Player : BattlegroundObject
{
    public override void on_timer_end()
    {
        is_stop = true;
    }
}

public sealed partial class Player : BattlegroundObject
{
    private GameObject board { get { return GameObject.Find("PaintBoard"); } }
    private bool is_stop = false;

    private Vector2 direction
    {
        get
        {
            Vector2 direction = new Vector2();

            if (Input.GetKey(KeyCode.W))
            {
                direction += Vector2.up;
            }

            if (Input.GetKey(KeyCode.A))
            {
                direction += Vector2.left;
            }

            if (Input.GetKey(KeyCode.S))
            {
                direction += Vector2.down;
            }

            if (Input.GetKey(KeyCode.D))
            {
                direction += Vector2.right;
            }

            direction.Normalize();

            return direction;
        }
    }

    private bool press_draw
    {
        get
        {
            return Input.GetKey(KeyCode.Space);
        }
    }

    private bool press_down_draw
    {
        get
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }

    private void send_draw()
    {
        var clone = Instantiate(GameObject.Find("Green"), gameObject.transform.position, new Quaternion());

        if (!board.contains(clone))
        {
            Destroy(clone);
            return;
        }

        foreach (var item in GameObject.FindGameObjectsWithTag("circle"))
        {
            if (clone != item && clone.overlays(item))
            {
                Destroy(clone);
                return;
            }
        }

        ActInSessionRequest request = new ActInSessionRequest();

        request.session_id = PaintSplatManager.instance.session_id;
        request.player_id = PaintSplatManager.instance.player_id;
        request.pos.x = gameObject.transform.position.x - board.transform.position.x;
        request.pos.y = gameObject.transform.position.y - board.transform.position.y;
        Debug.Log((request.pos.x, request.pos.y));
        PaintSplatManager.instance.act_in_session(request, null);

        Destroy(clone);
    }

    void Update()
    {
        if (is_stop)
        {
            return;
        }

        if (press_draw)
        {
            if (press_down_draw)
            {
                send_draw();
            }
            return;
        }

        try_move(10 * direction * Time.deltaTime);
    }
}
