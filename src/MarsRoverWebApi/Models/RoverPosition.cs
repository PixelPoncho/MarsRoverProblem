namespace MarsRoverWebApi.Models
{
  /// Represents the direction/heading a rover is facing
  /// Uses compass points: N (North), E (East), S (South), W (West)
  public enum Direction
  {
    N, // North (positive Y direction)
    E, // East (positive X direction)
    S, // South (negative Y direction)
    W // West (negative X direction)
  }

  /// Represents a rover's current position and heading on the plateau
  public class RoverPosition
  {
    /// X coordinate on the plateau
    public int X { get; set; }

    /// Y coordinate on the plateau
    public int Y { get; set; }

    /// Direction the rover is facing (N, E, S, or W)
    public Direction Direction { get; set; }

    public RoverPosition(int x, int y, Direction direction)
    {
      X = x;
      Y = y;
      Direction = direction;
    }

    /// Creates a copy of this position
    /// Useful for tracking position changes during simulation
    public RoverPosition Clone()
    {
      return new RoverPosition(X, Y, Direction);
    }

    public override string ToString()
    {
      return $"{X} {Y} {Direction}";
    }
  }
}