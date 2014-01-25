using UnityEngine;
using System.Collections;

public class Level : MonoSingleton<Level> {
	public enum TEAM
	{
		NONE = 0,
		BLUE = 1,
		RED = 2,
		GREEN = 3,
		YELLOW = 4
	};

	public float cellSize = 1;
	public int levelRows = 10;
	public int levelCols = 10;
	public int activatedCells = 0;
	public bool gameOver = false;

	private Cell[,] cells;
	public GameObject cellPrefab;
	public GameObject powerupPrefab;

	public int scoreBlue = 0;
	public int scoreRed = 0;
	public int scoreGreen = 0;
	public int scoreYellow = 0;

	void Start () 
	{
		cells = new Cell[levelRows,levelCols];
		GameObject cellContainer = new GameObject();
		cellContainer.name = "Cells";
		for (int i = 0; i < levelRows; i++)
		{
			for (int j = 0; j < levelCols; j++)
			{
				GameObject c = GameObject.Instantiate(cellPrefab, new Vector3(j + 0.5f,-0.5f,i+0.5f),Quaternion.identity) as GameObject;
				c.name = "Cell " + i + "," + j;
				c.transform.parent = cellContainer.transform;
				cells[i,j] = c.GetComponent<Cell>();
			}
		}

		StartCoroutine(PowerupCountdown());
	}
	
	void Update () 
	{
		if (Input.GetButtonDown("Jump")) ActivateCells(TEAM.BLUE);
	}

	public Cell GetCell(int row, int col)
	{
		return cells[row,col];
	}

	public Cell GetCell(Vector2 pos)
	{
		return cells[(int)pos.x,(int)pos.y];
	}

	public void ActivateCells(Level.TEAM team)
	{
		foreach (Cell c in cells)
		{
			if (c.cellTeam == team)
			{
				if (c.ActivateCell()) activatedCells++;
				if (activatedCells >= 96) 
				{
					gameOver = true;
					Debug.Log ("GAME OVER");
				}
			}
		}
	}

	public void AddScore(Level.TEAM team)
	{
		switch(team)
		{
		case TEAM.BLUE:
			scoreBlue += 1;
			break;
		case TEAM.GREEN:
			scoreGreen += 1;
			break;
		case TEAM.RED:
			scoreRed += 1;
			break;
		case TEAM.YELLOW:
			scoreYellow += 1;
			break;
		}
	}

	IEnumerator PowerupCountdown()
	{
		for (float timer = Random.Range(5,10); timer >= 0; timer -= Time.deltaTime)
			yield return 0;

		GameObject.Instantiate(powerupPrefab, new Vector3((int)Random.Range(0,10) + 0.5f,0.5f,(int)Random.Range(0,10) + 0.5f),Quaternion.identity);
		if (!gameOver)StartCoroutine(PowerupCountdown());
	}

	Vector2 GetFreeCell()
	{
		int row = Random.Range (0,10);
		int col = Random.Range (0,10);
		if (GetCell(row,col).isActivated == false) return new Vector2(row,col);
		else return GetFreeCell();
	}
}
