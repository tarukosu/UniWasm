using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace UniWasm
{
    public class SemlLoader : MonoBehaviour
    {
        [SerializeField]
        private string path = null;

        private Transform resourceRoot;

        private ContentsStore contentsStore;

        private void Awake()
        {
            contentsStore = new ContentsStore()
            {
                RootTransform = transform
            };

            var resourceObject = new GameObject("resource");
            resourceObject.SetActive(false);
            resourceRoot = resourceObject.transform;
            resourceRoot.SetParent(transform, false);
            LoadFromFile(path);
        }

        public void LoadFromFile(string path)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(path);

            var scene = xmlDocument.SelectSingleNode("//scene");
            LoadScene(scene);

            var resource = xmlDocument.SelectSingleNode("//resource");
            LoadResource(resource);
        }

        private void LoadResource(XmlNode resource)
        {
            Debug.Log("LoadResource");
            foreach (XmlNode node in resource.ChildNodes)
            {
                var t = InstantiateNode(path, node, resourceRoot);

                var id = ReadAttribute(node, "id", null);
                if (!string.IsNullOrEmpty(id))
                {
                    Debug.Log(id);
                    contentsStore.ResourceObjects.Add(id, t.gameObject);
                }
            }
        }

        private void LoadScene(XmlNode scene)
        {
            foreach (XmlNode node in scene.ChildNodes)
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

            t.localPosition = ReadVector3(node, "position", 0);
            t.localScale = ReadVector3(node, "scale", 1);

            // child elements
            foreach (XmlNode child in node.ChildNodes)
            {
                InstantiateNode(path, child, t);
            }

            return t;
        }

        private string ReadAttribute(XmlNode node, string key, string defaultValue = "")
        {
            var value = node.Attributes[key];
            if (value == null)
            {
                return defaultValue;
            }
            return value.Value;
        }

        private float ReadAttribute(XmlNode node, string key, float defaultValue = 0)
        {
            var stringValue = ReadAttribute(node, key, "");
            if (float.TryParse(stringValue, out var value))
            {
                return value;
            }
            return defaultValue;
        }

        private Vector3 ReadVector3(XmlNode node, string key, float defaultValue = 0)
        {
            var x = ReadAttribute(node, $"{key}.x", defaultValue);
            var y = ReadAttribute(node, $"{key}.y", defaultValue);
            var z = ReadAttribute(node, $"{key}.z", defaultValue);
            return new Vector3(x, y, z);
        }

        protected virtual WasmBehaviour AttatchScript(string path, XmlNode node, Transform parent)
        {
            var src = node.Attributes["src"];
            if (src == null)
            {
                return null;
            }

            Debug.Log(src.Value);
            var srcPath = Path.Combine(path, "..", src.Value);
            Debug.Log(srcPath);

            var wasm = parent.gameObject.AddComponent<WasmFromUrl>();
            if (srcPath.StartsWith("http"))
            {
                _ = wasm.LoadWasmFromUrl(srcPath, contentsStore);
            }
            else
            {
                wasm.LoadWasm(srcPath, contentsStore);
            }
            return wasm;
        }

        private Transform InstantiateElement(XmlNode node, Transform parent)
        {
            var go = new GameObject();
            return go.transform;
        }

        protected virtual Transform InstantiatePrimitive(XmlNode node, Transform parent)
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
                case "sphere":
                    primitiveType = PrimitiveType.Sphere;
                    break;
                case "cylinder":
                    primitiveType = PrimitiveType.Cylinder;
                    break;
            }

            var go = GameObject.CreatePrimitive(primitiveType);
            return go.transform;
        }
    }
}
