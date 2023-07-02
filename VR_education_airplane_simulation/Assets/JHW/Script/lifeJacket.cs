using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

partial class lifeJacket : MonoBehaviour
{
    [SerializeField] GameObject equipedJacketModel;
    [SerializeField] GameObject targetObj;

    [SerializeField] GameObject equipedJacket;
    [SerializeField] List<GameObject> beltPos;

    [SerializeField] GameObject BeltGrabUX;
    [SerializeField] GameObject BeltDropUX;
    [SerializeField] GameObject lifeJacketDropPos; // �������� ���ƾ� �� ��ġ
    [SerializeField] GameObject BeltStartPos; // ��Ʈ �տ� ��� ����
    [SerializeField] GameObject BeltEndPos; // ��Ʈ ���� ����
    [SerializeField] GameObject CharacterCenter; // ĳ���� �߽���

    [SerializeField] TextMeshProUGUI txt; // �ڸ�

    private Vector3 originLifeJacketPos;
    private Vector3 originBeltStartPos;
    private Vector3 originBeltEndPos;

    private void Start()
    {
        originLifeJacketPos = this.transform.position;
        originBeltStartPos = BeltStartPos.transform.position;
        originBeltEndPos = BeltEndPos.transform.position;
    }

    public void JacketSelectEntered(XRBaseInteractor interactor)
    {
        lifeJacketDropPos.SetActive(true);
    }
    public void JacketSelectExited(XRBaseInteractor interactor)
    {
        Debug.Log(originLifeJacketPos); // ���� �������� ��ġ
        Debug.Log(transform.position); // ������ ������ �� ��ǥ

        Debug.Log("�Ÿ� = " + Vector3.Distance(lifeJacketDropPos.transform.position, transform.position));
        if (Vector3.Distance(lifeJacketDropPos.transform.position, transform.position) >= 0.17f) // ���ƾ� �� ���� �� ���Ҵٸ� ���� ��ġ��
        {
            this.transform.position = originLifeJacketPos;
            this.transform.rotation = Quaternion.Euler(0, 90, 0);
            return;
        }
        lifeJacketDropPos.SetActive(false);
        this.gameObject.SetActive(false);
        equipedJacket.SetActive(true);
        // �ڸ� ����
        string key = "lifeJacket_script3";
        txt.GetComponent<LocalizeStringEvent>().StringReference.SetReference("LifeJacket_StringTable", key);
        TextToSpeach.Instance.SpeechText(txt.GetComponent<LocalizeStringEvent>().StringReference.GetLocalizedString(key));
        txt.DOText(txt.text, txt.GetComponent<LocalizeStringEvent>().StringReference.GetLocalizedString(key).Length * 0.1f).From("");
    }
    public void JacketBeltSelectEntered(XRBaseInteractor interactor)
    {

        // UX OFF/ON
        BeltGrabUX.SetActive(false);
        BeltDropUX.SetActive(true);


        // ������ ���� ��ġ �ľ��ؼ� ��Ʈ ������
        //Vector3 startPos = beltPos[0].transform.position;
        //Vector3 destPos = beltPos[beltPos.Count-1].transform.position;
        //Vector3 diff = startPos - destPos;
    }

    public void JacketBeltSelectExited(XRBaseInteractor interactor)
    {
        Debug.Log("�Ÿ� = " + Vector3.Distance(BeltEndPos.transform.position, BeltStartPos.transform.position));
        if (Vector3.Distance(BeltEndPos.transform.position, BeltStartPos.transform.position) >= 0.15f) // ���ƾ� �� ���� �� ���Ҵٸ� ���� ��ġ��
        {
            BeltStartPos.transform.position = originBeltStartPos;
            BeltStartPos.transform.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        BeltGrabUX.SetActive(true);
        BeltDropUX.SetActive(false);

        // �ڸ� ����
        string key = "lifeJacket_script4";
        txt.GetComponent<LocalizeStringEvent>().StringReference.SetReference("LifeJacket_StringTable", key);
        TextToSpeach.Instance.SpeechText(txt.GetComponent<LocalizeStringEvent>().StringReference.GetLocalizedString(key));
        txt.DOText(txt.text, txt.GetComponent<LocalizeStringEvent>().StringReference.GetLocalizedString(key).Length * 0.1f).From("");

        // �ڸ� �� �а��� �̵�
        StartCoroutine(MoveToExit(txt.GetComponent<LocalizeStringEvent>().StringReference.GetLocalizedString(key).Length * 0.1f + 3.0f));

        for (int i = 0; i < CharacterCenter.transform.childCount; i++)
        {
            //beltPos[i].transform.position = new Vector3(beltPos[i].transform.position.x, BeltEndPos.transform.position.y, beltPos[i].transform.position.z);
            beltPos[i].transform.DOMove(CharacterCenter.transform.GetChild(i).position, .5f);
        }
        beltPos[CharacterCenter.transform.childCount - 1].transform.rotation = Quaternion.Euler(0, 0, 90);


    }
}

partial class lifeJacket
{

