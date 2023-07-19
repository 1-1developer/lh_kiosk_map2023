using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UIElements;


public class MainScreenNav : MenuScreen
{
    const string MAPBT = "MapBt";
    const string HOMEBT = "HomeBt";
    const string BUBBLE = "MapBubble";
    const string SPOT = "MapSpot";
    const string BG = "BGbt";
    const string INFOBACK = "infoBackBt";
    const string MAPPOP = "MapPopWindow";
    const string TOPINFO = "Topinfo";
    const string MOTIONS = "MotionScreen";
    const string INFOG = "infoG";
    const string RETURN = "returnBt";

    public Sprite[] spotSprites;

    public Sprite[] POPinfos;
    public Sprite[] Topinfos;

    //���� ���ǵ� �Ｚ ��
    public Sprite[] Routes0; //���â��
    public Sprite[] Routes1; //���â��
    public Sprite[] Routes2; //���â��
    public Sprite[] Routes3; //��õ����,��õ���
    public Sprite[] Routes4; //��õ����,��õ���
    public Sprite[] Routes5; //��õ����,��õ���
    public Sprite[] Routes6; //�����ֿռ�2
    public Sprite[] Routes7; //�����ֿռ�2
    public Sprite[] Routes8; //�����ֿռ�2
    public Sprite[] Routes9; //�����ֿռ�
    public Sprite[] Routes10; //�����ֿռ�
    public Sprite[] Routes11; //�����ֿռ�
    public Sprite[] Routes12; //�ϳ�����
    public Sprite[] Routes13; //�ϳ�����
    public Sprite[] Routes14; //�ϳ�����

    Button m_HomeBt;
    Button m_BgBt;
    Button m_infoBackBt;

    VisualElement m_MostionScreen; //��ã�� ��� ���, ���� info ��������
    VisualElement m_infoG; // info ��������
    VisualElement m_MapPopWindow;
    VisualElement m_Topinfo;

    List<Sprite[]> allRoutes = new List<Sprite[]>();
    List<Button> m_MapBts = new List<Button>();
    List<Button> m_MapSpots = new List<Button>();
    List<VisualElement> m_MapBubble = new List<VisualElement>();

    List<VisualElement> m_MapPickers = new List<VisualElement>();
    List<Button> m_MapPickerBt = new List<Button>();

    int index;
    public int r_index;
    int m_MapID; //��������ID
    int m_SpotID; //����ö�� ����ID
    int m_IDID; //����ö�� ����ID

    bool MapSelected =false;
    public bool Motionstop =false;

    Coroutine runningCoroutine = null;
    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        m_HomeBt = m_Root.Q<Button>(HOMEBT);
        m_BgBt = m_Root.Q<Button>(BG);
        m_infoBackBt = m_Root.Q<Button>(INFOBACK);

        m_MostionScreen = m_Root.Q<VisualElement>(MOTIONS);
        m_infoG = m_Root.Q<VisualElement>(INFOG);
        m_MapPopWindow = m_Root.Q<VisualElement>(MAPPOP);
        m_Topinfo = m_Root.Q<VisualElement>(TOPINFO);


