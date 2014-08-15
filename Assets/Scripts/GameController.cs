using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public Candy candy;

    public int rowNum = 0;
    public int columNum = 0;

    public GameObject gameController;

    private Candy lastSelectedCandy = null;
    private ArrayList candyArray;

    private ArrayList candyMatches;

	// Use this for initialization
	void Start () 
    {
        candyArray = new ArrayList();
        for (int rowIndex = 0; rowIndex < rowNum; rowIndex++)
        {
            ArrayList tempArray = new ArrayList();
            for (int columIndex = 0; columIndex < columNum; columIndex++)
            {
                tempArray.Add(AddCandy(rowIndex, columIndex));
            }
            candyArray.Add(tempArray);
        }
        if (CheckMatches())
        {
            RemoveMatches();
        }
	}

    private Candy AddCandy(int rowIndex, int columIndex)
    {
        Candy c = Instantiate(candy) as Candy;
        c.transform.parent = this.transform;
        c.columIndex = columIndex;
        c.rowIndex = rowIndex + 1;
        c.gameController = this;
        c.UpdatePosition();
        c.rowIndex--;
        c.TweenToPosition();
        return c;
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
        //Remove(candy);
        //return;
        if (lastSelectedCandy == null)
        {
            lastSelectedCandy = candy;
            return;
        }
        else
        {
            if (Mathf.Abs(lastSelectedCandy.rowIndex - candy.rowIndex) + Mathf.Abs(lastSelectedCandy.columIndex - candy.columIndex) == 1)
            {
                Exchange(lastSelectedCandy, candy);
                if (CheckMatches())
                {
                    RemoveMatches();
                }
            }
            lastSelectedCandy = null;
        }
    }

    private void Exchange(Candy lastSelected, Candy nowCandy)
    {
        SetCandy(lastSelected.rowIndex, lastSelected.columIndex, nowCandy);
        SetCandy(nowCandy.rowIndex, nowCandy.columIndex, lastSelected);

        int rowIndex = lastSelected.rowIndex;
        lastSelected.rowIndex = nowCandy.rowIndex;
        nowCandy.rowIndex = rowIndex;

        int columIndex = lastSelected.columIndex;
        lastSelected.columIndex = nowCandy.columIndex;
        nowCandy.columIndex = columIndex;

        //lastSelected.UpdatePosition();
        //nowCandy.UpdatePosition();
        lastSelected.TweenToPosition();
        nowCandy.TweenToPosition();
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
            candyToMove.TweenToPosition();

            SetCandy(rowIndex - 1, candy.columIndex, candyToMove);
        }
        SetCandy(rowNum - 1, candy.columIndex, AddCandy(rowNum - 1, candy.columIndex));
    }

    private bool CheckHorizontalMatches()
    {
        bool result = false;
        for (int rowIndex = 0; rowIndex < rowNum; rowIndex++)
        {
            for (int columIndex = 0; columIndex < columNum - 2; columIndex++)
            {
                if ((GetCandy(rowIndex, columIndex).candyType == GetCandy(rowIndex, columIndex + 1).candyType) &&
                    (GetCandy(rowIndex, columIndex + 1).candyType == GetCandy(rowIndex, columIndex + 2).candyType))
                {
                    AddMathes(GetCandy(rowIndex, columIndex));
                    AddMathes(GetCandy(rowIndex, columIndex + 1));
                    AddMathes(GetCandy(rowIndex, columIndex + 2));
                    result = true;
                }
            }
        }
        return result;
    }

    private bool CheckVerticalMatches()
    {
        bool result = false;
        for (int columIndex = 0; columIndex < columNum; columIndex++)
        {
            for (int rowIndex = 0; rowIndex < rowNum - 2; rowIndex++)
            {
                if ((GetCandy(rowIndex, columIndex).candyType == GetCandy(rowIndex + 1, columIndex).candyType) &&
                    (GetCandy(rowIndex + 1, columIndex).candyType == GetCandy(rowIndex + 2, columIndex).candyType))
                {
                    AddMathes(GetCandy(rowIndex, columIndex));
                    AddMathes(GetCandy(rowIndex + 1, columIndex));
                    AddMathes(GetCandy(rowIndex + 2, columIndex));
                    result = true;
                }
            }
        }
        return result;
    }

    public bool CheckMatches()
    {
        return CheckVerticalMatches() || CheckHorizontalMatches();
    }

    private void AddMathes(Candy candy)
    {
        if (candyMatches == null)
        {
            candyMatches = new ArrayList();
        }
        if (candyMatches.IndexOf(candy) == -1)
        {
            candyMatches.Add(candy);
        }
    }

    private void RemoveMatches()
    {
        foreach (Candy candyToRemove in candyMatches)
        {
            Remove(candyToRemove);
        }
        if (CheckMatches())
        {
            RemoveMatches();
        }
        candyMatches = new ArrayList();
    }
}
