using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.XR.Interaction.Toolkit;

public class Window : MonoBehaviour
{
    [SerializeField] GameObject window;
    [SerializeField] TextMeshProUGUI txt; // �ڸ�
    [SerializeField] GameObject stewardess;

    // â��
    [SerializeField] GameObject windowHandle_model;
    private bool isWindowGrabbed = false; // â�� ������ ��Ҵ��� ����
    public float targetYPosition = 6.6f; // ������Ʈ�� �ø� ��ǥ ��ġ
    //private Vector3 existHandlePos; // �������� ���� Transform
    //public Vector3 changeHandlePos; // �������� �ٲ� Transform

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (windowHandle_model.transform.position.y >= targetYPosition)
        {
            this.gameObject.SetActive(false);

            // �ڸ� ����
            string key = "EduBackrestAngle_script9";
            txt.GetComponent<LocalizeStringEvent>().StringReference.SetReference("EduBackrestAngle_String_Table", key);
            TextToSpeach.Instance.SpeechText(txt.GetComponent<LocalizeStringEvent>().StringReference.GetLocalizedString(key));
            txt.DOText(txt.text, txt.GetComponent<LocalizeStringEvent>().StringReference.GetLocalizedString(key).Length * 0.1f).From("");
            stewardess.GetComponent<Animator>().SetBool("Talk", true);
        }
    }

    //public void WindowSelectExited(XRBaseInteractor interactor)
    //{
    //    if (windowHandle_model.transform.position.y >= targetYPosition)
    //    {
    //        Debug.Log("1");
    //        this.gameObject.SetActive(false);
    //    }
    //}
}
