using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Utility 
{
    [Serializable]
    public struct ID : IComparable<ID>, IEquatable<ID>
    {
        // 필드 (Fields)
        [SerializeField] private int m_Id;

        // Public 메서드
        public ID(int id) => m_Id = id;
        public override string ToString() => m_Id.ToString();
        public override int GetHashCode() => m_Id.GetHashCode();
        public int CompareTo(ID other) => m_Id.CompareTo(other.m_Id);
        public bool Equals(ID other) => m_Id.Equals(other.m_Id);

        public override bool Equals(object obj)
        {
            if (obj is ID other)
                return this == other;
            return false;
        }

        // Others
        public static implicit operator ID(int number) => new ID(number);
        public static explicit operator int(ID id) => id.m_Id;
        public static ID operator +(ID a, ID b) => new ID(a.m_Id + b.m_Id);
        public static ID operator -(ID a, ID b) => new ID(a.m_Id - b.m_Id);
        public static ID operator *(ID a, ID b) => new ID(a.m_Id * b.m_Id);
        public static ID operator /(ID a, ID b) => new ID(a.m_Id / b.m_Id);
        public static bool operator <(ID a, ID b) => a.CompareTo(b) < 0;
        public static bool operator >(ID a, ID b) => a.CompareTo(b) > 0;
        public static bool operator <=(ID a, ID b) => a.CompareTo(b) <= 0;
        public static bool operator >=(ID a, ID b) => a.CompareTo(b) >= 0;
        public static bool operator ==(ID a, ID b) => a.CompareTo(b) == 0;
        public static bool operator !=(ID a, ID b) => a.CompareTo(b) != 0;
    } // Scope by class ID

} // namespace Root