    [Header("=== ��� �������� ===")]
    [SerializeField] GameObject npcObj;
    [SerializeField] GameObject xrOriginObj;
    [SerializeField] Transform npcMovePos; // npc �̵���ġ
    [SerializeField] Transform xrOriginMovePos; // xr �̵���ġ

    [SerializeField] GameObject leftHandle; // ���� ������
    [SerializeField] GameObject rightHandle; // ������ ������
    [SerializeField] GameObject jacketTube; // ��Ŷ Ʃ��

    bool isRightSelected;
    bool isLeftSelected;

    IEnumerator MoveToExit(float _duration)
    {
        // �ڸ� �� ���������� ���
        yield return new WaitForSeconds(_duration);

        // �÷��̾�, npc �̵�
        npcObj.transform.GetComponent<Transform>().position = npcMovePos.position;
        xrOriginObj.transform.GetComponent<Transform>().position = xrOriginMovePos.position;

        // �ڸ� ����
        string key = "lifeJacket_script5";
        txt.GetComponent<LocalizeStringEvent>().StringReference.SetReference("LifeJacket_StringTable", key);
        TextToSpeach.Instance.SpeechText(txt.GetComponent<LocalizeStringEvent>().StringReference.GetLocalizedString(key));
        txt.DOText(txt.text, txt.GetComponent<LocalizeStringEvent>().StringReference.GetLocalizedString(key).Length * 0.1f).From("");

        // �ڸ� �� ������ ���� ��ũ��Ʈ ����
        StartCoroutine(InflateJacket(txt.GetComponent<LocalizeStringEvent>().StringReference.GetLocalizedString(key).Length * 0.1f + 3.0f));
    }

    IEnumerator InflateJacket(float _duration)
    {
        // �ڸ� �� ���������� ���
        yield return new WaitForSeconds(_duration);

        // �ڸ� ����
        string key = "lifeJacket_script6";
        txt.GetComponent<LocalizeStringEvent>().StringReference.SetReference("LifeJacket_StringTable", key);
        TextToSpeach.Instance.SpeechText(txt.GetComponent<LocalizeStringEvent>().StringReference.GetLocalizedString(key));
        txt.DOText(txt.text, txt.GetComponent<LocalizeStringEvent>().StringReference.GetLocalizedString(key).Length * 0.1f).From("");

        // ���� ������, ������ ������ setActive true
        leftHandle.SetActive(true);
        rightHandle.SetActive(true);
    }

    public void leftHandleSelected()
    {
        isLeftSelected = true;
        if (isRightSelected) InflateLifeJacket();
        leftHandle.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void rightHandleSelected()
    {
        isRightSelected = true;
        if (isLeftSelected) InflateLifeJacket();
        rightHandle.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void leftHandleExited()
    {
        isLeftSelected = false;
        leftHandle.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void rightHandleExited()
    {
        isRightSelected = false;
        rightHandle.transform.GetChild(0).gameObject.SetActive(true);
    }


    private void InflateLifeJacket()
    {
        leftHandle.transform.parent.DOLocalMoveY(1.2f, 0.5f);
        jacketTube.GetComponent<Transform>().DOScale(130f, .5f);
        leftHandle.SetActive(false);
        rightHandle.SetActive(false);
    }
}


