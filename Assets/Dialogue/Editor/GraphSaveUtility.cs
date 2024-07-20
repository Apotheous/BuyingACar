using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
//using Subtegral.DialogueSystem.DataContainers;
using UnityEngine.UIElements;

public class GraphSaveUtility 
{
    private DialogueGraphView _targetGraphView;
    private List<Edge> Edges => _targetGraphView.edges.ToList();//***.Cast<Edge>().ToList() benim eklediðim kýsým
    private List<DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();


    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }
    public void SaveGraph(string fileName)
    {
        if (!Edges.Any()) return; //if there are no edges(no connections) then return

        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
        for (int i = 0; i < connectedPorts.Length; i++)
        {
            var outputNode = connectedPorts[i].output.node as DialogueNode;
            var inputNode = connectedPorts[i].input.node as DialogueNode;

            dialogueContainer.NodeLinks.Add(item: new NodeLinkData
            {
                BaseNodeGuid = outputNode.GUID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.GUID
            });
        }

        foreach (var dialogueNode in Nodes.Where(node => !node.EntryPoint))
        {
            dialogueContainer.DialogueNodeData.Add(new DialogueNodeData
            {
                Guid = dialogueNode.GUID,
                DialogueText = dialogueNode.DialogueText,
                Position = dialogueNode.GetPosition().position
            });
        }

        if (!AssetDatabase.IsValidFolder(path: "Assets/Resources"))
        {
            AssetDatabase.CreateFolder(parentFolder: "Assets", newFolderName: "Resources");
        }

        AssetDatabase.CreateAsset(dialogueContainer, path: $"Assets/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    } 
    public void LoadGraph(string fileName)
    {

    }
}
