using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static bool contains(this GameObject obj, GameObject gObj)
    {
        Vector2 scale = obj.transform.lossyScale / 2;
        Vector2 pos = obj.transform.position;
        System.Tuple<Vector2, Vector2> region = new System.Tuple<Vector2, Vector2>(
            new Vector2(pos.x - scale.x, pos.y + scale.y),
            new Vector2(pos.x + scale.x, pos.y - scale.y)
        );

        Vector2 obj_scale = gObj.transform.lossyScale / 2;
        Vector2 obj_pos = gObj.transform.position;
        System.Tuple<Vector2, Vector2> obj_region = new System.Tuple<Vector2, Vector2>(
            new Vector2(obj_pos.x - obj_scale.x, obj_pos.y + obj_scale.y),
            new Vector2(obj_pos.x + obj_scale.x, obj_pos.y - obj_scale.y)
        );

        return (region.Item1.x < obj_region.Item1.x &&
            region.Item1.y > obj_region.Item1.y &&
            region.Item2.x > obj_region.Item2.x &&
            region.Item2.y < obj_region.Item2.y);
    }

    public static bool overlays(this GameObject obj, GameObject gObj)
    {
        Vector2 scale = obj.transform.lossyScale / 2;
        Vector2 pos = obj.transform.position;
        System.Tuple<Vector2, Vector2> region = new System.Tuple<Vector2, Vector2>(
            new Vector2(pos.x - scale.x, pos.y + scale.y),
            new Vector2(pos.x + scale.x, pos.y - scale.y)
        );

        Vector2 obj_scale = gObj.transform.lossyScale / 2;
        Vector2 obj_pos = gObj.transform.position;
        List<Vector2> obj_region = new List<Vector2>();
        obj_region.Add(new Vector2(obj_pos.x - obj_scale.x, obj_pos.y + obj_scale.y));
        obj_region.Add(new Vector2(obj_pos.x + obj_scale.x, obj_pos.y - obj_scale.y));
        obj_region.Add(new Vector2(obj_pos.x - obj_scale.x, obj_pos.y - obj_scale.y));
        obj_region.Add(new Vector2(obj_pos.x + obj_scale.x, obj_pos.y + obj_scale.y));


        bool ret = false;
        foreach (var item in obj_region)
        {
            ret = ret || (region.Item1.x <= item.x &&
                    region.Item1.y >= item.y &&
                    region.Item2.x >= item.x &&
                    region.Item2.y <= item.y);
        }

        return ret;
    }
}
