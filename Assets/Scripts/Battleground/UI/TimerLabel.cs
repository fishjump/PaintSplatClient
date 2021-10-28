using System;
using TMPro;
using UnityEngine;

public sealed partial class TimerLabel : BattlegroundObject
{
    public override void on_timer_changed(TimeSpan duration)
    {
        update_text(duration);
    }

    public override void on_timer_end()
    {
        show_winner();
    }
}

public sealed partial class TimerLabel : BattlegroundObject
{
    private void update_text(TimeSpan duration)
    {
        var tb = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        tb.text = duration.ToString(@"hh\:mm\:ss");
    }

    private void get_session_info_callback(GetSessioInfoResponse data)
    {
        var tb = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        tb.text = string.Format("Winner: {0}", data.winner_id);
        var board = GameObject.Find("PaintBoard");
        board.GetComponent<Renderer>().enabled = false;
        for (int i = 0; i < board.transform.childCount; i++)
        {
            board.transform.GetChild(i).GetComponent<Renderer>().enabled = false;
        }
    }

    private void show_winner()
    {
        GetSessionInfoRequest request = new GetSessionInfoRequest();
        request.session_id = PaintSplatManager.instance.session_id;
        PaintSplatManager.instance.get_session_info(request, get_session_info_callback);
    }
}
