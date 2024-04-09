using UnityEngine;

namespace Utilities
{
    public static class TransformExtensions
    {
        public static Transform FindDeepChild(this Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name)
                    return child;

                Transform found = child.FindDeepChild(name);
                if (found != null)
                    return found;
            }
            return null;
        }
    }
}
