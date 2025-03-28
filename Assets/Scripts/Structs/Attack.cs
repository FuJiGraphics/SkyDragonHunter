
using UnityEngine;
using SkyDragonHunter.Utility;
using SkyDragonHunter.Interfaces;

namespace SkyDragonHunter.Structs {

    public struct Attack
    {
        public GameObject attacker;
        public GameObject defender;    // ���� �޴� ���
        public AlphaUnit damage;       // ������
        public bool isCritical;
    }; // struct Attack

} // namespace SkyDragonHunter.Structs