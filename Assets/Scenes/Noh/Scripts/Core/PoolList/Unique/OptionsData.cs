using System.IO;
using UnityEngine;

/// <summary>
/// �ɼ�â ������ �̱��� ���� �������̶� �����ȵ� �ϴ� �Ҹ�������
/// </summary>
public class OptionsData : Singleton<OptionsData>
{
    /// <summary>
    /// ����� �Ȳ���� ������ٴҺ���
    /// </summary>
    AudioSource audioSetting;
    protected override void Awake()
    {
        audioSetting = GetComponent<AudioSource>();
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="value"></param>
    public void SetVolumn(float value)
    {
        audioSetting.volume = value;
    }
    public void SetMute(bool flag)
    {
        audioSetting.mute = flag;
    }
    public void EffectSoundPlay(EnumList.EffectSound soundType)
    {
        switch (soundType)
        {
            case EnumList.EffectSound.Explosion1:
                break;
            case EnumList.EffectSound.Explosion2:
                break;
            case EnumList.EffectSound.Explosion3:
                break;
            default:
                break;
        }
    }
    public void GetAudioFile()
    {
   
            Debug.Log(System.IO.File.Exists("Assets/SoundFiles/BGM/Piano Instrumental 1.wav"));
            AudioSource adTest = new AudioSource();
            

        //Component comp;
    }
}