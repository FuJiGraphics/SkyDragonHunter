
namespace SkyDraonHunter.Utility {

    public static class DoubleRandom
    {
        private static System.Random _random = new System.Random();

        public static double Range(double minInclusive, double maxInclusive)
        {
            return minInclusive + _random.NextDouble() * (maxInclusive - minInclusive);
        }
    }

} // namespace SkyDragonHunter.Utility