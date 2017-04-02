using UnityEngine;
using System.Collections;

public class HexaEntity : MonoBehaviour
{
    public Snake snake;
    public HexaDirection hexaDirection;

    [Header("Auto")]
    public EntityPhysics entityPhysics;

    [Space(15)]
    public HexaGridPosition positionCurrent;
    public HexaGridPosition positionPrevious;
    public HexaGridPosition positionNext;

    [Space(5)]
    public HexaTile tileCurrent;
    //public HexaTile tilePrevious;
    public HexaTile tileNext;

    [Space(5)]
    public Direction directionPrevious;

    [Space(15)]
    public Speed speed;
    public int speedChangeTileDuration;

    [Header("Frame Logic")]
    public bool turnLeft;
    public bool turnRight;
    public bool hasUpdated;

    [Header("Debug")]
    public string debugMessage;
    private KeyCode keyLeft = KeyCode.LeftArrow;
    private KeyCode keyRight = KeyCode.RightArrow;
    private KeyCode keyGrow = KeyCode.Space;
    public bool markedForDeletion;

    Rect leftZone;
    Rect rightZone;

    public float timeBeforeStart;


    void Awake()
    {
        timeBeforeStart = 3;
    }

    void Start()
    {
        GameController.instance.RegisterEntity(this);

        if (snake != null) snake.Init(this);

        positionNext.Copy(positionCurrent);
        positionPrevious.Copy(positionCurrent);


        if (Application.platform == RuntimePlatform.Android)
        {
            float w = Screen.width;
            float h = Screen.height;
            float centerX = w / 2f;
            leftZone = new Rect(0, 0, centerX - 10, h);
            rightZone = new Rect(centerX + 10, 0, centerX - 10, h);
        }
    }

    /*void OnGUI(){
		if (timeBeforeStart > 0){
			if(GUI.Button(new Rect(0,0,150,100),"PAUSE")){
				timeBeforeStart = 0;
			}
		}
	}*/

    void Update() {
        if (markedForDeletion) return;
        
        if (timeBeforeStart > 0) {
            timeBeforeStart -= Time.deltaTime;
            GameBoardUI.main.SetDecompteTextActive(true);
            GameBoardUI.main.SetDecompteText(timeBeforeStart);
            if (timeBeforeStart < 0) {
                timeBeforeStart = 0;
                GameBoardUI.main.SetDecompteTextActive(false);
            }
            
            return;
        }


        hasUpdated = true;

#if UNITY_ANDROID && !UNITY_EDITOR
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Began) {
                if (!turnLeft && leftZone.Contains(touch.position)) {
                    turnLeft = true;
                }
                if (!turnRight && rightZone.Contains(touch.position)) {
                    turnRight = true;
                }
            }
        }
#else
        if (!turnLeft && Input.GetKeyDown(keyLeft)) {
            turnLeft = true;
        }
        if (!turnRight && Input.GetKeyDown(keyRight)) {
            turnRight = true;
        }
        if (snake && Input.GetKeyDown(keyGrow)) {
            snake.Grow(1);
        }

