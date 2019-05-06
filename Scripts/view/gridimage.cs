using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class gridimage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public static Action<Transform> OnEnter;
    public static Action OnExit;
    public static Action<Transform> OnClickL;
    public static Action<Transform> OnClickM;
    public static Action<Transform> OnClickR;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnEnter != null)
        {
            OnEnter(transform);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnExit != null)
        {
            OnExit();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if(eventData.button== PointerEventData.InputButton.Left)
        {
            Debug.Log("Left");
            OnClickL(transform);
        }
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            Debug.Log("Middle");
            //OnClickM(transform);
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right");
            OnClickR(transform);
        }
    }

    
}
