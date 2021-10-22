using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattlegroundObject
{
    /// <summary>
    /// if timer script is atttached, execute when timer is end
    /// </summary>
    void on_timer_end();

    /// <summary>
    /// if timer script is atttached, execute when timer changes
    /// </summary>
    void on_timer_changed(TimeSpan duration);

    /// <summary>
    /// return true if out of border, otherwise return false
    /// </summary>
    bool out_of_border(Vector2 vec);

    /// <summary>
    /// return false and stop move if out of border, otherwise move and return true
    /// </summary>
    bool try_move(Vector2 delta);
}

public abstract class BattlegroundObject : MonoBehaviour, IBattlegroundObject
{
    private Vector2 pos { get { return gameObject.transform.position; } }
    private Vector2 scale { get { return gameObject.transform.localScale; } }
    private Vector2 border
    {
        get
        {
            float half_screen_height = Camera.main.orthographicSize;
            float half_screen_width = (float)Screen.width / Screen.height * half_screen_height;

            return new Vector2(half_screen_width, half_screen_height);
        }
    }

    public virtual void on_timer_end() { }

    public virtual void on_timer_changed(TimeSpan duration) { }

    public virtual bool out_of_border(Vector2 vec)
    {

        if (vec.x > 0 && vec.x + scale.x / 2 > border.x)
        {
            return true;
        }

        if (vec.x < 0 && vec.x - scale.x / 2 < -border.x)
        {
            return true;
        }

        if (vec.y > 0 && vec.y + scale.y / 2 > border.y)
        {
            return true;
        }

        if (vec.y < 0 && vec.y - scale.y / 2 < -border.y)
        {
            return true;
        }

        return false;
    }

    public virtual bool try_move(Vector2 delta)
    {
        if (out_of_border(pos + delta))
        {
            return false;
        }

        gameObject.transform.Translate(delta, Space.World);

        return true;
    }
}
