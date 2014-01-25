using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

	public float cellSize = 1;
	public int levelRows = 10;
	public int levelCols = 10;

	private Cell[,] cells;
	public GameObject cellPrefab;

	void Awake()
	{
		cells = new Cell[levelRows,levelCols];
	}

	void Start () {

		GameObject cellContainer = new GameObject();
		cellContainer.name = "Cells";
		for (int i = 0; i < levelRows; i++)
		{
			for (int j = 0; j < levelCols; j++)
			{
				GameObject c = GameObject.Instantiate(cellPrefab, new Vector3(i,-0.5f,j),Quaternion.identity) as GameObject;
				c.name = "Cell " + i + "," + j;
				c.transform.parent = cellContainer.transform;
				cells[i,j] = c.GetComponent<Cell>();
			}
		}
	}
	
	void Update () {
	
	}
}
