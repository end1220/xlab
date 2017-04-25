
using UnityEngine;

using System.Collections;



public class GUIHelper
{
	protected static bool clippingEnabled;
	protected static Rect clippingBounds;
	protected static Material lineMaterial;

	/* @ Credit: "http://cs-people.bu.edu/jalon/cs480/Oct11Lab/clip.c" */
	protected static bool clip_test(float p, float q, ref float u1, ref float u2)
	{
		float r;
		bool retval = true;
		if (p < 0.0)
		{
			r = q / p;
			if (r > u2)
				retval = false;
			else if (r > u1)
				u1 = r;
		}
		else if (p > 0.0)
		{
			r = q / p;
			if (r < u1)
				retval = false;
			else if (r < u2)
				u2 = r;
		}
		else
			if (q < 0.0)
				retval = false;

		return retval;
	}

	protected static bool segment_rect_intersection(Rect bounds, ref Vector2 p1, ref Vector2 p2)
	{
		float u1 = 0.0f, u2 = 1.0f, dx = p2.x - p1.x, dy;
		if (clip_test(-dx, p1.x - bounds.xMin, ref u1, ref u2))
			if (clip_test(dx, bounds.xMax - p1.x, ref u1, ref u2))
			{
				dy = p2.y - p1.y;
				if (clip_test(-dy, p1.y - bounds.yMin, ref u1, ref u2))
					if (clip_test(dy, bounds.yMax - p1.y, ref u1, ref u2))
					{
						if (u2 < 1.0)
						{
							p2.x = p1.x + u2 * dx;
							p2.y = p1.y + u2 * dy;
						}
						if (u1 > 0.0)
						{
							p1.x += u1 * dx;
							p1.y += u1 * dy;
						}
						return true;
					}
			}
		return false;
	}

	private static void EnsureMaterial()
	{
		if (!lineMaterial)
		{
			lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
		   "SubShader { Pass {" +
		   "   BindChannels { Bind \"Color\",color }" +
		   "   Blend SrcAlpha OneMinusSrcAlpha" +
		   "   ZWrite Off Cull Off Fog { Mode Off }" +
		   "} } }");
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	public static void BeginGroup(Rect position)
	{
		clippingEnabled = true;
		clippingBounds = new Rect(0, 0, position.width, position.height);
		GUI.BeginGroup(position);
	}

	public static void EndGroup()
	{
		GUI.EndGroup();
		clippingBounds = new Rect(0, 0, Screen.width, Screen.height);
		clippingEnabled = false;
	}

	public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color)
	{
		if (clippingEnabled)
			if (!segment_rect_intersection(clippingBounds, ref pointA, ref pointB))
				return;

		EnsureMaterial();
		lineMaterial.color = color;
		lineMaterial.SetPass(0);
		GL.Begin(GL.LINES);
		GL.Color(color);
		GL.Vertex3(pointA.x, pointA.y, 0);
		GL.Vertex3(pointB.x, pointB.y, 0);
		GL.End();
	}


