using System;
using System.Collections.Generic;
using System.Linq;

public class GeneticAlgorithm
{
    private readonly Random _random = new Random();
    private const int PopulationSize = 100;
    private const int Generations = 1000;
    private const double MutationRate = 0.01;

    public List<(int X, int Y)> FindOptimalPath(Store store, List<Article> shoppingList)
    {
        var population = InitializePopulation(shoppingList);

        for (int generation = 0; generation < Generations; generation++)
        {
            var newPopulation = new List<List<Article>>();

            for (int i = 0; i < PopulationSize / 2; i++)
            {
                var parent1 = Select(population, store);
                var parent2 = Select(population, store);
                var offspring1 = Crossover(parent1, parent2);
                var offspring2 = Crossover(parent2, parent1);

                Mutate(offspring1);
                Mutate(offspring2);

                newPopulation.Add(offspring1);
                newPopulation.Add(offspring2);
            }

            population = newPopulation;
        }

        var bestIndividual = population.OrderBy(ind => CalculateFitness(ind, store)).First();
        return DecodePath(bestIndividual, store);
    }

    private List<List<Article>> InitializePopulation(List<Article> shoppingList)
    {
        var population = new List<List<Article>>();

        for (int i = 0; i < PopulationSize; i++)
        {
            var individual = shoppingList.OrderBy(x => _random.Next()).ToList();
            population.Add(individual);
        }

        return population;
    }

    private List<Article> Select(List<List<Article>> population, Store store)
    {
        var tournament = population.OrderBy(x => _random.Next()).Take(5).ToList();
        return tournament.OrderBy(ind => CalculateFitness(ind, store)).First();
    }

    private List<Article> Crossover(List<Article> parent1, List<Article> parent2)
    {
        var crossoverPoint = _random.Next(parent1.Count);
        var offspring = new List<Article>();

        offspring.AddRange(parent1.Take(crossoverPoint));
        offspring.AddRange(parent2.Where(x => !offspring.Contains(x)));

        return offspring;
    }

    private void Mutate(List<Article> individual)
    {
        if (_random.NextDouble() < MutationRate)
        {
            var index1 = _random.Next(individual.Count);
            var index2 = _random.Next(individual.Count);

            var temp = individual[index1];
            individual[index1] = individual[index2];
            individual[index2] = temp;
        }
    }

    private double CalculateFitness(List<Article> individual, Store store)
    {
        double totalDistance = 0;
        var currentLocation = store.Entrance;

        foreach (var article in individual)
        {
            totalDistance += GetManhattanDistance(currentLocation, article.Location);
            currentLocation = article.Location;
        }

        totalDistance += GetManhattanDistance(currentLocation, store.CashRegister);
        return totalDistance;
    }

    private int GetManhattanDistance((int X, int Y) from, (int X, int Y) to)
    {
        return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
    }

    private List<(int X, int Y)> DecodePath(List<Article> individual, Store store)
    {
        var path = new List<(int X, int Y)> { store.Entrance };
        var occupiedLocations = store.Articles.Select(a => a.Location).ToList();
        var currentLocation = store.Entrance;

        foreach (var article in individual)
        {
            var articleLocation = article.Location;
            var segmentPath = GetPath(currentLocation, articleLocation, occupiedLocations);

            path.AddRange(segmentPath);
            currentLocation = articleLocation;
            occupiedLocations.Remove(articleLocation);
        }

        var cashRegisterLocation = store.CashRegister;
        var lastSegmentPath = GetPath(currentLocation, cashRegisterLocation, occupiedLocations);

        path.AddRange(lastSegmentPath);

        return path;
    }



    private List<(int X, int Y)> GetPath((int X, int Y) start, (int X, int Y) end, List<(int X, int Y)> occupiedLocations)
    {
        var path = new List<(int X, int Y)>();
        var current = start;

        while (current.X != end.X || current.Y != end.Y)
        {
            if (current.X != end.X)
            {
                var nextX = current.X < end.X ? current.X + 1 : current.X - 1;
                var nextStep = (nextX, current.Y);

                if (!occupiedLocations.Contains(nextStep))
                {
                    current = nextStep;
                    path.Add(current);
                }
                else
                {
                    if (current.Y != end.Y)
                    {
                        var nextY = current.Y < end.Y ? current.Y + 1 : current.Y - 1;
                        nextStep = (current.X, nextY);

                        if (!occupiedLocations.Contains(nextStep))
                        {
                            current = nextStep;
                            path.Add(current);
                        }
                        else
                        {
                            return path;
                        }
                    }
                    else
                    {
                        return path;
                    }
                }
            }
            else if (current.Y != end.Y)
            {
                var nextY = current.Y < end.Y ? current.Y + 1 : current.Y - 1;
                var nextStep = (current.X, nextY);

                if (!occupiedLocations.Contains(nextStep))
                {
                    current = nextStep;
                    path.Add(current);
                }
                else
                {
                    if (current.X != end.X)
                    {
                        var nextX = current.X < end.X ? current.X + 1 : current.X - 1;
                        nextStep = (nextX, current.Y);

                        if (!occupiedLocations.Contains(nextStep))
                        {
                            current = nextStep;
                            path.Add(current);
                        }
                        else
                        {
                            return path;
                        }
                    }
                    else
                    {
                        return path;
                    }
                }
            }
        }

        return path;
    }

}
