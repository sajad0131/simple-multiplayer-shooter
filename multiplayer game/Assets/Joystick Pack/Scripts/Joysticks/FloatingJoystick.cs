using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
        runArrow.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        
        base.OnPointerDown(eventData);
    }

    public void Update()
    {
        if (run)
        {
            runArrow.gameObject.SetActive(true);
        }
        else
        {
            runArrow.gameObject.SetActive(false);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        runArrow.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
}