	/*
	// The texture used by DrawLine(Color)
	private static Texture2D _coloredLineTexture;

	// The color used by DrawLine(Color)
	private static Color _coloredLineColor;

	/// <summary>
	/// Draw a line between two points with the specified color and a thickness of 1
	/// </summary>
	/// <param name="lineStart">The start of the line</param>
	/// <param name="lineEnd">The end of the line</param>
	/// <param name="color">The color of the line</param>
	public static void DrawLine(Vector2 lineStart, Vector2 lineEnd, Color color)
	{
		DrawLine(lineStart, lineEnd, color, 1);
	}

	/// <summary>
	/// Draw a line between two points with the specified color and thickness
	/// Inspired by code posted by Sylvan
	/// http://forum.unity3d.com/threads/17066-How-to-draw-a-GUI-2D-quot-line-quot?p=407005&viewfull=1#post407005
	/// </summary>
	/// <param name="lineStart">The start of the line</param>
	/// <param name="lineEnd">The end of the line</param>
	/// <param name="color">The color of the line</param>
	/// <param name="thickness">The thickness of the line</param>
	public static void DrawLine(Vector2 lineStart, Vector2 lineEnd, Color color, int thickness)
	{
		if (_coloredLineTexture == null || _coloredLineColor != color)
		{
			_coloredLineColor = color;
			_coloredLineTexture = new Texture2D(1, 1);
			_coloredLineTexture.SetPixel(0, 0, _coloredLineColor);
			_coloredLineTexture.wrapMode = TextureWrapMode.Repeat;
			_coloredLineTexture.Apply();
		}
		DrawLineStretched(lineStart, lineEnd, _coloredLineTexture, thickness);
	}

	/// <summary>
	/// Draw a line between two points with the specified texture and thickness.
	/// The texture will be stretched to fill the drawing rectangle.
	/// Inspired by code posted by Sylvan
	/// http://forum.unity3d.com/threads/17066-How-to-draw-a-GUI-2D-quot-line-quot?p=407005&viewfull=1#post407005
	/// </summary>
	/// <param name="lineStart">The start of the line</param>
	/// <param name="lineEnd">The end of the line</param>
	/// <param name="texture">The texture of the line</param>
	/// <param name="thickness">The thickness of the line</param>
	public static void DrawLineStretched(Vector2 lineStart, Vector2 lineEnd, Texture2D texture, int thickness)
	{
		Vector2 lineVector = lineEnd - lineStart;
		float angle = Mathf.Rad2Deg * Mathf.Atan(lineVector.y / lineVector.x);
		if (lineVector.x < 0)
		{
			angle += 180;
		}

		if (thickness < 1)
		{
			thickness = 1;
		}

		// The center of the line will always be at the center
		// regardless of the thickness.
		int thicknessOffset = (int)Mathf.Ceil(thickness / 2);

		GUIUtility.RotateAroundPivot(angle,
									 lineStart);
		GUI.DrawTexture(new Rect(lineStart.x,
								 lineStart.y - thicknessOffset,
								 lineVector.magnitude,
								 thickness),
						texture);
		GUIUtility.RotateAroundPivot(-angle, lineStart);
	}

	/// <summary>
	/// Draw a line between two points with the specified texture and a thickness of 1
	/// The texture will be repeated to fill the drawing rectangle.
	/// </summary>
	/// <param name="lineStart">The start of the line</param>
	/// <param name="lineEnd">The end of the line</param>
	/// <param name="texture">The texture of the line</param>
	public static void DrawLine(Vector2 lineStart, Vector2 lineEnd, Texture2D texture)
	{
		DrawLine(lineStart, lineEnd, texture, 1);
	}

	/// <summary>
	/// Draw a line between two points with the specified texture and thickness.
	/// The texture will be repeated to fill the drawing rectangle.
	/// Inspired by code posted by Sylvan and ArenMook
	/// http://forum.unity3d.com/threads/17066-How-to-draw-a-GUI-2D-quot-line-quot?p=407005&viewfull=1#post407005
	/// http://forum.unity3d.com/threads/28247-Tile-texture-on-a-GUI?p=416986&viewfull=1#post416986
	/// </summary>
	/// <param name="lineStart">The start of the line</param>
	/// <param name="lineEnd">The end of the line</param>
	/// <param name="texture">The texture of the line</param>
	/// <param name="thickness">The thickness of the line</param>
	public static void DrawLine(Vector2 lineStart, Vector2 lineEnd, Texture2D texture, int thickness)
	{
		Vector2 lineVector = lineEnd - lineStart;
		float angle = Mathf.Rad2Deg * Mathf.Atan(lineVector.y / lineVector.x);
		if (lineVector.x < 0)
			angle += 180;

		if (thickness < 1)
			thickness = 1;

		// The center of the line will always be at the center
		// regardless of the thickness.
		int thicknessOffset = (int)Mathf.Ceil(thickness / 2);

		Rect drawingRect = new Rect(lineStart.x,
									lineStart.y - thicknessOffset,
									Vector2.Distance(lineStart, lineEnd),
									(float)thickness);
		GUIUtility.RotateAroundPivot(angle, lineStart);
		//GUI.BeginGroup(drawingRect);
		{
			int drawingRectWidth = Mathf.RoundToInt(drawingRect.width);
			int drawingRectHeight = Mathf.RoundToInt(drawingRect.height);

			for (int y = 0; y < drawingRectHeight; y += texture.height)
			{
				for (int x = 0; x < drawingRectWidth; x += texture.width)
				{
					GUI.DrawTexture(new Rect(x,y,texture.width,texture.height),texture);
				}
			}
		}
		//GUI.EndGroup();
		GUIUtility.RotateAroundPivot(-angle, lineStart);
	}*/

};

