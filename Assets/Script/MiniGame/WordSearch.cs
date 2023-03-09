using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct WordGrid {
    public char[,] word;

    public WordGrid(char[,] word) {
        this.word = word;
    }

}

public class WordSearch : MonoBehaviour {

    List<WordGrid> wordGrid;

    Letter start, end;
    public List<string> choices;
    Text[] choiceDisplay;

    [SerializeField]
    Letter prefab_letter;
    [SerializeField]
    Text prefab_text;
    [SerializeField]
    Transform startChoice;

    LineRenderer line;
    Letter[][] letters;

    [SerializeField]
    Slider timerUI;

    float timer, maxTime;

    bool timerEnd;

    [SerializeField]
    GameObject losePanel, winPanel;

    void Awake() {
        line = GetComponent<LineRenderer>();
        maxTime = GameData.activeStage.miniGameTime;
        line.SetPosition(0, Vector3.one * 1000);
        line.SetPosition(1, Vector3.one * 1000);
        choices = new List<string>();
        choiceDisplay = new Text[GameData.activeStage.choices.Length];
        for (int i = 0; i < GameData.activeStage.choices.Length; i++) {
            choices.Add(GameData.activeStage.choices[i].ToUpper());
            choiceDisplay[i] = (Text)Instantiate(prefab_text, startChoice);
            choiceDisplay[i].rectTransform.localPosition = i * Vector3.down * 20;
            choiceDisplay[i].text = choices[i];
            choiceDisplay[i].transform.GetChild(0).gameObject.SetActive(false);
            print(i);
        }

        wordGrid = new List<WordGrid>();

        getData();

        letters = new Letter[10][];
        for (int i = 0; i < 10; i++) {
            letters[i] = new Letter[10];
            for (int j = 0; j < 10; j++) {
                Letter temp = (Letter)Instantiate(prefab_letter, Vector3.zero, Quaternion.identity);
                temp.transform.parent = transform;
                temp.transform.localPosition = new Vector3(j - 5f * 0.8f, i - 5f * 0.8f, 0) * 0.7f;
                char let = wordGrid[GameData.activeStage.index].word[j, i];
                if (let == '-') {
                    let = (char)Random.Range(97, 123);
                    wordGrid[GameData.activeStage.index].word[j, i] = let;
                }
                let = char.ToUpper(let);
                temp.init(this, new Vector2Int(j, i), let);
                letters[i][j] = temp;
            }
        }
        transform.eulerAngles = new Vector3(0, 0, 90 * (int)(Random.Range(0, 4)));
        for (int i = 0; i < letters.Length; i++) {
            for (int j = 0; j < letters.Length; j++) {
                letters[i][j].transform.eulerAngles = Vector3.zero;
            }
        }

        updateList();
        StartCoroutine(RunTimer());
    }

    private void Update() {
        if (!timerEnd && start != null) {
            line.SetPosition(0, start.transform.position);
            Vector3 input = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            input.z = 0;
            line.SetPosition(1, input);
            if (Input.GetMouseButtonUp(0)) {
                RaycastHit2D hit = Physics2D.Raycast(input, -Vector2.up);
                Letter temp = null;
                if (hit.collider != null) {
                    temp = hit.collider.GetComponent<Letter>();
                }
                onUp(temp);
            }
        }

    }

    public void onDown(Letter letter) {
        start = letter;
    }

    public void onUp(Letter letter) {
        if (!timerEnd && letter != null && start != letter) {
            end = letter;
            Letter[] let = getLetters().ToArray();
            string str = "";
            for (int i = 0; i < let.Length; i++) {
                str += let[i].letter();
                let[i].changeColor(Color.white);
            }
            print(str);
            bool correct = false;
            for (int i = 0; i < choices.Count; i++) {
                if (str == choices[i]) {
                    correct = true;
                    choices.Remove(str);
                    for (int j = 0; j < choiceDisplay.Length; j++) {
                        if (choiceDisplay[j].text == str) {
                            choiceDisplay[j].transform.GetChild(0).gameObject.SetActive(true);
                            break;
                        }
                    }
                    if (choices.Count == 0) {
                        winPanel.SetActive(true);
                        timerEnd = true;
                    }
                    break;
                }
            }
            print("CORRECT: " + correct);
            if (!correct) {
                for (int i = 0; i < let.Length; i++) {
                    let[i].fadeTo(Color.black);
                }
            } else {
                for (int i = 0; i < let.Length; i++) {
                    let[i].fadeTo(Color.yellow);
                    let[i].correct = true;
                }
            }
        }
        updateList();
        print("UP");
        line.SetPosition(0, Vector3.one * 1000);
        line.SetPosition(1, Vector3.one * 1000);
        start = null;
        end = null;
    }

