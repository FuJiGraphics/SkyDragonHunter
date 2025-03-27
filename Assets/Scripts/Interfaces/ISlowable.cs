
namespace SkyDragonHunter.Interfaces
{
    public interface ISlowable
    {
        public void OnSlowBegin(float slowMultiplier);
        public void OnSlowEnd(float slowMultiplier);

    } // Scope by interface ISlowable
} // namespace SkyDragonHunter.Interfaces