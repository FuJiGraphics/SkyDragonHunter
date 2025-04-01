
using SkyDragonHunter.Interfaces;
using UnityEngine;

namespace SkyDragonHunter.Scriptables
{
    public abstract class ItemDefinition : ScriptableObject
        , IUsable
        , IItemType
        , IInventoryItem
    {
        // �ʵ� (Fields)
        public GameObject previewPrefab;

        // �Ӽ� (Properties)
        public abstract ItemType Type { get; }

        // �ܺ� ���Ӽ� �ʵ� (External dependencies field)
        // �̺�Ʈ (Events)
        // ����Ƽ (MonoBehaviour �⺻ �޼���)

        // Public �޼���
        public abstract Sprite GetIcon();
        public abstract string GetName();

        public abstract void Use(GameObject caster, GameObject receiver);

        // Private �޼���
        // Others

    } // Scope by class ItemDefinition
} // namespace SkyDragonHunter.Scriptables