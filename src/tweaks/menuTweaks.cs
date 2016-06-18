using System;
using RA3Tweaks.Tweaks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class MenuTweaks : MenuBase
{
    private Text helpMessage;
    private Queue modelsToExport = null;
    private bool isExporting
    {
        get { return this.modelsToExport != null && this.modelsToExport.Count > 0; }
    }

    public override void Start()
    {
        base.Start();

        // Since script is not exported with AssetBundles, 
        // we need to hook up all the events at runtime
        this.helpMessage = this.transform.FindAChild<Text>("HelpMessage");

        // Add the handlers for the export buttons
        var exportJsonButton = this.transform.FindAChild<Button>("BtnExportJson");
        Debug.Log(exportJsonButton);
        exportJsonButton.onClick.AddListener(this.OnExportJson);
        AddHoverEvents(exportJsonButton, "Write out the components.json file to disk");

        var exportModelsButton = this.transform.FindAChild<Button>("BtnExportModels");
        exportModelsButton.onClick.AddListener(this.OnExportModels);
        AddHoverEvents(exportModelsButton, "Write out all the component model files to disk (warning - slow!)");

        // Add the handlers for the back button
        var backButton = this.transform.FindAChild<Button>("BtnBack");
        backButton.onClick.AddListener(this.OnBack);
        AddHoverEvents(backButton, "Back to main menu");
    }

    public void OnExportJson()
    {
        Debug.Log("Clicked OnExportJson");

        DataExporter.ExportJson();
    }

    public void OnExportModels()
    {
        Debug.Log("Clicked OnExportModels");

        // Only export if we aren't already doing so
        if (!this.isExporting)
        {
            this.modelsToExport = new Queue();
            string allComponents = DataExporter.GetComponentsJson();

            // Get the names of all the prefabs
            string find = "\"componentprefabname\": \"";
            int start = allComponents.IndexOf(find);
            while (start >= 0)
            {
                start += find.Length;
                int end = allComponents.IndexOf("\",", start);
                string name = allComponents.Substring(start, (end - start));
                this.modelsToExport.Enqueue(name);

                start = allComponents.IndexOf(find, end + 1);
                Debug.Log("Found model name '" + name + "'");
            }

            // Use a coroutine to stop the UI from hanging with the amount of data it needs to process
            StartCoroutine(this.ExportModels());
        }
    }

    public void OnBack()
    {
        // Cancel exporting models
        if (this.isExporting)
        {
            this.modelsToExport.Clear();
        }
        this.helpMessage.text = "";

        this.menuManager.Pop();
        this.menuManager.Push("MenuMain");
    }

    private IEnumerator ExportModels()
    {
        while (this.modelsToExport.Count > 0)
        {
            this.helpMessage.text = string.Format("Exporting models... ({0} remaining)", this.modelsToExport.Count);
            
            // Export the next model
            string model = this.modelsToExport.Dequeue() as string;
            GameObject go = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("ComponentPrefabs/" + model));
            DataExporter.ExportModel(go, model);
            GameObject.Destroy(go);

            // Free up the UI for a bit
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void AddHoverEvents(Button b, string helpMessage)
    {
        // Button that we are adding events to
        Text buttonText = b.GetComponentInChildren<Text>();
        Color originalColor = buttonText.color;

        // The global help message area in the parent container
        this.helpMessage.text = "";

        AddEventHandler(b, EventTriggerType.PointerEnter, () =>
        {
            if (this.isExporting)
            {
                return;
            }

            this.helpMessage.text = helpMessage;
            buttonText.color = Color.white;
        });

        AddEventHandler(b, EventTriggerType.PointerExit, () =>
        {
            if (this.isExporting)
            {
                return;
            }

            this.helpMessage.text = "";
            buttonText.color = originalColor;
        });
    }

    private static void AddEventHandler(Button b, EventTriggerType type, Action callback)
    {
        var trigger = b.GetComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback = new EventTrigger.TriggerEvent();

        var call = new UnityEngine.Events.UnityAction<BaseEventData>((a) => { callback(); });
        entry.callback.AddListener(call);
        trigger.triggers.Add(entry);
    }
}