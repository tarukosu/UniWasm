using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniWasm
{
    public class Resource
    {
        public string Id { set; get; }
        public GameObject GameObject { get; internal set; }
    }

    public class ContentsStore
    {
        private int idCounter = 1;

        public readonly Dictionary<string, GameObject> Objects = new Dictionary<string, GameObject>();

        public readonly List<Resource> Resources = new List<Resource>();

        public Transform RootTransform;

        public ContentsStore()
        {
            // Resource index 0 is empty
            var emptyResource = new Resource()
            {
                Id = "",
                GameObject = null
            };
            Resources.Add(emptyResource);
        }

        public int RegisterObject(GameObject gameObject)
        {
            var id = idCounter;
            idCounter += 1;

            Objects.Add(id.ToString(), gameObject);
            return id;
        }

        public void RegisterResource(Resource resource)
        {
            Resources.Add(resource);
        }

        public bool TryGetResourceIndexById(string id, out int resourceIndex)
        {
            resourceIndex = Resources.FindIndex(x => x.Id == id);
            if (resourceIndex == -1)
            {
                return false;
            }
            return true;
        }

        public bool TryGetResourceByResourceIndex(int resourceIndex, out Resource resource)
        {
            if (resourceIndex <= 0 || resourceIndex > Resources.Count - 1)
            {
                resource = null;
                return false;
            }
            resource = Resources[resourceIndex];
            return true;
        }
    }
}
