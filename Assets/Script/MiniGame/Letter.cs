using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour {

    char let;
    WordSearch wordSearch;
    TextMesh textMesh;
    public Vector2Int coordinate;
    public bool correct;
    Color color;

    public void init(WordSearch word, Vector2Int coord,char letter) {
        textMesh = GetComponentInChildren<TextMesh>();
        wordSearch = word;
        let = letter;
        textMesh.text = let.ToString();
        coordinate = coord;
        correct = false;
        color = Color.black;
    }

    public char letter() {
        return let;
    }


    private void OnMouseDown() {
        wordSearch.onDown(this);
    }
    /*
    public void OnMouseUp() {
        wordSearch.onUp(this);
    }*/

    public void changeColor(Color color) {
        if (!correct) {
            textMesh.color = color;
            this.color = color;
        }
    }

    public void fadeTo(Color color) {
        if (!correct) {
            StartCoroutine(fade(color));
        }
    }

    IEnumerator fade(Color color) {
        yield return new WaitForSecondsRealtime(0.5f);
        float timer = 0;
        this.color = color;
        Color temp = textMesh.color;
        while(timer < 1) {
            textMesh.color = Color.Lerp(temp, color, timer);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
