using SkyDragonHunter.Structs;
using UnityEngine;
using UnityEngine.Pool;

namespace SkyDragonHunter.Interfaces
{
    public interface IProjectable
    {
        public void Fire(Vector2 position, Vector2 direction, float projectileSpeed, Attack attack, GameObject owner, ObjectPool<GameObject> ownerPool = null);

    } // Scope by interface IDestruction
} // namespace SkyDragonHunter.Interfaces