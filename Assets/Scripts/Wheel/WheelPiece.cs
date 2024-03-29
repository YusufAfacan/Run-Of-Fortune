﻿using UnityEngine ;

namespace EasyUI.PickerWheelUI {
   [System.Serializable]
   public class WheelPiece {
      public Sprite Icon ;
      public string Label ;

      public int amount ;

      
      [Range (0f, 100f)]
      public float Chance = 100f ;

      [HideInInspector] public int Index ;
      [HideInInspector] public double _weight = 0f ;
   }
}
