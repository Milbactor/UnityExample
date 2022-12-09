using UnityEngine;
using System.Collections;

public class BenchyBenchmarkWidget : MonoBehaviour {

    public BenchyRuntime.Location Location;
    public GUIStyle DetailStyle;

    [NeverProfileMethod]
    void OnGUI() {
        BenchyRuntime.RenderWidget(Location, BenchyRuntime.WidgetType.BenchmarkScore, null, DetailStyle);
    }
}
