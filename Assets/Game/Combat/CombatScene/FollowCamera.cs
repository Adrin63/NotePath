using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform character1;
    public Transform character2;
    public Transform BG;
    public Transform DownSide;

    Transform target;
    Camera miCamera;

    public float speed = 12.0f;
    public float proportionalFollowDistance = 5.0f;

    Vector2 originalPosition;
    Vector2 distance2;
    float m_ViewPositionX, m_ViewPositionY, m_ViewWidth, m_ViewHeight;

    bool activeA = false;
    bool activeS = false;
    bool activeD = false;



    void Start()
    {
        originalPosition = new Vector2(transform.position.x, transform.position.y);
        miCamera = GetComponent<Camera>();

        m_ViewPositionX = originalPosition.x;
        m_ViewPositionY = originalPosition.y;

        m_ViewWidth = 1;
        m_ViewHeight = 1;

        miCamera.enabled = true;
        target = BG;
    }

    void LateUpdate()
    { 
        if(target == character1)
        {
            Vector2 distance2 = new Vector2(target.position.x - transform.position.x + 1.5f, target.position.y - transform.position.y -1);

            Vector2 displacement2 = distance2.normalized * speed * Time.deltaTime * distance2.magnitude / proportionalFollowDistance;

            if (displacement2.magnitude > distance2.magnitude) { displacement2 = distance2; }

            Vector3 displacement3 = displacement2;

            transform.position += displacement3;
        }
        else
        {
            Vector2 distance2 = target.position - transform.position;

            Vector2 displacement2 = distance2.normalized * speed * Time.deltaTime * distance2.magnitude / proportionalFollowDistance;

            if (displacement2.magnitude > distance2.magnitude) { displacement2 = distance2; }

            Vector3 displacement3 = displacement2;

            transform.position += displacement3;
        }

        if(activeA || activeD)
        {
            if (miCamera.orthographicSize > 3.0f)
                miCamera.orthographicSize -= 2 * Time.deltaTime;
        }
        else if (activeS)
        {
            if (miCamera.orthographicSize < 5.0f)
                miCamera.orthographicSize += 2 * Time.deltaTime;
        }
    }

    public void MoveLeft()
    {
        activeA = true;
        activeD = false;
        activeS = false;

        target = character1;

        if (miCamera)
        {
            miCamera.orthographic = true;
            miCamera.rect = new Rect(m_ViewPositionX, m_ViewPositionY, m_ViewWidth, m_ViewHeight);
        }
    }

    public void MoveRight()
    {
        activeD = true;
        activeA = false;
        activeS = false;

        target = character2;

        if (miCamera)
        {
            miCamera.orthographic = true;
            miCamera.rect = new Rect(m_ViewPositionX, m_ViewPositionY, m_ViewWidth, m_ViewHeight);
        }
    }

    public void MoveCenter()
    {
        activeS = true;
        activeA = false;
        activeD = false;

        target = BG;

        if (miCamera)
        {
            miCamera.orthographic = true;
            miCamera.rect = new Rect(m_ViewPositionX, m_ViewPositionY, m_ViewWidth, m_ViewHeight);
        }
    }

    public void MoveDown()
    {
        activeS = true;
        activeA = false;
        activeD = false;

        target = DownSide;

        if (miCamera)
        {
            miCamera.orthographic = true;
            miCamera.rect = new Rect(m_ViewPositionX, m_ViewPositionY, m_ViewWidth, m_ViewHeight);
        }
    }
}
