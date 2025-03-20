using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class XLoaderTest : MonoBehaviour
{
    // 필드 (Fields)
    // 속성 (Properties)
    // 외부 종속성 필드 (External dependencies field)
    // 이벤트 (Events)
    // 유니티 (MonoBehaviour 기본 메서드)
    private void Start()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "tables/stage_table.xlsx");
        XLoader.Load(path);
    }

    private void Update()
    {
        
    }

    // Public 메서드
    // Private 메서드
    // Others

} // Scope by class XLoaderTest
