// This class will give static access to the events strings.
public enum TetrisAppNotification
{
    BlockMovementStopped,
    CameraMoveAttempt,
    GridResized,
    OnBlockMoved,
    OnBlockSpawned,
    OnDropClicked,
    OnMoveBlockClicked,
    OnRotateBlockClicked,
    OnSwitchDisplayClicked,
    PrepareMove,
}

public enum TetrisMoveDirections
{
    Left,
    Right,
    Forward,
    Back
}

public enum TetrisRotateDirection
{
    XPositive,
    XNegative,
    YPositive,
    YNegative,
    ZPositive,
    ZNegative
}