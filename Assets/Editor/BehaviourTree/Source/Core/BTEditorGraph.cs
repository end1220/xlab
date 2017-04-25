﻿using UnityEngine;
using System.Collections.Generic;
using Lite.BevTree;
using UnityEditor;
using System;

namespace Lite.BevTreeEditor
{
	public class BTEditorGraph : ScriptableObject
	{
		private const int SELECT_MOUSE_BUTTON = 0;
		private const int CONTEXT_MOUSE_BUTTON = 1;

		private BTAsset m_asset = null;
		private BehaviourTree m_bevTree = null;
		private List<BTEditorGraphNode> m_selection;
		private BTEditorGraphNode m_masterRoot;
		private Stack<BTEditorGraphNode> m_rootStack;
		private bool m_drawSelectionBox;
		private bool m_isBehaviourTreeReadOnly;
		private bool m_canBeginBoxSelection;
		private Vector2 m_selectionBoxStartPos;

		public bool ReadOnly
		{
			get
			{
				return m_isBehaviourTreeReadOnly || EditorApplication.isPlaying;
			}
		}

		public int Depth
		{
			get
			{
				return Mathf.Max(m_rootStack.Count - 1, 0);
			}
		}

		public Rect? SelectionBox { get; set; }

		private BTEditorGraphNode WorkingRoot
		{
			get
			{
				return m_rootStack.Peek();
			}
		}

		public BehaviourTree BTree { get { return m_bevTree; } }


		private void OnCreated()
		{
			m_masterRoot = null;
			m_rootStack = new Stack<BTEditorGraphNode>();
			m_drawSelectionBox = false;
			m_isBehaviourTreeReadOnly = false;
			m_selectionBoxStartPos = Vector2.zero;
			SelectionBox = null;
		}

		private void OnDestroy()
		{
			BTEditorGraphNode.DestroyImmediate(m_masterRoot);
			m_masterRoot = null;
			m_rootStack.Clear();
		}

		public void SetBehaviourTree(BTAsset asset, BehaviourTree behaviourTree)
		{
			this.m_asset = asset;
			this.m_bevTree = behaviourTree;

			if(m_masterRoot != null)
			{
				BTEditorGraphNode.DestroyImmediate(m_masterRoot);
				m_masterRoot = null;
				m_rootStack.Clear();
			}

			m_isBehaviourTreeReadOnly = behaviourTree.ReadOnly;
			m_masterRoot = BTEditorGraphNode.CreateRoot(this, behaviourTree.Root);
			m_rootStack.Push(m_masterRoot);
			BTUndoSystem.Clear();
		}

		public void DrawGUI(Rect screenRect)
		{
			DrawTreeInfo();

			if(WorkingRoot != null)
			{
				WorkingRoot.Update();
				WorkingRoot.Draw();
				DrawSelectionBox();
				HandleEvents(screenRect);
			}
		}

		private void DrawTreeInfo()
		{
			if (m_asset == null || string.IsNullOrEmpty(m_asset.description))
				return;
			Vector2 commentSize = BTEditorStyle.TreeCommentLabel.CalcSize(new GUIContent(m_asset.description));
			Rect CommentPos = new Rect(10, 32, commentSize.x, commentSize.y);
			EditorGUI.LabelField(CommentPos, string.Format("<color=green>{0}</color>", m_asset.description), BTEditorStyle.TreeCommentLabel);
			
		}

		private void DrawSelectionBox()
		{
			if(m_drawSelectionBox)
			{
				Vector2 mousePosition = BTEditorCanvas.Current.Event.mousePosition;
				Rect position = new Rect();
				position.x = Mathf.Min(m_selectionBoxStartPos.x, mousePosition.x);
				position.y = Mathf.Min(m_selectionBoxStartPos.y, mousePosition.y);
				position.width = Mathf.Abs(mousePosition.x - m_selectionBoxStartPos.x);
				position.height = Mathf.Abs(mousePosition.y - m_selectionBoxStartPos.y);

				GUI.Box(position, "", BTEditorStyle.SelectionBox);
				BTEditorCanvas.Current.Repaint();

				SelectionBox = new Rect(BTEditorCanvas.Current.WindowSpaceToCanvasSpace(position.position), position.size);
			}
			else
			{
				SelectionBox = null;
			}
		}

