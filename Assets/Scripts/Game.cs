using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public List<GameObject> ButtPref;
    public List<int> butts;
    public List<int> buttsdef;
    public GameObject ButtsPan;
    public float timer;
    public bool timeon;
    public int score;
    public int scoreneed;
    public int lvl;
    public int lvlopen;
    int columns = 12;
    int rows = 16;

    public TextMeshProUGUI timetxt;
    public TextMeshProUGUI scoretxt;

    public GameObject WinPan;
    public GameObject LosePan;
    public TextMeshProUGUI time1txt;
    public TextMeshProUGUI score1txt;
    public TextMeshProUGUI time2txt;
    public TextMeshProUGUI score2txt;

    public GameObject MenuPan;

    public GameObject LvlsPan;
    public List<GameObject> Lvlsb;

    public GameObject SettingsPan;

    public GameObject GamePan;

    public AudioSource a1;
    public AudioSource a2;

    public Slider sliderM;
    public Slider sliderS;

    public GameObject lp;

    public bool set;

    public void Start()
    {
        ResetTable();
        LoadData();
        Invoke("Sts", 2f);
    }

    void Sts()
    {
        PanOpen(0);
    }

    public static List<int> ShiftColumnsRight(List<int> matrix, int rows, int columns)
    {
        List<int> result = new List<int>(new int[rows * columns]);
        int newColumnIndex = columns - 1;

        for (int col = columns - 1; col >= 0; col--)
        {
            bool isEmptyColumn = true;

            for (int row = 0; row < rows; row++)
            {
                if (matrix[row * columns + col] != -1)
                {
                    isEmptyColumn = false;
                    break;
                }
            }

            if (!isEmptyColumn)
            {
                for (int row = 0; row < rows; row++)
                {
                    result[row * columns + newColumnIndex] = matrix[row * columns + col];
                }
                newColumnIndex--;
            }
        }

        for (int col = 0; col <= newColumnIndex; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                result[row * columns + col] = -1;
            }
        }

        return result;
    }

    public List<int> GetMatchingNeighbors(int number, List<int> butts)
    {
        if (number < 0 || number >= 192)
        {
            return new List<int>();
        }

        int rows = 16;
        int columns = 12;

        List<int> matchingNeighbors = new List<int>();
        HashSet<int> visited = new HashSet<int>();

        int x = number % columns;
        int y = number / columns;

        void FindNeighbors(int currentX, int currentY)
        {
            if (currentX < 0 || currentX >= columns || currentY < 0 || currentY >= rows)
                return;

            int neighborIndex = currentY * columns + currentX;

            if (visited.Contains(neighborIndex))
                return;

            if (butts[neighborIndex] == butts[number])
            {
                matchingNeighbors.Add(neighborIndex);
                visited.Add(neighborIndex);

                FindNeighbors(currentX - 1, currentY);
                FindNeighbors(currentX + 1, currentY);
                FindNeighbors(currentX, currentY - 1);
                FindNeighbors(currentX, currentY + 1);
            }
        }

        FindNeighbors(x, y);

        return matchingNeighbors;
    }

    public void NextLvl()
    {
        if (lvl < 6)
        {
            lvl++;
            GameStart(lvl);
        }
        else
        {
            GameStart(lvl);
        }
    }

    public void SetSet(bool set1)
    {
        set = set1;
        if (set1)
        {
            timeon = false;
        }
    }

    public void Back()
    {
        if (set)
        {
            timeon = true;
            set = false;
            PanOpen(1);
        }
        else
        {
            PanOpen(0);
        }
    }

    public void RetryLvl()
    {
        GameStart(lvl);
    }

    public void GameStart(int lvl1)
    {
        if (lvl1 <= lvlopen)
        {
            lvl = lvl1;
            PanOpen(1);
            score = 0;
            timer = 80 - 5 * lvl1;
            scoreneed = 500 + 50 * lvl1;

            timeon = true;
            GenerateCircles();
            ResetTable();
        }
    }

    public void PanOpen(int i)
    {
        List<GameObject> list = new List<GameObject>();
        list.Add(MenuPan);
        list.Add(GamePan);
        list.Add(LvlsPan);
        list.Add(SettingsPan);
        list.Add(WinPan);
        list.Add(LosePan);
        list.Add(lp);
        for (int a = 0; a < list.Count; a++)
        {
            list[a].gameObject.SetActive(i == a);
        }
    }

    public void GenerateCircles()
    {
        butts = buttsdef;
    }

    public void Phis()
    {
        for (int a = 0; a < 191; a++)
        {
            if (a < 180 && butts[a] != -1)
            {
                if (butts[a + 12] == -1)
                {
                    int c = butts[a];
                    butts[a + 12] = c;
                    butts[a] = -1;
                }
            }
        }
    }

    public void Phis2()
    {
        for (int a = 180; a < 191; a++)
        {
            // Empty method for now.
        }
    }

    public void ResetTable()
    {
        for (int a = 0; a < 15; a++)
        {
            Phis();
        }
        for (int a = 0; a < 15; a++)
        {
            butts = ShiftColumnsRight(butts, rows, columns);
        }

        for (int i = 0; i < ButtsPan.transform.childCount; i++)
        {
            Destroy(ButtsPan.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < 192; i++)
        {
            if (butts[i] != -1)
            {
                GameObject a = Instantiate(ButtPref[butts[i]], new Vector2(0, 0), Quaternion.identity, ButtsPan.transform);
                a.GetComponent<Butts>().color = butts[i];
                a.GetComponent<Butts>().pos = i;
                a.GetComponent<Butts>().game = this;
            }
            else
            {
                GameObject a = Instantiate(ButtPref[7], new Vector2(0, 0), Quaternion.identity, ButtsPan.transform);
            }
        }
    }

    public void Lose()
    {
        timeon = false;
        PanOpen(5);
    }

    public void Win()
    {
        timeon = false;
        if (lvl == lvlopen)
        {
            lvlopen++;
        }
        PanOpen(4);
    }

    public void Button(int a)
    {
        List<int> b = GetMatchingNeighbors(a, butts);
        if (b.Count > 1)
        {
            a1.Play();
            foreach (int i in b)
            {
                butts[i] = -1;
                score += 10;
            }
        }

        ResetTable();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("lvl", lvlopen);
        PlayerPrefs.SetFloat("s1", sliderM.value);
        PlayerPrefs.SetFloat("s2", sliderS.value);
    }

    public void LoadData()
    {
        lvlopen = PlayerPrefs.GetInt("lvl");
        if (PlayerPrefs.HasKey("s1"))
        {
            sliderM.value = PlayerPrefs.GetFloat("s1");
            sliderS.value = PlayerPrefs.GetFloat("s2");
        }
    }

    public void Okey()
    {
        timeon = false;
    }

    public void AA()
    {
        Application.Quit();
    }

    string ConvertSecondsToTime(int seconds)
    {
        int minutes = seconds / 60;
        int remainingSeconds = seconds % 60;

        return string.Format("{0:D2}:{1:D2}", minutes, remainingSeconds);
    }

    private void Update()
    {
        if (timeon)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                Lose();
            }

            if (score > scoreneed)
            {
                Win();
            }
        }

        SaveData();

        timetxt.text = ConvertSecondsToTime(Mathf.FloorToInt(timer));
        scoretxt.text = "Score: " + score + "/" + scoreneed;
        score1txt.text = "Level " + (lvl + 1);
        score2txt.text = "Level " + (lvl + 1);

        GetComponent<AudioSource>().volume = sliderM.value;

        a1.volume = sliderS.value;
    }
}
