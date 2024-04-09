using System.Linq;
using UnityEditor;
using UnityEngine;

namespace QuickEditor.Graphic
{
    /// <summary>
    /// Class for drawing line graphs in the Unity Editor.
    /// </summary>
    public class PlottingGraph
    {
        // Properties for graph dimensions and appearance
        public int GraphWidth { get; set; } = 1200;
        public int GraphHeight { get; set; } = 400;
        public int GraphPadding { get; set; } = 45;
        public int GraphValuesCount { get; set; } = 10;

        // Data for the graph
        private Vector2[] m_graphData;
        private readonly string m_graphName;

        /// <summary>
        /// Constructor to initialize the graph with data and a name.
        /// </summary>
        /// <param name="graphName">Name of the graph.</param>
        /// <param name="graphData">Array of Vector2 representing the graph data points.</param>
        public PlottingGraph(string graphName, Vector2[] graphData)
        {
            m_graphName = graphName;
            m_graphData = graphData;
        }

        /// <summary>
        /// Draws the line graph in the Unity Editor.
        /// </summary>
        public void DrawLineGraph(Editor editor)
        {
            if (m_graphData == null || m_graphData.Length == 0)
            {
                return;
            }

            // Calculate the rectangle for drawing the graph
            Rect graphRect = GUILayoutUtility.GetRect(GraphWidth, GraphHeight);
            graphRect.x += GraphPadding;
            graphRect.y += GraphPadding;
            graphRect.width -= GraphPadding * 2;
            graphRect.height -= GraphPadding * 2;

            // Draw the graph name label
            Handles.Label(new Vector3(graphRect.xMin, graphRect.yMin - 10f), m_graphName, EditorStyles.boldLabel);

            // Draw the background of the graph
            DrawGraphBackground(graphRect);

            // Calculate min/max values for normalization
            float xMin = m_graphData.Min(point => point.x);
            float xMax = m_graphData.Max(point => point.x);
            float yMin = m_graphData.Min(point => point.y);
            float yMax = m_graphData.Max(point => point.y);

            // Get current mouse position for interaction
            var mousePosition = Event.current.mousePosition;

            // Iterate through data points and draw lines
            for (int i = 0; i < m_graphData.Length - 1; i++)
            {
                Vector2 pointA = m_graphData[i];
                Vector2 pointB = m_graphData[i + 1];

                // Normalize points for drawing
                float xNormalizedA = Mathf.InverseLerp(xMin, xMax, pointA.x);
                float yNormalizedA = Mathf.InverseLerp(yMin, yMax, pointA.y);
                float xNormalizedB = Mathf.InverseLerp(xMin, xMax, pointB.x);
                float yNormalizedB = Mathf.InverseLerp(yMin, yMax, pointB.y);

                // Calculate positions for drawing
                Vector2 pointAPosition = new(
                    graphRect.x + xNormalizedA * graphRect.width,
                    graphRect.y + (1 - yNormalizedA) * graphRect.height
                );
                Vector2 pointBPosition = new(
                    graphRect.x + xNormalizedB * graphRect.width,
                    graphRect.y + (1 - yNormalizedB) * graphRect.height
                );

                // Draw line between points
                Handles.DrawLine(pointAPosition, pointBPosition);

                // Check if mouse is over the point and draw information if so
                if (TryDrawPointInfo(pointAPosition, pointA, mousePosition))
                {
                    Handles.color = Color.red;
                }
                DrawPointWireCube(pointAPosition);
                Handles.color = Color.white;

                if (TryDrawPointInfo(pointBPosition, pointB, mousePosition))
                {
                    Handles.color = Color.red;
                }
                DrawPointWireCube(pointBPosition);

                Handles.color = Color.white;
            }

            // Draw labels for the graph
            DrawGraphLabels(graphRect, xMin, xMax, yMin, yMax);

            // Refresh editor to show changes
            editor.Repaint();
        }

        // Draw a wire cube at the given point
        private void DrawPointWireCube(Vector2 pointAPosition)
        {
            Handles.DrawWireCube(pointAPosition, Vector3.one * 3);
        }

        // Draw labels for the graph
        private void DrawGraphLabels(Rect graphRect, float xMin, float xMax, float yMin, float yMax)
        {
            int valuesCount = GraphValuesCount;
            if (m_graphData.Length < valuesCount)
            {
                valuesCount = m_graphData.Length;
            }

            float xStep = (xMax - xMin) / valuesCount;
            float yStep = (yMax - yMin) / valuesCount;

            for (int i = 0; i <= valuesCount; i++)
            {
                float xValue = xMin + i * xStep;
                float yValue = yMin + i * yStep;

                Vector3 xLabelPosition = new(graphRect.x + i * (graphRect.width / valuesCount), graphRect.y + graphRect.height + 10, 0);
                Vector3 yLabelPosition = new(graphRect.x - 7.5f * yValue.ToString("0.0").Length, graphRect.y + (valuesCount - i) * (graphRect.height / valuesCount), 0);

                Handles.Label(xLabelPosition, ((int)xValue).ToString());
                Handles.Label(yLabelPosition, yValue.ToString("0.0"));
            }
        }

        // Draw the background of the graph
        private void DrawGraphBackground(Rect graphRect)
        {
            GUI.Box(graphRect, GUIContent.none);

            Handles.DrawLine(new Vector3(graphRect.x, graphRect.y + graphRect.height, 0), new Vector3(graphRect.x + graphRect.width, graphRect.y + graphRect.height, 0));
            Handles.DrawLine(new Vector3(graphRect.x, graphRect.y, 0), new Vector3(graphRect.x, graphRect.y + graphRect.height, 0));
        }

        // Try to draw information about the point if the mouse is over it
        private bool TryDrawPointInfo(Vector2 point, Vector2 value, Vector2 mousePosition)
        {
            Rect rect = new Rect(point.x, point.y - 150, 15, 300);
            if (rect.Contains(mousePosition))
            {
                GUIStyle style = new();
                style.normal.textColor = Color.red;
                Handles.Label(point, value.ToString(), style);
                return true;
            }
            return false;
        }

        public void UpdateGraphData(Vector2[] graphData)
        {
            m_graphData = graphData;
        }
    }
}
