using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Lite;
using Lite.AStar;
using Lite.Graph;


public class MapEditor : MonoBehaviour
{
	int width = 50;
	int height = 30;
	int[,] nodeMarkList;
	string mapSavePath;
	string navSavePath;
	bool navMode;

	GraphAStarMap graph;
	int nodeCount = 0;
	int edgeCount = 0;

	Texture lineTex;
	Texture dotBlueTex;
	Texture dotRedTex;

	private int gridWidth = 20;
	private int gridHeight = 20;

	private int stepx = 20;
	private int stepy = 20;

	int offsetX = 120;
	int offsetY = 10;
	int gw = 20;
	int gh = 20;

	void Start()
	{
		nodeMarkList = new int[width, height];
		for (int x = 0; x < width; ++x)
		{
			for (int y = 0; y < height; ++y)
			{
				nodeMarkList[x, y] = 0;
			}
		}

		graph = new GraphAStarMap();

		mapSavePath = Application.dataPath + "/../map.txt";
		LoadMap(mapSavePath, nodeMarkList);
		navSavePath = Application.dataPath + "/../nav.txt";
		LoadNav(navSavePath, graph);
		CalcNavCount();

		lineTex = Resources.Load("Textures/line") as Texture;
		dotBlueTex = Resources.Load("Textures/dotBlue") as Texture;
		dotRedTex = Resources.Load("Textures/dotRed") as Texture;
		//
		navMode = true;
	}

	
	void OnGUI()
	{
		if (GUI.Button(new Rect(5, 85, 60, 20), "switch"))
		{
			navMode = !navMode;
		}

		if (Event.current.type.Equals(EventType.Repaint))
		{
			if (navMode)
			{
				DrawBlock();
				DrawGraph();
			}
			else
			{
				DrawEditorMode();
			}
		}

		if (navMode)
		{
			GUI.Label(new Rect(5, 10, 100, 20), "node " + nodeCount);
			GUI.Label(new Rect(5, 30, 100, 20), "edge " + edgeCount);
			if (GUI.Button(new Rect(5, 110, 60, 20), "fill"))
			{
				Fill();
			}
			if (GUI.Button(new Rect(5, 135, 60, 20), "save"))
			{
				SaveNav();
			}
		}
		else
		{
			if (GUI.Button(new Rect(5, 110, 60, 20), "save"))
			{
				SaveMap();
			}
		}

	}

	#region editor

	static public void LoadMap(string path, int[,] data)
	{
		StreamReader sr = new StreamReader(path, Encoding.ASCII);
		string line;
		int y = 0;
		while ((line = sr.ReadLine()) != null)
		{
			char[] arr = line.ToCharArray();
			for (int x = 0; x < arr.Length; ++x)
				data[x, y] = int.Parse(arr[x].ToString());
			y++;
		}
		sr.Close();
	}

	void SaveMap()
	{
		StringBuilder builder = new StringBuilder();
		for (int y = 0; y < height; ++y)
		{
			for (int x = 0; x < width; ++x)
			{
				builder.Append(nodeMarkList[x, y] == 0 ? "0" : "1");
			}
			if (y != height-1) builder.Append("\n");
		}
		
		FileStream fs = new FileStream(mapSavePath, FileMode.Create);
		StreamWriter sw = new StreamWriter(fs, Encoding.ASCII);
		sw.Write(builder.ToString());
		sw.Close();
		fs.Close();
		UnityEngine.Debug.Log("Saved map to " + mapSavePath);
	}

	static public void LoadNav(string path, GraphAStarMap graph)
	{
		StreamReader sr = new StreamReader(path, Encoding.ASCII);
		string line;
		while ((line = sr.ReadLine()) != null)
		{
			string[] nodeInfoArray = line.Split(',');
			if (nodeInfoArray.Length != 4)
				continue;
			GraphAStarNode node = graph.AddNode<GraphAStarNode>();
			node.id = int.Parse(nodeInfoArray[0]);
			node.x = int.Parse(nodeInfoArray[1]);
			node.y = int.Parse(nodeInfoArray[2]);
			int edgeCount = int.Parse(nodeInfoArray[3]);
			for (int i = 0; i < edgeCount; i++)
			{
				line = sr.ReadLine();
				string[] edgeInfoArray = line.Split(',');
				graph.AddEdge(int.Parse(edgeInfoArray[0]), int.Parse(edgeInfoArray[1]), int.Parse(edgeInfoArray[2]));
			}
		}
		sr.Close();
	}

