using UnityEngine;
using System.Collections;

public class BenchyOverviewWidget : MonoBehaviour {

    public BenchyRuntime.Location Location;
    public GUIStyle HeadingStyle;
    public GUIStyle DetailStyle;

    [NeverProfileMethod]
    void OnGUI() {
        BenchyRuntime.RenderWidget(Location, BenchyRuntime.WidgetType.Overview, HeadingStyle, DetailStyle);
    }
}
