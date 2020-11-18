using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniWasm
{
    public class SemlLoader : MonoBehaviour
    {
        [SerializeField]
        private string path = null;

        private void Awake()
        {
            LoadFromFile(path);
        }

        public void LoadFromFile(string path)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(path);

            var body = xmlDocument.SelectSingleNode("//body");

            foreach (XmlNode node in body.ChildNodes)
            {
                /*
                Debug.Log(node.ToString());
                Debug.Log(node.Name);
                Debug.Log(node.NodeType);
                */

                InstantiateNode(path, node, transform);

            }
        }

        protected Transform InstantiateNode(string path, XmlNode node, Transform parent)
        {
            var tag = node.Name.ToLower();
            Transform t = null;
            switch (tag)
            {
                case "primitive":
                    t = InstantiatePrimitive(node, parent);
                    break;
                case "element":
                    t = InstantiateElement(node, parent);
                    break;
                case "script":
                    AttatchScript(path, node, parent);
                    break;
            }

            if (t == null)
            {
                return null;
            }

            t.SetParent(parent, false);

            var positionX = node.Attributes["position.x"];
            if (positionX != null)
            {
                if (float.TryParse(positionX.Value, out var x))
                {
                    t.localPosition = new Vector3(x, 0, 0);
                } 
            }


            // child elements
            foreach (XmlNode child in node.ChildNodes)
            {
                InstantiateNode(path, child, t);
            }

            return t;
        }

        private void AttatchScript(string path, XmlNode node, Transform parent)
        {
            var src = node.Attributes["src"];
            if(src == null)
            {
                return;
            }

            Debug.Log(src.Value);
            var srcPath = Path.Combine(path, "..", src.Value);
            Debug.Log(srcPath);

            var wasm = parent.gameObject.AddComponent<WasmFromUrl>();
            if (srcPath.StartsWith("http"))
            {
                _ = wasm.LoadWasmFromUrl(srcPath);
            }
            else
            {
                wasm.LoadWasm(srcPath);
            }
        }

        private Transform InstantiateElement(XmlNode node, Transform parent)
        {
            var go = new GameObject();
            return go.transform;
        }

        private Transform InstantiatePrimitive(XmlNode node, Transform parent)
        {
            var type = node.Attributes["type"];
            if (type == null)
            {
                return null;
            }

            PrimitiveType primitiveType = PrimitiveType.Cube;
            switch (type.Value.ToLower())
            {
                case "cube":
                    primitiveType = PrimitiveType.Cube;
                    break;
            }

            var go = GameObject.CreatePrimitive(primitiveType);
            // go.transform.SetParent(parent);
            return go.transform;
        }
    }
}
