
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance { get; private set; }

    public float displayTime = 4.0f;

    private VisualElement m_NonPlayerDialogue;
    private float m_TimerDisplay;
    private VisualElement m_Healthbar;
    private VisualElement m_WinScreen;
    private VisualElement m_LoseScreen;
    private VisualElement m_StartScreen;
    private Button m_StartButton;
    private Button m_WinRestartButton;
    private Button m_LoseRestartButton;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();

        m_Healthbar = uiDocument.rootVisualElement
            .Q<VisualElement>("HealthBar");

        SetHealthValue(1.0f);

        m_NonPlayerDialogue = uiDocument.rootVisualElement
            .Q<VisualElement>("NPCDialogue");

        m_NonPlayerDialogue.style.display = DisplayStyle.None;
        m_TimerDisplay = -1.0f;

        m_LoseScreen = uiDocument.rootVisualElement
            .Q<VisualElement>("LoseScreenContainer");

        m_WinScreen = uiDocument.rootVisualElement
            .Q<VisualElement>("WinScreenContainer");
        m_StartScreen = uiDocument.rootVisualElement
            .Q<VisualElement>("StartScreenContainer");
        m_StartButton = uiDocument.rootVisualElement
            .Q<Button>("StartButton");
        m_WinRestartButton = uiDocument.rootVisualElement
            .Q<Button>("WinRestartButton");
        m_LoseRestartButton = uiDocument.rootVisualElement
            .Q<Button>("LoseRestartButton");

        m_WinScreen.style.display = DisplayStyle.None;
        m_LoseScreen.style.display = DisplayStyle.None;
        m_StartScreen.style.display = DisplayStyle.None;
    }

    public void ShowStartScreen(Action startGame, Action restartGame)
    {
        m_StartButton.clicked += () =>
        {
            m_StartScreen.style.display = DisplayStyle.None;
            startGame?.Invoke();
        };
        m_WinRestartButton.clicked += () => restartGame?.Invoke();
        m_LoseRestartButton.clicked += () => restartGame?.Invoke();
        m_StartScreen.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f;
    }

    public void SetHealthValue(float percentage)
    {
        m_Healthbar.style.width =
            Length.Percent(100 * percentage);
    }

    private void Update()
    {
        if (m_TimerDisplay > 0)
        {
            m_TimerDisplay -= Time.deltaTime;

            if (m_TimerDisplay < 0)
            {
                m_NonPlayerDialogue.style.display =
                    DisplayStyle.None;
            }
        }
    }

    public void DisplayDialogue()
    {
        m_NonPlayerDialogue.style.display =
            DisplayStyle.Flex;

        m_TimerDisplay = displayTime;
    }

    public void DisplayWinScreen()
    {
        m_WinScreen.style.display = DisplayStyle.Flex;
        m_WinScreen.style.opacity = 1.0f;
    }

    public void DisplayLoseScreen()
    {
        m_LoseScreen.style.display = DisplayStyle.Flex;
        m_LoseScreen.style.opacity = 1.0f;
    }
}
