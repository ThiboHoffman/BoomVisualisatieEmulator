using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
public class CreateTree : MonoBehaviour
{
    public GameObject prefab;

    public Vector3 pos = new Vector3(0F, 10F, 0F);
    public int _depth;

    public Material OrangeMat;
    public Material RedMat;
    public Material GreenMat;

    static public TreeNode<int> StartNode = null;

    // Start is called before the first frame update
    void Start()
    {

        StartNode = new TreeNode<int>(1);
        var depth1 = StartNode.AddChildren(new int[] { 2, 6 });
        var depth2 = StartNode[0].AddChildren(new int[] { 3, 4, 5 });
        var depth3 = StartNode[1].AddChildren(new int[] { 7, 11 });
        var depth4 = StartNode[1][0].AddChildren(new int[] { 8, 9 });
        var depth5 = StartNode[1][0][0].AddChildren(new int[] { 10 });

        _depth = 4;
        Visualize2DLayout(StartNode);

        StartCoroutine(VisitInPostOrder());
    }

    public void Visualize2DLayout<T>(TreeNode<T> startNode) {
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
        drawValue(obj, 1, startNode.Value.ToString());
        obj.name = "Node" + 1.ToString();
        startNode.Name = "Node" + 1.ToString();
        startNode.Node = new Node(prefab, obj);
        Debug.Log("Drawn node " + startNode.Value + " at " + obj.transform.position);
        VisualizeChildren2D<T>(startNode, new List<TreeNode<T>>());
    }

    public int CurrentDepth = 0;

    public void VisualizeChildren2D<T>(TreeNode<T> CurrentNode, List<TreeNode<T>> waiting) {
        int amountOfChildren = CurrentNode.Children.Count;
        int currentChild = 0;
        foreach (TreeNode<T> node in CurrentNode.Children) 
        {
            if (amountOfChildren % 2 == 0) {
                var calc = (float)currentChild - 0.5F*(float)amountOfChildren + 0.5F;
                var temp = CurrentNode.Node.Obj.transform.position + new Vector3((float)calc*(float)(_depth - CalculateDepthOfNode(node)), -1F , 0);
                GameObject obj = Instantiate(prefab, temp, Quaternion.identity);
                drawValue(obj, 1, node.Value.ToString());
                obj.name = "Node" + amountOfChildren.ToString();
                node.Name = "Node" + amountOfChildren.ToString();
                Debug.Log("Drawn node " + node.Value + " at " + obj.transform.position + " with depth " + CalculateDepthOfNode(node));
                node.Node = new Node(prefab, obj);

                var lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
                lineRenderer.startColor = Color.black;
                lineRenderer.endColor = Color.black;
                lineRenderer.startWidth = 0.01f;
                lineRenderer.endWidth = 0.01f;
                lineRenderer.positionCount = 2;
                lineRenderer.useWorldSpace = true;    
                                
                lineRenderer.SetPosition(0, node.Parent.Node.Obj.transform.position);
                lineRenderer.SetPosition(1, temp);
                
            } else {
                var calc2 = ((float)currentChild - (float)amountOfChildren) + (float)Math.Ceiling((float)amountOfChildren/2F);
                var temp2 = CurrentNode.Node.Obj.transform.position+ new Vector3((float)calc2, -1F , 0);
                GameObject obj = Instantiate(prefab, temp2, Quaternion.identity);
                drawValue(obj, 1, node.Value.ToString());
                obj.name = "Node" + amountOfChildren.ToString();
                node.Name = "Node" + amountOfChildren.ToString();
                Debug.Log("Drawn node " + node.Value + " at " + obj.transform.position + " with depth " + CalculateDepthOfNode(node));
                node.Node = new Node(prefab, obj);

                var lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
                lineRenderer.startColor = Color.black;
                lineRenderer.endColor = Color.black;
                lineRenderer.startWidth = 0.01f;
                lineRenderer.endWidth = 0.01f;
                lineRenderer.positionCount = 2;
                lineRenderer.useWorldSpace = true;    
                                
                lineRenderer.SetPosition(0, node.Parent.Node.Obj.transform.position); //x,y and z position of the starting point of the line
                lineRenderer.SetPosition(1, temp2); //x,y and z position of the end point of the line
                
            }
            currentChild++;
        }
        List<TreeNode<T>> range = CurrentNode.Children.ToList();
        foreach(TreeNode<T> node in range) 
        {  
            waiting.Add(node);
        }
        waiting.Remove(CurrentNode);
        if (waiting.Count > 0) {
            VisualizeChildren2D(waiting[0], waiting);
        }
    }

