using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public GameObject uiCanvas;
        public List<GameObject> environment;

        private void Awake()
        {
            instance = this;
        }

        public bool CheckForEnironmentCollision(Vector3 position)
        {
            foreach (var piece in environment)
            {
                
                piece.TryGetComponent(out Collider envCol);

                if (!envCol) return false;
                
                if (envCol.bounds.Contains(position))
                    return true;
            }

            return false;
        }



        public void CanvasActive(bool value)
        {
            uiCanvas.SetActive(value);
        }
    }
}