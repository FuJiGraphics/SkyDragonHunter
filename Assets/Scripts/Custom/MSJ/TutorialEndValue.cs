using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class TutorialEndValue
{
    // 필드 (Fields)
    public static bool oneClear { get; private set; } = false;
    private static bool IsStartTutorial = true;
    private static bool tutorialEnd = false;
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
        tutorialEnd = end;

    }

    public static void SetTrueOneClear()
    {
        oneClear = true;
    }

    public static bool GetIsStartTutorial()
    {
        return IsStartTutorial;
    }

    public static bool GetTutorialEnd()
    {
        return tutorialEnd;
    }
    // Private 메서드
    // Others

} // Scope by class TutorialEndValue

