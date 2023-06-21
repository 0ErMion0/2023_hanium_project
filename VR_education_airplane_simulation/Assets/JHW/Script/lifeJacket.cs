using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class lifeJacket : MonoBehaviour
{
    [SerializeField] GameObject equipedJacketModel;
    [SerializeField] GameObject targetObj;

    [SerializeField] GameObject equipedJacket;
    [SerializeField] List<GameObject> beltPos;

    public void EquipJacket()
    {
        equipedJacket.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void ChangeHandPos()
    {

    }

    public void JacketBeltSelected()
    {

    }

    public void JacketBeltSelectEntered()
    {
        // ������ ���� ��ġ �ľ��ؼ� ��Ʈ ������
        Vector3 startPos = beltPos[0].transform.position;
        Vector3 destPos = beltPos[beltPos.Count-1].transform.position;
        Vector3 diff = startPos - destPos;

        // ��Ʈ ��ġ ������
        for (int i = 1; i < beltPos.Count-2; i++)
        {
            Debug.Log(beltPos[i].transform.position);
        }
    }

    public void JacketBeltSelectExited()
    {

    }
}
