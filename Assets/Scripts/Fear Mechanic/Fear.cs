using UnityEngine;
//using TMPro;
//Commented out the text part of Fear Meter
public class Fear : MonoBehaviour
{
    //[HideInInspector]
    public float fearMeter;
    //public TMP_Text text;
    public float fearRate;
    [HideInInspector]
    public bool isSanity100;
    [SerializeField]
    int minFear;
    public Ghost boo;
    public LevelManager levelManager;
    private bool spooked;
    public bool fearful;
    void Start()
    {

        
        levelManager = FindAnyObjectByType<LevelManager>();
        ///<summary>
        /// Starts fear at 0%
        /// </summary>
        fearMeter = 0;
        minFear = 1;
        isSanity100 = false;
        fearful = true;
    }

    void Update()
    {
        if (boo == null)
        {
            boo = FindAnyObjectByType<Ghost>();
        }
        
        ///<summary>
        /// Makes the fear tick down at a constant rate
        /// Modifiable by changing <param> fearRate </param> 
        /// </summary>
        fearMeter = Mathf.Clamp(fearMeter, minFear, 100);
        if (fearful)
        {

            if (fearMeter < 100)
            {
                fearMeter += Time.deltaTime * fearRate;
                fearMeter.ToString();
            }

            //text.text = "Fear: " + (int)fearMeter + "%";
            if (boo != null && fearMeter > 50)
            {
                boo.IncreaseAggression((int)(fearMeter * 0.01 * Time.deltaTime));
            }


            ///<summary>
            ///detects if the players sanity drops to 0
            /// </summary>

            if (fearMeter >= 100)
            {
                isSanity100 = true;

                //text.text = "You Blacked Out";
                levelManager.LoseLevel();


            }
            if (boo != null)
            {
                if (boo.IsGhostHunting())
                {
                    IsScared();
                }

                else if (!boo.IsGhostHunting())
                {
                    IsNotScared();
                }
            }
        }
    }

    private void IsScared()
    {
        if (!spooked)
        { 
            ChangeFearRate(0.5f);
            spooked = true;
        }
    }

    private void IsNotScared()
    {
        if (spooked)
        {
            ChangeFearRate(-0.5f);
            spooked = false;
        }
            
    }

    /// <summary>
    /// Changes the rate at which fear depletes.
    /// </summary>
    /// <param name="newRate"> Default is .25 </param>
    public void ChangeFearRate(float newRate)
        {
            fearRate += newRate;
        }

    /// <summary>
    /// Changes the fear by the amount that is in the parameter (If you want to take away fear, put a negative number in the parameter)
    /// </summary>
    /// <param name="fearChange">the amount of fear being changed (fear goes from 0-100) </param>
    public void ChangeFearMeter(float fearChange)
    {
        fearMeter += fearChange;
    }
    /// <summary>
    /// Stops the fear from increasing
    /// </summary>
    public void PauseFear()
    {
        fearful = false;
    }
    /// <summary>
    /// Makes the fear start increasing
    /// </summary>
    public void StartFear()
    {
        fearful = true;
    }
}
