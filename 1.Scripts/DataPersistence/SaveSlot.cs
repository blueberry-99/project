using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileID = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;

    [Header("Clear Data Button")]
    [SerializeField] private Button clearButton;

    public bool isSaveSlotEmpty;

    public void SetData(GameData data)
    {
        if (data == null)
        {
            isSaveSlotEmpty = true;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            clearButton.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            isSaveSlotEmpty = false;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            clearButton.transform.parent.gameObject.SetActive(true);
        }
    }

    public string GetProfileID()
    {
        return this.profileID;
    }
}
