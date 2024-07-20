using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    private readonly Vector2 defaultNodeSize = new Vector2(x: 150, y: 200); 
    public DialogueGraphView() 
    {
        styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path:"DialogueGraph"));
        SetupZoom(ContentZoomer.DefaultMinScale,ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(index :0,grid);
        grid.StretchToParentSize();

        AddElement(GenerateEntryPointNode());
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatibleports =new List<Port>();
        ports.ForEach(funcCall: (port) => 
        { 
            if (startPort != port && startPort.node != port.node)
                    compatibleports.Add(port);
        });
        return compatibleports;
    }
   
    private Port GeneratePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity,typeof(float));//Arbitrary type
    }

    private DialogueNode GenerateEntryPointNode()
    {
        var node = new DialogueNode
        {
            title ="START",
            GUID = Guid.NewGuid().ToString(),
            DialogueText="ENTRYPOÝNT",
            EntryPoint = true
        };
        var generatePort = GeneratePort(node, Direction.Output);
        generatePort.portName = "Next";
        node.outputContainer.Add(generatePort);

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(x:100,y:200,width:100 ,height:150));
        return node;
    }
    public void CreateNode(string nodeName)
    {
        AddElement(CreateDialogueNode(nodeName));
    }
    public DialogueNode CreateDialogueNode(string nodeName)
    {
        var dialogueNode = new DialogueNode
        {
            title = nodeName,
            DialogueText = nodeName,
            GUID = Guid.NewGuid().ToString()
        };

        var inputport = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputport.portName = "Input";
        dialogueNode.inputContainer.Add(inputport);

        var button = new Button(clickEvent: () => { AddChoicePort(dialogueNode); });
        button.text = "New Choice";
        dialogueNode.titleContainer.Add(button);



        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(position: Vector2.zero, defaultNodeSize));

        return dialogueNode;
    }

    private void AddChoicePort(DialogueNode dialogueNode)
    {
        var generatedPort = GeneratePort(dialogueNode, Direction.Output);

        var outputCount = dialogueNode.outputContainer.Query(name: "connector").ToList().Count;
        generatedPort.portName = $"Choice {outputCount}";

        dialogueNode.outputContainer.Add(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();

    }
}
