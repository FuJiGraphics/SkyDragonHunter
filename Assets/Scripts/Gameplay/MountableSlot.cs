using NPOI.SS.Formula.Functions;
using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Gameplay {

    public class MountableSlot : MonoBehaviour
    {
        // 필드 (Fields)
        public Vector3 offset;
        [Tooltip("Scene에 Load된 GameObject만 slot에 지정 가능합니다.")]
        [SerializeField] private GameObject slot;
        TransformBackup m_BackupPos;

        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            Init();
        }

        // Public 메서드
        public bool IsEmpty()
            => slot == null;

        public void Mounting(GameObject obj)
        {
            if (slot != null)
            {
                Debug.LogWarning("Warning: 이미 가득 찬 Slot입니다.");
                return;
            }
            slot = obj;
            m_BackupPos = TransformBackup.FromTransform(obj.transform);
            slot.transform.SetParent(this.transform);
            slot.transform.localPosition = Vector3.zero;
        }

        public void Dismounting()
        {           
            slot.transform.SetParent(null);
            m_BackupPos.ApplyTo(slot.transform);
            slot = null;
        }

        // Private 메서드
        private void Init()
        {
            if (slot != null)
            {
                m_BackupPos = TransformBackup.FromTransform(slot.transform);
                slot.transform.SetParent(this.transform);
                slot.transform.localPosition = Vector3.zero;
            }
        }

        [ContextMenu("Test Dismounting")]
        private void TestDismounting()
        {
            if (slot != null)
            {
                Dismounting();
            }
        }

    } // Scope by class MountableSlot
} // namespace SkyDragonHunter.Gameplay
