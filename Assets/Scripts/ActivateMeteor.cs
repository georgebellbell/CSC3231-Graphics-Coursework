using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivateMeteor : MonoBehaviour
{
    [SerializeField] GameObject meteor;

    [SerializeField] TextMeshProUGUI instruction;

    private void OnEnable()
    {
        instruction.enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            meteor.SetActive(true);
        }
    }

    private void OnDisable()
    {
        instruction.enabled = false;
    }

}
