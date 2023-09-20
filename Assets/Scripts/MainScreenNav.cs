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
    const string DOT = "MapDot";
    const string BG = "BGbt";
    const string INFOBACK = "infoBackBt";
    const string MAPPOP = "MapPopWindow";
    const string TOPINFO = "Topinfo";
    const string TOPINFO2 = "Topinfo2";
    const string MOTIONS = "MotionScreen";
    const string INFOG = "infoG";
    const string RETURN = "returnBt";
    const string GPICKER = "PickerG";

    const string MAPPICKER = "MapPicker";
    const string PICKERBT = "PickerBt";

    public Sprite[] spotSprites;
    public Sprite[] DotSprites;

    public Sprite[] POPinfos;
    public Sprite[] Topinfos;

    //???? ?????? ???? ??
    public Sprite[] Routes0; //????????
    public Sprite[] Routes1; //????????
    public Sprite[] Routes2; //????????
    public Sprite[] Routes3; //????????,????????
    public Sprite[] Routes4; //????????,????????
    public Sprite[] Routes5; //????????,????????
    public Sprite[] Routes6; //??????????2
    public Sprite[] Routes7; //??????????2
    public Sprite[] Routes8; //??????????2
    public Sprite[] Routes9; //??????????
    public Sprite[] Routes10; //??????????
    public Sprite[] Routes11; //??????????
    public Sprite[] Routes12; //????????
    public Sprite[] Routes13; //????????
    public Sprite[] Routes14; //????????

    Button m_HomeBt;
    Button m_BgBt;
    Button m_infoBackBt;

    VisualElement m_MostionScreen; //?????? ???? ????, ???? info ????????
    VisualElement m_infoG; // info ????????
    VisualElement m_MapPopWindow;
    VisualElement m_Topinfo;
    VisualElement m_Topinfo2;

    VisualElement m_GPicker;

    List<Sprite[]> allRoutes = new List<Sprite[]>();
    List<Button> m_MapBts = new List<Button>();
    List<Button> m_MapSpots = new List<Button>();

    List<VisualElement> m_MapDots = new List<VisualElement>();
    List<VisualElement> m_MapBubble = new List<VisualElement>();

    List<VisualElement> m_MapPickers = new List<VisualElement>();
    List<Button> m_MapPickerBt = new List<Button>();

    int index;
    int[] indexsss= new int[3];
    int[] indexs = new int[18];
    public int r_index;
    int m_MapID; //????????ID
    int m_SpotID; //???????? ????ID
    int m_IDID; //???????? ????ID

    bool MapSelected =false;
    public bool Motionstop =false;

    Coroutine runningCoroutine = null;

    int dotms = 20;
    int spotms = 70;
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
        m_Topinfo2 = m_Root.Q<VisualElement>(TOPINFO2);
        m_GPicker = m_Root.Q<VisualElement>(GPICKER);


        for (int i = 0; i < 6; i++)
        {
            m_MapBts.Add( m_Root.Q<Button>(MAPBT+$"{i}"));
            m_MapBubble.Add( m_Root.Q<VisualElement>(BUBBLE + $"{i}"));
        }
        for (int i = 0; i < 3; i++)
        {
            m_MapSpots.Add(m_Root.Q<Button>(SPOT + $"{i}"));
        }
        for (int i = 0; i < 18; i++)
        {
            m_MapDots.Add(m_Root.Q<VisualElement>(DOT + $"{i}"));
        }
        for (int i = 0; i < 18; i++)
        {
            m_MapPickers.Add(m_Root.Q<VisualElement>(MAPPICKER + $"{i}"));
        }
        for (int i = 0; i < 36; i++)
        {
            m_MapPickerBt.Add(m_Root.Q<Button>(PICKERBT + $"{i}"));
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

        m_MapSpots[0].schedule.Execute(ac=> loopTexture(spotSprites,m_MapSpots[0],0)).Every(spotms);
        m_MapSpots[1].schedule.Execute(ac => loopTexture(spotSprites, m_MapSpots[1],1)).Every(spotms);
        m_MapSpots[2].schedule.Execute(ac => loopTexture(spotSprites, m_MapSpots[2],2)).Every(spotms);

        initDotIndex();

        m_MapDots[0].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[0], 0)).Every(dotms);
        m_MapDots[1].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[1], 1)).Every(dotms);
        m_MapDots[2].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[2], 2)).Every(dotms);
        m_MapDots[3].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[3], 3)).Every(dotms);
        m_MapDots[4].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[4], 4)).Every(dotms);
        m_MapDots[5].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[5], 5)).Every(dotms);
        m_MapDots[6].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[6], 6)).Every(dotms);
        m_MapDots[7].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[7], 7)).Every(dotms);
        m_MapDots[8].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[8], 8)).Every(dotms);
        m_MapDots[9].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[9], 9)).Every(dotms);
        m_MapDots[10].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[10], 10)).Every(dotms);
        m_MapDots[11].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[11], 11)).Every(dotms);
        m_MapDots[12].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[12], 12)).Every(dotms);
        m_MapDots[13].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[13], 13)).Every(dotms);
        m_MapDots[14].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[14], 14)).Every(dotms);
        m_MapDots[15].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[15], 15)).Every(dotms);
        m_MapDots[16].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[16], 16)).Every(dotms);
        m_MapDots[17].schedule.Execute(ac => loopTexture(DotSprites, m_MapDots[17], 17)).Every(dotms);



        m_MapPickerBt[0].RegisterCallback<ClickEvent>(evt => onMapPickerBt(0));
        m_MapPickerBt[1].RegisterCallback<ClickEvent>(evt => onMapPickerBt(1));
        m_MapPickerBt[2].RegisterCallback<ClickEvent>(evt => onMapPickerBt(2));
        m_MapPickerBt[3].RegisterCallback<ClickEvent>(evt => onMapPickerBt(3));
        m_MapPickerBt[4].RegisterCallback<ClickEvent>(evt => onMapPickerBt(4));
        m_MapPickerBt[5].RegisterCallback<ClickEvent>(evt => onMapPickerBt(5));
        m_MapPickerBt[6].RegisterCallback<ClickEvent>(evt => onMapPickerBt(6));
        m_MapPickerBt[7].RegisterCallback<ClickEvent>(evt => onMapPickerBt(7));
        m_MapPickerBt[8].RegisterCallback<ClickEvent>(evt => onMapPickerBt(8));
        m_MapPickerBt[9].RegisterCallback<ClickEvent>(evt => onMapPickerBt(9));
        m_MapPickerBt[10].RegisterCallback<ClickEvent>(evt => onMapPickerBt(10));
        m_MapPickerBt[11].RegisterCallback<ClickEvent>(evt => onMapPickerBt(11));
        m_MapPickerBt[12].RegisterCallback<ClickEvent>(evt => onMapPickerBt(12));
        m_MapPickerBt[13].RegisterCallback<ClickEvent>(evt => onMapPickerBt(13));
        m_MapPickerBt[14].RegisterCallback<ClickEvent>(evt => onMapPickerBt(14));
        m_MapPickerBt[15].RegisterCallback<ClickEvent>(evt => onMapPickerBt(15));
        m_MapPickerBt[16].RegisterCallback<ClickEvent>(evt => onMapPickerBt(16));
        m_MapPickerBt[17].RegisterCallback<ClickEvent>(evt => onMapPickerBt(17));
        m_MapPickerBt[18].RegisterCallback<ClickEvent>(evt => onMapPickerBt(18));
        m_MapPickerBt[19].RegisterCallback<ClickEvent>(evt => onMapPickerBt(19));
        m_MapPickerBt[20].RegisterCallback<ClickEvent>(evt => onMapPickerBt(20));
        m_MapPickerBt[21].RegisterCallback<ClickEvent>(evt => onMapPickerBt(21));
        m_MapPickerBt[22].RegisterCallback<ClickEvent>(evt => onMapPickerBt(22));
        m_MapPickerBt[23].RegisterCallback<ClickEvent>(evt => onMapPickerBt(23));
        m_MapPickerBt[24].RegisterCallback<ClickEvent>(evt => onMapPickerBt(24));
        m_MapPickerBt[25].RegisterCallback<ClickEvent>(evt => onMapPickerBt(25));
        m_MapPickerBt[26].RegisterCallback<ClickEvent>(evt => onMapPickerBt(26));
        m_MapPickerBt[27].RegisterCallback<ClickEvent>(evt => onMapPickerBt(27));
        m_MapPickerBt[28].RegisterCallback<ClickEvent>(evt => onMapPickerBt(28));
        m_MapPickerBt[29].RegisterCallback<ClickEvent>(evt => onMapPickerBt(29));
        m_MapPickerBt[30].RegisterCallback<ClickEvent>(evt => onMapPickerBt(30));
        m_MapPickerBt[31].RegisterCallback<ClickEvent>(evt => onMapPickerBt(31));
        m_MapPickerBt[32].RegisterCallback<ClickEvent>(evt => onMapPickerBt(32));
        m_MapPickerBt[33].RegisterCallback<ClickEvent>(evt => onMapPickerBt(33));
        m_MapPickerBt[34].RegisterCallback<ClickEvent>(evt => onMapPickerBt(34));
        m_MapPickerBt[35].RegisterCallback<ClickEvent>(evt => onMapPickerBt(35));


    }
    void initDotIndex()
    {
        for (int i = 0; i < indexs.Length; i++)
        {
            indexs[i] = 0;
        }
        for (int i = 0; i < indexsss.Length; i++)
        {
            indexsss[i] = 0;
        }
    }
    private void onMapPickerBt(int v)
    {
        AudioManager.PlayDefaultButtonSound();
        m_infoBackBt.style.display = DisplayStyle.Flex;
        m_MapPopWindow.style.backgroundImage = POPinfos[v].texture;
    }

    private void OnBackBt(ClickEvent evt)
    {
        AudioManager.PlayDefaultButtonSound();
        m_infoBackBt.style.display = DisplayStyle.None;
    }
    private void OnMapBt(int v)
    {
        MapBtHighlight(v);
        if (!m_MapBts[v].ClassListContains("MapBt--focus"))
        {
            m_MapBts[v].AddToClassList("MapBt--focus");
        }
        m_GPicker.style.display = DisplayStyle.None;
        m_infoG.style.display = DisplayStyle.None;
        m_MostionScreen.style.display = DisplayStyle.None;
        MapSelected = true;
        AudioManager.PlayDefaultButtonSound();
        showBubble(v);
        m_MapID = v;
    }
    private void OnMapSpot(int v)
    {
        if (MapSelected)//????????????
        {
            m_SpotID = v;
            checkID(m_MapID, m_SpotID);
            m_MostionScreen.style.display = DisplayStyle.Flex;
            initMap();
            m_infoG.style.display = DisplayStyle.Flex;
        }
        AudioManager.PlayDefaultButtonSound();
    }


    void checkID(int m,int p)//???? ????
    {
        int ID = m*3+p;
        setinfo(ID);
        m_IDID = ID;
        r_index = 0;
        showPickers(ID);
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
        runningCoroutine = StartCoroutine(PlayTextureRoutine());
        m_GPicker.style.display = DisplayStyle.Flex;
    }
    void setinfo(int i)
    {
        if( i == 9 || i== 10 || i ==11 )
        {
            m_Topinfo2.style.display = DisplayStyle.Flex;
            m_Topinfo.style.display = DisplayStyle.None;
            m_Topinfo2.style.backgroundImage = Topinfos[i].texture; 
        }
        else
        {
            m_Topinfo.style.display = DisplayStyle.Flex;
            m_Topinfo2.style.display = DisplayStyle.None;
            m_Topinfo.style.backgroundImage = Topinfos[i].texture;
        }
    }
    private IEnumerator PlayTextureRoutine()
    {

        while (r_index < allRoutes[m_IDID].Length)
        {
            // ?????? ???? ????
            PlayTextureOnce(allRoutes[m_IDID], m_MostionScreen);
            yield return new WaitForSeconds(0.03f); // 0.1?????? ????
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
    void loopTexture( Sprite[] sprites,VisualElement v,int i)
    {
        if (indexs[i] > sprites.Length-1)
        {
            indexs[i] = 0;
            return;
        }
        v.style.backgroundImage = sprites[indexs[i]].texture;
        indexs[i]++;
    }
    void loopTexture( Sprite[] sprites, Button v, int i)
    {
        if (index > sprites.Length - 1)
        {
            index = 0;
            return;
        }
        v.style.backgroundImage = sprites[index].texture;
        Debug.Log($"{v.name}:{index}");
        index++;
    }
    private void OnBgBt(ClickEvent evt)
    {
        AudioManager.PlayDefaultButtonSound();
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
        initNav();
    }
    void MapBtHighlight(int i)
    {
        Button vv = m_MapBts[i];
        foreach (Button b in m_MapBts)
        {
            if (vv == b)
            {
                b.RemoveFromClassList("MapBt--un");
                b.AddToClassList("MapBt--focus");

            }
            else
            {
                b.AddToClassList("MapBt--un");
                b.RemoveFromClassList("MapBt--focus");

            }
        }
    }
    void initMapBt()
    {
        for (int i = 0; i < m_MapBts.Count; i++)
        {
            m_MapBts[i].RemoveFromClassList("MapBt--un");
            m_MapBts[i].RemoveFromClassList("MapBt--focus");
        }
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
    void showPickers(int index)
    {
        VisualElement v = m_MapPickers[index];
        foreach (VisualElement a in m_MapPickers)
        {
            if (a == v)
            {
                a.style.display = DisplayStyle.Flex;
            }
            else
            {
                a.style.display = DisplayStyle.None;
            }
        }
    }
    public void initNav()
    {
        m_GPicker.style.display = DisplayStyle.None;
        m_infoG.style.display = DisplayStyle.None;
        m_MostionScreen.style.display = DisplayStyle.None;
        initMapBt();
    }
}
