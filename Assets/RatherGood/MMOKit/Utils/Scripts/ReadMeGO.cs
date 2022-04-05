using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ratherGood
{
    /// <summary>
    /// Show some info in editor
    /// </summary>
    public class ReadMeGO : MonoBehaviour
    {

        [TextArea(10,20)]
        public string text;


    }
}