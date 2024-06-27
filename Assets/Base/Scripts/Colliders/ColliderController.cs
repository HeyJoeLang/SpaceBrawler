using UnityEngine;
public class ColliderController : Singleton<ColliderController>
{
    public LayerMask ShardLayer;
    public LayerMask UFOLayer;

    public bool HitWall(int checkWallLayer)
    {
        return checkWallLayer == LayerMaskToLayer(ShardLayer);
    }
    public bool HitUFO(int checkUFOLayer)
    {
        return checkUFOLayer == LayerMaskToLayer(UFOLayer);
    }
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