		private void HandleEvents(Rect screenRect)
		{
			if(BTEditorCanvas.Current.Event.type == EventType.MouseDown && BTEditorCanvas.Current.Event.button == SELECT_MOUSE_BUTTON)
			{
				if(screenRect.Contains(BTEditorCanvas.Current.Event.mousePosition))
				{
					ClearSelection();

					m_canBeginBoxSelection = true;
					m_selectionBoxStartPos = BTEditorCanvas.Current.Event.mousePosition;
					BTEditorCanvas.Current.Event.Use();
				}
			}
			else if(BTEditorCanvas.Current.Event.type == EventType.MouseDrag && BTEditorCanvas.Current.Event.button == SELECT_MOUSE_BUTTON)
			{
				if(screenRect.Contains(BTEditorCanvas.Current.Event.mousePosition))	
				{
					if(!m_drawSelectionBox && m_canBeginBoxSelection)
					{
						m_drawSelectionBox = true;
					}

					BTEditorCanvas.Current.Event.Use();
				}
			}
			else if(BTEditorCanvas.Current.Event.type == EventType.MouseUp)
			{
				if(screenRect.Contains(BTEditorCanvas.Current.Event.mousePosition))
				{
					if(BTEditorCanvas.Current.Event.button == SELECT_MOUSE_BUTTON)
					{
						if(m_drawSelectionBox)
						{
							m_drawSelectionBox = false;
						}

						BTEditorCanvas.Current.Event.Use();
					}
					else if(BTEditorCanvas.Current.Event.button == CONTEXT_MOUSE_BUTTON)
					{
						GenericMenu menu = BTContextMenuFactory.CreateGraphContextMenu(this);
						menu.DropDown(new Rect(BTEditorCanvas.Current.Event.mousePosition, Vector2.zero));

						BTEditorCanvas.Current.Event.Use();
					}
				}

				m_canBeginBoxSelection = false;
			}
		}

		public void OnPushNodeGroup(BTEditorGraphNode node)
		{
			if(node != null && node.Node is NodeGroup)
			{
				BTUndoSystem.RegisterUndo(new UndoNodeGroupPush(node));
				m_rootStack.Push(node);

				SelectSingle(node);
			}
		}

		public void OnPopNodeGroup()
		{
			if(m_rootStack.Count > 1)
			{
				var oldWorkingRoot = m_rootStack.Pop();

				SelectEntireNodeGroup(oldWorkingRoot);
				BTUndoSystem.RegisterUndo(new UndoNodeGroupPop(oldWorkingRoot));
			}
		}

		public void OnNodeSelect(BTEditorGraphNode node)
		{
			if(BTEditorCanvas.Current.Event.shift && (node.Node is Composite || node.Node is Decorator))
			{
				SelectBranch(node);
			}
			else if(BTEditorCanvas.Current.Event.control || SelectionBox.HasValue)
			{
				if(!m_selection.Contains(node))
				{
					if(node.Node is NodeGroup && !IsRoot(node))
						SelectEntireNodeGroupAdditive(node);
					else
						SelectSingleAdditive(node);
				}
			}
			else
			{
				if(node.Node is NodeGroup && !IsRoot(node))
					SelectEntireNodeGroup(node);
				else
					SelectSingle(node);
			}
		}

		public void OnNodeDeselect(BTEditorGraphNode node)
		{
			if(m_selection.Remove(node))
			{
				node.OnDeselected();
			}
		}

		public void OnNodeBeginDrag(BTEditorGraphNode node, Vector2 position)
		{
			if(m_selection.Contains(node))
			{
				BTUndoSystem.BeginUndoGroup("Moved node(s)");
				for(int i = 0; i < m_selection.Count; i++)
				{
					BTUndoSystem.RegisterUndo(new UndoNodeMoved(m_selection[i]));
					m_selection[i].OnBeginDrag(position);
				}
			}
		}

