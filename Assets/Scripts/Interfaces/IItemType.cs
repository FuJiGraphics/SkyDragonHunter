
namespace SkyDragonHunter.Interfaces
{
    public enum ItemType
    {
        Potion, Weapon,
    };

    public interface IItemType
    {
        ItemType Type { get; }

    } // Scope by interface ISlowable
} // namespace SkyDragonHunter.Interfaces