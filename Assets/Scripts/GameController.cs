using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public Candy candy;

    public int rowNum = 0;
    public int columNum = 0;

    public GameObject gameController;

    private Candy lastSelectedCandy = null;
    private ArrayList candyArray;

	// Use this for initialization
	void Start () 
    {
        candyArray = new ArrayList();
        for (int rowIndex = 0; rowIndex < rowNum; rowIndex++)
        {
            ArrayList tempArray = new ArrayList();
            for (int columIndex = 0; columIndex < columNum; columIndex++)
            {
                Candy c = Instantiate(candy) as Candy;
                c.transform.parent = this.transform;
                c.columIndex = columIndex;
                c.rowIndex = rowIndex;
                c.gameController = this;
                c.UpdatePosition();
                tempArray.Add(c);
            }
            candyArray.Add(tempArray);
        }
	}

    private Candy GetCandy(int rowIndex, int columIndex)
    {
        ArrayList temp = candyArray[rowIndex] as ArrayList;
        return temp[columIndex] as Candy;
    }

    private void SetCandy(int rowIndex, int columIndex, Candy candy)
    {
        ArrayList temp = candyArray[rowIndex] as ArrayList;
        temp[columIndex] = candy;
    }

    public void Select(Candy candy)
    {
        Remove(candy);
        return;
        if (lastSelectedCandy == null)
        {
            lastSelectedCandy = candy;
            return;
        }
        else
        {
            Exchange(lastSelectedCandy, candy);
            lastSelectedCandy = null;
        }
    }

    private void Exchange(Candy lastSelected, Candy nowCandy)
    {
        int rowIndex = lastSelected.rowIndex;
        lastSelected.rowIndex = nowCandy.rowIndex;
        nowCandy.rowIndex = rowIndex;

        int columIndex = lastSelected.columIndex;
        lastSelected.columIndex = nowCandy.columIndex;
        nowCandy.columIndex = columIndex;

        lastSelected.UpdatePosition();
        nowCandy.UpdatePosition();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void Remove(Candy candy)
    {
        candy.Dispose();
        for (int rowIndex = candy.rowIndex + 1; rowIndex < rowNum; rowIndex++)
        {
            Candy candyToMove = GetCandy(rowIndex, candy.columIndex);
            candyToMove.rowIndex--;
            candyToMove.UpdatePosition();

            SetCandy(rowIndex - 1, candy.columIndex, candyToMove);
        }
    }
}
