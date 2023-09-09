using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

    // ��ȣ�ۿ�
    bool windowAct = false;
    bool backrestAct = false;
    bool tableAct = false;
    bool tablePinAct = false;
    bool bagAct = false;

    // Start is called before the first frame update
    void Start()
    {
        // ��ȣ�ۿ� ����
        windowHandle_model.GetComponent<XRGrabInteractable>().enabled = false;
        backRestButton.transform.GetChild(2).GetComponent<XRGrabInteractable>().enabled = false;
        table.GetComponent<XRGrabInteractable>().enabled = false;
        tablePin.GetComponent<XRGrabInteractable>().enabled = false;

        NextButtonOnClick();
    }

    // ���� �ڸ� �о����
    int scriptIndex = 0;
    IEnumerator NextScript()
    {
        string key = "EduBackrestAngle_script" + scriptIndex;

        localizeStringEvent.StringReference.SetReference("EduBackrestAngle_String_Table", key);
        string scriptValue = localizeStringEvent.StringReference.GetLocalizedString(key);
        TextToSpeach.Instance.SpeechText(scriptValue.Replace('\n', ' '));
        //TextToSpeach.Instance.SpeechText(scriptValue);

        // �ڸ��� ũ������ �� ��������Ʈ ����
        RectTransform rect = scriptText.transform.parent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 50 + (scriptValue.Split("\n").Length - 1) * 20);
        scriptText.transform.parent.GetComponent<Image>().sprite = stewardess.GetComponent<Stewardess>().textSprites[scriptValue.Split("\n").Length - 1];

        scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("").SetEase(Ease.Linear);
        stewardess.GetComponent<Animator>().SetBool("Talk", false); // �̹� true�� �����Ǿ��ִ� ��찡 �־ false�� ���� ���� true�� ����
        stewardess.GetComponent<Animator>().SetBool("Talk", true);
        stewardess.GetComponent<Stewardess>().RandomTalkAnimation(); // Talk �ִϸ��̼� ��������

        switch (scriptIndex)
        {
            case 8: // â�� �ø���, ���� ��ư ��Ȱ��ȭ
                windowHandle_model.GetComponent<XRGrabInteractable>().enabled = true; // â�� ��ȣ�ۿ� o
                windowAct = true;
                windowHandle_model.transform.GetChild(1).gameObject.SetActive(true); // ui on
                break;
            case 9: // ��ư ������, ���� ��ư ��Ȱ��ȭ
                backRestButton.transform.GetChild(2).GetComponent<XRGrabInteractable>().enabled = true; ; // ����� ��ȣ�ۿ� o
                backrestAct = true;
                backRestButton.transform.GetChild(0).gameObject.SetActive(true); // ui on
                break;
            case 10: // å�� ���󺹱��ϸ�, ���� ��ư ��Ȱ��ȭ
                table.GetComponent<XRGrabInteractable>().enabled = true; // å�� ��ȣ�ۿ� o

                table.transform.rotation = Quaternion.Euler(70, 180, 0);
                tablePin.transform.rotation = Quaternion.Euler(0, 0, 90);

                table.transform.GetChild(1).gameObject.SetActive(true); // grab ui on
                tableAct = true;
                tablePinAct = false;
                break;
            case 11: // ���� �Ʒ��� �θ�, ���� ��ư ��Ȱ��ȭ
                baggage.SetActive(true);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(false);
                break;
            case 20: // ���� ȭ������ �̵�
                PlayerPrefs.SetInt("Chapter2", 1); // Ŭ���� ���� ����
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

        // å�� ���� ����
        if (tableAct)
        {
            if (table.transform.eulerAngles.x == 0 || table.transform.eulerAngles.x <= 3)// ||table.transform.eulerAngles.x >= 357)
            {
                Debug.Log("Table true");

                tableAct = false;
                tablePinAct = true;
                //isTableGrabbed = true;

                tableGoal.SetActive(false); // goal ui off
                tablePin.transform.GetChild(1).gameObject.SetActive(true); // grab ui on

                table.GetComponent<XRGrabInteractable>().enabled = false; // å�� ��ȣ�ۿ� x
                tablePin.GetComponent<XRGrabInteractable>().enabled = true; // å���� ��ȣ�ۿ� o
            }
        }
        // å���� ���� ����
        if (tablePinAct)
        {
            if (tablePin.transform.eulerAngles.z == 0 || tablePin.transform.eulerAngles.z <= 5 ||
                   tablePin.transform.eulerAngles.z >= 355) // || tablePin.transform.eulerAngles.z >= 360)
            {
                Debug.Log("Pin true");

                tablePin.GetComponent<XRGrabInteractable>().enabled = false; // å���� ��ȣ�ۿ� x
                tablePinAct = false;
                //isTablePinGrabbed = true;
                tablePinGoal.SetActive(false); // goal ui off

                NextButtonOnClick(); // ���� ��ũ��Ʈ��
            }
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
        backBtn = true;
    }

    // å�� ��Ʈ��
    public void TableSelectEntered()
    {
        table.transform.GetChild(1).gameObject.SetActive(false); // grab ui off
        tableGoal.SetActive(true); // goal ui on
    }
    //public void TableSelectExited()
    //{
    //    // <<����?>> �̰� exited ����� �Ȱǰ�? �� if�� ������ �ȵǴ°��� ������ �´µ� ��... �� �� �̻��ϳ�..
    //    // ���� 0���϶� �ȵǴ°ǰ�..? �ƴ�.. �� .��.. ��,, 0��.. ����.. �ٵ� 360�� �ߴµ�......
    //    //if(!isTableGrabbed && tableAct)
    //    //{
    //    print("å�� ��� �ߴ�"); //if�� ������ �ȵǴ°ſ��� ����..
    //    if (table.transform.eulerAngles.x == 0 || table.transform.eulerAngles.x <= 3)// ||table.transform.eulerAngles.x >= 357)
    //    {
    //            Debug.Log("Table true");

    //            // <��� �ʿ�>
    //            // ��->å��->�� �� ���
    //            // å���� ����� ������ �ʾƵ� �۵��Ǵ� ���� �Ǵ� å��� ���� ����� �Ǿ��־ å�� ������ �� ���� �ٽ� ����������ϴ� ���� �߻�
    //            //tablePinAct = true;
    //            // => å�� ��ǥ ���� �����غ�������, �� �ٽ� �ǵ鿩�ߵǴ� ���� ����
    //            // ===> �ӽù���: ���� ������� �� �� goal ���� �����ϸ� ���� �ڸ�

    //            //Tabel.GetComponent<XRGrabInteractable>().enabled = false; // å�� ��ȣ�ۿ� �Ұ���
    //            //TabelPin.GetComponent<XRGrabInteractable>().enabled = true; // å���� ��ȣ�ۿ� ����
                
    //            //tableAct = false;
    //            //tablePinAct = true;
    //            //isTableGrabbed = true;

    //            tableGoal.SetActive(false); // goal ui off
    //            tablePin.transform.GetChild(1).gameObject.SetActive(true); // grab ui on

    //            table.GetComponent<XRGrabInteractable>().enabled = false; // å�� ��ȣ�ۿ� x
    //            tablePin.GetComponent<XRGrabInteractable>().enabled = true; // å���� ��ȣ�ۿ� o
    //    }
    //}
    // �׷� ���� isTableGrabbed����� tableAct ������ �̿��ؼ� ������Ʈ���� �� ������ �־���ϳ�?

    // å���� ��Ʈ��
    public void TablePinSelectEntered()
    {
        tablePin.transform.GetChild(1).gameObject.SetActive(false); // grab ui off
        tablePinGoal.SetActive(true); // goal ui on
     }
    //public void TablePinSelectExited()
    //{
    //    //if(!isTablePinGrabbed && tablePinAct)
    //    //{
    //    //if(tablePin.transform.rotation.z <= goalTablePinRot)
    //    if (tablePin.transform.eulerAngles.z == 0 || tablePin.transform.eulerAngles.z <= 5 ||
    //        tablePin.transform.eulerAngles.z >= 355) // || tablePin.transform.eulerAngles.z >= 360)
    //        {
    //            Debug.Log("Pin true");
                
    //            tablePin.GetComponent<XRGrabInteractable>().enabled = false; // å���� ��ȣ�ۿ� x
    //            //tablePinAct = false;
    //            //isTablePinGrabbed = true;
    //            tablePinGoal.SetActive(false); // goal ui off

    //            NextButtonOnClick(); // ���� ��ũ��Ʈ��
    //        }
    //    //}
    //}


    // ���� ��Ʈ��
    public void baggageSelectEntered()
    {
        baggage.transform.GetChild(0).gameObject.SetActive(false); // Grab UX off

        dropBaggage.SetActive(true); // drop pos object on

    }

    // ���� ��Ʈ��
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

partial class BackrestAngleManager
{
    public void popup_reStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void popup_toMainTitle()
    {
        SceneManager.LoadScene("MainTitle");
    }
    public void popup_exitPopup(GameObject popup)
    {
        popup.SetActive(false);
    }
}