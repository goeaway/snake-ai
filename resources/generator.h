UTourNumber tourToNumber[ARENA_SIZE];
 
/* Take an x,y coordinate, and turn it into an index in the tour */
TourNumber getPathNumber(Coord x, Coord y) {
  return tourToNumber[x + ARENA_WIDTH*y];
}
 
Distance path_distance(Coord a, Coord b) {
  if(a<b)
    return b-a-1;
  return b-a-1+ARENA_SIZE;
}
 
struct Maze {
  struct Node {
    bool visited:1;
    bool canGoRight:1;
    bool canGoDown:1;
  };
  Node nodes[ARENA_SIZE/4];
  void markVisited(Coord x, Coord y) {
    nodes[x+y*ARENA_WIDTH/2].visited = true;
  }
  void markCanGoRight(Coord x, Coord y) {
    nodes[x+y*ARENA_WIDTH/2].canGoRight = true;
  }
  void markCanGoDown(Coord x, Coord y) {
    nodes[x+y*ARENA_WIDTH/2].canGoDown = true;
  }
  bool canGoRight(Coord x, Coord y) {
    return nodes[x+y*ARENA_WIDTH/2].canGoRight;;
  }
  bool canGoDown(Coord x, Coord y) {
    return nodes[x+y*ARENA_WIDTH/2].canGoDown;
  }
  bool canGoLeft(Coord x, Coord y) {
    if(x==0) return false;
    return nodes[(x-1)+y*ARENA_WIDTH/2].canGoRight;
  }
 
  bool canGoUp(Coord x, Coord y) {
    if(y==0) return false;
    return nodes[x+(y-1)*ARENA_WIDTH/2].canGoDown;
  }
 
  bool isVisited(Coord x, Coord y) {
    return nodes[x+y*ARENA_WIDTH/2].visited;
  }
 
  void generate() {
    memset(nodes, 0, sizeof(nodes));
    generate_r(-1,-1,0,0);
    generateTourNumber();
#ifdef LOG_TO_FILE
    writeMazeToFile();
    writeTourToFile();
#endif
  }
  void generate_r(Coord fromx, Coord fromy, Coord x, Coord y) {
    if(x < 0 || y < 0 || x >= ARENA_WIDTH/2 || y >= ARENA_HEIGHT/2)
      return;
    if(isVisited(x,y))
      return;
    markVisited(x,y);
 
    if(fromx != -1) {
      if(fromx < x)
        markCanGoRight(fromx, fromy);
      else if(fromx > x)
        markCanGoRight(x,y);
      else if(fromy < y)
        markCanGoDown(fromx, fromy);
      else if(fromy > y)
        markCanGoDown(x,y);
 
      //Remove wall between fromx and fromy
    }
 
    /* We want to visit the four connected nodes randomly,
     * so we just visit two randomly (maybe already visited)
     * then just visit them all non-randomly.  It's okay to
     * visit the same node twice */
    for(int i = 0; i < 2; i++) {
      int r = rand()%4;
      switch(r) {
        case 0: generate_r(x, y, x-1, y); break;
        case 1: generate_r(x, y, x+1, y); break;
        case 2: generate_r(x, y, x, y-1); break;
        case 3: generate_r(x, y, x, y+1); break;
      }
    }
    generate_r(x, y, x-1, y);
    generate_r(x, y, x+1, y);
    generate_r(x, y, x, y+1);
    generate_r(x, y, x, y-1);
  }
 
