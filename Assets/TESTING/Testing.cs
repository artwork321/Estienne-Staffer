using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TESTING {
    public class Testing : MonoBehaviour
    {
        void Start()
        {   
            GameManager.instance.StartCase();
        }
    }
}
