using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : DontDestroy<SoundManager>
{
    public enum AudioType
    {
        BGM,
        SFX,
        Max
    }
    public enum BgmClip
    {
        BGM01,
        Max
    }
    public enum SfxClip
    {
        Get_Coin,
        Get_Gem,
        Get_invincible,
        Get_Item,
        Mon_Die,
        Max
    }
    
    AudioSource[] m_audio;
    [SerializeField]
    AudioClip[] m_bgmClips;
    [SerializeField]
    AudioClip[] m_sfxClips;
    const int MaxPlay = 3;
    Dictionary<AudioClip, int> m_sfxPlayList = new Dictionary<AudioClip, int>();
    public void PlayBGM(BgmClip bgm)
    {
        
        m_audio[(int)AudioType.BGM].clip = m_bgmClips[(int)bgm];        //현재 실행중인거에서 bgm으로 클립을 바꾸기
        m_audio[(int)AudioType.BGM].Play();     //audio 실행
    }
    IEnumerator Coroutine_CheckSFX(AudioClip clip )
    {
        yield return new WaitForSeconds(clip.length);   //오디오의 재생 길이. 길이가 되면
        if (m_sfxPlayList[clip] > 0)
            m_sfxPlayList[clip]--;
        else m_sfxPlayList.Remove(clip); 
    }
    public void PlaySFX(SfxClip sfx)
    {
        var clip = m_sfxClips[(int)sfx];        //해당 클립 가져오기
        if(m_sfxPlayList.ContainsKey(clip))     //Dictioanry 에 해당 클립이 있으면
        {
            if (m_sfxPlayList[clip] < MaxPlay)      //dictionary 의 int값이 max보다 작으면
                m_sfxPlayList[clip]++;              
            else       //더크면
                return;
        }
        else           // 값이 없으면
        {
            m_sfxPlayList.Add(clip, 1);     //딕셔너리에 하나 추가
        }
        StartCoroutine(Coroutine_CheckSFX(clip));
        m_audio[(int)AudioType.SFX].PlayOneShot(m_sfxClips[(int)sfx]);     //다른곳에서 재생하는 오디오를 취소하지않고 실행하기
    }
    public void SetVolumeBGM(int level)
    {
        m_audio[(int)AudioType.BGM].volume = level; //볼륨 조절
    }
    public void SetVolumeSFX(int level)
    {
        m_audio[(int)AudioType.SFX].volume = level;     // 볼륨 조절
    }
    public void SetVolume(float level)      //전체 볼륨 조절
    {
        m_audio[(int)AudioType.BGM].volume = level;
        m_audio[(int)AudioType.SFX].volume = level;
    }
    public void SetMute(bool isMute)
    {
        m_audio[(int)AudioType.BGM].mute = isMute;
        m_audio[(int)AudioType.SFX].mute = isMute;
        
    }
    // Start is called before the first frame update
    protected override void OnAwake()
    {
        m_audio = new AudioSource[(int)AudioType.Max];
        m_audio[(int)AudioType.BGM] = gameObject.AddComponent<AudioSource>();   //gameobject에 audio source 추가
        m_audio[(int)AudioType.BGM].loop = true;            //소리 루프 돌리기
        m_audio[(int)AudioType.BGM].playOnAwake = false;                    //시작할때 끄기
        m_audio[(int)AudioType.BGM].rolloffMode = AudioRolloffMode.Linear;  //평범하게 소리 줄어드는

        m_audio[(int)AudioType.SFX] = gameObject.AddComponent<AudioSource>();   //gameobject에 audio source 추가
        m_audio[(int)AudioType.SFX].loop = false;       
        m_audio[(int)AudioType.SFX].playOnAwake = false;                    //시작할때 끄기
        m_audio[(int)AudioType.SFX].rolloffMode = AudioRolloffMode.Linear;  //평범하게 소리 줄어드는

        m_bgmClips = Resources.LoadAll<AudioClip>("Sound/BGM");
        
        m_sfxClips = Resources.LoadAll<AudioClip>("Sound/SFX");
        //PlayBGM(BgmClip.BGM01);


    }

    

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for(int i=0; i<10; i++)
            {
                PlaySFX(SfxClip.Mon_Die);
            }
        }
    }
}
