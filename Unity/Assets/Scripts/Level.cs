using UnityEngine;
using System.Collections;

public class Level : MonoSingleton<Level> {

	public float cellSize = 1;
	public int levelRows = 10;
	public int levelCols = 10;

	private Cell[,] cells;
	public GameObject cellPrefab;

	void Start () 
	{
		cells = new Cell[levelRows,levelCols];
		GameObject cellContainer = new GameObject();
		cellContainer.name = "Cells";
		for (int i = 0; i < levelRows; i++)
		{
			for (int j = 0; j < levelCols; j++)
			{
				GameObject c = GameObject.Instantiate(cellPrefab, new Vector3(i + 0.5f,-0.5f,j+0.5f),Quaternion.identity) as GameObject;
				c.name = "Cell " + i + "," + j;
				c.transform.parent = cellContainer.transform;
				cells[i,j] = c.GetComponent<Cell>();
			}
		}
	}
	
	void Update () 
	{
	
	}

	public Cell GetCell(int row, int col)
	{
		return cells[row,col];
	}
}
