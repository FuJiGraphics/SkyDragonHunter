
using UnityEngine;
using SkyDragonHunter.Utility;
using SkyDragonHunter.Interfaces;

namespace SkyDragonHunter.Structs {

    public struct Attack
    {
        public GameObject defender;    // ���� �޴� ���
        public AlphaUnit damage;       // ������
        public AlphaUnit critical;     // ġ��Ÿ

        public bool IsCritical => critical > 0.0;
    }; // struct Attack

} // namespace SkyDragonHunter.Structs