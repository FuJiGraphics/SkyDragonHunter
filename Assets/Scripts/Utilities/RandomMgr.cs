
using SkyDragonHunter.Tables;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDraonHunter.Utility {

    public static class RandomMgr
    {
        private static readonly string s_ArgError = "[RandomMgr]: Error! Start가 End보다 작아야 합니다.";
        private static readonly string s_ArgNegativeError = "[RandomMgr]: 가중치는 음수가 될 수 없습니다.";
        private static readonly string s_ArgElementExceptionError = "[RandomMgr]: NullReference!!";
        private static System.Random s_DoubleRandom = new System.Random();

        public static T Random<T>(List<T> targetList)
        {
            if (targetList == null || targetList.Count == 0)
            {
                Debug.LogError(s_ArgElementExceptionError);
                throw new ArgumentException(s_ArgElementExceptionError);
            }

            int size = targetList.Count;
            int randIndex = UnityEngine.Random.Range(0, size);
            return targetList[randIndex];
        }

        public static T RandomWithWeights<T>(params (T candidate, float weight)[] param)
        {
            float total = 0f;
            for (int i = 0; i < param.Length; i++)
            {
                if (param[i].weight < 0f)
                {
                    Debug.LogError(s_ArgNegativeError);
                    throw new ArgumentException(s_ArgNegativeError);
                }
                total += param[i].weight;
            }

            float rand = UnityEngine.Random.Range(0f, total);
            float sum = 0f;

            for (int i = 0; i < param.Length; i++)
            {
                sum += param[i].weight;
                if (rand <= sum)
                    return param[i].candidate;
            }

            return param[^1].candidate;
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