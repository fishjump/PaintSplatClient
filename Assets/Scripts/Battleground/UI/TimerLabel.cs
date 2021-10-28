using System;
using TMPro;

public sealed partial class TimerLabel : BattlegroundObject
{
    public override void on_timer_changed(TimeSpan duration)
    {
        update_text(duration);
    }
}

public sealed partial class TimerLabel : BattlegroundObject
{
    private void update_text(TimeSpan duration)
    {
        var tb = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        tb.text = duration.ToString(@"hh\:mm\:ss");
    }
}
