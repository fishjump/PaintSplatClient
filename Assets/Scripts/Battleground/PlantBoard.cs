using System.Collections;
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

    private List<GameObject> points = new List<GameObject>();

    private void draw(DrawParam param)
    {
        if (!gameObject.contains(param.obj))
        {
            return;
        }

        foreach (var item in points)
        {
            if (param.obj.overlays(item))
            {
                return;
            }
        }

        GameObject clone = Instantiate(param.obj, param.obj.transform.position, param.obj.transform.rotation);
        points.Add(clone);

        var list = clone.GetComponents<MonoBehaviour>();
        foreach (var item in list)
        {
            Destroy(item);
        }
        clone.transform.SetParent(gameObject.transform, true);

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

    // Start is called before the first frame update
    void Start()
    {
        update_direction();
    }


    // Update is called once per frame
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
