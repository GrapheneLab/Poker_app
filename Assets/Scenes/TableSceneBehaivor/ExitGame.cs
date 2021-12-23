using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour {

    public GameObject scene;
    public GameObject continue_button;

    public bool make_grayscale = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnClick()
    {     
        StartCoroutine(Example());

        continue_button.SetActive(false);
        //gameObject.SetActive(false);

        CLEOS.skip_mainloop = true;
    }

    double quantityToDouble(string q)
    {
        q = q.Replace(CLEOS.symbol, string.Empty);
        q = q.Replace(" ", string.Empty);
        q = q.Replace(".", ",");
        return Convert.ToDouble(q);
    }

    IEnumerator Example()
    {
        bool run = true;
        while (run == true)
        {
            if (make_grayscale == true)
                scene.GetComponent<TableSceneBehaivor>().make_grayscale(true);

            scene.GetComponent<TableSceneBehaivor>().reset_table();
            while (scene.GetComponent<TableSceneBehaivor>().current_account.table_id_.Count != 0)
            {
                scene.GetComponent<TableSceneBehaivor>().get_accounts();
                yield return new WaitForSeconds(1);
            }

            Debug.Log("+++ aouit from table complete, account quantity = " + scene.GetComponent<TableSceneBehaivor>().current_account.quantity_);
            if (quantityToDouble(scene.GetComponent<TableSceneBehaivor>().current_account.quantity_) > 0)
            {
                Debug.Log("+++ try to withdraw");
                scene.GetComponent<TableSceneBehaivor>().sned_withdraw_action();
                Debug.Log("+++ withdraw sended");
                while (quantityToDouble(scene.GetComponent<TableSceneBehaivor>().current_account.quantity_) > 0)
                {
                    Debug.Log("+++ withdraw whait loop");
                    scene.GetComponent<TableSceneBehaivor>().get_accounts();
                    yield return new WaitForSeconds(1);
                }
            }

            Debug.Log("+++ withdraw done");
            run = false;            
        }
        Debug.Log("+++ Application.Quit() try");
        Application.Quit();
        Debug.Log("+++ Application.Quit() ok");
    }
}
