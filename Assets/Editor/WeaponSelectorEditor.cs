using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SkyDragonHunter {

    [CustomEditor(typeof(WeaponSelector))]
    public class WeaponSelectorEditor : Editor
    {
        // 필드 (Fields)
        private WeaponSelector selector;
        private SerializedProperty attachmentProp;
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void OnEnable()
        {
            selector = (WeaponSelector)target;
            attachmentProp = serializedObject.FindProperty("attachmentName");
            UpdateSlotList();
            UpdateSkinList();
        }


        // Public 메서드
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // 슬롯 드롭다운
            if (selector.availableSlotNames != null && selector.availableSlotNames.Length > 0)
            {
                int index = Mathf.Max(0, System.Array.IndexOf(selector.availableSlotNames, selector.slotName));
                index = EditorGUILayout.Popup("Slot", index, selector.availableSlotNames);
                selector.slotName = selector.availableSlotNames[index];
            }
            else
            {
                EditorGUILayout.LabelField("No slot list found");
            }

            // 스킨 드롭다운
            if (selector.availableSkinNames != null && selector.availableSkinNames.Length > 0)
            {
                int index = Mathf.Max(0, System.Array.IndexOf(selector.availableSkinNames, selector.skinName));
                index = EditorGUILayout.Popup("Skin Name", index, selector.availableSkinNames);
                selector.skinName = selector.availableSkinNames[index];
            }
            else
            {
                EditorGUILayout.LabelField("No skin list found");
            }

            // 어태치먼트 드롭다운 (SpineAttachment 속성으로 자동 제공)
            EditorGUILayout.PropertyField(attachmentProp);

            if (GUILayout.Button("Apply"))
            {
                selector.ApplyAttachment();
            }

            serializedObject.ApplyModifiedProperties();
        }
        // Private 메서드
        private void UpdateSlotList()
        {
            var skeletonAnim = selector.GetComponent<SkeletonAnimation>();
            if (skeletonAnim != null && skeletonAnim.Skeleton != null)
            {
                var slots = skeletonAnim.Skeleton.Slots.Items;
                selector.availableSlotNames = new string[skeletonAnim.Skeleton.Slots.Count];
                for (int i = 0; i < selector.availableSlotNames.Length; i++)
                {
                    selector.availableSlotNames[i] = slots[i].Data.Name;
                }
            }
            else
            {
                selector.availableSlotNames = new string[0];
            }
        }

        private void UpdateSkinList()
        {
            var skeletonAnim = selector.GetComponent<SkeletonAnimation>();
            if (skeletonAnim != null && skeletonAnim.SkeletonDataAsset != null)
            {
                var skins = skeletonAnim.SkeletonDataAsset.GetSkeletonData(true).Skins;
                selector.availableSkinNames = new string[skins.Count];
                for (int i = 0; i < skins.Count; i++)
                {
                    selector.availableSkinNames[i] = skins.Items[i].Name;
                }
            }
            else
            {
                selector.availableSkinNames = new string[0];
            }
        }
        // Others

    } // Scope by class WeaponSelectorEditor

} // namespace Root