		public void OnNodeDrag(BTEditorGraphNode node, Vector2 position)
		{
			if(m_selection.Contains(node))
			{
				for(int i = 0; i < m_selection.Count; i++)
				{
					m_selection[i].OnDrag(position);
				}
			}
		}

		public void OnNodeEndDrag(BTEditorGraphNode node)
		{
			if(m_selection.Contains(node))
			{
				for(int i = 0; i < m_selection.Count; i++)
				{
					m_selection[i].OnEndDrag();
				}

				BTUndoSystem.EndUndoGroup();
			}
		}

		public void OnNodeCreateChild(BTEditorGraphNode parent, Type childType)
		{
			if(parent != null && childType != null)
			{
				BTEditorGraphNode child = parent.OnCreateChild(childType);
				if(child != null)
				{
					BTUndoSystem.RegisterUndo(new UndoNodeCreated(child));
				}
			}
		}

		public void OnNodeSwitchType(BTEditorGraphNode target, Type newType)
		{
			if(target == null || newType == null)
				return;

			BTEditorGraphNode parentNode = target.Parent;
			Vector2 oldPosition = target.NodePosition;
			int oldIndex = target.Parent.GetChildIndex(target);

			BehaviourNode node = BTUtils.CreateNode(newType);
			if(node != null)
			{
				if(node is Decorator)
				{
					Decorator original = target.Node as Decorator;
					Decorator decorator = node as Decorator;

					decorator.SetChild(original.GetChild());
				}
				else if(node is Composite)
				{
					Composite original = target.Node as Composite;
					Composite composite = node as Composite;

					for(int i = 0; i < original.ChildCount; i++)
						composite.AddChild(original.GetChild(i));
				}
				
				BTUndoSystem.BeginUndoGroup("Changed node type");
				BTUndoSystem.RegisterUndo(new UndoNodeDeleted(target));
				target.OnDelete();

				BTEditorGraphNode newNode = parentNode.OnInsertChild(oldIndex, node);
				if(newNode != null)
				{
					newNode.NodePosition = oldPosition;
					BTUndoSystem.RegisterUndo(new UndoNodeCreated(newNode));
				}

				BTUndoSystem.EndUndoGroup();
			}
		}

		public void OnNodeDelete(BTEditorGraphNode node)
		{
			if(node != null)
			{
				BTUndoSystem.RegisterUndo(new UndoNodeDeleted(node));
				node.OnDelete();
			}
		}

		public void OnNodeDeleteChildren(BTEditorGraphNode node)
		{
			if(node != null)
			{
				BTUndoSystem.BeginUndoGroup("Delete children");
				int childIndex = 0;
				while(node.ChildCount > 0)
				{
					BTUndoSystem.RegisterUndo(new UndoNodeDeleted(node.GetChild(0), childIndex));
					node.OnDeleteChild(0);
					childIndex++;
				}
				BTUndoSystem.EndUndoGroup();
			}
		}

		public void OnCopyNode(BTEditorGraphNode source)
		{
			if(CanCopy(source))
			{
				BTEditorCanvas.Current.Clipboard = BTUtils.SerializeNode(source.Node);
			}
		}

		public bool CanCopy(BTEditorGraphNode source)
		{
			return source != null && source.Node != null;
		}

		public void OnPasteNode(BTEditorGraphNode destination)
		{
			if(CanPaste(destination))
			{
				BehaviourNode node = BTUtils.DeserializeNode(BTEditorCanvas.Current.Clipboard);
				BTEditorGraphNode child = destination.OnCreateChild(node);
				if(child != null)
				{
					SelectBranch(child);

					var undoState = new UndoNodeCreated(child);
					undoState.Title = "Pasted " + child.Node.Title;

					BTUndoSystem.RegisterUndo(undoState);
				}
			}
		}

