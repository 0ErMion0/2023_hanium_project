using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class lifeJacket : MonoBehaviour
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
        txt.text = "���� �°��� ���������� �����ּ���.";
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
        txt.text = "���������� ��Ʈ�� ���ּ���.";
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


        for(int i = 0; i < CharacterCenter.transform.childCount; i++)
        {
            //beltPos[i].transform.position = new Vector3(beltPos[i].transform.position.x, BeltEndPos.transform.position.y, beltPos[i].transform.position.z);
            beltPos[i].transform.DOMove(CharacterCenter.transform.GetChild(i).position, .5f);
        }
        beltPos[CharacterCenter.transform.childCount - 1].transform.rotation = Quaternion.Euler(0, 0, 90);
    }
}
