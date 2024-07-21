using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    public readonly Vector2 defaultNodeSize = new Vector2(x: 150, y: 200);
    private NodeSearchWindow _searchWindow;
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
        AddSearchWindow();
    }

    private void AddSearchWindow()
    {
        _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        nodeCreationRequest = context => 
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition),_searchWindow); 
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

        node.capabilities &= ~Capabilities.Movable;
        node.capabilities &= ~Capabilities.Deletable;

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

        dialogueNode.styleSheets.Add(styleSheet: Resources.Load<StyleSheet>(path: "Node"));

        var button = new Button(clickEvent: () => { AddChoicePort(dialogueNode); });
        button.text = "New Choice";
        dialogueNode.titleContainer.Add(button);

        var textfield = new TextField(string.Empty);
        textfield.RegisterValueChangedCallback(evt =>
        {
            dialogueNode.DialogueText = evt.newValue;
            dialogueNode.title = evt.newValue;
        });
        textfield.SetValueWithoutNotify(dialogueNode.title);
        dialogueNode.mainContainer.Add(textfield);

        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(position: Vector2.zero, defaultNodeSize));

        return dialogueNode;
    }

    public void AddChoicePort(DialogueNode dialogueNode, string overriddenPortName = "")
    {
        var generatedPort = GeneratePort(dialogueNode, Direction.Output);

        var oldLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(oldLabel);

        var outputPortCount = dialogueNode.outputContainer.Query(name: "connector").ToList().Count;

        var choicePortName = string.IsNullOrEmpty(overriddenPortName)
            ? $"Choice {outputPortCount + 1}"
            : overriddenPortName;

        var textField = new TextField
        {
            name = string.Empty,
            value = choicePortName
        };
        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue); 
        generatedPort.contentContainer.Add(new Label(" "));
        generatedPort.contentContainer.Add(textField);
        var deleteButton = new Button(clickEvent: () => RemovePort(dialogueNode, generatedPort))
        {
            text = "X"
        };
        generatedPort.contentContainer.Add(deleteButton);  

        generatedPort.portName = choicePortName;    
        dialogueNode.outputContainer.Add(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();

    }

    private void RemovePort(DialogueNode dialogueNode, Port generatedPort)
    {
        var targetEdge = edges.ToList().Where(x =>
            x.output.portName == generatedPort.portName &&
            x.output.node == generatedPort.node);

        if (!targetEdge.Any()) return;
        var edge = targetEdge.First();
        edge.input.Disconnect(edge);
        RemoveElement(targetEdge.First());

        dialogueNode.outputContainer.Remove(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();

    }
}
