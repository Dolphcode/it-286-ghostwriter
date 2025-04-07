using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is solely for changing computer screens
/// </summary>
public class LobbyComputerManager : MonoBehaviour
{
    [SerializeField] List<GameObject> screenRefs;
    [SerializeField] List<GameObject> infoRefs;

    // State
    int screenIndex = 0;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < infoRefs.Count; i++)
        {
            if (i == screenIndex)
            {
                infoRefs[i].SetActive(true);
                screenRefs[i].SetActive(true);
            } else
            {
                infoRefs[i].SetActive(false);
                screenRefs[i].SetActive(false);
            }
        }
    }

    // Callback functions for showing things
    public void ShowScreen(int index)
    {
        screenIndex = index;
    }
}