        for (int i = 0; i < 6; i++)
        {
            m_MapBts.Add( m_Root.Q<Button>(MAPBT+$"{i}"));
            m_MapBubble.Add( m_Root.Q<VisualElement>(BUBBLE + $"{i}"));
        }
        for (int i = 0; i < 3; i++)
        {
            m_MapSpots.Add(m_Root.Q<Button>(SPOT + $"{i}"));
        }
        setAllRoutes();
    }
    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();
        m_HomeBt.RegisterCallback<ClickEvent>(OnHomeBt);
        m_BgBt.RegisterCallback<ClickEvent>(OnBgBt);
        m_infoBackBt.RegisterCallback<ClickEvent>(OnBackBt);

        m_MapBts[0].RegisterCallback<ClickEvent>(evt => OnMapBt(0));
        m_MapBts[1].RegisterCallback<ClickEvent>(evt => OnMapBt(1));
        m_MapBts[2].RegisterCallback<ClickEvent>(evt => OnMapBt(2));
        m_MapBts[3].RegisterCallback<ClickEvent>(evt => OnMapBt(3));
        m_MapBts[4].RegisterCallback<ClickEvent>(evt => OnMapBt(4));
        m_MapBts[5].RegisterCallback<ClickEvent>(evt => OnMapBt(5));

        initMap();
        m_MapSpots[0].RegisterCallback<ClickEvent>(evt => OnMapSpot(0));
        m_MapSpots[1].RegisterCallback<ClickEvent>(evt => OnMapSpot(1));
        m_MapSpots[2].RegisterCallback<ClickEvent>(evt => OnMapSpot(2));

        m_MapSpots[0].schedule.Execute(ac=> loopTexture(spotSprites,m_MapSpots[0])).Every(30);
        m_MapSpots[1].schedule.Execute(ac => loopTexture(spotSprites, m_MapSpots[1])).Every(30);
        m_MapSpots[2].schedule.Execute(ac => loopTexture(spotSprites, m_MapSpots[2])).Every(30);

        //m_jobHandle = m_MostionScreen.schedule.Execute(ac => PlayTextureOnce(allRoutes[m_IDID], m_MostionScreen)).Every(10);

    }

    private void OnBackBt(ClickEvent evt)
    {
        m_infoBackBt.style.display = DisplayStyle.None;
    }
    private void OnMapBt(int v)
    {
        m_infoG.style.display = DisplayStyle.None;
        m_MostionScreen.style.display = DisplayStyle.None;
        MapSelected = true;
        AudioManager.PlayDefaultButtonSound();
        showBubble(v);
        m_MapID = v;
    }
    private void OnMapSpot(int v)
    {
        if (MapSelected)//������������
        {
            m_SpotID = v;
            checkID(m_MapID, m_SpotID);
            m_MostionScreen.style.display = DisplayStyle.Flex;
            initMap();
            m_infoG.style.display = DisplayStyle.Flex;
        }
        AudioManager.PlayDefaultButtonSound();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine(runningCoroutine);
            Debug.Log("stop");
        }
    }
    void checkID(int m,int p)//��Ʈ �Ǻ�
    {
        int ID = m*3+p;
        setinfo(ID);
        m_IDID = ID;
        r_index = 0;
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
        runningCoroutine = StartCoroutine(PlayTextureRoutine());
        Debug.Log("start");

        //m_jobHandle = m_MostionScreen.schedule.Execute(ac => PlayTextureOnce(allRoutes[ID], m_MostionScreen)).Every(10);
        //m_MostionScreen.schedule.Execute(ac => PlayTextureOnce(allRoutes[ID], m_MostionScreen)).Every(10);

    }
    void setinfo(int i)
    {
        m_Topinfo.style.backgroundImage = Topinfos[i].texture;
    }
    private IEnumerator PlayTextureRoutine()
    {

        while (r_index < allRoutes[m_IDID].Length)
        {
            // �ؽ�ó ó�� ����
            PlayTextureOnce(allRoutes[m_IDID], m_MostionScreen);
            yield return new WaitForSeconds(0.05f); // 0.1�ʸ��� ����
        }

    }
    void PlayTextureOnce(Sprite[] sprites, VisualElement v)
    {
        if (r_index > sprites.Length - 1)
        {
            StopCoroutine(runningCoroutine);
            Debug.Log("stop");
            return;
        }
        v.style.backgroundImage = sprites[r_index].texture;
        r_index++;
    }
    void loopTexture( Sprite[] sprites,VisualElement v)
    {
        if (index > sprites.Length-1)
        {
            index = 0;
            return;
        }
        v.style.backgroundImage = sprites[index].texture;
        index++;
    }
    void loopTexture( Sprite[] sprites, Button v)
    {
        if (index > sprites.Length - 1)
        {
            index = 0;
            return;
        }
        v.style.backgroundImage = sprites[index].texture;
        index++;
    }
    private void OnBgBt(ClickEvent evt)
    {
        initMap();
    }
    private void initMap()
    {
        for (int i = 0; i < 6; i++)
        {
            m_MapBubble[i].style.display = DisplayStyle.None;
        }
    }


    private void OnHomeBt(ClickEvent evt)
    {
        AudioManager.PlayDefaultButtonSound();
        m_MainMenuUIManager.ShowHomeScreen();
    }
    void showBubble(int i)
    {
        VisualElement vv = m_MapBubble[i];
        foreach (VisualElement b in m_MapBubble)
        {
            if (vv == b)
            {
                b.style.display = DisplayStyle.Flex;
                b.AddToClassList("MapBubble--on");
            }
            else
            {
                b.RemoveFromClassList("MapBubble--on");
                b.style.display = DisplayStyle.None;
            }
        }
    }
    void setAllRoutes()
    {
        allRoutes.Add(Routes0);
        allRoutes.Add(Routes1);
        allRoutes.Add(Routes2);
        allRoutes.Add(Routes3);
        allRoutes.Add(Routes4);
        allRoutes.Add(Routes5);
        allRoutes.Add(Routes3);
        allRoutes.Add(Routes4);
        allRoutes.Add(Routes5);
        allRoutes.Add(Routes6);
        allRoutes.Add(Routes7);
        allRoutes.Add(Routes8);
        allRoutes.Add(Routes9);
        allRoutes.Add(Routes10);
        allRoutes.Add(Routes11);
        allRoutes.Add(Routes12);
        allRoutes.Add(Routes13);
        allRoutes.Add(Routes14);
    }
}
