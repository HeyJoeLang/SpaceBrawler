using UnityEngine;

public class ColliderUtils
{
    public static int LayerMaskToLayer(LayerMask Layer)
    {
        var n = Layer.value;
        var mask = 0;
        while (n > 1)
        {
            n = n >> 1;
            mask++;
        }
        return mask;
    }
}
