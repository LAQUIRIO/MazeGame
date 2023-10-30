namespace Maze
{
    public class Map : IMap
    {
        
        private readonly Random _rand = new ();
        private readonly IMapProvider _mapProvider;
        private Direction[,] _directionMap;
        public IMapVector Goal { get; private set; }
        public int Height { 
            get { 
                return MapGrid.GetLength(0);
            }
        }
        public bool IsGameFinished {get { return Goal.Equals(Player.Position); }}
        public Block[,] MapGrid { get; private set; }
        public IPlayer Player { get; private set; }
        public int Width
        {
            get
            {
                return MapGrid.GetLength(1);
            }
        }

        public Map(IMapProvider mapProvider)
        {
            _mapProvider = mapProvider;
            MapGrid = new Block[0, 0];
        }
        private void PlacePlayer()
        {
            int y=0, x =0;
            while (MapGrid[y,x]==Block.Solid)
            {
                y = _rand.Next(0, MapGrid.GetLength(0));
                x = _rand.Next(0, MapGrid.GetLength(1));

            }
            Player = new Player(MapGrid,Direction.N,x,y);

        }
        private void SelectGoal()
        {
            MapVector goal = new MapVector();
            double maxLength = -1;
            for (int i = 0; i < _directionMap.GetLength(0); i++)
            {
                for (int j = 0;  j < _directionMap.GetLength(1); j++)
                {
                    Direction dirs = _directionMap[i, j];

                    if (dirs == Direction.N || dirs == Direction.E || dirs == Direction.S || dirs == Direction.W)
                    {
                        MapVector g = new MapVector(DirecToGrid(j), DirecToGrid(i));
                        if(maxLength < (Player.Position - g).Magnitude())
                        {
                            maxLength = (Player.Position - g).Magnitude();
                            goal = new MapVector(DirecToGrid(j), DirecToGrid(i));
                        }
                    }
                }
            }
            Goal = goal;
            
        }

        private void Initializer()
        {
            PopulateMap();
            PlacePlayer();
            SelectGoal();

        }

        public void CreateMap()
        {
            _directionMap = _mapProvider.CreateMap();
            MapGrid = new Block[_directionMap.GetLength(0) * 2 + 1, _directionMap.GetLength(1) * 2 + 1];
            Initializer();
        }

        public void CreateMap(int width, int height)
        {
            _directionMap = _mapProvider.CreateMap(width, height);
            MapGrid = new Block[_directionMap.GetLength(0) * 2 + 1, _directionMap.GetLength(1) * 2 + 1];
            Initializer();
        }

        private void PopulateMap()
        {
            int LengthY = _directionMap.GetLength(0);
            int LengthX = _directionMap.GetLength(1);
            for (int y = 0; y < LengthY; y++)
            {
                for (int x = 0; x < LengthX; x++)
                {
                    if (_directionMap[y, x] != Direction.None)
                    {
                        MapGrid[DirecToGrid(y), DirecToGrid(x)] = Block.Empty;
                    }
                    //if path can go South and it wont reach the outside wall
                    if (LengthY - 1 > y && (_directionMap[y, x] & Direction.S) > 0)
                    {
                        MapGrid[y * 2 + 2, DirecToGrid(x)] = Block.Empty;
                    }
                    //if path can go East and it wont reach the outside wall
                    if (LengthX - 1 > x && (_directionMap[y, x] & Direction.E) > 0)
                    {
                        MapGrid[DirecToGrid(y), x * 2 + 2] = Block.Empty;
                    }
                }
            }
        }

        private static int DirecToGrid(int v)
        {
            return v * 2 + 1;
        }

        public void SaveDirectionMap(string path)
        {   
            throw new NotImplementedException();
        }
    }
}
