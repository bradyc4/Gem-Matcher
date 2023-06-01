using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;               // The number of gem sized units in the board's width
    public int height;              // The number of gem sized units in the board's height
    public GameObject[] gemTypes;   // The array that holds the different types of gems
    public GameObject[,] allGems;   // The 2D array that holds references to all the spawned gems on the board
    public List<GameObject> movingGems;
    public List<GameObject> matchingGems;
    public bool isMoving = false;
    public bool isMatching = false;
    public int[] score;
    public int blueScore = 0;
    public int greenScore = 0;
    public int purpleScore = 0;
    public int redScore = 0;
    public int yellowScore = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
        allGems = new GameObject[width, height];    // initializes the allGems array with the specified width and height
        movingGems = new List<GameObject>();
        SetUp();    // Spawns the gem game objects on the board, and fills the 2D array with their reference variables
        FindMatches();
        score = new int[5]{0, 0, 0, 0, 0};
    }

    void Update(){
        if(movingGems.Count!=0){
            isMoving = true;
        } else if(isMoving){
            isMoving = false;
            p("finding matches");
            FindMatches();
            Print2DArray();
        }
        if(matchingGems.Count!=0){
            isMatching = true;
        } else if(isMatching){
            isMatching = false;
            DecreaseRowCo();
        }
    }

    private void SetUp(){
        for(int x=0; x < width; x++){
            for(int y = 0; y < height; y++){

                Vector2 tempPosition = new Vector2(x, y);   // storing the x and y coordinates in a Vector2 object

                int gemToUse = Random.Range(0, gemTypes.Length);    // creating a random int variable to initialize a gem of a random type

                GameObject gem = Instantiate(gemTypes[gemToUse], tempPosition, Quaternion.identity); // initialize a gem object of random type, at the specified coordinates, without rotation
                gem.transform.parent = this.transform;  // set the transform parent of this gem to be the board
                gem.name = "( " + x + ", " + y + " )";  // name this gem using its coordinates on the board
                allGems[x, y] = gem;                    // assign this gem to it's corresponding spot in the 2D array containing all the gems' references
            }
        }
    }

    private void DecreaseRowCo(){
        int nullCount = 0;
        for(int x=0; x < width; x++){
            for(int y=0; y < height; y++){
                if(allGems[x, y] == null){
                    nullCount++;
                } else if(nullCount > 0){
                    allGems[x, y].GetComponent<Gem>().row -= nullCount;
                    allGems [x, y] = null;
                }
            }
            if(nullCount>0){
                for(int y = height; y < height+nullCount; y++){
                    Vector2 tempPosition = new Vector2(x, y);
                    int gemToUse = Random.Range(0, gemTypes.Length);
                    GameObject gem = Instantiate(gemTypes[gemToUse], tempPosition, Quaternion.identity);
                    gem.transform.parent = this.transform;
                    //gem.name = "( " + x + ", " + y + " )";
                    gem.GetComponent<Gem>().row -= nullCount;
                }
            }
            nullCount = 0;
        }
    }

    private void FindMatches(){
        for(int x=0; x < width; x++){
            for(int y = 0; y < height; y++){
                if(allGems[x, y] != null){
                    GameObject gemgem = allGems[x, y];
                    string thisTag = gemgem.tag;
                    GameObject one;
                    GameObject two;
                    if(x < width-2 && allGems[x+1, y] != null && allGems[x+2, y] != null){
                        one = allGems[x+1, y];
                        two = allGems[x+2, y];
                        if(one.CompareTag(thisTag) && two.CompareTag(thisTag)){
                            gemgem.GetComponent<Gem>().isMatched = true;
                            one.GetComponent<Gem>().isMatched = true;
                            two.GetComponent<Gem>().isMatched = true;
                        }
                    }
                    if(y < height-2 && allGems[x, y+1] != null && allGems[x, y+2] != null){
                        one = allGems[x, y+1];
                        two = allGems[x, y+2];
                        if(one.CompareTag(thisTag) && two.CompareTag(thisTag)){
                            gemgem.GetComponent<Gem>().isMatched = true;
                            one.GetComponent<Gem>().isMatched = true;
                            two.GetComponent<Gem>().isMatched = true;
                        }
                    }
                }
            }
        }
    }

    private void DestroyMatchesAt(int x, int y){
        p("ismatched = "+allGems[x, y].GetComponent<Gem>().isMatched);
        if(allGems[x, y].GetComponent<Gem>().isMatched){
            Destroy(allGems[x, y]);
            allGems[x, y] = null;
        }
    }

    public void DestroyMatches(){
        p("about to destroy matches");
        for(int x=0; x<width; x++){
            for(int y=0; y<height; y++){
                if(allGems[x, y] != null){
                    DestroyMatchesAt(x, y);
                }
            }
        }
    }

    void p(object s){
        Debug.Log(s);
    }

    void Print2DArray(){
        string s = "";
        for(int x=0; x < width; x++){
            for(int y = 0; y < height; y++){
                if(allGems[x, y] == null){
                    s = s + "- ";
                } else {
                    s = s + "o ";
                }
            }
            Debug.Log(s);
            s = "";
        }
    }
}






/*
    private void FindMatches(){
        for(int x=0; x < width-2; x++){
            for(int y = 0; y < height-2; y++){
                GameObject gemgem = allGems[x, y];
                GameObject x0y1 = allGems[x, y+1];
                GameObject x0y2 = allGems[x, y+2];
                GameObject x1y0 = allGems[x+1, y];
                GameObject x2y0 = allGems[x+2, y];
                if(gemgem.CompareTag(x0y1.tag) && gemgem.CompareTag(x0y2.tag) && gemgem.CompareTag(x1y0.tag) && gemgem.CompareTag(x2y0.tag)){
                    gemgem.GetComponent<Gem>().isMatched = true;
                    x0y1.GetComponent<Gem>().isMatched = true;
                    x0y2.GetComponent<Gem>().isMatched = true;
                    x1y0.GetComponent<Gem>().isMatched = true;
                    x2y0.GetComponent<Gem>().isMatched = true;
                }
            }
        }
    }
*/