
using SkyDragonHunter.Interfaces;
using UnityEngine;

namespace SkyDragonHunter.Scriptables
{
    public abstract class ItemDefinition : ScriptableObject
        , IUsable
        , IItemType
        , IInventoryItem
    {
        // 필드 (Fields)
        public GameObject previewPrefab;

        // 속성 (Properties)
        public abstract ItemType Type { get; }

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)

        // Public 메서드
        public abstract Sprite GetIcon();
        public abstract string GetName();

        public abstract void Use(GameObject caster, GameObject receiver);

        // Private 메서드
        // Others

    } // Scope by class ItemDefinition
} // namespace SkyDragonHunter.Scriptables