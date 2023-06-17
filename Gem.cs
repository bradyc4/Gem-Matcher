using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int column;                  // the column of this gem
    public int row;                     // the row of this gem
    public bool isMatched = false;
    private bool isMoving = false;
    private Animator anim;
    private Board board;                // reference to the game Board script component on the Board gameobject
    private GameObject otherGem;        // reference to the adjacent gem to be swapped
    private Vector2 firstTouchPosition; // the position of the mouse click
    private Vector2 finalTouchPosition; // the position of where the mouse is unclicked

    private Vector2 tempPosition;       // the position the gem will move towards

    public float swipeAngle = 0;        // The angle of the swipe / mouse drag

    
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        board = FindObjectOfType<Board>();  // This finds the board
        column = (int)transform.position.x; // This determines which column the gem is in by getting its x position
        row = (int)transform.position.y;    // row with y position


    }

    void p(object s){
        Debug.Log(s);
    }

    void Update()
    {
        if(isMatched){
            anim.SetTrigger("isMatched");
        }

        if(Mathf.Abs(column - transform.position.x) > 0.1 || Mathf.Abs(row - transform.position.y) > 0.1){ // if the column value and the x value do not match, move the gem until they do
            isMoving = true;
            if(!board.movingGems.Contains(this.gameObject)){
                board.movingGems.Add(this.gameObject);
                board.haltedGems.Add(this.gameObject);
            }
                
            tempPosition = new Vector2(column, row); // This is where the gem should be
            transform.position = Vector2.Lerp(transform.position, tempPosition, .005f); // This moves the position of this gem to the desired position slowly each frame
        } 
        else if(isMoving){
            transform.position = new Vector2(column, row); // This directly sets this gem's position to the desired position
            board.allGems[column, row] = this.gameObject; // This assigns this gem to the new position in the 2D array.
            board.movingGems.Remove(this.gameObject);
            isMoving = false;
        }

    }

    public void AddToMatchesArray(){
        if(this.gameObject.CompareTag("Blue")){
            board.blueScore++;
        } else if(this.gameObject.CompareTag("Green")){
            board.greenScore++;
        } else if(this.gameObject.CompareTag("Purple")){
            board.purpleScore++;
        } else if(this.gameObject.CompareTag("Red")){
            board.redScore++;
        } else if(this.gameObject.CompareTag("Yellow")){
            board.yellowScore++;
        }
        board.matchingGems.Add(this.gameObject);
    }
    
    public void DestroySelf(){
        board.allGems[column, row] = null;
        board.matchingGems.Remove(this.gameObject);
        board.haltedGems.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    private void OnMouseDown(){                     // This is a built in function that detects if the mouse is clicking on a collider associated with this object
        firstTouchPosition = Input.mousePosition;   // Records the mouses position on click
    }
    private void OnMouseUp(){
        finalTouchPosition = Input.mousePosition;   // Records the mouses position on release
        if(!board.isMoving && !board.isMatching){
            CalculateAngle();
        }
    }
    void CalculateAngle(){
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI; // This converts radians to degrees
        MovePieces();
    }
    void MovePieces(){
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width-1){ // checks if it's a right swipe and the column of this gem is not on the right edge
            //Right Swipe
            otherGem = board.allGems[column+1, row];    // gets the gem to the right
            otherGem.GetComponent<Gem>().column -= 1;   // sets the column of that gem 1 leftward
            column += 1;                                // this gems column is set 1 rightward

        } else if(swipeAngle > 45 && swipeAngle <= 135 && row < board.height-1){ // checks if it's an up swipe and the row of this gem is not on the top edge
            //Up Swipe
            otherGem = board.allGems[column, row+1];    // gets the gem above
            otherGem.GetComponent<Gem>().row -= 1;      // sets the row of that gem 1 downward
            row += 1;                                   // this gem's row is set 1 upward

        } else if((swipeAngle > 135 || swipeAngle <= -135) && column > 0){ // checks if it's a left swipe and the row of this gem is not on the left edge
            //Left Swipe
            otherGem = board.allGems[column-1, row];    // gets the gem to the left
            otherGem.GetComponent<Gem>().column += 1;   // sets the column of that gem 1 to the right
            column -= 1;                                // this gem's column is set 1 leftward

        } else if(swipeAngle < -45 && swipeAngle >= -135 && row > 0){ // checks if it's a down swipe and the row of this gem is not on the bottom edge
            //Down Swipe
            otherGem = board.allGems[column, row-1];    // gets the gem below
            otherGem.GetComponent<Gem>().row += 1;      // sets the row of that gem 1 upward
            row -= 1;                                   // this gem's row is set 1 downward

        }
    }
}


/*
    void FindMatches(){
        List<GameObject> matchList = new List<GameObject>();
        matchList.Add(this.gameObject);
        for(int x=column+1; x<board.width; x++){
            GameObject otherGem1 = board.allGems[x, row];
            if(otherGem1.CompareTag(this.tag)){
                matchList.Add(otherGem1);
            }
            else{
                break;
            }
        }
        for(int x=column-1; x>=0; x--){
            GameObject otherGem1 = board.allGems[x, row];
            if(otherGem1.CompareTag(this.tag)){
                matchList.Add(otherGem1);
            }
            else{
                break;
            }
        }
        for(int y=row+1; y<board.height; y++){
            GameObject otherGem1 = board.allGems[column, y];
            if(otherGem1.CompareTag(this.tag)){
                matchList.Add(otherGem1);
            }
            else{
                break;
            }
        }
        for(int y=row-1; y>=0; y--){
            GameObject otherGem1 = board.allGems[column, y];
            if(otherGem1.CompareTag(this.tag)){
                matchList.Add(otherGem1);
            }
            else{
                break;
            }
        }
        if(matchList.Count >= 3){
            for(int i=0; i<matchList.Count; i++){
                matchList[i].GetComponent<Gem>().isMatched = true;
            }
        }
        //if(column > 0 && column < board.width - 1){ GameObject leftGem1 = board.allGems[column-1, row]; }
    }


    void FindMatches(){
        string thisTag = this.gameObject.tag;
        GameObject one;
        GameObject two;
        if(column < board.width-2){
            one = board.allGems[column+1, row];
            two = board.allGems[column+2, row];
            if(one.CompareTag(thisTag) && two.CompareTag(thisTag)){
                isMatched = true;
                one.GetComponent<Gem>().isMatched = true;
                two.GetComponent<Gem>().isMatched = true;
            }
        }
        if(row < board.height-2){
            one = board.allGems[column, row+1];
            two = board.allGems[column, row+2];
            if(one.CompareTag(thisTag) && two.CompareTag(thisTag)){
                isMatched = true;
                one.GetComponent<Gem>().isMatched = true;
                two.GetComponent<Gem>().isMatched = true;
            }
        }
    }

*/