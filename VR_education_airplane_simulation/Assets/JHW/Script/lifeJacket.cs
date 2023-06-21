using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class lifeJacket : MonoBehaviour
{
    [SerializeField] GameObject equipedJacketModel;
    [SerializeField] GameObject targetObj;

    [SerializeField] GameObject equipedJacket;
    [SerializeField] List<GameObject> beltPos;

    [SerializeField] GameObject GrabUX;
    [SerializeField] GameObject lifeJacketDropPos;
    [SerializeField] TextMeshProUGUI txt;


    public void JacketSelectEntered(GameObject _obj)
    {
        lifeJacketDropPos.SetActive(true);
        this.transform.position = _obj.transform.position;
        txt.text = "���� �°��� ���������� �����ּ���.";
    }
    public void JacketSelectExited()
    {
        lifeJacketDropPos.SetActive(false);
        this.gameObject.SetActive(false);
        equipedJacket.SetActive(true);
        txt.text = "���������� ��Ʈ�� ���ּ���.";

    }

    public void JacketBeltSelected()
    {

    }

    public void JacketBeltSelectEntered()
    {
        // UX OFF
        GrabUX.SetActive(false);

        // ������ ���� ��ġ �ľ��ؼ� ��Ʈ ������
        Vector3 startPos = beltPos[0].transform.position;
        Vector3 destPos = beltPos[beltPos.Count-1].transform.position;
        Vector3 diff = startPos - destPos;

        // ��Ʈ ��ġ ������
        for (int i = 1; i < beltPos.Count-2; i++)
        {
            //Debug.Log(diff);
            //Debug.Log(beltPos[i].transform.position);
            Vector3 newPos = beltPos[i].transform.position - diff/i;
            Debug.Log(newPos);
            beltPos[i].transform.position = newPos;
        }
    }

    public void JacketBeltSelectExited()
    {
        GrabUX.SetActive(true);
    }
}
