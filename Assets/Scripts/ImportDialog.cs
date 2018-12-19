using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class ImportDialog : MonoBehaviour {

	public Visualizer visualizer;

    private GameObject scatterplotTogglePrefab;

	private Canvas renderer;
    private GameObject panel;
    private BoxCollider collider;
    private Dropdown inputFiles;
    private GameObject toggleList;
    private Scrollbar scrollbar;
    private Button buttonImport;

    private vrJoystick joy;
    private bool lastFramePressed = false;

    private string csvDirectoryName = "Datasets";
	private string csvDataDirectory;

	// Use this for initialization
	void Start () {
        this.scatterplotTogglePrefab = Resources.Load("Prefabs/checkbox") as GameObject;

        this.renderer = this.gameObject.GetComponent<Canvas>();
        this.collider = this.gameObject.GetComponent<BoxCollider>();
        this.panel = GameObject.Find("Panel");

		this.csvDataDirectory = Application.dataPath;
		this.csvDataDirectory = Path.Combine(csvDataDirectory, csvDirectoryName);

		if(!Directory.Exists(this.csvDataDirectory)){
			Directory.CreateDirectory(this.csvDataDirectory);
		}

		this.inputFiles = GameObject.Find("dropdown_input_files").GetComponent<Dropdown>();
        this.inputFiles.onValueChanged.AddListener(InputFileSelectionChanged);

		foreach(string file in Directory.GetFiles(this.csvDataDirectory, "*.csv")){
			FileInfo fileInfo = new FileInfo(file);
			this.inputFiles.options.Add(new Dropdown.OptionData(){text = fileInfo.Name});
		}

        this.toggleList = GameObject.Find("toggle_list");
        this.scrollbar = GameObject.Find("scrollbar").GetComponent<Scrollbar>();

		this.buttonImport = GameObject.Find("button_import").GetComponent<Button>();
		this.buttonImport.onClick.AddListener(onButtonImportClicked);

        this.joy = MiddleVR.VRDeviceMgr.GetJoystickByIndex(0);
		setVisible(false);

        // Call manually to trigger the loading of the CSV file
        InputFileSelectionChanged(0);
    }

    private void InputFileSelectionChanged(int elem)
    {
        var filePath = Path.Combine(this.csvDataDirectory, this.inputFiles.options.ToArray()[elem].text);
        this.visualizer.LoadDataSource(filePath);

        CreateScatterplotToggles();
    }

    private void CreateScatterplotToggles()
    {
        foreach (Transform toggle in this.toggleList.transform)
        {
            Destroy(toggle.gameObject);
        }

        string[] possibleScatterplots = this.visualizer.GetPossibleScattersplots();
        int possibilities = possibleScatterplots.GetLength(0);
        for (int possibility = 0; possibilities > possibility; ++possibility)
        {
            GameObject toggle = Instantiate(scatterplotTogglePrefab, this.toggleList.transform) as GameObject;
            toggle.GetComponentInChildren<Text>().text = possibleScatterplots[possibility];
            toggle.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(-200, -25 * possibility + 120) / 100, Quaternion.identity);
        }
        this.toggleList.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20 * possibilities);
        
        this.scrollbar.value = 1;
    }

    // Update is called once per frame
    void Update () {

        if (10 >= this.toggleList.transform.childCount)
        {
            this.scrollbar.size = 1;
        }

        bool pressed = joy.IsButtonPressed(3);
		if(pressed && !lastFramePressed){
			setVisible(!renderer.enabled);
		}
        lastFramePressed = pressed;

        if (renderer.enabled)
        {
            GameObject head = GameObject.Find("HeadNode");
            transform.position = head.transform.position + head.transform.forward * 7;
            transform.rotation = Quaternion.LookRotation(transform.position - head.transform.position);
        }
    }

	void setVisible(bool value){
		renderer.enabled = value;
        collider.enabled = value;
        panel.SetActive(value);
	}

	void onButtonImportClicked(){
        List<int> scatterplotIndices = new List<int>();
        for (int i = 0; this.toggleList.transform.childCount > i; ++i)
        {
            if (this.toggleList.transform.GetChild(i).GetComponent<Toggle>().isOn)
            {
                scatterplotIndices.Add(i);
            }
        }

		this.visualizer.CreateScatterplotMatrix(scatterplotIndices.ToArray());
		setVisible(false);
	}
}