	void SaveNav()
	{
		StringBuilder builder = new StringBuilder();

		var list = graph.GetNodeList();
		for (int i = 0; i < list.Count; ++i)
		{
			GraphAStarNode node = list[i] as GraphAStarNode;
			List<GraphEdge> edges = graph.GetEdgeList(node.id);
			builder.Append(string.Format("{0},{1},{2},{3}\n", node.id, node.x, node.y, edges.Count));
			for (int e = 0; e < edges.Count; ++e)
			{
				GraphEdge edge = edges[e];
				builder.Append(string.Format("{0},{1},{2}\n", edge.from, edge.to, edge.cost));
			}
		}
		
		FileStream fs = new FileStream(navSavePath, FileMode.Create);
		StreamWriter sw = new StreamWriter(fs, Encoding.ASCII);
		sw.Write(builder.ToString());
		sw.Close();
		fs.Close();
		UnityEngine.Debug.Log("Saved nav to " + navSavePath);
	}

	#endregion


	#region generate Graph

	void Fill()
	{
		int seedx = 0;
		int seedy = 0;
		//graph = new GraphAStarMap();
		DoFlood(graph, seedx, seedy);

		CalcNavCount();
	}

	private void DoFlood(GraphAStarMap graph, int x, int y)
	{
		if (IsInBlock(x,y))
			return;
		GraphAStarNode node = graph.GetNodeAt(x, y);
		if (node != null)
			return;
		node = graph.AddNode<GraphAStarNode>();
		node.x = x;
		node.y = y;
		
		int x1 = x - stepx;
		int y1 = y;
		int x2 = x;
		int y2 = y + stepy;
		int x3 = x + stepx;
		int y3 = y;
		int x4 = x;
		int y4 = y - stepy;

		TryAddEdge(node, x1, y1, 10);
		TryAddEdge(node, x2, y2, 10);
		TryAddEdge(node, x3, y3, 10);
		TryAddEdge(node, x4, y4, 10);
		TryAddEdge(node, x1, y4, 14);
		TryAddEdge(node, x1, y2, 14);
		TryAddEdge(node, x3, y2, 14);
		TryAddEdge(node, x3, y4, 14);

		DoFlood(graph, x1, y1);
		DoFlood(graph, x2, y2);
		DoFlood(graph, x3, y3);
		DoFlood(graph, x4, y4);
	}

	private bool IsInBlock(int posx, int posy)
	{
		int x = posx / gridWidth;
		int y = posy / gridHeight;
		if (x < 0 || x >= width || y < 0 || y >= height)
			return true;
		return nodeMarkList[x,y] == 1;
	}

	private void TryAddEdge(GraphAStarNode node, int x, int y, int cost)
	{
		GraphAStarNode neighbour = graph.GetNodeAt(x, y);
		if (neighbour != null)
		{
			graph.AddEdge(node.id, neighbour.id, cost);
			graph.AddEdge(neighbour.id, node.id, cost);
		}
	}

	#endregion

	#region draw GUI

	void CalcNavCount()
	{
		nodeCount = graph.GetNodeCount();
		edgeCount = 0;
		var list = graph.GetNodeList();
		for (int i = 0; i < list.Count; ++i)
		{
			edgeCount += graph.GetEdgeList(list[i].id).Count;
		}
	}

	void DrawEditorMode()
	{
		for (int x = 0; x < width; ++x)
		{
			for (int y = 0; y < height; ++y)
			{
				if (GUI.Button(new Rect(offsetX + x * gw + 0.05f * gw, offsetY + y * gh + 0.05f * gh, 0.9f * gw, 0.9f * gh), nodeMarkList[x, y] == 0 ? "" : "1"))
				{
					nodeMarkList[x, y] = nodeMarkList[x, y] == 0 ? 1 : 0;
				}
			}
		}
	}

	void DrawBlock()
	{
		for (int x = 0; x < width; ++x)
		{
			for (int y = 0; y < height; ++y)
			{
				if (nodeMarkList[x, y] == 1)
					GUI.Button(new Rect(offsetX + x * gw + 0.05f * gw, offsetY + y * gh + 0.05f * gh, 0.9f * gw, 0.9f * gh), "");
			}
		}
	}

	void DrawGraph()
	{
		Color lineColor = new Color(94/255.0f, 103/255.0f, 169/255.0f, 1f);

		var list = graph.GetNodeList();
		for (int i = 0; i < list.Count; ++i)
		{
			GraphAStarNode node = list[i] as GraphAStarNode;

			List<GraphEdge> edges = graph.GetEdgeList(node.id);
			for (int e = 0; e < edges.Count; ++e)
			{
				GraphEdge edge = edges[e];
				GraphAStarNode toNode = graph.GetNodeByID(edge.to) as GraphAStarNode;

				GUIHelper.DrawLine(new Vector2(offsetX + node.x, offsetY + node.y), new Vector2(offsetX + toNode.x, offsetY + toNode.y), lineColor);

			}
			//Graphics.DrawTexture(new Rect(offsetX + node.x, offsetY + node.y - 2, 4, 4), dotBlueTex, new Rect(0.0f, 0.0f, 1f, 1f), 0, 0, 0, 0, null);
		}

	}

	#endregion
}