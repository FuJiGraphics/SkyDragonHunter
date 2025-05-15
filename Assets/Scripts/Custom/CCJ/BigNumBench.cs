using SkyDragonHunter.Structs;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;

namespace SkyDragonHunter.Test {

    public class BigNumBench : MonoBehaviour
    {

        // 필드 (Fields)
        // 속성 (Properties)
        // 외부 종속성 필드 (External dependencies field)
        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Reset()
        {
            string maxDoubleValue = "1000";

            BigInteger ai1 = BigInteger.Parse(maxDoubleValue);
            BigInteger ai2 = BigInteger.Parse(maxDoubleValue);
            BigNum b1 = new BigNum(maxDoubleValue);
            BigNum b2 = new BigNum(maxDoubleValue);

            Stopwatch sw = new Stopwatch();

            sw.Start();
            BigInteger ra = ai1 + ai2;
            sw.Stop();
            UnityEngine.Debug.Log($"C# BigInteger 덧셈 경과 시간: {sw.Elapsed.TotalMilliseconds} ms");
            UnityEngine.Debug.Log($"결과값: {ra}");

            sw.Start();
            BigNum rb = b1 + b2;
            sw.Stop();
            UnityEngine.Debug.Log($"BigNum 덧셈  경과 시간: {sw.Elapsed.TotalMilliseconds} ms");
            UnityEngine.Debug.Log($"결과값: {rb}");

            sw.Start();
            ra = ai1 * ai2;
            sw.Stop();
            UnityEngine.Debug.Log($"C# BigInteger 덧셈 경과 시간: {sw.Elapsed.TotalMilliseconds} ms");
            UnityEngine.Debug.Log($"결과값: {ra}");

            sw.Start();
            rb = b1 * b2;
            sw.Stop();
            UnityEngine.Debug.Log($"BigNum 덧셈  경과 시간: {sw.Elapsed.TotalMilliseconds} ms");
            UnityEngine.Debug.Log($"결과값: {rb}");
        }

        private void Update()
        {
            
        }
    
        // Public 메서드
        // Private 메서드
        // Others
    
    } // Scope by class BigNumBench
} // namespace SkyDragonHunter