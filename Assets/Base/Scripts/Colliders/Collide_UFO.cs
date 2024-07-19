using UnityEngine;

namespace heyjoelang
{
    public class Collide_UFO : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (ColliderController.Instance.HitUFO(other.gameObject.layer))
            {
                Debug.LogFormat("{0} hit {1}", gameObject.name, other.gameObject.name);
                other.GetComponent<EnemyShip>().Explode();
            }
        }
    }
}
