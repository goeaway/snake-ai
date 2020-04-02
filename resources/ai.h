SnakeDirection aiGetNewSnakeDirection(Coord x, Coord y) {
  const TourNumber pathNumber = getPathNumber(x,y);
  const Distance distanceToFood = path_distance(pathNumber, getPathNumber(food.x, food.y));
  const Distance distanceToTail = path_distance(pathNumber, getPathNumber(snake.tail_x, snake.tail_y));
  Distance cuttingAmountAvailable = distanceToTail - snake.growth_length - 3 /* Allow a small buffer */;
  const Distance numEmptySquaresOnBoard = ARENA_SIZE - snake.drawn_length - snake.growth_length - food.value;
  // If we don't have much space (i.e. snake is 75% of board) then don't take any shortcuts */
  if (numEmptySquaresOnBoard < ARENA_SIZE / 2)
    cuttingAmountAvailable = 0;
  else if(distanceToFood < distanceToTail) { /* We will eat the food on the way to the tail, so take that into account */
    cuttingAmountAvailable -= food.value;
    /* Once we ate that food, we might end up with another food suddenly appearing in front of us */
    if ((distanceToTail - distanceToFood) * 4 > numEmptySquaresOnBoard) /* 25% chance of another number appearing */
      cuttingAmountAvailable -= 10;
  }
  Distance cuttingAmountDesired = distanceToFood;
  if(cuttingAmountDesired < cuttingAmountAvailable)
    cuttingAmountAvailable = cuttingAmountDesired;
  if(cuttingAmountAvailable < 0)
    cuttingAmountAvailable = 0;
  // cuttingAmountAvailable is now the maximum amount that we can cut by
 
  bool canGoRight = !check_for_collision(x+1, y);
  bool canGoLeft =  !check_for_collision(x-1, y);
  bool canGoDown =  !check_for_collision(x, y+1);
  bool canGoUp =    !check_for_collision(x, y-1);
 
  SnakeDirection bestDir;
  int bestDist = -1;
  if(canGoRight) {
    Distance dist = path_distance(pathNumber, getPathNumber(x+1, y));
    if(dist <= cuttingAmountAvailable && dist > bestDist) {
      bestDir = Right;
      bestDist = dist;
    }
  }
  if(canGoLeft) {
    Distance dist = path_distance(pathNumber, getPathNumber(x-1, y));
    if(dist <= cuttingAmountAvailable && dist > bestDist) {
      bestDir = Left;
      bestDist = dist;
    }
  }
  if(canGoDown) {
    Distance dist = path_distance(pathNumber, getPathNumber(x, y+1));
    if(dist <= cuttingAmountAvailable && dist > bestDist) {
      bestDir = Down;
      bestDist = dist;
    }
  }
  if(canGoUp) {
    Distance dist = path_distance(pathNumber, getPathNumber(x, y-1));
    if(dist <= cuttingAmountAvailable && dist > bestDist) {
      bestDir = Up;
      bestDist = dist;
    }
  }
  if (bestDist >= 0)
    return bestDir;
 
  if (canGoUp)
    return Up;
  if (canGoLeft)
    return Left;
  if (canGoDown)
    return Down;
  if (canGoRight)
    return Right;
  return Right;
}