  SnakeDirection findNextDir(Coord x, Coord y, SnakeDirection dir) {
    if(dir == Right) {
      if(canGoUp(x,y))
          return Up;
      if(canGoRight(x,y))
        return Right;
      if(canGoDown(x,y))
        return Down;
      return Left;
    } else if(dir == Down) {
      if(canGoRight(x,y))
          return Right;
      if(canGoDown(x,y))
        return Down;
      if(canGoLeft(x,y))
        return Left;
      return Up;
    } else if(dir == Left) {
      if(canGoDown(x,y))
        return Down;
      if(canGoLeft(x,y))
        return Left;
      if(canGoUp(x,y))
          return Up;
      return Right;
    } else if(dir == Up) {
      if(canGoLeft(x,y))
        return Left;
      if(canGoUp(x,y))
          return Up;
      if(canGoRight(x,y))
        return Right;
      return Down;
    }
    return (SnakeDirection)-1; //Unreachable
  }
  void setTourNumber(Coord x, Coord y, TourNumber number) {
    if(getPathNumber(x,y) != 0)
      return; /* Back to the starting node */
    tourToNumber[x + ARENA_WIDTH*y] = number;
  }
 
  void generateTourNumber() {
    const Coord start_x = 0;
    const Coord start_y = 0;
    Coord x = start_x;
    Coord y = start_y;
    const SnakeDirection start_dir = canGoDown(x,y)?Up:Left;
    SnakeDirection dir = start_dir;
    TourNumber number = 0;
    do {
      SnakeDirection nextDir = findNextDir(x,y,dir);
      switch(dir) {
        case Right:
          setTourNumber(x*2,y*2,number++);
          if(nextDir == dir || nextDir == Down || nextDir == Left)
            setTourNumber(x*2+1,y*2,number++);
          if(nextDir == Down || nextDir == Left)
            setTourNumber(x*2+1,y*2+1,number++);
          if(nextDir == Left)
            setTourNumber(x*2,y*2+1,number++);
          break;
        case Down:
          setTourNumber(x*2+1,y*2,number++);
          if(nextDir == dir || nextDir == Left || nextDir == Up)
            setTourNumber(x*2+1,y*2+1,number++);
          if(nextDir == Left || nextDir == Up)
            setTourNumber(x*2,y*2+1,number++);
          if(nextDir == Up)
            setTourNumber(x*2,y*2,number++);
          break;
        case Left:
          setTourNumber(x*2+1,y*2+1,number++);
          if(nextDir == dir || nextDir == Up || nextDir == Right)
            setTourNumber(x*2,y*2+1,number++);
          if(nextDir == Up || nextDir == Right)
            setTourNumber(x*2,y*2,number++);
          if(nextDir == Right)
            setTourNumber(x*2+1,y*2,number++);
          break;
        case Up:
          setTourNumber(x*2,y*2+1,number++);
          if(nextDir == dir || nextDir == Right || nextDir == Down)
            setTourNumber(x*2,y*2,number++);
          if(nextDir == Right || nextDir == Down)
            setTourNumber(x*2+1,y*2,number++);
          if(nextDir == Down)
            setTourNumber(x*2+1,y*2+1,number++);
          break;
      }
      dir = nextDir;
 
      switch(nextDir) {
        case Right: ++x; break;
        case Left: --x; break;
        case Down: ++y; break;
        case Up: --y; break;
      }
 
    } while(number != ARENA_SIZE); //Loop until we return to the start
  }
#ifdef LOG_TO_FILE
  void writeTourToFile() {
    FILE *f = fopen("maps.txt", "w+");
    for(Coord y = 0; y < ARENA_HEIGHT; ++y) {
      for(Coord x = 0; x < ARENA_WIDTH; ++x)
        fprintf(f, "%4d", getPathNumber(x,y));
      fprintf(f, "\n");
    }
    fclose(f);
  }
  void writeMazeToFile() {
    FILE *f = fopen("maze.txt", "w+");
    for(Coord y = 0; y < ARENA_HEIGHT/2; ++y) {
      fprintf(f, "#");
      for(Coord x = 0; x < ARENA_WIDTH/2; ++x)
        if(canGoRight(x,y) && canGoDown(x,y))
          fprintf(f, "+");
        else if(canGoRight(x,y))
          fprintf(f, "-");
        else if(canGoDown(x,y))
          fprintf(f, "|");
        else
          fprintf(f, " ");
      fprintf(f, "#\n");
    }
    fclose(f);
  }
#endif
};
 
void aiInit() {
  Maze maze;
  maze.generate();
}