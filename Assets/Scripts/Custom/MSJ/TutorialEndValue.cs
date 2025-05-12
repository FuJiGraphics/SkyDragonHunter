using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class TutorialEndValue
{
    // 필드 (Fields)
    public static bool OneClear { get; set; } = false;
    public static bool IsStartTutorial { get; set; } = true;
    public static bool TutorialEnd { get; set; } = false;
    // 속성 (Properties)
    // 외부 종속성 필드 (External dependencies field)
    // 이벤트 (Events)
    // 유니티 (MonoBehaviour 기본 메서드)
    // Public 메서드
    public static void SetValueIsStartTutorial(bool isStart)
    {
        IsStartTutorial = isStart;
    }

    public static void SetValueTutorialEnd(bool end)
    {
        TutorialEnd = end;
    }

    public static void SetTrueOneClear()
    {
        OneClear = true;
    }

    public static bool GetIsStartTutorial()
    {
        return IsStartTutorial;
    }

    public static bool GetTutorialEnd()
    {
        return TutorialEnd;
    }
    // Private 메서드
    // Others

} // Scope by class TutorialEndValue

