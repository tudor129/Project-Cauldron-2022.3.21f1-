using UnityEngine;
using TMPro;
using GameDevTV.Inventories;

namespace GameDevTV.UI.Inventories
{
    /// <summary>
    /// Root of the tooltip prefab to expose properties to other classes.
    /// </summary>
    public class ItemTooltip : MonoBehaviour
    {
        // CONFIG DATA
        [SerializeField] TextMeshProUGUI titleText = null;
        [SerializeField] TextMeshProUGUI bodyText = null;

        // PUBLIC

        public void Setup(GameDevTV.Inventories.ItemSO itemSo)
        {
            titleText.text = itemSo.GetDisplayName();
            bodyText.text = itemSo.GetDescription();
        }
    }
}
