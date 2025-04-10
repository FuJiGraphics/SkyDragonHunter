using SkyDragonHunter.Interfaces;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.test
{

    public class ItemStatus : MonoBehaviour
    {
        // 필드 (Fields)
        public string itemName;
        public int price;
        public int MaxCount;
        public float pullRate;
        public Sprite itemImage;
        public Sprite currencyTypeIcon;
        private SpriteRenderer spriteRenderer;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = itemImage;
        }
        // Public 메서드
        // Private 메서드
        // Others

    } // Scope by class ItemStatus

} // namespace Root