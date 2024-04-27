using System.Collections;
using UnityEngine;

namespace Utilities
{
    public static class ObjectPoolUtilities
    {
        public static void ReturnToPoolAfterDelays(this MonoBehaviour monoBehaviour, GameObject objectToReturn, float delay)
        {
            monoBehaviour.StartCoroutine(ReturnStatusEffectToPoolAfterDelayCoroutine(delay, objectToReturn));
        }

        static IEnumerator ReturnStatusEffectToPoolAfterDelayCoroutine(float delay, GameObject objectToReturn)
        {
            yield return new WaitForSeconds(delay);
            ObjectPoolManager.Instance.ReturnStatusEffectToPool(objectToReturn);
        }
    }
}
