using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class abGameController : MonoBehaviour
{
    public Image panel;
	public Text txtUser;
	public Text txtToken;
	public Text txtSchool;
	public Text txtAccess;
	public Text txtVersion;


	// Use this for initialization
	void Start ()
	{
        if (globalData.loggedIn)
        {
            txtUser.text = globalData.loggedinUser.userName;
            txtToken.text = globalData.loggedinUser.token;
            txtSchool.text = globalData.loggedinUser.schoolName;
            if (globalData.loggedinUser.admin.Equals("1"))
            {
                txtAccess.text = "Admin";
            }
            else if (globalData.loggedinUser.levelCoordinator.Equals("1"))
            {
                txtAccess.text = "Level Coordinator";
            }
            else if (globalData.loggedinUser.S1.Equals("1"))
            {
                txtAccess.text = "Starter 1";
            }
            else if (globalData.loggedinUser.S2.Equals("1"))
            {
                txtAccess.text = "Starter 2";
            }
            else if (globalData.loggedinUser.B1.Equals("1"))
            {
                txtAccess.text = "Beginner 1";
            }
            else if (globalData.loggedinUser.B2.Equals("1"))
            {
                txtAccess.text = "Beginner 2";
            }
            else if (globalData.loggedinUser.I1.Equals("1"))
            {
                txtAccess.text = "Intermediate 1";
            }
            else if (globalData.loggedinUser.I2.Equals("1"))
            {
                txtAccess.text = "Intermediate 2";
            }
            else if (globalData.loggedinUser.I3.Equals("1"))
            {
                txtAccess.text = "Intermediate 3";
            }
            else if (globalData.loggedinUser.A1.Equals("1"))
            {
                txtAccess.text = "Advanced 1";
            }
            else if (globalData.loggedinUser.A2.Equals("1"))
            {
                txtAccess.text = "Advanced 2";
            }
            txtVersion.text = "Version: " + globalData.version;

            txtUser.text = "Logged-In User: " + txtUser.text;
            txtToken.text = "Registered Token: " + txtToken.text;
            txtSchool.text = "School: " + txtSchool.text;
            txtAccess.text = "Access Level: " + txtAccess.text;
        }
        else
        {

        }
    }

     

	public void CloseButtonClicked ()
	{
        if (globalData.loggedIn)
        {
            SceneManager.LoadScene("ListLevel", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("LogIn", LoadSceneMode.Single);
        }
    }
}
