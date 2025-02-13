using System.Linq;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    Portal[] portals;

    void Awake()
    {
        portals = FindObjectsByType<Portal>(FindObjectsSortMode.None);

    }

    void OnPreCull()
    {
        Debug.Log(portals.Count());

        for (int i = 0; i < portals.Length; i++)
        {
            //portals[i].PrePortalRender();
        }
        for (int i = 0; i < portals.Length; i++)
        {
            //portals[i].Render();
        }

        for (int i = 0; i < portals.Length; i++)
        {
            //portals[i].PostPortalRender();
        }

    }

}