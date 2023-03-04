
using UnityEngine;

public class Node : MonoBehaviour
{
    private GameObject player;
    private Player playerScript;
    private Node node;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        node = null;
        playerScript = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        node = this;
        //playerScript.SelectNode(node);
    }

    public void OnMouseUp()
    {
        node = null;
        //playerScript.DeselectNode();
    }
}
