using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class moveBarPlayer : MonoBehaviour
{
    Transform Endpos;

    RectTransform uiElement;
    float start = -5.35f;
    public float uiMinX = -575f;
    public float uiMaxX = 575f;


    private float initialEndPosX;

    // Start is called before the first frame update
    void Start()
    {
        Endpos = GameObject.Find("EndMap").transform;
        uiElement = this.GetComponent<RectTransform>();

        initialEndPosX = 780.7f - start;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.isGameover)
        {
            return;
        }



        float currentEndPosX = Endpos.position.x - start;

        // EndPos가 처음 위치에서 얼마나 이동했는지 계산
        float totalMovement = initialEndPosX - currentEndPosX;

        // 비율 계산 (0부터 1까지)
        float percentage = totalMovement / initialEndPosX;

        float newX = Mathf.Lerp(uiMinX, uiMaxX, percentage);

        // Apply the new position to the UI element
        Vector3 uiPosition = uiElement.localPosition;
        uiPosition.x = newX;
        uiElement.localPosition = uiPosition;
    }
}
