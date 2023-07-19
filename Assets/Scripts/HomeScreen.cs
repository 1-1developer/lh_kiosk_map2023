using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HomeScreen : MenuScreen
{
    const string STARTBT = "StartBt";
    const string GMAIN = "G_Mainbt";
    const string MAINBT = "MainBt0";
    const string MAINBT1 = "MainBt1";
    const string GLOW = "Glow";
    const string GLOWT = "Glow0";

    Button m_StartBt;
    Button m_MainBt0;
    Button m_MainBt1;
    VisualElement m_G_mainbt;

    List<VisualElement> m_MapBtGlows = new List<VisualElement>();
    List<VisualElement> m_TrafficBtGlows = new List<VisualElement>();
    // Start is called before the first frame update
    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_StartBt = m_Root.Q<Button>(STARTBT);
        m_G_mainbt = m_Root.Q<VisualElement>(GMAIN);
        m_MainBt0 = m_Root.Q<Button>(MAINBT);
        m_MainBt1 = m_Root.Q<Button>(MAINBT1);

        for (int i = 0; i < 6; i++)
        {
            m_TrafficBtGlows.Add(m_Root.Q<VisualElement>(GLOWT + $"{i}"));
            m_MapBtGlows.Add(m_Root.Q<VisualElement>(GLOW + $"{i}"));
        }

    }
    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();
        m_StartBt.RegisterCallback<ClickEvent>(OnStartBt);

        m_MainBt0.RegisterCallback<ClickEvent>(evt => OnMainBt(0));
        m_MainBt1.RegisterCallback<ClickEvent>(evt => OnMainBt(1));
    }

    private void OnStartBt(ClickEvent evt)
    {
        AudioManager.PlayDefaultButtonSound();
        ScreenStart();
    }

    void ScreenStart()
    {
        m_StartBt.AddToClassList("StartBt--pade");
        m_StartBt.RegisterCallback<TransitionEndEvent>(OnStart);
    }

    private void OnStart(TransitionEndEvent evt)
    {
        m_StartBt.style.display = DisplayStyle.None;
        m_G_mainbt.style.display = DisplayStyle.Flex;
    }

    private void OnMainBt(int v)
    {
        LoopGlow();
        AudioManager.PlayDefaultButtonSound();
        if (v==0)
        {
            m_MainMenuUIManager.ShowNavScreen();

        }
        else
        {
            m_MainMenuUIManager.ShowTrafficScreen();

        }
        m_G_mainbt.style.display = DisplayStyle.None;
        m_StartBt.style.display = DisplayStyle.Flex;
    }
    void LoopGlow()
    {
        Glow(0);
        Glow(1);
        Glow(2);
        Glow(3);
        Glow(4);
        Glow(5);
    }
    void Glow(int a)
    {
        m_MapBtGlows[a].ToggleInClassList("GlowMap--un");

        m_MapBtGlows[a].RegisterCallback<TransitionEndEvent>
            (
            evt => m_MapBtGlows[a].ToggleInClassList("GlowMap--un")
            );

        m_TrafficBtGlows[a].ToggleInClassList("GlowMap--un");

        m_TrafficBtGlows[a].RegisterCallback<TransitionEndEvent>
            (
            evt => m_TrafficBtGlows[a].ToggleInClassList("GlowMap--un")
            );
    }

}
