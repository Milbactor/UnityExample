using UnityEngine;
using System.Collections;

public class BenchyExportPathWidget : MonoBehaviour {

    public BenchyRuntime.Location Location;
    public GUIStyle DetailStyle;

    [NeverProfileMethod]
    void Start () {
        //Use BenchyRuntime.ResetExportFilePath(); to reset the export file path to the devices / pc / mac default
        //You can also set your own export path example: BenchyRuntime.ExportFilePath = @"c:\somefolder\test";
    }

    [NeverProfileMethod]
    void OnGUI () {
        BenchyRuntime.RenderWidget(Location, BenchyRuntime.WidgetType.ExportPath, null, DetailStyle);
    }
}

  
