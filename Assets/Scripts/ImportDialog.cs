using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

/// <summary>
/// This sets up the ImportDialog and handles user interactions.
/// It communicates with the Visualizer to load CSV-Files, create
/// ScatterplotMatrices and change the size of DataPoints.
/// </summary>
public class ImportDialog : MonoBehaviour
{
	public Visualizer visualizer;

    private GameObject scatterplotTogglePrefab;

	private Canvas renderer;
    private GameObject panel;
    private BoxCollider collider;
    private Dropdown inputFiles;
    private GameObject toggleList;
    private Scrollbar scrollbar;
    private Button selectAll;
    private Button deselectAll;
    private Button buttonImport;
    private Button decrement;
    private Text pointSize;
    private Button increment;

    /// <summary>
    /// Reference to the MiddleVR HeadNode.
    /// </summary>
    private GameObject headNode;
    /// <summary>
    /// Reference to the buttons of the wand.
    /// </summary>
    private vrButtons wandButtons;

    private string csvDirectoryName = "Datasets";
    /// <summary>
    /// All the found CSV-Files
    /// </summary>
    private TextAsset[] dataFiles;

	// Use this for initialization
	void Start()
    {
        scatterplotTogglePrefab = Resources.Load("Prefabs/checkbox") as GameObject;

        renderer = gameObject.GetComponent<Canvas>();
        collider = gameObject.GetComponent<BoxCollider>();
        panel = GameObject.Find("Panel");

		inputFiles = GameObject.Find("dropdown_input_files").GetComponent<Dropdown>();
        inputFiles.onValueChanged.AddListener(InputFileSelectionChanged);

        //dataFiles = Resources.LoadAll<TextAsset>(csvDirectoryName);
        FileInfo[] fileInfos = new DirectoryInfo(Application.streamingAssetsPath).GetFiles("*.csv");

        dataFiles = new TextAsset[fileInfos.Length];
        int textAssetIndex = 0;
        foreach (FileInfo fileInfo in fileInfos)
        {
            inputFiles.options.Add(new Dropdown.OptionData() { text = fileInfo.Name });
            dataFiles[textAssetIndex] = new TextAsset(File.ReadAllText(fileInfo.FullName));
            dataFiles[textAssetIndex++].name = fileInfo.Name;
        }
        inputFiles.RefreshShownValue();

        toggleList = GameObject.Find("toggle_list");
        scrollbar = GameObject.Find("scrollbar").GetComponent<Scrollbar>();

        selectAll = GameObject.Find("button_select_all").GetComponent<Button>();
        selectAll.onClick.AddListener(OnSelectAll);
        deselectAll = GameObject.Find("button_deselect_all").GetComponent<Button>();
        deselectAll.onClick.AddListener(OnDeselectAll);

        buttonImport = GameObject.Find("button_import").GetComponent<Button>();
		buttonImport.onClick.AddListener(OnButtonImportClicked);

        decrement = GameObject.Find("button_decrement").GetComponent<Button>();
        decrement.onClick.AddListener(OnButtonDecrement);
        pointSize = GameObject.Find("label_point_size_value").GetComponent<Text>();
        pointSize.text = visualizer.pointSize.ToString();
        increment = GameObject.Find("button_increment").GetComponent<Button>();
        increment.onClick.AddListener(OnButtonIncrement);

        headNode = GameObject.Find("HeadNode");
        wandButtons = MiddleVR.VRDeviceMgr.GetJoystickByIndex(0).GetButtonsDevice();
		SetVisible(false);

        // Call manually to trigger the loading of the CSV file
        InputFileSelectionChanged(0);
    }

    /// <summary>
    /// Once a new CSV-File from the DropdownMenu is chosen,
    /// this method is called. The Visualizer than loads the file
    /// and the possible Scatterplots are shown to the user.
    /// </summary>
    /// <param name="elem"></param>
    private void InputFileSelectionChanged(int elem)
    {
        visualizer.LoadDataSource(dataFiles[elem]);

        CreateScatterplotToggles();
    }

    /// <summary>
    /// This creates a toggle button for every possible Scatterplot
    /// of the currently selected CSV-File.
    /// </summary>
    private void CreateScatterplotToggles()
    {
        foreach (Transform toggle in toggleList.transform)
        {
            Destroy(toggle.gameObject);
        }

        string[] possibleScatterplots = visualizer.GetPossibleScattersplots();
        int possibilities = possibleScatterplots.GetLength(0);
        for (int possibility = 0; possibilities > possibility; ++possibility)
        {
            GameObject toggle = Instantiate(scatterplotTogglePrefab, toggleList.transform) as GameObject;
            toggle.GetComponentInChildren<Text>().text = possibleScatterplots[possibility];
            toggle.GetComponent<RectTransform>().anchoredPosition = (Vector3.down * 25 * possibility);
        }
        toggleList.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 25 * possibilities);
        
        scrollbar.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (8 >= toggleList.transform.childCount)
        {
            scrollbar.size = 1;
        }
        
        if (wandButtons.IsToggled(3))
        {
			SetVisible(!renderer.enabled);
		}

        if (renderer.enabled)
        {
            transform.position = headNode.transform.position + headNode.transform.forward * 7;
            transform.rotation = Quaternion.LookRotation(transform.position - headNode.transform.position);
        }
    }

    /// <summary>
    /// Sets wether the ImportDialog is shown.
    /// </summary>
    /// <param name="value"></param>
	void SetVisible(bool value)
    {
		renderer.enabled = value;
        collider.enabled = value;
        panel.SetActive(value);
	}

    /// <summary>
    /// Handle for the select-all button.
    /// </summary>
    void OnSelectAll()
    {
        for (int i = 0; toggleList.transform.childCount > i; ++i)
        {
            toggleList.transform.GetChild(i).GetComponent<Toggle>().isOn = true;
        }
    }

    /// <summary>
    /// Handle for the deselect-all button.
    /// </summary>
    void OnDeselectAll()
    {
        for (int i = 0; toggleList.transform.childCount > i; ++i)
        {
            toggleList.transform.GetChild(i).GetComponent<Toggle>().isOn = false;
        }
    }

    /// <summary>
    /// Handle for the import button.
    /// Iterates through all scatterplot toggle buttons and creates
    /// an array of the indeces of the activated ones.
    /// This array is provided to the Visualizer so that it nows
    /// which Scatterplots are to be created.
    /// </summary>
	void OnButtonImportClicked()
    {
        List<int> scatterplotIndices = new List<int>();
        for (int i = 0; toggleList.transform.childCount > i; ++i)
        {
            if (toggleList.transform.GetChild(i).GetComponent<Toggle>().isOn)
            {
                scatterplotIndices.Add(i);
            }
        }

		visualizer.CreateScatterplotMatrix(scatterplotIndices.ToArray());
		SetVisible(false);
	}

    /// <summary>
    /// Handle for the minus button.
    /// </summary>
    void OnButtonDecrement()
    {
        visualizer.pointSize = Mathf.Max(0, (float)Math.Round(visualizer.pointSize - 0.001, 3));
        pointSize.text = visualizer.pointSize.ToString();
    }

    /// <summary>
    /// Handle for the plus button.
    /// </summary>
    void OnButtonIncrement()
    {
        visualizer.pointSize = (float)Math.Round(visualizer.pointSize + 0.001, 3);
        pointSize.text = visualizer.pointSize.ToString();
    }
}
