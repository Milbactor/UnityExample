using UnityEngine;
using System.Collections;

public class BenchyOnDeviceWidget : MonoBehaviour {

    public BenchyRuntime.Location Location;
    public GUIStyle DetailStyle;

    [NeverProfileMethod]
    void OnGUI() {
        BenchyRuntime.RenderWidget(Location, BenchyRuntime.WidgetType.MainControls, null, DetailStyle);
    }
}
