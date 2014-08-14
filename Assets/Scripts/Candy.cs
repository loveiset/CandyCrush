using UnityEngine;
using System.Collections;

public class Candy : MonoBehaviour {

    public int rowIndex = 0;
    public int columIndex = 0;

    public float xOffset = 0f;
    public float yOffset = 0f;
    public int candyType;

    public GameObject[] candyBG;
    private GameObject candy;

    public GameController gameController;

	// Use this for initialization
	void Start () 
    {

	}

    void OnMouseDown()
    {
        gameController.Select(this);
    }

    private void AddRandomBG()
    {
        if (candy == null)
        {
            candyType = Random.Range(0, candyBG.Length);
            candy = Instantiate(candyBG[candyType]) as GameObject;
            candy.transform.parent = this.transform;
        }
    }

	// Update is called once per frame
	public void UpdatePosition () 
    {
        AddRandomBG();
        transform.position = new Vector3(columIndex + xOffset, rowIndex + yOffset, 0f);
	}

    public void Dispose()
    {
        gameController = null;
        Destroy(candy.gameObject);

        Destroy(this.gameObject);
    }
}
