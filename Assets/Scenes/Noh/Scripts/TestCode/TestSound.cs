using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ʈ���� �׽�Ʈ��
/// </summary>
public class TestSound : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        //OptionsData.Instance.EffectSoundPlay(EnumList.EffectSound.Explosion1);
        OptionsData.Instance.GetAudioFile();
    }
}
