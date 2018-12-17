using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImportDialog : MonoBehaviour {

	public Visualizer visualizer;

	private Canvas renderer;
    private Dropdown inputFiles;
    private Dropdown dataFieldFrom;
    private Dropdown dataFieldTo;
    private Toggle hasFieldLabel;
    private Text labelFieldLabelCell;
    private Dropdown fieldLabelCell;
    private Button buttonImport;

    private vrJoystick joy;
    private bool lastFramePressed = false;

    private string csvDirectoryName = "Datasets";
	private string csvDataDirectory;

	// Use this for initialization
	void Start () {
		renderer = this.gameObject.GetComponent<Canvas>();
		setVisible(false);

		this.csvDataDirectory = Application.dataPath;
		this.csvDataDirectory = Path.Combine(csvDataDirectory, csvDirectoryName);

		if(!Directory.Exists(this.csvDataDirectory)){
			Directory.CreateDirectory(this.csvDataDirectory);
		}

		this.inputFiles = GameObject.Find("dropdown_input_files").GetComponent<Dropdown>();

		foreach(string file in Directory.GetFiles(this.csvDataDirectory, "*.csv")){
			FileInfo fileInfo = new FileInfo(file);
			this.inputFiles.options.Add(new Dropdown.OptionData(){text = fileInfo.Name});
		}

		this.dataFieldFrom = GameObject.Find("dropdown_data_from").GetComponent<Dropdown>();
		this.dataFieldTo = GameObject.Find("dropdown_data_to").GetComponent<Dropdown>();

		this.hasFieldLabel = GameObject.Find("checkbox_has_field_label").GetComponent<Toggle>();
		this.hasFieldLabel.onValueChanged.AddListener(onHasFieldLabelChanged);

		this.fieldLabelCell = GameObject.Find("dropdown_field_label_cell").GetComponent<Dropdown>();
		this.fieldLabelCell.interactable = this.hasFieldLabel.isOn;

		for(int num = 0; num < 20; num++){
			Dropdown.OptionData option = new Dropdown.OptionData(){text = num.ToString()};
			this.dataFieldFrom.options.Add(option);
			this.dataFieldTo.options.Add(option);
			this.fieldLabelCell.options.Add(option);
		}

		this.buttonImport = GameObject.Find("button_import").GetComponent<Button>();
		this.buttonImport.onClick.AddListener(onButtonImportClicked);

        this.joy = MiddleVR.VRDeviceMgr.GetJoystickByIndex(0);
	}

	// Update is called once per frame
	void Update () {
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
	}

	void onHasFieldLabelChanged(bool value){
		this.fieldLabelCell.interactable = value;
	}

	void onButtonImportClicked(){		
		var filePath = Path.Combine(this.csvDataDirectory, this.inputFiles.options.ToArray()[this.inputFiles.value].text);
		this.visualizer.LoadData(filePath);	
		setVisible(false);
	}
}
