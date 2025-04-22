
namespace SkyDragonHunter.Interfaces
{
    public enum ItemType
    {
        None = 0,
        Gold,
        Diamond,
        Ticket,
    };

    public enum ItemUnit
    {
        Number,
        AlphaUnit,
    };

    public interface IItemType
    {
        ItemType Type { get; }

    } // Scope by interface IItemType

} // namespace SkyDragonHunter.Interfaces