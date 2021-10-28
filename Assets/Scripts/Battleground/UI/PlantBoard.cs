using System.Collections.Generic;
using UnityEngine;

public class DrawParam
{
    public DrawParam(GameObject obj, Color color, Vector2 position)
    {
        this.obj = obj;
        this.color = color;
        this.position = position;
    }
    public GameObject obj;
    public Color color;
    public Vector2 position;
}

public sealed partial class PlantBoard : BattlegroundObject
{
    public override void on_timer_end()
    {
        is_stop = true;
    }
}


public sealed partial class PlantBoard : BattlegroundObject
{
    // properties
    private Vector2 delta { get { return speed * direction * Time.deltaTime; } }

    private Vector2 scale { get { return gameObject.transform.lossyScale / 2; } }
    private Vector2 pos { get { return gameObject.transform.position; } }

    // fields
    private bool is_stop = false;
    private uint frame_counter = 0;
    private float speed = 10;
    private Vector2 direction = new Vector2(0, 0);

    private List<GameObject> circles = new List<GameObject>();

    private GameObject red_circle { get { return GameObject.Find("Red"); } }
    private GameObject blue_circle { get { return GameObject.Find("Blue"); } }
    private GameObject yellow_circle { get { return GameObject.Find("Yellow"); } }
    private GameObject green_circle { get { return GameObject.Find("Green"); } }


    private void draw(DrawParam param)
    {
        GameObject clone = Instantiate(blue_circle, param.obj.transform.position, param.obj.transform.rotation);

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

        if (frame_counter % 6000 == 0)
        {
            update_speed();
        }

        move();

        frame_counter++;
    }
}
