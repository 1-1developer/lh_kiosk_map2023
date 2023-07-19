using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using System;

public class MainScreenTraffic : MenuScreen
{
    const string HOMEBT = "HomeBtT";
    const string BG = "BGbtt";
    const string TBUBBLE = "TraficBubble";
    const string POP = "TrafficPopWindow";

    public Sprite[] popContents;

    Button m_HomeBt;
    VisualElement m_TrafficPopWindow;

    List<VisualElement> m_TrafficBts = new List<VisualElement>();
    Button m_BgBt;

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_HomeBt = m_Root.Q<Button>(HOMEBT);
        m_TrafficPopWindow = m_Root.Q<VisualElement>(POP);
        m_BgBt = m_Root.Q<Button>(BG);
        for (int i = 0; i < 4; i++)
        {
            m_TrafficBts.Add(m_Root.Q<VisualElement>(TBUBBLE + $"{i}"));
        }
    }

    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();
        m_HomeBt.RegisterCallback<ClickEvent>(OnHomeBt);
        m_BgBt.RegisterCallback<ClickEvent>(OnBackBt);

        m_TrafficBts[0].RegisterCallback<ClickEvent>(evt => onTraffic(0));
        m_TrafficBts[1].RegisterCallback<ClickEvent>(evt => onTraffic(1));
        m_TrafficBts[2].RegisterCallback<ClickEvent>(evt => onTraffic(2));
        m_TrafficBts[3].RegisterCallback<ClickEvent>(evt => onTraffic(3));
    }

    private void OnBackBt(ClickEvent evt)
    {
        AudioManager.PlayDefaultButtonSound();
        m_TrafficPopWindow.style.display = DisplayStyle.None;
    }

    private void onTraffic(int v)
    {
        AudioManager.PlayDefaultButtonSound();
        m_TrafficPopWindow.style.backgroundImage = popContents[v].texture;
        m_TrafficPopWindow.style.display = DisplayStyle.Flex;
    }

    private void OnHomeBt(ClickEvent evt)
    {
        AudioManager.PlayDefaultButtonSound();
        m_MainMenuUIManager.ShowHomeScreen();
    }
}
