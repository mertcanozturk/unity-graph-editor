# PlottingGraph: Library for Drawing Line Graphs in Unity Editor

This library is used for creating line graphs within the Unity Editor. It allows you full control over graph dimensions, interface properties, and graph data. You can use it to visually represent your graphs and create interactive experiences on them.

## Features:

![](https://github.com/mertcanozturk/unity-graph-editor/blob/main/plotting-graph.gif)

You can adjust properties such as graph width, height, padding, and the number of values.
Easily visualize graph data and create interactive experiences on the graph.
Customize the graph background, data points, labels, and the line itself.
Provides a user-friendly interface for direct use within the Unity Editor.

## Usage:

Use the PlottingGraph class to create your line graphs and provide the data.
Call the DrawLineGraph method to draw the graphs within the Unity Editor.
Add detailed information visualizations by clicking on data points on the graph.

```c#
using UnityEditor;
using UnityEngine;

namespace QuickEditor.Graphic
{
    [CustomEditor(typeof(PlottingGraphTest))]
    public class PlottingGraphTestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PlottingGraphTest graphTest = (PlottingGraphTest)target;
            PlottingGraph graph = new PlottingGraph("test plotting graph", graphTest.graphData);
            graph.DrawLineGraph(this);
        }
    }
}
```

This library helps you quickly create and visualize graphs, enhancing your workflow within the Unity Editor. It's an ideal tool for visually representing your graphical data.
