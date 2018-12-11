using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImportDialog : MonoBehaviour {

	public VisualizerController visualizer;

	Canvas renderer;
	Dropdown inputFiles;
	Dropdown dataFieldFrom;
	Dropdown dataFieldTo;
	Toggle hasFieldLabel;
	Text labelFieldLabelCell;
	Dropdown fieldLabelCell;
	Button buttonImport;

	private string csvDirectoryName = "Datasets";
	private string csvDataDirectory;

	// Use this for initialization
	void Start () {
		renderer = this.gameObject.GetComponent<Canvas>();
		renderer.enabled = false;

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
		this.dataFieldFrom.options.Add(new Dropdown.OptionData(){text = "0"});
		this.dataFieldFrom.options.Add(new Dropdown.OptionData(){text = "1"});

		this.dataFieldTo = GameObject.Find("dropdown_data_to").GetComponent<Dropdown>();

		this.hasFieldLabel = GameObject.Find("checkbox_has_field_label").GetComponent<Toggle>();
		this.hasFieldLabel.onValueChanged.AddListener(onHasFieldLabelChanged);

		this.fieldLabelCell = GameObject.Find("dropdown_field_label_cell").GetComponent<Dropdown>();
		this.fieldLabelCell.interactable = this.hasFieldLabel.isOn;

		this.buttonImport = GameObject.Find("button_import").GetComponent<Button>();
		this.buttonImport.onClick.AddListener(onButtonImportClicked);

	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.X)){
			renderer.enabled = !renderer.enabled;
		}	
	}

	void onHasFieldLabelChanged(bool value){
		this.fieldLabelCell.interactable = value;
	}

	void onButtonImportClicked(){		
		var filePath = Path.Combine(this.csvDataDirectory, this.inputFiles.options.ToArray()[this.inputFiles.value].text);
		this.visualizer.LoadData(filePath);	
	}
}
