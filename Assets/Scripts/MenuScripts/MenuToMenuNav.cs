using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuToMenuNav : MonoBehaviour
{
    [Header ("Setup")]
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Selectable elementToSelect;
    
    [Header ("Visualization")]
    [SerializeField] private bool _showVisualization;
    [SerializeField] private Color _navColor;


    // Visual reference to show which UI element the navigation will select from the game object this script is attached to
    // if using a keyboard or controller when switching menus
    void OnDrawGizmos()
    {
        if (!_showVisualization)
        {
            return;
        }
        if (_eventSystem == null)
        {
            return;
        }

        Gizmos.color = _navColor;
        Gizmos.DrawLine(gameObject.transform.position, elementToSelect.gameObject.transform.position);
    }

    public void JumpToElement()
    {
        if (_eventSystem == null)
        {
            Debug.Log("Missing event system, ensure one exists and is attached!");
        }
        if (elementToSelect == null)
        {
            Debug.Log("Which element should the navigation go?");
        }

        _eventSystem.SetSelectedGameObject(elementToSelect.gameObject);
    }
}
