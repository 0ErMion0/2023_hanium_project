using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Vector3 offsetVector;
    [SerializeField] float playerSpeed;
    [SerializeField] float rotateSpeed = 5;
    [SerializeField] GameObject CamOffset;

    Animator ani;
    bool isKeyDown;
    Vector3 dir;
    

    private void Start()
    {
        ani = GetComponent<Animator>();
        isKeyDown = false;
        // ī�޶�
        //cam.transform.position = this.transform.position + offsetVector;
        CamOffset.transform.position = this.transform.position + offsetVector;
    }

    private void Update()
    {
        // ����Ű
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");
        if (hAxis == 0 && vAxis == 0) { ani.SetBool("walk",false); isKeyDown = false; }
        else { isKeyDown = true; ani.SetBool("walk", true); dir = new Vector3(hAxis, 0, vAxis); }

        // �̵� �� ȸ��
        if (isKeyDown)
        {
            transform.position += dir * playerSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);
            // ī�޶�
            //cam.transform.position = this.transform.position + offsetVector;
            CamOffset.transform.position = this.transform.position + offsetVector;
        }


    }
}
