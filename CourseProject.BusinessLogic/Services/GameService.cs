using CourseProject.BusinessLogic.Interfaces;
using CourseProject.Data;
using CourseProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BusinessLogic.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationContext _context;
        private readonly IUsersService _userService;
        public int[][] ComputerField { get; private set; } = new int[10][];
        public int[][] UserField { get; private set; }

        public GameService(ApplicationContext context, IUsersService service)
        {
            _context = context;
            _userService = service;

            for (int i = 0; i < 10; i++)
            {
                ComputerField[i] = new int[10];
            }
        }

        public async Task<int> CreateGame(int[][] field, string userName)
        {
            this.UserField = field;
            GenerateComputerField();

            Game game = new Game { UserId = _userService.GetUserByUserName(userName).Id };
            _context.Games.Add(game);


            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    if(UserField[i][j] == 0)
                    {
                        MarkedCell cellUser = new MarkedCell
                        {
                            CellOwner = CellOwner.USER,
                            CellType = CellType.EMPTY,
                            Row = i,
                            Col = j,
                            Game = game
                        };


                        _context.Add(cellUser);
                    }
                    else
                    {
                        MarkedCell cellUser = new MarkedCell
                        {
                            CellOwner = CellOwner.USER,
                            CellType = CellType.SHIP,
                            Row = i,
                            Col = j,
                            Game = game
                        };

                        _context.Add(cellUser);
                       
                    }
                }
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (ComputerField[i][j] == 0)
                    {
                        MarkedCell cellComputer = new MarkedCell
                        {
                            CellOwner = CellOwner.COMPUTER,
                            CellType = CellType.EMPTY,
                            Row = i,
                            Col = j,
                            Game = game
                        };


                        _context.Add(cellComputer);
                    }
                    else
                    {
                        MarkedCell cellComputer = new MarkedCell
                        {
                            CellOwner = CellOwner.COMPUTER,
                            CellType = CellType.SHIP,
                            Row = i,
                            Col = j,
                            Game = game
                        };

                        _context.Add(cellComputer);

                    }
                }
            }



            await _context.SaveChangesAsync();

            return game.Id;
        }

        public async Task<int[][]> GetComputerField(int gameId)
        {
            int[][] field = new int[10][];
            for(int i = 0; i < 10; i++)
            {
                field[i] = new int[10];
            }

            List<MarkedCell> computerCells = await _context.MarkedCells
                .Where(c => c.GameId == gameId && c.CellOwner == CellOwner.COMPUTER).ToListAsync();

            foreach(var cell in computerCells)
            {
                field[cell.Row][cell.Col] = (int)cell.CellType;
            }

            return field;

        }

        public async Task<int[][]> GetUserField(int gameId)
        {
            int[][] field = new int[10][];
            for (int i = 0; i < 10; i++)
            {
                field[i] = new int[10];
            }

            List<MarkedCell> computerCells = await _context.MarkedCells
                .Where(c => c.GameId == gameId && c.CellOwner == CellOwner.USER).ToListAsync();

            foreach (var cell in computerCells)
            {
                field[cell.Row][cell.Col] = (int)cell.CellType;
            }

            return field;
        }

        private void GenerateComputerField()
        {

            PlaceShip(4);
            PlaceShip(3);
            PlaceShip(3);
            PlaceShip(2);
            PlaceShip(2);
            PlaceShip(2);
            PlaceShip(1);
            PlaceShip(1);
            PlaceShip(1);
            PlaceShip(1);

        }

        private (int, int, int) GetCoordinates(int size)
        {
            Random random = new Random();
            int dir = random.Next(0, 2);
            int x;
            int y;

            if (dir == 0)
            {
                x = random.Next(0, 10);
                y = random.Next(0, 10 - size);
            }
            else
            {
                x = random.Next(0, 10 - size);
                y = random.Next(0, 10);
            }

            bool res = CheckRules(x, y, dir, size);

            if (!res) return GetCoordinates(size);

            return (x, y, dir);
        }

        private void PlaceShip(int size)
        {
            var coordinates = GetCoordinates(size);
            if(coordinates.Item3 == 0)
            {
                for(int i = coordinates.Item2; i < coordinates.Item2 + size; i++)
                {
                    ComputerField[coordinates.Item1][i] = 1;
                }
            }
            else
            {
                for (int i = coordinates.Item1; i < coordinates.Item1 + size; i++)
                {
                    ComputerField[i][coordinates.Item2] = 1;
                }
            }
        }

        private bool CheckRules(int startRow, int startCol, int dir, int size)
        {

            int fromX = 0, toX = -1, fromY = 0, toY = -1;

            fromX = (startRow == 0) ? startRow : startRow - 1;

            if (startRow + size == 10 && dir == 1) toX = startRow + size;
            else if(startRow + size < 10 && dir == 1) toX = startRow + size + 1;
            else if(startRow == 9 && dir == 0) toX = startRow + 1;
            else if (startRow < 9 && dir == 0) toX = startRow + 2;

            fromY = (startCol == 0) ? startCol : startCol - 1;
            if (startCol + size == 10 && dir == 0) toY = startCol + size;
            else if (startCol + size < 10 && dir == 0) toY = startCol + size + 1;
            else if (startCol == 9 && dir == 1) toY = startCol + 1;
            else if (startCol < 9 && dir == 1) toY = startCol + 2;

            if (toX == -1 || toY == -1) return false;

            for (int i = fromX; i < toX; i++)
            {
                for(int j = fromY; j < toY; j++)
                {
                    if (ComputerField[i][j] == 1) return false;
                }
            }

            return true;
        }

        public async Task<CellType> MakeUsersShot(int col, int row, int gameId, CellOwner owner)
        {
            MarkedCell cell = await _context.MarkedCells
                .FirstOrDefaultAsync(c => c.GameId == gameId && c.Col == col && c.Row == row && c.CellOwner == owner);

            if(cell != null)
            {
                if(cell.CellType == CellType.EMPTY)
                {
                    cell.CellType = CellType.MISS;
                }
                else if(cell.CellType == CellType.SHIP)
                {
                    cell.CellType = CellType.HIT;
                    Game game = await _context.Games.FindAsync(gameId);
                    ++game.UserHits;
                }
            }

            await _context.SaveChangesAsync();

            return cell.CellType;
        }

        public async Task<(int, int, CellType)> MakeComputerShot(int gameId)
        {
            Random random = new Random();
            int col = random.Next(0, 10);
            int row = random.Next(0, 10);

            MarkedCell cell = await _context.MarkedCells
                .FirstOrDefaultAsync(c => c.GameId == gameId && c.Col == col && c.Row == row && c.CellOwner == CellOwner.USER);

            if(cell.CellType == CellType.HIT || cell.CellType == CellType.MISS)
            {
                return await MakeComputerShot(gameId);
            }

            if (cell.CellType == CellType.EMPTY)
            {
                cell.CellType = CellType.MISS;
            }
            else if (cell.CellType == CellType.SHIP)
            {
                cell.CellType = CellType.HIT;
                Game game = await _context.Games.FindAsync(gameId);
                ++game.ComputerHits;
            }

            await _context.SaveChangesAsync();

            return (cell.Col, cell.Row, cell.CellType);
        }

        public async Task<(bool, string)> CheckWinner(int gameId)
        {
            Game game = await _context.Games.FindAsync(gameId);
            if(game.UserHits == 20)
            {
                return (true, "User");
            } 
            else if(game.ComputerHits == 20)
            {
                return (true, "Computer");
            }
            else
            {
                return (false, null);
            }
        }
    }
}
