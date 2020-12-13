﻿using System.Collections.Generic;
using UnityEngine;

namespace UniWasm
{
    public class ContentsStore
    {
        private int idCounter = 1;

        public readonly Dictionary<string, GameObject> Objects = new Dictionary<string, GameObject>();

        public readonly Dictionary<string, GameObject> ResourceObjects = new Dictionary<string, GameObject>();

        public Transform RootTransform;

        public int RegisterObject(GameObject gameObject)
        {
            var id = idCounter;
            idCounter += 1;

            Objects.Add(id.ToString(), gameObject);
            return id;
        }
    }
}