#endif
    }

    void FixedUpdate()
    {
        if (markedForDeletion) return; //TODO : pause around here too
        if (timeBeforeStart > 0) return;


        speed.IncreaseStep();

        if (speed.StepIsAtMax)
        {
            speed.ResetStep();

            if (!positionPrevious.HasSameValues(positionCurrent) && !positionCurrent.HasSameValues(positionNext))
            {
                positionPrevious.Copy(positionCurrent);
                directionPrevious = hexaDirection.direction;
            }

            positionCurrent.Copy(positionNext);

            if (turnLeft)
            {
                turnLeft = false;
                turnRight = false;
                hexaDirection.TurnLeft();
            }
            if (turnRight)
            {
                turnRight = false;
                hexaDirection.TurnRight();
            }

            RetrieveNextPosition();

            if (CanMove())
            {
                if (snake)
                {
                    snake.DisplaceChain();
                    snake.block = 0;

                    if (speedChangeTileDuration > 0)
                    {
                        speedChangeTileDuration--;
                        if (speedChangeTileDuration == 0) speed.type = Speed.Type.Normal;
                    }
                }
            }
            else
            {
                positionNext.Copy(positionCurrent);//rewind
                speed.StepToMax();

                if (snake && snake.block == 0) snake.Block();
            }

        }

        if (markedForDeletion) return;//quick fix, will change later if needed
        entityPhysics.Move(positionCurrent, positionNext, speed.StepSpeedRatio);
        entityPhysics.Rotate(hexaDirection.direction);

        if (snake) snake.UpdateChain();

        hasUpdated = false;
    }


    public void SetGridPosition(HexaGridPosition position)
    {
        positionCurrent.Copy(position);
        positionPrevious.Copy(position);
        positionNext.Copy(position);
    }
    public void SetDirection(Direction direction)
    {
        hexaDirection.direction = direction;
        directionPrevious = direction;
    }


    private void RetrieveNextPosition()
    {
        tileCurrent = Level.instance.GetTileAt(positionCurrent);

        positionNext.Copy(positionCurrent);
        positionNext.Move(hexaDirection.direction);

        if (tileCurrent.elevation[(int)hexaDirection.direction] == HexaTile.Elevation._1) positionNext.y += 1;
        if (tileCurrent.elevation[(int)hexaDirection.direction] == HexaTile.Elevation._2) positionNext.y += 2;

        tileNext = Level.instance.GetTileAt(positionNext);
    }

    private bool CanMove()
    {
        tileCurrent = Level.instance.GetTileAt(positionCurrent);

        // Can we exit tile from our direction?
        HexaTile.Property currentProperty = tileCurrent.property[(int)hexaDirection.direction];
        if (currentProperty == HexaTile.Property.Blocked) { debugMessage = "Current: Blocked"; return false; }
        if (currentProperty == HexaTile.Property.EntryOnly) { debugMessage = "Current: OnlyEntry"; return false; }

        //HexaTile.Elevation currentElevation = tileCurrent.elevation[(int)hexaDirection.direction];


        // Is next tile valid?
        if (tileNext == null)
        {
            HexaTile.Elevation slopeDownElevation;

            // Slope down -> Elevation -1
            positionNext.y--;
            tileNext = Level.instance.GetTileAt(positionNext);

            if (tileNext == null)
            {
                positionNext.y--;
                tileNext = Level.instance.GetTileAt(positionNext);

                if (tileNext == null) { debugMessage = "Next: NULL | Down -1 -> -2: No slope found (NULL)"; return false; }

                slopeDownElevation = tileNext.elevation[(int)hexaDirection.Opposite];
                if (slopeDownElevation == HexaTile.Elevation._2) return true;

                debugMessage = "Next: NULL | Down -2: slope cannot be accessed from there";
                return false;
            }

            slopeDownElevation = tileNext.elevation[(int)hexaDirection.Opposite];
            if (slopeDownElevation == HexaTile.Elevation._1) return true;

            debugMessage = "Next: NULL | Down -1: slope cannot be accessed from there";
            return false;
        }

        // Can we enter next tile from our direction?
        HexaTile.Property nextProperty = tileNext.property[(int)hexaDirection.Opposite];
        if (nextProperty == HexaTile.Property.Blocked) { debugMessage = "Next: Blocked"; return false; }
        if (nextProperty == HexaTile.Property.ExitOnly) { debugMessage = "Next: OnlyExit"; return false; }
        //if (nextEntryPoint == HexaTile.EntryPoint.Blocked || nextEntryPoint == HexaTile.EntryPoint.OnlyExit) return false;

        return true;
    }


    //Might need it later
    public void TimeOut()
    {
        Destroy();
    }

    public void Destroy()
    {
        if (markedForDeletion) return; markedForDeletion = true;

        GameController.instance.UnRegisterEntity(this);
        CameraFollow.main.StopFollowing();
        if (snake != null) snake.Destroy();
        Destroy(gameObject);
    }
}
