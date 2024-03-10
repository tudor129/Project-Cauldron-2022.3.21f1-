using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomUI/ThemeSO", fileName = "ThemeSO")]
public class ThemeSO : SerializedScriptableObject
{
   [Header("Primary")]
   public Color _primary_bg;
   public Color _primary_text;
   
   [Header("Secondary")]
   public Color _secondary_bg;
   public Color _secondary_text;
   
   [Header("Tertiary")]
   public Color _tertiary_bg;
   public Color _tertiary_text;

   [Header("Other")]
   public Color _disable;
   
   public Color GetBackgroundColor(Style style)
   {
      switch (style)
      {
         case Style.Primary:
            return _primary_bg;
         case Style.Secondary:
            return _secondary_bg;
         case Style.Tertiary:
            return _tertiary_bg;
         default:
            return _disable;
      }
   }
   
   public Color GetTextColor(Style style)
   {
      switch (style)
      {
         case Style.Primary:
            return _primary_text;
         case Style.Secondary:
            return _secondary_text;
         case Style.Tertiary:
            return _tertiary_text;
         default:
            return _disable;
      }
   }


}
