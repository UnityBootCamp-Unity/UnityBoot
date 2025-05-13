using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
//1. 사운드 구분을 위한 enum을 설계합니다.
public enum SOUND_TYPE //유형만 나눈 enum
{
    BGM,SFX
}
//세부적으로 나누는 enum (이 프로젝트에서는 이거 사용)
public enum BGM
{
    Title,InGame,Boss
}
public enum SFX
{
    Bullet
}

[Serializable]
public class BGMClip
{
    public BGM type;
    public AudioClip clip;
}

[Serializable]
public class SFXClip
{
    public SFX type;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    //2. 클래스에 필요한 필드 값 설계
    [Header("오디오 믹서")]
    public AudioMixer audioMixer;
    public string bgmParameter = "BGM"; //오디오 믹서에 만들어둔 이름
    public string sfxParameter = "SFX";

    [Header("오디오 소스")]
    public AudioSource bgm;
    public AudioSource sfx;

    [Header("오디오 클립")]
    public List<BGMClip> bgm_list;
    public List<SFXClip> sfx_list;

    private Dictionary<BGM, AudioClip> bgm_dict; //BGM 유형에 따른 오디오 클립
    private Dictionary<SFX, AudioClip> sfx_dict; //SFX 유형에 따른 오디오 클립

    //3. 사운드 매니저는 전체 게임에서 1개만 필요하다.(싱글톤)
    //프로퍼티 형태로 만들어보는 인스턴스
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            //딕셔너리 생성
            bgm_dict = new Dictionary<BGM, AudioClip>();
            sfx_dict = new Dictionary<SFX, AudioClip>();

            //유형 별로 등록(BGM)
            foreach(var bgm in bgm_list)
            {
                bgm_dict[bgm.type] = bgm.clip;
            }
            //유형 별로 등록(SFX)
            foreach (var sfx in sfx_list)
            {
                sfx_dict[sfx.type] = sfx.clip;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //1. UI의 BGM, SFX 트는 기능 구현
    //2. UI의 Slider의 onValueChanged 이벤트 쪽에서 적용할 함수 구현
    public void PlayBGM(BGM bgm_type)
    {
        //bgm 딕셔너리 명단에 해당 BGM이 존재한다면 플레이를 진행합니다.
        //C# 매개변수 한정자 out
        //타입 앞에 붙습니다. ex) void Function(out int value);
        //out 한정자가 붙은 매개변수는 참조로 전달이 됩니다.
        //함수 내부에서 무조건 적으로 값을 설정해줘야 합니다.
        if(bgm_dict.TryGetValue(bgm_type, out var clip))
        {
            //지금 클립이 동일하다면, 배경음악이 틀어지고 있다라고 해석할 수 있음.
            if (bgm.clip == clip)
            {
                return;
            }

            bgm.clip = clip;
            bgm.loop = true;
            bgm.Play();
        }
    }

    public void PlaySFX(SFX sfx_type)
    {
        //sfx 딕셔너리 명단에 해당 SFX가 존재한다면 bgm을 1번 실행합니다.
        if (sfx_dict.TryGetValue(sfx_type, out var clip))
        {
            //효과음은 일시적인 플레이를 진행합니다.
            sfx.PlayOneShot(clip);
        }
    }

    //Audio Mixer의 볼륨 단위는 0 db ~ - 80 d까지로 설정되어있습니다.
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat(bgmParameter, Mathf.Log10(volume) * 20);
        //슬라이더 UI 최소 값이 0.0001로 해당 수치로 계산하면 -80
        //최대 값 1인 경우 0으로 계산됩니다.
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(sfxParameter, Mathf.Log10(volume) * 20);
    }

    //누르면 무음이 되는 MuteBGM과 MuteSFX를 구현해주세요.
    //힌트 : -80 수치가 mute
    //UI 중에서는 Toggle
    public void MuteBGM(bool mute)
    {
        //Toggle 키를 체크했다는 전제로 짠 코드
        //삼향 연산
        //조건 ? T : F 로 작성되며 조건이 맞으면 T에 있는 값을, 아니면 F에 있는 값을 처리합니다.
        audioMixer.SetFloat(bgmParameter, mute ? -80.0f : 0f);
    }

    public void MuteSFX(bool mute)
    {
        //Toggle 키를 체크했다는 전제로 짠 코드
        //삼향 연산
        //조건 ? T : F 로 작성되며 조건이 맞으면 T에 있는 값을, 아니면 F에 있는 값을 처리합니다.
        audioMixer.SetFloat(sfxParameter, mute ? -80.0f : 0f);
    }

}