    public List<Letter> getLetters() {

        List<Letter> temp = new List<Letter>();
        Vector2Int diff = end.coordinate - start.coordinate;
        if (diff.x == 0 || diff.y == 0 || Mathf.Abs(diff.x) == Mathf.Abs(diff.y)) {
            Vector2Int add = Vector2Int.zero;
            if (diff.x < 0) {
                add.x = -1;
            }
            if (diff.y < 0) {
                add.y = -1;
            }
            if (diff.x > 0) {
                add.x = 1;
            }
            if (diff.y > 0) {
                add.y = 1;
            }
            Vector2Int origin = start.coordinate;
            while (origin != end.coordinate + add) {
                temp.Add(letters[origin.y][origin.x]);
                origin += add;
            }
        }

        return temp;
    }

    IEnumerator RunTimer() {
        timer = maxTime;
        Vector3 sliderPos = timerUI.transform.localPosition;
        while (timer > 0 && !timerEnd) {
            timer -= Time.deltaTime;
            timerUI.value = timer / maxTime;
            if (timerUI.value < 0.2f) {
                Vector3 temp = timerUI.transform.localPosition;
                temp += sliderPos + Random.insideUnitSphere * 0.02f;
                temp.z = 0;
                timerUI.transform.localPosition = temp;
            }
            if (timer <= 0) {
                timerUI.gameObject.SetActive(false);
                timerEnd = true;
                onLose();
            }
            yield return null;
        }
    }

    void onLose() {
        losePanel.SetActive(true);
    }

    public void updateList() {
        /*list.text = "";
        for (int i = 0; i < choices.Count; i++) {
            list.text += choices[i] + "\n";
        }*/
    }

