using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Drift
{
    public class VehicleRaycastSystem : MonoBehaviour
    {
        [SerializeField]
        private float _maxDist = 4f;
        [SerializeField]
        private int _roadLayer = 7;

        private void Update()
        {
            if(!Physics.Raycast(new Ray(transform.position, Vector3.down), out RaycastHit hit, _maxDist, _roadLayer))
            {
                Debug.Log("dfgdfg");
            }
        }
    }
}
