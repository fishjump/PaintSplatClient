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

    private void draw()
    {
        GameObject plant_board = GameObject.Find("PlantBoard");
        if (plant_board == null)
        {
            return;
        }

        plant_board.SendMessage("draw", new DrawParam(gameObject, Color.red, new Vector2()));
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
                draw();
            }
            return;
        }

        try_move(10 * direction * Time.deltaTime);
    }
}
