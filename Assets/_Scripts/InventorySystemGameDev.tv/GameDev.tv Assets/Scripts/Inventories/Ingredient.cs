using UnityEngine;
using UnityEngine.Serialization;

namespace GameDevTV.Inventories
{
    [CreateAssetMenu(fileName = "Ingredient", menuName = "Crafting/Ingredient", order = 1)]
    public class Ingredient : ItemSO
    {
        [FormerlySerializedAs("materialType")]
        [Tooltip("The type of crafting material.")]
        [SerializeField] string _materialType;
        [FormerlySerializedAs("rarity")]
        [Tooltip("The rarity of the crafting material.")]
        [SerializeField] int _rarity;
        [Tooltip("The amount of the crafting material.")]
        [SerializeField] int _amount;

        public string GetMaterialType()
        {
            return _materialType;
        }

        public int GetRarity()
        {
            return _rarity;
        }

        public void IncreaseIngredientAmount()
        {
            _amount++;
        }
        
        public void DecreaseIngredientAmount()
        {
            _amount--;
        }
    }
}
