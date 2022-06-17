using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Indie.Pixel.Cameras
{
    public class TopDownCameraController : MonoBehaviour
    {

        #region Variables
        public Transform target;
        public float height = 10;
        public float distance = 20;
        public float angle = 45;
        #endregion



        #region Methods
        void Start()
        {
            HandleCamera();
        }


        void Update()
        {
            HandleCamera();
        }
        #endregion



        #region Helper Methods
        protected virtual void HandleCamera()
        {
            if(!target)
            {
                return;
            }


            //GET WORLD POSITION
            Vector3 worldPosition = (Vector3.forward * -distance) + (Vector3.up * height);
            Debug.DrawLine(target.position, worldPosition, Color.red);


            //ROTATE
            Vector3 rotatedVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;
            Debug.DrawLine(target.position, rotatedVector, Color.green);

            //MOVE OUR POSITION
            Vector3 flatTargetPosition = target.position;
            flatTargetPosition.y = 0f;

            //FINAL POSITION VECTOR
            Vector3 finalPosition = flatTargetPosition + rotatedVector;
            Debug.DrawLine(target.position, finalPosition, Color.blue);

            //APPLIED IN CAMERA
            transform.position = finalPosition;
            transform.LookAt(target.position);
        }
        #endregion
    }
}

