
using SkyDragonHunter.Tables;
using System;
using UnityEngine;

namespace SkyDraonHunter.Utility {

    public static class RandomMgr
    {
        private static readonly string s_ArgError = "[RandomMgr]: Error! Start가 End보다 작아야 합니다.";
        private static readonly string s_ArgNegativeError = "[RandomMgr]: 가중치는 음수가 될 수 없습니다.";
        private static System.Random s_DoubleRandom = new System.Random();

        public static T Range<T>(T[] candidates, float[] weights) 
            where T : Enum
        {
            if (candidates.Length != weights.Length)
            {
                Debug.LogError(s_ArgError);
                throw new ArgumentException(s_ArgError);
            }

            float total = 0f;
            for (int i = 0; i < weights.Length; i++)
            {
                if (weights[i] < 0f)
                {
                    Debug.LogError(s_ArgNegativeError);
                    throw new ArgumentException(s_ArgNegativeError);
                }
                total += weights[i];
            }

            float rand = UnityEngine.Random.Range(0f, total);
            float sum = 0f;

            for (int i = 0; i < candidates.Length; i++)
            {
                sum += weights[i];
                if (rand <= sum)
                    return candidates[i];
            }

            return candidates[^1];
        }

        public static T Range<T>(T start, T end) where T : Enum
        {
            int s = Convert.ToInt32(start);
            int e = Convert.ToInt32(end);

            if (s > e)
            {
                Debug.LogError(s_ArgError);
                throw new ArgumentException(s_ArgError);
            }

            int randomValue = UnityEngine.Random.Range(s, e + 1);
            return (T)Enum.ToObject(typeof(T), randomValue);
        }

        public static double Range(double minInclusive, double maxInclusive)
        {
            return minInclusive + s_DoubleRandom.NextDouble() * (maxInclusive - minInclusive);
        }
    }

} // namespace SkyDragonHunter.Utility