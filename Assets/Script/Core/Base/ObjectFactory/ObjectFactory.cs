using UnityEngine;
using UnityEngine.SceneManagement;



public class ObjectFactory : SingletonBase<ObjectFactory>
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private GameObject optionWindow;

    [SerializeField]
    private GameObject playerWindow;

    [SerializeField]
    private GameObject nonPlayerWindow;


    public GameObject InstanceObject(EnumList.ObjectGenelateList genelateObject) {
        GameObject obj = null;
        switch (genelateObject)
        {
            case EnumList.ObjectGenelateList.Monster:

                break;

            default:

                break;
        }
        return obj;
    }

    protected override void OnInitialize(Scene scene, LoadSceneMode mode)
    {
        base.OnInitialize(scene, mode);
        if (scene.isLoaded) {
            Debug.LogWarning($"{scene.name}���� �ε��� �Ϸ� �Ǿ���");
            //�� �ε��� �Ϸ�ȵ� ó���� ����
            if (GameObject.FindObjectsOfType<Canvas>().Length > 1 ) {
                //ĵ������ �ΰ��̻��ϰ�� �ߺ��ؼ� â�� ���°��̾ƴ϶� 
                //�ϳ��� ĵ������ ȭ�鿡 ���ü��ֵ��� �ٸ� ĵ������ ���ߴ� �۾����ʿ�
            }
            else 
            {
                //ĵ������ �ϳ��ϰ��� �ڽ��� ĵ������ ǥ���ϵ��� �ϴ·���
            }
        }
        
    }
}
