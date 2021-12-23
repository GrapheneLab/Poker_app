using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;

public class NodesList : MonoBehaviour
{
    struct NodePing
    {
        public string url;
        public string ipAddr;
        public GameObject target;
        public Ping ping;

        public void restart()
        {
            ping = new Ping(ipAddr);
        }
    };

   // SQLiteDB db = SQLiteDB.Instance;
   // string DBName = "nodes_db.db";

    List<NodePing> pingers = new List<NodePing>();

    void Start()
    {
      //  db.DBLocation = Application.persistentDataPath;
       // db.DBName = DBName;
       /*
        if (db.Exists)
        {
           // Debug.Log("DBExist at - " + db.DBLocation + "/" + db.DBName);
            ConnectToDB();
        }
        else
        {

          //  Debug.Log("Creating new DB at - " + db.DBLocation + "/" + db.DBName);
            CreateDB();
        }
        */
        Redraw();
    }

    public void Redraw()
    {
        Reset();
        Draw();
    }

    public int GetNodesCount( )
    {
        return GetNodes().Count;
    }

    void setVisible( int index, bool visible )
    {
        var recordObject = GameObject.Find("Record" + index.ToString());
        recordObject.GetComponent<UISprite>().enabled = visible;

        var recordIpObject = GameObject.Find("Record" + index.ToString() + "Label");
        recordIpObject.GetComponent<UILabel>().enabled = visible;

        var recordPingObject = GameObject.Find("Record" + index.ToString() + "PingLabel");
        recordPingObject.GetComponent<UILabel>().enabled = visible;

        //var recordConnect = GameObject.Find("Record" + index.ToString() + "Connect");
       // recordConnect.GetComponent<UISprite>().enabled = visible;
       // recordConnect.GetComponent<UIButton>().enabled = visible;

        var recordForgot = GameObject.Find("Record" + index.ToString() + "Forgot");
        recordForgot.GetComponent<UISprite>().enabled = visible;
        recordForgot.GetComponent<UIButton>().enabled = visible;
    }

    void Draw()
    {
        var n = GetNodes();
        int i = 0;        
        foreach (var node in n)
        {
            setVisible(i, true);
            var recordObject = GameObject.Find("Record" + i.ToString());
            var recordIpObject = GameObject.Find("Record" + i.ToString() + "Label" );
            var recordPingObject = GameObject.Find("Record" + i.ToString() + "PingLabel");

            recordIpObject.GetComponent<UILabel>().text = node;

            string clean_addr;
            if(node.Contains("http"))
            {
                var addr_words = node.Split('/');
                var addr_words1 = addr_words[2].Split(':');
                clean_addr = addr_words1[0];
            }else
            {
                var addr_words1 = node.Split(':');
                clean_addr = addr_words1[0];
            }

         //   Debug.Log( "clean ip = " + clean_addr );

            IPAddress[] addresses = Dns.GetHostAddresses(clean_addr);
    //        Debug.Log(addresses[0].ToString());
       
            NodePing np = new NodePing();
            np.url = node;
            np.ipAddr = addresses[0].ToString();
            np.target = recordPingObject;

        //    Debug.Log("FIRST PING - " + addresses[0].ToString());
            np.ping = new Ping(addresses[0].ToString());

            pingers.Add(np);
            i++;
        }
        StartCoroutine(PingUpdate());
    }

    IEnumerator PingUpdate()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var p in pingers)
        {
            if (p.ping.isDone)
            {
                int pingTime = p.ping.time;

               // Debug.Log(p.ipAddr + " done - " + pingTime.ToString() + " ms");
                p.target.GetComponent<UILabel>().text = pingTime.ToString() + " ms";
                if( pingTime < 50 )
                    p.target.GetComponent<UILabel>().color = Color.green;
                else
                    p.target.GetComponent<UILabel>().color = Color.red;
                p.restart();

 
            }else
            {
                p.target.GetComponent<UILabel>().text = "dead";
                p.target.GetComponent<UILabel>().color = Color.red;
                p.restart();
            }
        }
        StartCoroutine(PingUpdate());
    }

    void Reset()
    {
        for( int i = 0; i < 8; i++ )
        {
            setVisible(i, false);
        }
    }
    /*
    void InsertDefaultNodes()
    {
        AddRow("https://api.main.alohaeos.com", "443");       
        AddRow("https://eosbp.atticlab.net", "443");      
        AddRow("https://mainnet.genereos.io", "443");       
        AddRow("https://api-CLEOS.eos.blckchnd.com", "443");
        AddRow("http://192.168.1.146", "8011");
        AddRow("http://206.81.31.139", "8000");
    }

    void CreateDB()
    {
        bool result = false;
        result = db.CreateDatabase(DBName, true);
        if (!result)
            Debug.Log("Can't create DB!");
        
        result = db.ConnectToDefaultDatabase(DBName, false);
        if (!result)
            Debug.Log("Can't connect to DB!");
            
        result = CreateTable();
        Debug.Log("Can't create TABLE!");

        InsertDefaultNodes();
    }

    void ConnectToDB()
    {
        db.ConnectToDefaultDatabase(DBName, false);
    }

    bool CreateTable()
    {
        DBSchema schema = new DBSchema("Nodes");
        schema.AddField("IpAddr", SQLiteDB.DB_DataType.DB_VARCHAR, 100, false, true, true);
        schema.AddField("Port", SQLiteDB.DB_DataType.DB_VARCHAR, 100, false, false, false);
        return db.CreateTable(schema);
    }

    public void DeleteRow(string ip)
    {
        SQLiteDB.DB_ConditionPair condition = new SQLiteDB.DB_ConditionPair();
        condition.fieldName = "IpAddr";
        condition.value = ip;
        condition.condition = SQLiteDB.DB_Condition.EQUAL_TO;

        int i = db.DeleteRow("Nodes", condition);
        if (i > 0)
        {
            Debug.Log(i + " Record Deleted!");
            Redraw();
        }
    }

    public void AddRow(string ip, string port)
    {
        List<SQLiteDB.DB_DataPair> dataPairList = new List<SQLiteDB.DB_DataPair>();
        SQLiteDB.DB_DataPair data = new SQLiteDB.DB_DataPair();

        data.fieldName = "IpAddr";
        data.value = ip;
        dataPairList.Add(data);

        data.fieldName = "Port";
        data.value = port;
        dataPairList.Add(data);

        int i = db.Insert("Nodes", dataPairList);
        if (i > 0)
        {
            Debug.Log("Record Inserted!");
        }
    }
    */
    List<string> GetNodes()
    {
        List<string> r = new List<string>();

        string t = "https://jungle2.cryptolions.io" + ":" + "443";
        r.Add(t);

        t = "https://api.jungle.alohaeos.com" + ":" + "443";
        r.Add(t);

        t = "https://jungle-api.blckchnd.com" + ":" + "443";
        r.Add(t);
    
        t = "http://138.201.129.98" + ":" + "28888";
        r.Add(t);

        t = "http://192.168.1.146" + ":" + "8000";
        r.Add(t);

        return r;
    }
}

