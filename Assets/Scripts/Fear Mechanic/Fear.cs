using UnityEngine;
using TMPro;

public class Fear : MonoBehaviour
{
    [HideInInspector]
    public float fearMeter;
    public TMP_Text text;
    public float fearRate;
    [HideInInspector]
    public bool isSanity0;

    void Start()
    {
        ///<summary>
        /// Starts fear at 100%
        /// </summary>
        fearMeter = 100;
        isSanity0 = false;
    }

    void Update()
    {
        ///<summary>
        /// Makes the fear tick down at a constant rate
        /// Modifiable by changing <param> fearRate </param> 
        /// </summary>
        
        if (fearMeter > 0)
        {
            fearMeter -= Time.deltaTime * fearRate;
            fearMeter.ToString();
            text.text = "Fear: " + (int)fearMeter + "%";
        }

        ///<summary>
        ///detects if the players sanity drops to 0
        /// </summary>

        if (fearMeter <= 0)
        {
            isSanity0 = true;
            text.text = "You Blacked Out";


        }

        
    }
    /// <summary>
    /// Changes the rate at which fear depletes. Default is around fearRate = .25 (for now)...
    /// </summary>
    public void ChangeFearRate(float newRate)
        {
        fearRate = newRate;
        }

    /// <summary>
    /// Changes the fear by the amount that is in the parameter
    /// (If you want to take away fear, put a negative number in the parameter)
    /// </summary>
    public void ChangeFearMeter(float fearChange)
    {
        fearMeter += fearChange;
    }
}
