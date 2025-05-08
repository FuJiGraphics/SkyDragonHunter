using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter
{

    [ExecuteAlways]
    public class WeaponSelector : MonoBehaviour
    {
        // 필드 (Fields)
        [HideInInspector] public string slotName = "weapon";
        [HideInInspector] public string skinName = "";
        [SpineAttachment(true, slotField: "slotName")] public string attachmentName;

        [HideInInspector] public string[] availableSlotNames;
        [HideInInspector] public string[] availableSkinNames;
        // 에디터에서 슬롯 목록을 담아둘 공간
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            ApplyAttachment();
        }

        private void OnEnable()
        {
            ApplyAttachment();
        }

        // Public 메서드
        public void ApplyAttachment()
        {
            var skeletonAnimation = GetComponent<SkeletonAnimation>();
            if (skeletonAnimation == null || !skeletonAnimation.valid)
                return;

            var skeleton = skeletonAnimation.Skeleton;
            if (!string.IsNullOrEmpty(skinName))
            {
                var skin = skeleton.Data.FindSkin(skinName);
                if (skin != null)
                {
                    skeleton.SetSkin(skin);
                    skeleton.SetSlotsToSetupPose(); // 필수
                }
            }

            if (!string.IsNullOrEmpty(slotName) && !string.IsNullOrEmpty(attachmentName))
            {
                skeleton.SetAttachment(slotName, attachmentName);
            }

            skeletonAnimation.Update(0);
            skeletonAnimation.LateUpdate();
        }

        // Private 메서드
        private void OnValidate()
        {
            ApplyAttachment();
        }
        // Others

    } // Scope by class WeaponSelector

} // namespace Root