		public bool CanPaste(BTEditorGraphNode destination)
		{
			if(destination != null && destination.Node != null && !string.IsNullOrEmpty(BTEditorCanvas.Current.Clipboard))
			{
				if(destination.Node is NodeGroup)
				{
					return IsRoot(destination) && destination.ChildCount == 0;
				}
				else if(destination.Node is Decorator)
				{
					return destination.ChildCount == 0;
				}
				else if(destination.Node is Composite)
				{
					return true;
				}
			}

			return false;
		}

		public void IncreaseEditingDepth(BTEditorGraphNode node)
		{
			if(node != null && (node.Node is NodeGroup || node.Node is Root))
			{
				m_rootStack.Push(node);
			}
		}

		public void DecreaseEditingDepth()
		{
			if(m_rootStack.Count > 1)
			{
				m_rootStack.Pop();
			}
		}

		public bool IsRoot(BTEditorGraphNode node)
		{
			return node == WorkingRoot;
		}

		public void SelectEntireGraph()
		{
			ClearSelection();
			SelectBranchRecursive(WorkingRoot);
		}

		private void SelectEntireNodeGroup(BTEditorGraphNode node)
		{
			SelectBranch(node);
			node.OnSelected();
		}
		
		private void SelectEntireNodeGroupAdditive(BTEditorGraphNode node)
		{
			SelectBranchAdditive(node);
			node.OnSelected();
		}

		private void SelectBranch(BTEditorGraphNode root)
		{
			ClearSelection();
			SelectBranchRecursive(root);
		}

		private void SelectBranchAdditive(BTEditorGraphNode root)
		{
			SelectBranchRecursive(root);
		}

		private void SelectSingle(BTEditorGraphNode node)
		{
			ClearSelection();
			m_selection.Add(node);
			node.OnSelected();
		}

		private void SelectSingleAdditive(BTEditorGraphNode node)
		{
			m_selection.Add(node);
			node.OnSelected();
		}

		private void SelectBranchRecursive(BTEditorGraphNode node)
		{
			m_selection.Add(node);
			node.OnSelected();

			for(int i = 0; i < node.ChildCount; i++)
			{
				SelectBranchRecursive(node.GetChild(i));
			}
		}

		private void ClearSelection()
		{
			if(m_selection.Count > 0)
			{
				for(int i = 0; i < m_selection.Count; i++)
				{
					m_selection[i].OnDeselected();
				}

				m_selection.Clear();
			}
		}

		public void DeleteAllBreakpoints()
		{
			DeleteBreakpointsRecursive(m_masterRoot);
		}

		private void DeleteBreakpointsRecursive(BTEditorGraphNode node)
		{
			if(node != null && node.Node != null)
			{
				node.Node.Breakpoint = Breakpoint.None;
				for(int i = 0; i < node.ChildCount; i++)
				{
					DeleteBreakpointsRecursive(node.GetChild(i));
				}
			}
		}

		public string GetNodeHash(BTEditorGraphNode node)
		{
			List<byte> path = new List<byte>();
			for(BTEditorGraphNode n = node; n != null && n.Parent != null; n = n.Parent)
			{
				path.Add((byte)n.Parent.GetChildIndex(n));
			}
			path.Reverse();

			return Convert.ToBase64String(path.ToArray());
		}

		public BTEditorGraphNode GetNodeByHash(string path)
		{
			byte[] actualPath = Convert.FromBase64String(path);
			if(actualPath != null)
			{
				BTEditorGraphNode node = m_masterRoot;

				for(int i = 0; i < actualPath.Length; i++)
				{
					node = node.GetChild(actualPath[i]);
					if(node == null)
					{
						return null;
					}
				}

				return node;
			}

			return null;
		}

		public static BTEditorGraph Create()
		{
			BTEditorGraph graph = ScriptableObject.CreateInstance<BTEditorGraph>();
			graph.OnCreated();
			graph.hideFlags = HideFlags.HideAndDontSave;
			graph.m_selection = new List<BTEditorGraphNode>();

			return graph;
		}
	}
}