    void getData() {
        char[,] levelData = new char[10, 10] {
            { 'A', '-','-','-', '-','-', 'A','R','K', 'O' },
            { 'M', '-','-','-', '-','-', 'T','-','-', '-' },
            { 'P', '-','-','-', '-','-', 'E','-','-', '-' },
            { 'A', '-','-','-', '-','-', 'N','-','-', '-' },
            { 'L', '-','-','-', '-','-', 'G','-','-', 'E' },
            { 'A', '-','-','-', '-','-', 'A','-','-', 'T' },
            { 'Y', 'P','I','N', 'A','K', 'B','E','T', 'O' },
            { 'A', '-','-','-', '-','-', '-','-','-', 'M' },
            { 'L', 'O','N','G', 'G','A', 'N','I','S', 'A' },
            { '-', '-','-','-', '-','-', '-','-','-', 'K' },
        };
        wordGrid.Add(new WordGrid(levelData));
        levelData = new char[10, 10] {
            { 'P', 'I','N','U', 'N','E', 'G','-','-', '-' },
            { '-', '-','-','-', '-','-', '-','-','-', 'E' },
            { '-', 'G','N','O', 'R','U', 'G','A','-', 'K' },
            { '-', '-','B','-', '-','-', '-','-','-', 'A' },
            { '-', '-','L','-', '-','-', '-','G','-', 'C' },
            { '-', '-','O','-', '-','-', '-','N','-', 'T' },
            { '-', '-','O','-', '-','-', '-','O','-', 'R' },
            { '-', '-','D','-', '-','-', '-','B','-', 'O' },
            { '-', '-','-','-', '-','-', '-','A','-', 'H' },
            { 'B', 'I','N','U', 'N','G', 'O','R','-', 'S' },
        };
        wordGrid.Add(new WordGrid(levelData));
        levelData = new char[10, 10] {
            { '-', '-','-','-', '-','-', '-','-','-', '-' },
            { 'A', 'T','C','H', 'A','R', 'A','-','-', 'R' },
            { 'S', '-','O','-', '-','-', '-','-','-', 'A' },
            { 'N', '-','-','C', '-','-', '-','-','-', 'G' },
            { 'I', '-','-','-', 'I','-', '-','-','-', 'E' },
            { 'S', '-','-','-', '-','N', '-','-','-', 'N' },
            { 'I', '-','-','-', '-','-', 'O','-','-', 'I' },
            { 'A', '-','P','A', 'P','A', 'Y','A','-', 'V' },
            { 'R', '-','-','-', '-','-', '-','-','-', '-' },
            { '-', '-','-','-', 'N','O', 'C','R','O', 'M' },
        };
        wordGrid.Add(new WordGrid(levelData));
        levelData = new char[10, 10] {
            { 'B', '-','-','-', '-','-', 'N','-','-', '-' },
            { 'A', '-','-','-', 'H','-', 'I','-','-', '-' },
            { 'N', '-','-','-', 'I','-', 'W','-','-', '-' },
            { 'G', '-','-','-', 'C','-', 'A','-','-', '-' },
            { 'U', '-','-','-', 'H','-', 'L','-','-', '-' },
            { 'S', '-','-','-', 'A','-', 'I','-','-', 'N' },
            { '-', '-','-','-', 'R','-', 'K','-','-', 'O' },
            { '-', 'M','A','N', 'O','K', '-','-','-', 'M' },
            { '-', '-','-','-', 'N','-', '-','-','-', 'E' },
            { 'L', 'E','C','H', 'O','N', '-','-','-', 'L' },
        };
        wordGrid.Add(new WordGrid(levelData));
        levelData = new char[10, 10] {
            { 'E', 'G','G','S', '-','A', '-','-','-', '-' },
            { '-', '-','-','-', '-','D', '-','-','-', '-' },
            { '-', '-','-','-', '-','O', '-','-','-', '-' },
            { '-', '-','-','-', '-','B', 'A','Y','-', '-' },
            { '-', '-','-','-', '-','O', '-','-','-', '-' },
            { 'W', '-','-','-', '-','-', '-','-','-', '-' },
            { 'A', '-','-','N', 'E','K', 'C','I','H', 'C' },
            { 'T', '-','-','-', '-','-', '-','-','-', '-' },
            { 'I', '-','-','-', '-','-', '-','-','-', '-' },
            { 'S', 'I','N','I', 'G','A', 'N','G','-', '-' },
        };
        wordGrid.Add(new WordGrid(levelData));
        levelData = new char[10, 10] {
            { 'C', 'A','B','B', 'A','G', 'E','-','-', '-' },
            { '-', '-','S','-', '-','-', '-','T','-', 'B' },
            { '-', '-','C','-', '-','-', '-','I','-', 'U' },
            { '-', '-','A','-', '-','-', '-','N','-', 'L' },
            { '-', '-','D','-', '-','-', '-','A','-', 'A' },
            { '-', '-','-','-', '-','-', '-','P','-', 'L' },
            { '-', '-','-','-', '-','O', '-','A','-', 'O' },
            { '-', '-','-','-', '-','N', '-','-','-', '-' },
            { '-', '-','-','-', '-','E', '-','-','-', '-' },
            { 'K', 'A','L','D', 'E','R', 'E','T','A', '-' },
        };
        wordGrid.Add(new WordGrid(levelData));
        levelData = new char[10, 10] {
            { 'P', 'I','E','-', '-','-', '-','-','C', '-' },
            { '-', '-','-','-', '-','-', '-','-','R', '-' },
            { '-', '-','-','-', '-','-', '-','-','E', 'P' },
            { '-', '-','-','-', '-','-', '-','-','A', 'U' },
            { '-', '-','-','-', '-','-', '-','-','M', 'T' },
            { '-', '-','-','-', '-','-', '-','-','-', 'I' },
            { '-', '-','O','-', '-','-', '-','-','-', '-' },
            { '-', '-','K','E', 'S','O', '-','-','-', '-' },
            { '-', '-','U','-', '-','-', '-','-','-', '-' },
            { 'B', 'I','B','I', 'N','G', 'K','A','-', '-' },
        };
        wordGrid.Add(new WordGrid(levelData));
        levelData = new char[10, 10] {
            { '-', 'C','-','-', '-','-', '-','-','-', '-' },
            { '-', 'A','-','-', '-','-', '-','-','-', '-' },
            { '-', 'N','-','-', '-','-', '-','-','-', '-' },
            { '-', 'O','A','P', 'O','I', 'S','-','-', '-' },
            { 'N', 'L','-','-', '-','-', 'S','-','-', '-' },
            { 'I', 'A','-','-', 'G','-', 'E','-','-', 'T' },
            { 'S', '-','-','-', 'N','-', 'R','-','-', 'A' },
            { 'I', '-','-','-', 'I','-', 'P','-','-', 'R' },
            { 'O', '-','-','-', 'A','-', 'X','-','-', 'O' },
            { 'H', '-','-','-', 'L','-', 'E','-','-', '-' },
        };
        wordGrid.Add(new WordGrid(levelData));
        levelData = new char[10, 10] {
            { '-', '-','-','-', 'W','O', 'N','T','O', 'N' },
            { '-', '-','-','-', '-','A', '-','-','-', '-' },
            { '-', '-','-','-', '-','P', '-','-','-', '-' },
            { 'Y', '-','-','-', '-','O', '-','-','-', '-' },
            { 'O', '-','-','-', '-','I', '-','-','-', 'T' },
            { 'H', '-','-','-', '-','S', '-','-','-', 'I' },
            { 'C', '-','-','-', '-','-', '-','-','-', 'C' },
            { 'T', '-','S','E', 'S','A', 'M','E','-', 'N' },
            { 'A', '-','-','-', '-','-', '-','-','-', 'A' },
            { 'B', '-','-','-', '-','O', 'L','O','M', 'P' },
        };
        wordGrid.Add(new WordGrid(levelData));
        levelData = new char[10, 10] {
            { 'D', 'A','N','G', 'G','I', 'T','-','R', '-' },
            { '-', '-','-','-', '-','-', '-','-','I', '-' },
            { '-', '-','-','-', '-','-', '-','-','C', '-' },
            { 'S', 'H','A','L', 'L','O', 'T','S','E', '-' },
            { '-', '-','-','-', '-','-', '-','T','-', 'N' },
            { '-', '-','-','-', '-','-', '-','E','-', 'O' },
            { '-', '-','-','-', '-','-', '-','A','-', 'H' },
            { 'B', 'A','B','O', 'Y','-', '-','M','-', 'C' },
            { '-', '-','-','-', '-','-', '-','-','-', 'E' },
            { '-', '-','-','-', '-','-', '-','-','-', 'L' },
        };
        wordGrid.Add(new WordGrid(levelData));
        levelData = new char[10, 10] {
            { '-', '-','K','O', 'L','I', 'M','A','T', 'C' },
            { '-', '-','-','-', 'A','-', '-','-','-', 'R' },
            { 'S', 'I','L','I', 'B','-', '-','-','-', 'O' },
            { '-', '-','-','-', 'U','-', '-','-','-', 'C' },
            { '-', '-','-','-', 'Y','-', '-','-','-', 'O' },
            { '-', '-','-','G', 'O','-', '-','-','-', 'D' },
            { '-', '-','-','I', '-','-', '-','-','-', 'I' },
            { '-', '-','-','S', '-','-', '-','-','-', 'L' },
            { '-', '-','-','I', '-','-', '-','-','-', 'E' },
            { 'I', 'N','A','S', 'A','L', '-','-','-', '-' },
        };
        wordGrid.Add(new WordGrid(levelData));
        levelData = new char[10, 10] {
            { 'L', 'I','E','M', 'P','O', 'A','N','U', 'T' },
            { '-', '-','-','-', '-','-', '-','-','-', 'U' },
            { '-', '-','-','-', '-','W', '-','-','-', 'B' },
            { 'W', '-','-','-', '-','A', '-','-','-', 'I' },
            { 'A', '-','-','-', '-','L', '-','-','-', 'G' },
            { 'L', '-','-','-', '-','G', '-','-','-', '-' },
            { 'I', '-','-','-', '-','U', '-','-','-', '-' },
            { 'N', '-','-','-', '-','N', '-','-','-', '-' },
            { 'I', '-','-','-', '-','I', '-','-','-', '-' },
            { 'K', 'I','N','H', 'A','S', 'O','N','-', '-' },
        };
        wordGrid.Add(new WordGrid(levelData));
    }
}
