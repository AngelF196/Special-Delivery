using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ObjectTypeCounter : MonoBehaviour
{
    [SerializeField] private string targetTag;
    [SerializeField] private TextMeshProUGUI counterText;
    public int CurrentCount { get; private set; }

    // Event declaration
    public static event Action OnObjectRemoved;

    private void Awake()
    {
        RefreshCount();

        // Listen for removals
        OnObjectRemoved += HandleObjectRemoved;
    }

    private void OnDestroy()
    {
        OnObjectRemoved -= HandleObjectRemoved;
    }

    private void RefreshCount()
    {
        CurrentCount = GameObject.FindGameObjectsWithTag(targetTag).Length;

        counterText.text = $"x{CurrentCount}";
    }

    private void HandleObjectRemoved()
    {
        CurrentCount = Mathf.Max(0, CurrentCount - 1);

        counterText.text = $"x{CurrentCount}";
    }

    // Optional public accessor if you want external scripts to invoke safely
    public static void InvokeObjectRemoved()
    {
        OnObjectRemoved?.Invoke();
    }
}
