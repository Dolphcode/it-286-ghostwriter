using UnityEngine;
using TMPro;

public class Fear : MonoBehaviour
{
    [HideInInspector]
    public float fearMeter;
    public TMP_Text text;
    public float fearRate;
    [HideInInspector]
    public bool isSanity100;
    [SerializeField]
    int minFear;
    void Start()
    {

        ///<summary>
        /// Starts fear at 0%
        /// </summary>
        fearMeter = 0;
        minFear = 1;
        isSanity100 = false;
    }

    void Update()
    {
        ///<summary>
        /// Makes the fear tick down at a constant rate
        /// Modifiable by changing <param> fearRate </param> 
        /// </summary>
        fearMeter = Mathf.Clamp(fearMeter, minFear, 100);
        if (fearMeter < 100)
        {
            fearMeter += Time.deltaTime * fearRate;
            fearMeter.ToString();
            text.text = "Fear: " + (int)fearMeter + "%";
        }

        ///<summary>
        ///detects if the players sanity drops to 0
        /// </summary>

        if (fearMeter >= 100)
        {
            isSanity100 = true;
            text.text = "You Blacked Out";


        }

        
    }
    /// <summary>
    /// Changes the rate at which fear depletes.
    /// </summary>
    /// <param name="newRate"> Default is .25 </param>
    public void ChangeFearRate(float newRate)
        {
            fearRate = newRate;
            
        }

    /// <summary>
    /// Changes the fear by the amount that is in the parameter (If you want to take away fear, put a negative number in the parameter)
    /// </summary>
    /// <param name="fearChange">the amount of fear being changed (fear goes from 0-100) </param>
    public void ChangeFearMeter(float fearChange)
    {
        fearMeter += fearChange;
    }
}
