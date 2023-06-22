using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// �⺻������ ������ �����Ǵ� ��ü ����
/// EnumList.MultipleFactoryObjectList �̰��� ��ü�߰��ɳ��� �����߰��Ͻø�˴ϴ�.
/// </summary>
public class MultipleObjectsFactory : Singleton<MultipleObjectsFactory>
    {
        /// <summary>
        /// ����ȭ�鿡 ������ ������ƮǮ
        /// </summary>
        SaveDataPool saveDataPool;

        /// <summary>
        /// ���丮 ������ �ʱ�ȭ �Լ�
        /// </summary>
        /// <param name="scene">������ �����ʿ����</param>
        /// <param name="mode">������� �����ʿ����</param>
        protected override void Init(Scene scene, LoadSceneMode mode)
    {
#if UNITY_EDITOR
        Debug.LogWarning("������ �������϶����־ üũ 2��");
#endif
        saveDataPool = GetComponentInChildren<SaveDataPool>();
            base.Init(scene, mode);
            saveDataPool.Initialize();
        }

        /// <summary>
        /// ��ü �����ϱ�
        /// </summary>
        /// <param name="type">��ü����</param>
        /// <returns>������ ��ü</returns>
        public GameObject GetObject(EnumList.MultipleFactoryObjectList type)
        {
            GameObject obj = null;
            switch (type)
            {
                case EnumList.MultipleFactoryObjectList.SAVEDATAPOOL:
                    obj = saveDataPool?.GetObject()?.gameObject;
                    break;

                default:

                    break;
            }
            return obj;
        }
    }


