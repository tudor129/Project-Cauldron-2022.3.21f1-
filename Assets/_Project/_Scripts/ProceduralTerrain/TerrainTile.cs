using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainTile : MonoBehaviour
{
   [SerializeField] Vector2Int _tilePosition;

   void Start()
   {
      GetComponentInParent<WorldScrolling>().Add(gameObject, _tilePosition);
   }
}
