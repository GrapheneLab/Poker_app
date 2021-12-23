using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EosSharp;

public class SceneBehaivor : MonoBehaviour {

    public GameObject infoLabel;

  

    // Use this for initialization
    void Start () {


        var url = string.Format("{0}/v1/chain/get_info", "https://nodes.eos42.io" );

        //client.s

       // test();

        infoLabel.GetComponent<UILabel>().text = "finished!";
 
    }

    void test()
    {
        /*
        Eos CLEOS.eos = new Eos(new EosConfigurator()
        {
            HttpEndpoint = "https://nodes.eos42.io", //Mainnet
            ChainId = CLEOS.chain_id,
            ExpireSeconds = 60,
            SignProvider = new DefaultSignProvider("5JtmBuR5PurrzwsbWMZBRimNe5R6p7VK1LxVjFESHKge1ypjtK5")
        });
        */
       // CLEOS.connect();
        ////CLEOS.eos = CLEOS.CLEOS.eos;

       // var result = CLEOS.eos.GetInfo();

      //  result.Start();

       // Debug.Log(result);

      //  infoLabel.GetComponent<UILabel>().text = result.
    }

    // Update is called once per frame
    void Update () {
		
	}
}