    public int CalculateDepthOfNode<T>(TreeNode<T> Node) 
    {
        int Depth = 0;
        TreeNode<T> Current = Node; 
        while (Current.Parent != null)
        {
            Current = Current.Parent;
            Depth++;
        }
        return Depth;
    }

    static public TreeNode<int> SearchNodeWithName(String name) 
    {
        List<TreeNode<int>> waiting = StartNode.Children;

        while (waiting.Count != 0)
        {
            TreeNode<int> current = waiting[0];
            waiting.RemoveAt(0);
            Debug.Log(current.Name + "  ---------   " + name);
            if (current.Name.Contains(name)) return current;
            List<TreeNode<int>> children = current.Children;
            foreach (TreeNode<int> node in children) waiting.Add(node);
        }

        return null;
    }

    static GameObject addSide (int size, string text) {

        GameObject childObj = new GameObject ();
        TextMesh textObj = childObj.AddComponent<TextMesh>();
        textObj.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");;
        textObj.text = text;
        textObj.fontSize = (int) 13;
        textObj.color = Color.white;

        return childObj;
    }

    public void drawValue (GameObject mainObj, int size, string value) {

        GameObject side1 = addSide (size, value.ToString());
        side1.transform.SetParent (mainObj.transform);
        side1.transform.localScale = new Vector3(0.5F, 0.5F, 0.5F);
        side1.transform.localPosition = new Vector3(-0.35F, 0.35F, -0.5F);
        side1.transform.rotation = Quaternion.Euler(0,0,0);

        GameObject side2 = addSide (size, value.ToString());
        side2.transform.SetParent (mainObj.transform);
        side2.transform.localScale = new Vector3(0.5F, 0.5F, 0.5F);
        side2.transform.localPosition = new Vector3(0.35F, 0.35F, 0.5F);
        side2.transform.rotation = Quaternion.Euler(0,180,0);

        GameObject side5 = addSide (size, value.ToString());
        side5.transform.SetParent (mainObj.transform);
        side5.transform.localScale = new Vector3(0.5F, 0.5F, 0.5F);
        side5.transform.localPosition = new Vector3(-0.5F, 0.35F, 0.35F);
        side5.transform.rotation = Quaternion.Euler(0,90,0);

        GameObject side6 = addSide (size, value.ToString());
        side6.transform.SetParent (mainObj.transform);
        side6.transform.localScale = new Vector3(0.5F, 0.5F, 0.5F);
        side6.transform.localPosition = new Vector3(0.5F, 0.35F, -0.35F);
        side6.transform.rotation = Quaternion.Euler(0,270,0);
    }

    IEnumerator VisitInPreOrder() 
    {
        yield return new WaitForSeconds(3);

        GameObject info = GameObject.Find("TextPanel");
        info.GetComponent<UnityEngine.UI.Text>().text = "In preorde doorlopen: \n";

        yield return StartCoroutine(VisitInPreOrderRecursief(StartNode));
    }
    
