﻿using SkyDragonHunter.Structs;
using UnityEngine;

namespace SkyDragonHunter.Interfaces
{
    public interface ICrewEquipEventHandler
    {
        void OnEquip(int slotIndex);
        void OnUnequip(int slotIndex);

    } // Scope by interface ICrewEquipEventHandler
} // namespace SkyDragonHunter.Interfaces