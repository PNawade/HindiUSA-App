using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class pnlLevel : MonoBehaviour, IPointerClickHandler
{
	public Text uiLevelHindi;
	public Text uiLevelEnglish;
	public Text uiTermHindi;
	public Text uiTermEnglish;

	private string lstName;
	private string unixDirName;
	private GameObject parentGo;
	private GameObject controllerGo;
	private bool fSubMenu;

	public void Init (txtList.lvlMenu mnu)
	{
		lstName = mnu.lvlNameEnglish;
		unixDirName = mnu.unixDirName;

		if (mnu.lvlNameHindi == "") {
			uiLevelEnglish.text = mnu.lvlNameHindiRoman + " <color=#c0c0c0ff>(" + mnu.lvlNameEnglish + ")</color>";
			uiTermEnglish.text = mnu.termCount + " Sets ";
			uiLevelHindi.text = "";
			uiTermHindi.text = "";
		} else {
			uiLevelHindi.text = mnu.lvlNameHindi;
			uiTermHindi.text = mnu.termCount + " Sets ";
			uiLevelEnglish.text = "";
			uiTermEnglish.text = "";
		}
		if (mnu.subMenu == "1")
			fSubMenu = true;
		else
			fSubMenu = false;

		parentGo = GameObject.Find ("pnlBackground");
		controllerGo = GameObject.Find ("GameController");
		gameObject.transform.SetParent (parentGo.transform, false);
	}



	public void OnPointerClick (PointerEventData ped)
	{
		//Debug.Log ("touched : " + gameObject.name);
		if (authorized (lstName)) {
			controllerGo.GetComponent<llGameController> ().GoForward (lstName, unixDirName, fSubMenu);
		} else {
			print ("Unauthorized!");
			#if UNITY_IPHONE
			globalData.showNativeAlert ("Unauthorized!", "You have not been given authorization to access this level.");
			#endif
		}
	}

	private bool authorized (string lstName)
	{
		if (fSubMenu) {
			if (globalData.loggedinUser.admin.Equals ("1")) {
				return true;
			}
			switch (lstName) {
			case "STARTER-1":
				if (globalData.loggedinUser.S1.Equals ("1"))
					return true;
				break;
			case "STARTER-2":
				if (globalData.loggedinUser.S2.Equals ("1"))
					return true;
				break;
			case "BEGINNER-1":
				if (globalData.loggedinUser.B1.Equals ("1"))
					return true;
				break;
			case "BEGINNER-2":
				if (globalData.loggedinUser.B2.Equals ("1"))
					return true;
				break;
			case "INTERMEDIATE-1":
				if (globalData.loggedinUser.I1.Equals ("1"))
					return true;
				break;
			case "INTERMEDIATE-2":
				if (globalData.loggedinUser.I2.Equals ("1"))
					return true;
				break;
			case "INTERMEDIATE-3":
				if (globalData.loggedinUser.I3.Equals ("1"))
					return true;
				break;
			case "ADVANCE-1":
				if (globalData.loggedinUser.A1.Equals ("1"))
					return true;
				break;
			case "ADVANCE-2":
				if (globalData.loggedinUser.A2.Equals ("1"))
					return true;
				break;
			default:
				break;
			}
		} else {
			return true;
		}
		return false;
	}

}