    IEnumerator VisitInPreOrderRecursief<T>(TreeNode<T> CurrentNode) 
    {
        yield return new WaitForSeconds(3);

        CurrentNode.Node.Obj.GetComponent<Renderer>().material = OrangeMat;

        yield return new WaitForSeconds(3);

        print(CurrentNode.Value);
        CurrentNode.Node.Obj.GetComponent<Renderer>().material = GreenMat;
        GameObject info = GameObject.Find("TextPanel");
        info.GetComponent<UnityEngine.UI.Text>().text += CurrentNode.Value.ToString() + ", ";

        foreach (TreeNode<T> node in CurrentNode.Children) 
        {
            yield return StartCoroutine(VisitInPreOrderRecursief(node));
        }
    }

    IEnumerator VisitInPostOrder() 
    {
        yield return new WaitForSeconds(3);

        GameObject info = GameObject.Find("TextPanel");
        info.GetComponent<UnityEngine.UI.Text>().text = "In postorder doorlopen: \n";

        yield return StartCoroutine(VisitInPostOrderRecursief(StartNode));
    }

    IEnumerator VisitInPostOrderRecursief<T>(TreeNode<T> CurrentNode)
    {
        yield return new WaitForSeconds(3);

        CurrentNode.Node.Obj.GetComponent<Renderer>().material = OrangeMat;

        yield return new WaitForSeconds(3);

        if (CurrentNode.Children.Count == 0) 
        {
            print(CurrentNode.Value);
            CurrentNode.Node.Obj.GetComponent<Renderer>().material = GreenMat;
            GameObject info = GameObject.Find("TextPanel");
            info.GetComponent<UnityEngine.UI.Text>().text += CurrentNode.Value.ToString() + ", ";
        }

        int AmountOfChildren = CurrentNode.Children.Count;
        int Index = 0;
        foreach (TreeNode<T> node in CurrentNode.Children) 
        {
            yield return StartCoroutine(VisitInPostOrderRecursief(node));
            Index++;
            if (Index == AmountOfChildren) 
            {
                yield return new WaitForSeconds(3);

                print(CurrentNode.Value);
                CurrentNode.Node.Obj.GetComponent<Renderer>().material = GreenMat;
                GameObject info = GameObject.Find("TextPanel");
                info.GetComponent<UnityEngine.UI.Text>().text += CurrentNode.Value.ToString() + ", ";
            }
        }
    } 

    IEnumerator VisitInInOrder() 
    {
        yield return new WaitForSeconds(3);

        GameObject info = GameObject.Find("TextPanel");
        info.GetComponent<UnityEngine.UI.Text>().text = "In inorder doorlopen: \n";

        yield return StartCoroutine(VisitInInOrderRecursief(StartNode));
    }

    IEnumerator VisitInInOrderRecursief<T>(TreeNode<T> CurrentNode)
    {
        yield return new WaitForSeconds(3);

        CurrentNode.Node.Obj.GetComponent<Renderer>().material = OrangeMat;

        yield return new WaitForSeconds(3);

        if (CurrentNode.Children.Count == 0) 
        {
            print(CurrentNode.Value);
            CurrentNode.Node.Obj.GetComponent<Renderer>().material = GreenMat;
            GameObject info = GameObject.Find("TextPanel");
            info.GetComponent<UnityEngine.UI.Text>().text += CurrentNode.Value.ToString() + ", ";
        }

        Boolean notPrinted = true;
        foreach (TreeNode<T> node in CurrentNode.Children) 
        {
            yield return StartCoroutine(VisitInInOrderRecursief(node));

            yield return new WaitForSeconds(3);

            if (notPrinted) 
            {
                print(CurrentNode.Value);
                CurrentNode.Node.Obj.GetComponent<Renderer>().material = GreenMat;
                GameObject info = GameObject.Find("TextPanel");
                info.GetComponent<UnityEngine.UI.Text>().text += CurrentNode.Value.ToString() + ", ";
                notPrinted = false;
            }
        }
    } 

    void OnMouseDown()
     {
         print (name);    
     }

    // Update is called once per frame
    void Update()
    {
    }
}
