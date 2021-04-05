using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.IO.Compression;
using UnityEngine.Networking;
using System.Net;

public class MenuOpt : MonoBehaviour
{
	//sorry for this clutter of code I will organize it later lol im a gamer B)
	//Features to add: Use arrays to add more mods to the list, Support for other drives than C:, Uhh idk
	public GameObject MainUi;
	public GameObject MenuUi;
	public GameObject TabTOI;
	public GameObject Msg;
	public GameObject Gmsg;
	public GameObject Dwn;
	public GameObject Ld;
	public static float VLaunch = 0;
	public static float VTOI = 0;
	private bool HasTOI;
	public Text Edt;
	public Text Prog;
	public int Bruh;
	public string TOIPath;
	public string exeau;
	public string DiscordLink;
	public string githubLink;
	public string serverLink;
	public string uri;
	private string ae;
	private string ap;
	public string spl;
	private float serverVTOI;
	private float serverVL;
	public Slider slider;
	public string tempPath;
	public string AmongPath;
	public string Slash;
	public int modamt;
	private int ck = 0;
	public float clientversion;
	private string LaunchUpdateLink;
	private float lw;

	void Awake()
	{
		LoadPlayer();
		HasTOI = false;
		VLaunch = VLaunch + 0.001f;

		//How do I fix this?
		tempPath = "C:" + Slash + "Users" + Slash + Environment.UserName + Slash + "Downloads";
		//tempPath = ;
	}

	void Update()
    {
		if(VLaunch == 0)
        {
			VLaunch = clientversion;
			SaveSystem.SavePlayer(this);
		}

		if (Directory.Exists(TOIPath))
		{
			HasTOI = true;
		}

		if (!Directory.Exists(tempPath))
		{
			Directory.CreateDirectory(tempPath);
		}

		if (serverVTOI >= VTOI && HasTOI == true)
		{
			Edt.text = "Update";
		}

		if(serverVTOI == VTOI && HasTOI == true)
		{
			Edt.text = "Play";
		}

		if (serverVL >= VLaunch && ck == 0 && lw == 1)
		{
			UpdateG();
			ck = 1;
		}
		//checks for launcher updates
		//Debug.Log("SVL" + serverVL + ":VL" + VLaunch + ":LW" + lw);
		if (HasTOI == false)
		{
			Edt.text = "Download";
		}
	}

	public void Play ()
	{
		MainUi.SetActive(false);
		MenuUi.SetActive(true);
	}

	public void Dismiss()
    {
		Msg.SetActive(false);
	}

	public void UpdateG()
    {
		MainUi.SetActive(false);
		Gmsg.SetActive(true);
	}

	public void ContG()
	{
		Application.OpenURL(LaunchUpdateLink);
	}

	public void EndG()
	{
		Gmsg.SetActive(false);
		MainUi.SetActive(true);
	}

	public void TabTOIe()
	{
		TabTOI.SetActive(true);
		MenuUi.SetActive(false);
	}

	public void TabBACK()
	{
		TabTOI.SetActive(false);
		MenuUi.SetActive(true);
	}

	public void Save()
	{
		SaveSystem.SavePlayer(this);
	}

	public void LaunchTOI()
    {
		if(serverVTOI == VTOI && HasTOI == true)
		{
			System.Diagnostics.Process.Start(TOIPath + exeau);
		}
		if (HasTOI == false)
		{
			Download();
		}
		if (serverVTOI >= VTOI && HasTOI == true)
		{
			Directory.Delete(TOIPath, true);
			Download();
		}
	}

	public void discord()
    {
		Application.OpenURL(DiscordLink);
    }

	public void github()
	{
		Application.OpenURL(githubLink);
	}

	public void Quit ()
	{
		Application.Quit();
	}

	//Loading

	public void LoadPlayer()
	{
		GameData data = SaveSystem.LoadPlayer();
		VLaunch = data.VLaunch;
		VTOI = data.VTOI;
	}

	//Retrieving data from website

	void Start()
	{
		StartCoroutine(GetText());
	}

	IEnumerator GetText()
	{
		UnityWebRequest www = UnityWebRequest.Get(serverLink);
		yield return www.SendWebRequest();

		if (www.result != UnityWebRequest.Result.Success)
		{
			Debug.Log(www.error);
		}
		else
		{
			ae = (www.downloadHandler.text);
			Split();
		}
	}

	//Split data from website
	public void Split()
	{
		string[] splitArray = ae.Split(char.Parse(spl));
		ap = splitArray[13];
		string[] splitArray2 = ap.Split(char.Parse("@"));
		serverVL = float.Parse(splitArray2[2]);
		serverVTOI = float.Parse(splitArray2[4]);
		uri = splitArray2[5];
		LaunchUpdateLink = splitArray2[6];
		lw = float.Parse(splitArray2[0]);
	}

	//Download from website and installs

	public void Download()
    {
		Dwn.SetActive(false);
		Ld.SetActive(true);
		slider.value = 0.1f;
		WebClient client = new WebClient();
		client.DownloadFile(uri, tempPath);
		slider.value = 0.5f;
		Directory.CreateDirectory(TOIPath);
		slider.value = 0.6f;
		DirectoryCopy(AmongPath, TOIPath, true);
		slider.value = 0.7f;
		File.Delete(TOIPath + Slash + "steam_appid.txt");
		ZipFile.ExtractToDirectory(tempPath, TOIPath);
		slider.value = 0.8f;
		VTOI = serverVTOI;
		SaveSystem.SavePlayer(this);
		slider.value = 1.0f;
		Dwn.SetActive(true);
		Ld.SetActive(false);
	}










	//Directory copyer (Code was provided by microsoft's .NET documentation)

	private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
	{
		// Get the subdirectories for the specified directory.
		DirectoryInfo dir = new DirectoryInfo(sourceDirName);

		if (!dir.Exists)
		{
			throw new DirectoryNotFoundException(
				"Source directory does not exist or could not be found: "
				+ sourceDirName);
		}

		DirectoryInfo[] dirs = dir.GetDirectories();

		// If the destination directory doesn't exist, create it.       
		Directory.CreateDirectory(destDirName);

		// Get the files in the directory and copy them to the new location.
		FileInfo[] files = dir.GetFiles();
		foreach (FileInfo file in files)
		{
			string tempPath = Path.Combine(destDirName, file.Name);
			file.CopyTo(tempPath, false);
		}

		// If copying subdirectories, copy them and their contents to new location.
		if (copySubDirs)
		{
			foreach (DirectoryInfo subdir in dirs)
			{
				string tempPath = Path.Combine(destDirName, subdir.Name);
				DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
			}
		}
	}
}
