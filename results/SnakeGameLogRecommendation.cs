public override void CollectObservations(VectorSensor sensor)
{
    // Relative Position to Food (2 observations: X and Y differences)
    Vector2 foodDirection = currentFoodInstance.position - transform.position;
    sensor.AddObservation(foodDirection.normalized);

    // Distance to Walls (4 observations: Up, Down, Left, Right)
    sensor.AddObservation(Vector2.Distance(transform.position, borderTop.position));    // Up
    sensor.AddObservation(Vector2.Distance(transform.position, borderBottom.position)); // Down
    sensor.AddObservation(Vector2.Distance(transform.position, borderLeft.position));   // Left
    sensor.AddObservation(Vector2.Distance(transform.position, borderRight.position));  // Right

    // Directional Tail and Wall Detection (4 observations for each direction)
    // This assumes you have a method to check for obstacles in a given direction and return a normalized distance
    sensor.AddObservation(DetectObstacle(Direction.Up));
    sensor.AddObservation(DetectObstacle(Direction.Down));
    sensor.AddObservation(DetectObstacle(Direction.Left));
    sensor.AddObservation(DetectObstacle(Direction.Right));
}

private float DetectObstacle(Direction direction)
{
    // Placeholder for actual detection logic
    // This should raycast or otherwise detect distance to the nearest obstacle (tail segment or wall) in the given direction
    // Normalize or scale the distance appropriately for your environment
    // For example: return 0 if an obstacle is immediately adjacent, or 1 if no obstacles are in the immediate vicinity
    return 0f; // Implement detection logic based on your game's specifics
}
