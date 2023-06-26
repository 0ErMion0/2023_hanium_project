using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Localization.Tables;
using UnityEditor.Localization;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Localization.Settings;

// �̱��� �� ���� ����
partial class MenuManager : MonoBehaviour
{
    private static MenuManager instance = null;
    [SerializeField] StringTableCollection stringTableCollection;

    void Awake()
    {
        if (null == instance)
        {
            //�� Ŭ���� �ν��Ͻ��� ź������ �� �������� instance�� �ν��Ͻ��� ������� �ʴٸ�, �ڽ��� �־��ش�.
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //���� �� �̵��� �Ǿ��µ� �� ������ Hierarchy�� ������Ʈ ������ �� �����Ƿ�
            Destroy(this.gameObject);
        }
    }

    //���� �Ŵ��� �ν��Ͻ��� ������ �� �ִ� ������Ƽ. static�̹Ƿ� �ٸ� Ŭ�������� ���� ȣ���� �� �ִ�.
    public static MenuManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Start()
    {

        //// �ؽ�Ʈ ���� �ҷ�����, ���߿� tts ��� ������ ��� ����
        //List<Dictionary<string, object>> list = CSVReader.Read("Localization/MenuScript");

        //for (int i = 0; i < list.Count; i++)
        //{
        //    Debug.Log(list[i]["English(en)"].ToString());
        //    Debug.Log(list[i]["Korean(ko)"].ToString());
        //}

    }
}

// �޴�ȭ�� ��ư
partial class MenuManager : MonoBehaviour
{
    public void MenuButton_OnMouseClick(GameObject _target)
    {
        // ��ư Ŭ���� ������ �Լ�
    }
}