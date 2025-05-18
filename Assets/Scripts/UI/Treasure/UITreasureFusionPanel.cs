using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SkyDragonHunter.UI
{
    public class UITreasureFusionPanel : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private UITreasureFusionSlot[] m_Slots;
        [SerializeField] private Image m_ResultImage;

        // 속성 (Properties)
        public bool IsFull => UITreasureFusionSlot.IsFullAllSlot();
        public bool IsEmpty => UITreasureFusionSlot.IsAllEmpty();
        public ArtifactGrade CurrentGrade { get; private set; } = ArtifactGrade.None;

        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        // Public 메서드
        public void SetSlot(ArtifactDummy target)
        {
            foreach (var slot in m_Slots)
            {
                if (slot.IsEmpty)
                {
                    slot.SetSlot(target);
                    CurrentGrade = target.Grade;
                    break;
                }
            }
        }

        public void RemoveSlot(ArtifactDummy target)
        {
            foreach (var slot in m_Slots)
            {
                if (slot.ArtifactDummy == target)
                {
                    slot.ClearSlot();
                    break;
                }
            }
            if (IsEmpty)
            {
                CurrentGrade = ArtifactGrade.None;
            }
        }

        public bool HasArtifact(ArtifactDummy target)
        {
            bool result = false;
            foreach (var slot in m_Slots)
            {
                if (slot.ArtifactDummy == target)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void ClearAllSlot()
        {
            foreach (var slot in m_Slots)
            {
                slot?.ClearSlot();
            }
            CurrentGrade = ArtifactGrade.None;
        }

        public void Fusion()
        {
            if (!IsFull)
            {
                DrawableMgr.Dialog("Alert", "합성 개수가 부족합니다.");
                return;
            }

            ArtifactGrade nextGrade = ArtifactGrade.None;
            foreach (var slot in m_Slots)
            {
                nextGrade = slot.ArtifactDummy.NextGrade;
            }
            if (nextGrade == ArtifactGrade.None)
            {
                DrawableMgr.Dialog("Error", "[UITreasureFusionPanel]: 합성 실패! 보물 등급이 null입니다.");
                return;
            }

            foreach( var slot in m_Slots)
            {
                var info = GameMgr.FindObject<UITreasureInfo>("UITreasureInfo");
                info.gameObject.SetActive(false);
                ArtifactDummy removeTarget = slot.ArtifactDummy;
                slot.ClearSlot();
                AccountMgr.RemoveArtifact(removeTarget);
            }
            ClearAllSlot();

            ArtifactDummy newArtifact = new ArtifactDummy(nextGrade);
            AccountMgr.AddArtifact(newArtifact);
            DrawableMgr.DialogWithArtifactInfo("합성 결과", newArtifact);
            SaveLoadMgr.CallSaveGameData();
        }

        // Private 메서드
        // Others

    } // Scope by class UITreasureFusion
} // namespace Root
