using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

partial class BackrestAngleManager : MonoBehaviour
{
    // �ڸ�
    [SerializeField] TextMeshProUGUI scriptText;
    [SerializeField] LocalizeStringEvent localizeStringEvent;

    // �ڸ� ������ư
    [SerializeField] GameObject nextButton;

    // �¹���
    [SerializeField] GameObject stewardess;

    // ��ȣ�ۿ� Ȱ��ȭ
    bool windowAct = false;
    bool backrestAct = false;
    bool tableAct = false;
    bool tablePinAct = false;
    bool bagAct = false;

    // Start is called before the first frame update
    void Start()
    {
        NextButtonOnClick();
    }

    // ���� �ڸ� �о����
    int scriptIndex = 0;
    IEnumerator NextScript()
    {
        string key = "EduBackrestAngle_script" + scriptIndex;

        localizeStringEvent.StringReference.SetReference("EduBackrestAngle_String_Table", key);
        string scriptValue = localizeStringEvent.StringReference.GetLocalizedString(key);
        TextToSpeach.Instance.SpeechText(scriptValue);
        scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("");
        stewardess.GetComponent<Animator>().SetBool("Talk", true);

        switch (scriptIndex)
        {
            case 8: // â�� �ø���, ���� ��ư ��Ȱ��ȭ
                windowAct = true; // â�� ��ȣ�ۿ� Ȱ��ȭ
                windowHandle_model.transform.GetChild(1).gameObject.SetActive(true); // ui on
                break;
            case 9: // ��ư ������, ���� ��ư ��Ȱ��ȭ
                backrestAct = true;
                backRestButton.transform.GetChild(0).gameObject.SetActive(true); // ui on
                break;
            case 10: // å�� ���󺹱��ϸ�, ���� ��ư ��Ȱ��ȭ
                // ������: �� ���� å�� �ǵ鿴�ٸ�.. ������ �� �־����� ����..
                //table.transform.Rotate(minTableRot, 0, 0); // å�� ������
                //tablePin.transform.Rotate(0, 0, maxTablePinRot);// å���� ������

                table.transform.rotation = Quaternion.Euler(70, 180, 0);
                tablePin.transform.rotation = Quaternion.Euler(0, 0, 90);

                table.transform.GetChild(1).gameObject.SetActive(true); // grab ui on
                tableAct = true;
                tablePinAct = true;
                break;
            case 11: // ���� �Ʒ��� �θ�, ���� ��ư ��Ȱ��ȭ
                baggage.SetActive(true);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(false);
                break;
            case 20: // ���� ȭ������ �̵�
                yield return new WaitForSeconds(scriptValue.Length * 0.1f + 3f);
                SceneManager.LoadScene("MainTitle");
                break;
            default:
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
        }
    }
    public void NextButtonOnClick()
    {
        scriptIndex++;

        StartCoroutine(NextScript());
        nextButton.SetActive(false);

        // ����
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button1);
    }


    void Update()
    {
        // â�� �ڵ� ��ġ ����
        if (windowHandle_model.transform.position.y >= maxYPosition)
        {
            Vector3 newPosition = windowHandle_model.transform.position;
            newPosition.y = maxYPosition;
            windowHandle_model.transform.position = newPosition;
        }

        if (windowHandle_model.transform.position.y <= minYPosition)
        {
            Vector3 newPosition = windowHandle_model.transform.position;
            newPosition.y = minYPosition;
            windowHandle_model.transform.position = newPosition;
        }

        // ����� ���� ����
        if (backBtn && isRotating && backrestAct)
        {
            // ����� ���� �������
            float currentAngle = backRest.transform.rotation.eulerAngles.x;
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, 0f, 15.8f * Time.deltaTime);

            backRest.transform.rotation = Quaternion.Euler(currentAngle, 0, 0);

            // ��ǥ ȸ�� ������ �����ϸ� ȸ���� ���ߵ���
            if (Mathf.Abs(currentAngle - 0f) < 0.1f)
            {
                isRotating = false;
                backRest.transform.rotation = Quaternion.Euler(0, 0, 0);

                backRestButton.transform.GetChild(0).gameObject.SetActive(false); // ui off
                backrestAct = false;

                // ���� ��ũ��Ʈ��
                NextButtonOnClick();
            }
        }

        //// å�� ���� ����
        //if (table.transform.rotation.x >= maxTableRot)
        //{
        //    //table.transform.Rotate(maxTableRot, 0, 0);
        //    table.transform.rotation = Quaternion.Euler(maxTableRot, 0, 0);
        //}
        //if (table.transform.rotation.x <= minTableRot)
        //{
        //    //table.transform.Rotate(minTableRot, 0, 0);
        //    table.transform.rotation = Quaternion.Euler(minTableRot, 0, 0);
        //}

        ////å�� ���� ���� "å�� �ø�~����(0~290)"
        ////if (260 < table.transform.eulerAngles.x && table.transform.eulerAngles.x <= 290) // å�� ����x
        //{
        //    //table.transform.Rotate(maxTableRot, 0, 0);
        //    table.transform.rotation = Quaternion.Euler(290, 0, 0);
        //}
        ////if (0 <= table.transform.eulerAngles.x && table.transform.eulerAngles.x < 30) // å�� �ø�x
        //{
        //    //table.transform.Rotate(minTableRot, 0, 0);
        //    table.transform.rotation = Quaternion.Euler(0, 0, 0);
        //}

        //å�� ���� ����
        if (table.transform.eulerAngles.x >= 70 && table.transform.eulerAngles.x <= 170) // å�� ����x
        {
            //table.transform.Rotate(maxTableRot, 0, 0);
            table.transform.rotation = Quaternion.Euler(70, 180, 0);
        }
        if (table.transform.eulerAngles.x <= 0 || table.transform.eulerAngles.x > 260) // å�� �ø�x
        {
            //table.transform.Rotate(minTableRot, 0, 0);
            table.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        //Debug.Log("table: "+ table.transform.eulerAngles);
        //Debug.Log("table pin: " + tablePin.transform.eulerAngles);


        // å���� ���� ����
        if (90 <= tablePin.transform.eulerAngles.z && tablePin.transform.eulerAngles.z < 120)
        {
            //tablePin.transform.Rotate(0, 0, maxTablePinRot);
            tablePin.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        if (tablePin.transform.eulerAngles.z >= 360 && tablePin.transform.eulerAngles.z < 330)
        {
            //tablePin.transform.Rotate(0, 0, minTableRot);
            tablePin.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        // å�� �ڸ����� ���� �ڸ����� �Ѿ��
        if (!isTable && isTableGrabbed && isTablePinGrabbed)
        {
            tableAct = false;
            tablePinAct = false;
            isTable = true;
            table.GetComponent<XRGrabInteractable>().enabled = false; // å�� ��ȣ�ۿ� �Ұ���
            tablePin.GetComponent<XRGrabInteractable>().enabled = false; // å���� ��ȣ�ۿ� �Ұ���

            // ���� ��ũ��Ʈ��
            NextButtonOnClick();
        }
    }
}

partial class BackrestAngleManager : MonoBehaviour
{
    // â��
    [SerializeField] GameObject windowHandle_model;
    private bool isWindowGrabbed = false; // â�� ������ ��Ҵ��� ����
    public float targetYPosition = 6.6f; // ������Ʈ�� �ø� ��ǥ ��ġ
    private float minYPosition = 6.315f;
    private float maxYPosition = 6.69f;
    //public Vector3 existHandlePos; // �������� ���� Transform
    //public Vector3 changeHandlePos; // �������� �ٲ� Transform
    //bool hasTriggered = false;

    // �����
    [SerializeField] GameObject backRest;
    [SerializeField] GameObject backRestButton;
    private bool backBtn;
    private bool isRotating = true;
    private float rotationAmount = 0f; // ���� ȸ���� ����

    // å��
    [SerializeField] GameObject table;
    [SerializeField] GameObject tablePin;
    [SerializeField] GameObject tableGoal;
    [SerializeField] GameObject tablePinGoal;
    private bool isTableGrabbed = false;
    private bool isTablePinGrabbed = false;
    private bool isTable = false;
    private float maxTableRot = 0;
    private float minTableRot = -70;
    private float goalTableRot = 0; // (0~-70)
    private float maxTablePinRot = 90;
    private float minTablePinRot = 0;
    private float goalTablePinRot = 10; // (0~90)

    // ����
    [SerializeField] GameObject baggage; // �� ������Ʈ
    [SerializeField] GameObject dropBaggage; // �ٴڿ� �������� �� ���� �� ������Ʈ
    [SerializeField] GameObject originBaggagePos; // �ٴڿ� �������� �� ���� ��ġ


    // â�� ��Ʈ��
    public void WindowSelectEntered()
    {
        if (!isWindowGrabbed && windowAct)
        {
            windowHandle_model.transform.GetChild(1).gameObject.SetActive(false); // ui off
            windowHandle_model.transform.GetChild(2).gameObject.SetActive(true); // indi on
        }
    }
    public void WindowSelectExited()
    {
        if (!isWindowGrabbed && windowAct)
        {
            if (windowHandle_model.transform.position.y >= targetYPosition)
            {
                windowHandle_model.transform.GetChild(2).gameObject.SetActive(false); // indi off

                // ���� ��ũ��Ʈ��
                NextButtonOnClick();

                isWindowGrabbed = true;
                windowAct = false;

                windowHandle_model.GetComponent<XRGrabInteractable>().enabled = false; // â�� ��ȣ�ۿ� �Ұ���
            }
        }
    }

    // ����� ��Ʈ��, ������ if������ ���� ����
    public void BackRestBtnPressed()
    {
        // ����� ���� �������
        //BackRest.transform.rotation = Quaternion.Euler(0, 0, 0);
        //BackRest.transform.Rotate(new Vector3(15.8f, 0, 0) * Time.deltaTime);
        backBtn = true;
        //if (backRest.transform.rotation.x == 0)
        //{
        //    Debug.Log("Pressed2"); // �ƴ� �̰� �� ������ �ȵ�.. ���� ���� rotation ����..? �ٵ� ���� ���� ��ũ��ư ����;;;
        //    backRestButton.transform.GetChild(0).gameObject.SetActive(false); // ui off
        //    backrestAct = false;

        //    NextButtonOnClick(); // ���� ��ũ��Ʈ��
        //}
    }

    // å�� ��Ʈ��
    // 1. å�� ��ġ��
    // pin 0 -> 90, �� ���̿� table ������ �� ����
    // table 0 -> 90, �� ���̿� pin ������ �� ����
    // 2. å�� ����
    // table 90 -> table 0 , �� ���̿� pin�� ������ �� ����
    // pin 90 -> 0, �� ���̿� table�� ������ �� ����
    // ��� �Ϸ�Ǹ� ���� �ܰ��
    public void TableSelectEntered()
    {
        if (!isTableGrabbed && tableAct)
        {
            table.transform.GetChild(1).gameObject.SetActive(false); // grab ui off
            tableGoal.SetActive(true); // goal ui on
        }
    }
    public void TableSelectExited()
    {
        if(!isTableGrabbed && tableAct)
        {
            if(table.transform.eulerAngles.x >= 355 || table.transform.eulerAngles.x <= 5)
            {
                Debug.Log("Table true");

                // <��� �ʿ�>
                // ��->å��->�� �� ���
                // å���� ����� ������ �ʾƵ� �۵��Ǵ� ���� �Ǵ� å��� ���� ����� �Ǿ��־ å�� ������ �� ���� �ٽ� ����������ϴ� ���� �߻�
                //tablePinAct = true;
                // => å�� ��ǥ ���� �����غ�������, �� �ٽ� �ǵ鿩�ߵǴ� ���� ����
                // ===> �ӽù���: ���� ������� �� �� goal ���� �����ϸ� ���� �ڸ�

                //Tabel.GetComponent<XRGrabInteractable>().enabled = false; // å�� ��ȣ�ۿ� �Ұ���
                //TabelPin.GetComponent<XRGrabInteractable>().enabled = true; // å���� ��ȣ�ۿ� ����
                //tableAct = false;
                //tablePinAct = true;
                isTableGrabbed = true;

                tableGoal.SetActive(false); // goal ui off
                tablePin.transform.GetChild(1).gameObject.SetActive(true); // grab ui on
            }
        }
    }
    public void TablePinSelectEntered()
    {
        if (!isTablePinGrabbed && tablePinAct)
        {
            tablePin.transform.GetChild(1).gameObject.SetActive(false); // grab ui off
            tablePinGoal.SetActive(true); // goal ui on
        }
     }
        public void TablePinSelectExited()
    {
        if(!isTablePinGrabbed && tablePinAct)
        {
            //if(tablePin.transform.rotation.z <= goalTablePinRot)
            if (tablePin.transform.eulerAngles.z <= 5)
            {
                Debug.Log("Pin true");
                
                //TabelPin.GetComponent<XRGrabInteractable>().enabled = false; // å���� ��ȣ�ۿ� �Ұ���
                //tablePinAct = false;
                isTablePinGrabbed = true;
                tablePinGoal.SetActive(false); // goal ui off

                //NextButtonOnClick(); // ���� ��ũ��Ʈ��
            }
        }
    }


    // ���� ��Ʈ�� ��..��.. ��.. ��򰡿� �̹� �����ߴ� �� ����... �߳�.. ��,, �����ؾ߰ڴ�:>
    public void baggageSelectEntered()
    {
        baggage.transform.GetChild(0).gameObject.SetActive(false); // Grab UX off

        dropBaggage.SetActive(true); // drop pos object on

    }

    public void baggageSelectExited()
    {
        // �ٴڿ� �� ���� ���Ҵٸ� (�Ÿ��� �ִٸ�) ������ġ��
        if (Vector3.Distance(baggage.transform.position, dropBaggage.transform.position) > 0.4f) // distance ��� 0.982955
        {
            baggage.transform.GetChild(0).gameObject.SetActive(false); // Grab UX on
            baggage.transform.position = originBaggagePos.transform.position; // ���� ������ġ��
            dropBaggage.SetActive(false);
        }
        else // �ٴڿ� ���� ���Ҵٸ�
        {
            baggage.transform.position = dropBaggage.transform.position; // �� ��ġ ����������ġ�� ����
            baggage.GetComponent<XRGrabInteractable>().enabled = false; // ��ȣ�ۿ� �Ұ����ϰ�

            baggage.transform.GetChild(0).gameObject.SetActive(false); // ux off
            dropBaggage.SetActive(false); // ux off

            // ���� ��ũ��Ʈ��
            NextButtonOnClick();
        }
    }
}