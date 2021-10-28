using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // properties
    private bool is_timer_end { get { return timer <= TimeSpan.Zero; } }

    // fields
    private const float timer_period = 60;
    private TimeSpan timer = TimeSpan.FromSeconds(timer_period);


    // methods
    private void count_down()
    {
        timer -= TimeSpan.FromSeconds(Time.deltaTime);
        boardcast_timer_changed();
    }

    private void boardcast_timer_changed()
    {
        gameObject.BroadcastMessage("on_timer_changed", timer.Duration(), SendMessageOptions.DontRequireReceiver);
    }

    private void boardcast_timer_end()
    {
        gameObject.BroadcastMessage("on_timer_end", SendMessageOptions.DontRequireReceiver);
    }

    void Update()
    {
        if (is_timer_end)
        {
            boardcast_timer_end();
            return;
        }

        count_down();
    }
}
