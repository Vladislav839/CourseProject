using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CourseProject.BusinessLogic.Infrastructure;
using CourseProject.BusinessLogic.Interfaces;
using CourseProject.Data;
using CourseProject.Data.Models;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.BusinessLogic.Services
{
    public class StrategyGameService : GameService
    {

        public StrategyGameService(ApplicationContext context, IUsersService service, 
            IWebHostEnvironment appEnvironment, IRandomService randomService)
            : base(context, service, appEnvironment, randomService)
        {

        }

        protected override void GenerateComputerField()
        {
            var fileCount = Directory.EnumerateFiles(Path.Combine(
                                                _appEnvironment.ContentRootPath,
                                                "InitialStategies"), "*.txt", SearchOption.AllDirectories).Count();

            var strategy = _randomService.Next(fileCount);
            var numberOf90DegreeRotations = _randomService.Next(4);

            ComputerField = ReadField(Path.Combine(_appEnvironment.ContentRootPath,
                                                "InitialStategies", $"{strategy}.txt").ToString());

            for(var i = 0; i < numberOf90DegreeRotations; i++)
            {
                rotate90Clockwise(ComputerField);
            }

            PlaceShip(1);
            PlaceShip(1);
            PlaceShip(1);
            PlaceShip(1);

#if DEBUG
            printMatrix(ComputerField);
            Console.WriteLine();
#endif
        }

        public override async Task<(int, int, CellType)> MakeComputerShot(int gameId)
        {
            var weights = ReadField(Path.Combine(_appEnvironment.ContentRootPath,
                               "InitialStategies", "initial_weights.txt").ToString());

            await RecalculateWeight(weights, gameId);

#if DEBUG
            printMatrix(weights);
            Console.WriteLine();
#endif

            var cellsToShoot = GetMaxWeightCells(weights);
            (int row, int col) = cellsToShoot[_randomService.Next(cellsToShoot.Count)];

            MarkedCell cell = await _context.MarkedCells.FirstOrDefaultAsync(c => 
                                                                            c.GameId == gameId && 
                                                                            c.Col == col && 
                                                                            c.Row == row && 
                                                                            c.CellOwner == CellOwner.USER);

            //if (cell.CellType == CellType.HIT || cell.CellType == CellType.MISS)
            //{
            //    return await MakeComputerShot(gameId);
            //}

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

        private int[][] ReadField(string fileName)
        {
            int[][] field = new int[10][];

            for (int i = 0; i < 10; i++)
            {
                field[i] = new int[10];
            }

            string[] rows = File.ReadAllLines(fileName);

            for(var i = 0; i < rows.Length; i++)
            {
                var cells = rows[i].Split(' ');
                for(var j = 0; j < cells.Length; j++)
                {
                    field[i][j] = int.Parse(cells[j]);
                }
            }

            return field;
        }

        private void rotate90Clockwise(int[][] matrix)
        {
            int N = matrix.Length;

            for (int i = 0; i < N / 2; i++)
            {
                for (int j = i; j < N - i - 1; j++)
                {
                    int temp = matrix[i][j];
                    matrix[i][j] = matrix[N - 1 - j][i];
                    matrix[N - 1 - j][i] = matrix[N - 1 - i][N - 1 - j];
                    matrix[N - 1 - i][N - 1 - j] = matrix[j][N - 1 - i];
                    matrix[j][N - 1 - i] = temp;
                }
            }
        }

#if DEBUG
        private void printMatrix(int[][] arr)
        {
            int N = 10;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                    Console.Write(arr[i][j] + " ");
                Console.Write("\n");
            }
        }
#endif

        private async Task RecalculateWeight(int[][] weights, int gameId)
        {
            for(int row = 0; row < 10; row++)
            {
                for(int col = 0; col < 10; col++)
                {
                    MarkedCell cell = await _context.MarkedCells
                                                    .FirstOrDefaultAsync(c => 
                                                    c.GameId == gameId && 
                                                    c.Col == col && 
                                                    c.Row == row && 
                                                    c.CellOwner == CellOwner.USER);

                    if(cell.CellType == CellType.MISS)
                    {
                        weights[row][col] = 0;
                    }

                    if(cell.CellType == CellType.HIT)
                    {
                        weights[row][col] = 0;

                        if(row - 1 >= 0)
                        {
                            if(col - 1 >= 0)
                            {
                                weights[row][col] *= 50;
                            }
                            weights[row - 1][col] *= 50;

                            if(col + 1 < 10)
                            {
                                weights[row - 1][col + 1] = 0;
                            }
                        }

                        if(col - 1 >=  0)
                        {
                            weights[row][col - 1] *= 50;
                        }

                        if(col + 1 < 10)
                        {
                            weights[row][col + 1] *= 50;
                        }

                        if(row + 1 < 10)
                        {
                            if (col - 1 >= 0)
                            {
                                weights[row + 1][col - 1] = 0;
                            }
                            weights[row + 1][col] *= 50;
                            if(col + 1 < 10)
                            {
                                weights[row + 1][col + 1] = 0;
                            }
                        }
                    }
                }
            }
        }

        private List<(int, int)> GetMaxWeightCells(int[][] weights)
        {
            var weightsDict = new Dictionary<int, List<(int, int)>>();

            int maxWeight = 0;

            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    if(weights[row][col] > maxWeight)
                    {
                        maxWeight = weights[row][col];
                    }
                    weightsDict[weights[row][col]] = new List<(int, int)>();
                    weightsDict[weights[row][col]].Add((row, col));
                }
            }

            return weightsDict[maxWeight];
        }
    }
}
