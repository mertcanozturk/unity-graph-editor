using UnityEditor;

namespace QuickEditor.Graphic
{
    [CustomEditor(typeof(PlottingGraphTest))]
    public class PlottingGraphTestEditor : Editor
    {
        private PlottingGraph graph;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PlottingGraphTest graphTest = (PlottingGraphTest)target;

            if (graph == null)
            {
                graph = new PlottingGraph("test plotting graph", graphTest.graphData);
            }
            else
            {
                graph.UpdateGraphData(graphTest.graphData);
            }

            graph.DrawLineGraph(this);
        }
    }
}
