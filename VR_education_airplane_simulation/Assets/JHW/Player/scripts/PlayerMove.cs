using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Vector3 offsetVector;
    [SerializeField] float playerSpeed;
    [SerializeField] float rotateSpeed = 5;
    [SerializeField] GameObject playerXROrigin;

    Animator ani;
    bool isKeyDown;
    Vector3 dir;

    bool isMoveAble;

    private void Start()
    {
        isMoveAble = true;
    }

    private void Update()
    {
        // ����Ű
        //float hAxis = Input.GetAxisRaw("Horizontal");
        //float vAxis = Input.GetAxisRaw("Vertical");
        //if (hAxis == 0 && vAxis == 0) { ani.SetBool("walk",false); isKeyDown = false; }
        //else { isKeyDown = true; ani.SetBool("walk", true); dir = new Vector3(hAxis, 0, vAxis); }

        float X_Move = Input.GetAxisRaw("Oculus_GearVR_RThumbstickX");
        float Y_Move = Input.GetAxisRaw("Oculus_GearVR_RThumbstickY");
        
        // ���̽�ƽ ��¦�� ������� �̵����� �����ɷ� ����
        if(Mathf.Abs(X_Move) <= .5f && Mathf.Abs(Y_Move) <= .5f) {  isKeyDown = false; }
        else { dir = new Vector3 (X_Move,0, Y_Move); isKeyDown = true; }

        // �̵� �� ȸ��
        if (isKeyDown)
        {
            dir = transform.TransformDirection(dir);    // ���� ���͸� ���� ���ͷ�

            // ȸ��
            Vector3 rotationDirection = dir.normalized;
            if (rotationDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
                playerXROrigin.transform.rotation = Quaternion.Slerp(playerXROrigin.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            }

            // �̵�
            if(isMoveAble) playerXROrigin.transform.position += dir * playerSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isMoveAble = false;
    }
    private void OnTriggerExit(Collider other)
    {
        isMoveAble = true;
    }

}
