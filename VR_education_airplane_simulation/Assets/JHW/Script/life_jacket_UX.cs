using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class life_jacket_UX : MonoBehaviour
{
    public float alpha = 0.5f; // ���� �� (0.0 ~ 1.0)
    private bool isFade = true; // �����������ִ��� �ƴ���
    [SerializeField] private MeshRenderer renderer;

    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(SetJacketAlphaUX());
    }
    private void OnDisable()
    {
        StopCoroutine(SetJacketAlphaUX());
    }

    IEnumerator SetJacketAlphaUX()
    {
        // ���� ���ǰ� �ִ� ���͸����� �����Ͽ� ���ο� ���͸����� ����
        Material material = new Material(renderer.material);

        if (alpha <= 0.3) isFade = false;

        // ���ο� ���͸����� ���� ���� ����
        alpha = (isFade) ? alpha-0.01f : alpha+0.01f;
        Color color = material.color;
        color.a = alpha;
        material.color = color;

        // ��ü�� ���ο� ���͸����� �Ҵ�
        renderer.material = material;

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(SetJacketAlphaUX());

    }
}
