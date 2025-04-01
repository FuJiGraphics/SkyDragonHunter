
namespace SkyDragonHunter.Interfaces
{
    public enum ItemType
    {
        Potion, 
        Weapon,
        Gold,
        Gem,
        Scroll
    };

    public interface IItemType
    {
        ItemType Type { get; }

    } // Scope by interface ISlowable
} // namespace SkyDragonHunter.Interfaces