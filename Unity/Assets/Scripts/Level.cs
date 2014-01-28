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

	public float levelTime = 60;
	public float cellSize = 1;
	public int levelRows = 10;
	public int levelCols = 10;
	public int activatedCells = 0;
	public bool gameOver = false;
	public GameObject TimeUp;
	//public GameObject XWins;
	public GameObject GUIScoreBlue, GUIScoreRed, GUIScoreGreen, GUIScoreYellow;
	private Cell[,] cells;
	public GameObject cellPrefab;
	public GameObject powerupPrefab;

	public AudioClip wall, speed, explode, freeze, shotgun;

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
		if (Application.loadedLevelName != "Title") StartLevel();
		StartCoroutine(PowerupCountdown());
		//StartCoroutine(PowerupCountdown());
	}

	public void StartLevel()
	{
		for (int i = 0; i < levelRows; i++)
		{
			for (int j = 0; j < levelCols; j++)
			{
				cells[i,j].Restart(true);
			}
		}

		scoreBlue = 0;
		scoreRed = 0;
		scoreGreen = 0;
		scoreYellow = 0;
		levelTime = 60;

		GameObject p1 = GameObject.Find("Player1") as GameObject;
		p1.transform.position = new Vector3(0.5f,3,9.5f);
		GameObject p2 = GameObject.Find("Player2") as GameObject;
		p2.transform.position = new Vector3(9.5f,3,0.5f);
		GameObject p3 = GameObject.Find("Player3") as GameObject;
		p3.transform.position = new Vector3(9.5f,3,9.5f);
		GameObject p4 = GameObject.Find("Player4") as GameObject;
		p4.transform.position = new Vector3(0.5f,3,0.5f);

		gameOver = false;
		GUIScoreBlue.SetActive(false);
		GUIScoreRed.SetActive(false);
		GUIScoreYellow.SetActive(false);
		GUIScoreGreen.SetActive(false);
		
		TimeUp.SetActive(false);

		deletePowerUps ();
	}

	void Update () 
	{
		//if (Input.GetButtonDown("Jump")) ActivateCells(TEAM.BLUE);
		if (levelTime > 0) levelTime -= Time.deltaTime;
		else levelTime = 0;
		if(levelTime <= 0.0f && gameOver == false) 
		{
			gameOver = true;
			LevelComplete();
			levelTime = 0;
		}

		if (Input.GetKeyDown(KeyCode.R)) StartLevel();

		ResetScore(TEAM.BLUE);
		ResetScore(TEAM.RED);
		ResetScore(TEAM.YELLOW);
		ResetScore(TEAM.GREEN);
		
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
//				if (activatedCells >= 96) 
//				{
//					gameOver = true;
//					Debug.Log ("GAME OVER");
//				}
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

	public void ResetScore(Level.TEAM team)
	{
		switch(team)
		{
		case TEAM.BLUE:
			scoreBlue = 0;
			break;
		case TEAM.GREEN:
			scoreGreen = 0;
			break;
		case TEAM.RED:
			scoreRed = 0;
			break;
		case TEAM.YELLOW:
			scoreYellow = 0;
			break;
		}
	}

	IEnumerator PowerupCountdown()
	{
		for (float timer = Random.Range(3,6); timer >= 0; timer -= Time.deltaTime)
			yield return 0;
		Vector2 freeCell = GetFreeCell();
		GameObject.Instantiate(powerupPrefab, new Vector3(freeCell.y + 0.5f,5.5f,freeCell.x + 0.5f),Quaternion.identity);
		if (!gameOver) StartCoroutine(PowerupCountdown());
	}

	Vector2 GetFreeCell()
	{
		int row = Random.Range (0,10);
		int col = Random.Range (0,10);
		if (GetCell(row,col).isActivated == false) return new Vector2(row,col);
		else return GetFreeCell();
	}

	void LevelComplete()
	{
		TimeUp.SetActive(true);

		foreach (Cell c in cells)
		{
			if (c.cellTeam == TEAM.BLUE) AddScore(TEAM.BLUE);
			if (c.cellTeam == TEAM.RED) AddScore(TEAM.RED);
			if (c.cellTeam == TEAM.YELLOW) AddScore(TEAM.YELLOW);
			if (c.cellTeam == TEAM.GREEN) AddScore(TEAM.GREEN);
		}

		GUIScoreBlue.SetActive(true);
		GUIScoreBlue.guiText.text = "Blue: " + scoreBlue;
		GUIScoreRed.SetActive(true);
		GUIScoreRed.guiText.text = "Red: " + scoreRed;
		GUIScoreYellow.SetActive(true);
		GUIScoreYellow.guiText.text = "Yellow: " + scoreYellow;
		GUIScoreGreen.SetActive(true);
		GUIScoreGreen.guiText.text = "Green: " + scoreGreen;

	}

	public void freezeAllExcept(TEAM team)
	{
		Player p1 = GameObject.Find("Player1").GetComponent<Player>();
		Player p2 = GameObject.Find("Player2").GetComponent<Player>();
		Player p3 = GameObject.Find("Player3").GetComponent<Player>();
		Player p4 = GameObject.Find("Player4").GetComponent<Player>();

		if (p1.team != team) p1.freeze ();
		if (p2.team != team) p2.freeze ();
		if (p3.team != team) p3.freeze ();
		if (p4.team != team) p4.freeze ();
	}

	//Pintar celdas del alrededor del color 'team'
	public void explosion(Vector2 cellPos, TEAM team)
	{
		int x, y;
		x = (int)cellPos.x;
		y = (int)cellPos.y;

		bool t, b, l, r, tl, tr, bl, br;
		t = y < levelRows-1;
		b = y > 0;
		l = x > 0;
		r = x < levelCols - 1;
		tl = t && l;
		tr = t && r;
		bl = b && l;
		br = b && r;

		if (t) cells [y+1, x].Step (team);
		if (b) cells [y-1, x].Step (team);
		if (l) cells [y, x-1].Step (team);
		if (r) cells [y, x+1].Step (team);
		if (tl) cells [y+1, x-1].Step (team);
		if (tr) cells [y+1, x+1].Step (team);
		if (bl) cells [y-1, x-1].Step (team);
		if (br) cells [y-1, x+1].Step (team);
	}

	void deletePowerUps ()
	{
		GameObject[] powerUps;
		powerUps = GameObject.FindGameObjectsWithTag ("PowerUp");
		foreach(GameObject powerUp in powerUps)
		{
			GameObject.Destroy(powerUp);
		}
	}

	public void playPowerUpSound(Powerup.POWERUP_TYPE power_type)
	{
		switch(power_type)
		{
		case Powerup.POWERUP_TYPE.WALL:
			audio.PlayOneShot(wall);
			break;
		case Powerup.POWERUP_TYPE.EXPLOSION:
			audio.PlayOneShot(explode);
			break;
		case Powerup.POWERUP_TYPE.FREEZE:
			audio.PlayOneShot(freeze);
			break;
		case Powerup.POWERUP_TYPE.SHOTGUN:
			audio.PlayOneShot(shotgun);
			break;
		case Powerup.POWERUP_TYPE.SPEED:
			audio.PlayOneShot(speed);
			break;
		default:
			break;
		}
	}
}
