using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text.RegularExpressions;

public class AddNewNode : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnClick()
    {
      /*
        var obj = GameObject.Find("TextInputLabel");
        var scene = GameObject.Find("SceneBehaivorObject");

        if (scene.GetComponent<NodesList>().GetNodesCount() > 8)
            return;

        string node_ip = obj.GetComponent<UILabel>().text;

        Match m = Regex.Match(node_ip, @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{1,5}$", RegexOptions.IgnoreCase);
        if (m.Success)
        {
            string[] words = node_ip.Split(':');

            scene.GetComponent<NodesList>().AddRow(words[0], words[1]);
            scene.GetComponent<NodesList>().Redraw();

            
        }
        else
        {
            Debug.Log("FAIL!!!");
        }

        obj.GetComponent<UILabel>().text = "";
        */
    }
}
