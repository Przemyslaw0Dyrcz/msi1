using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShoppingOptimizer
{
    public partial class MainWindow : Window
    {
        private Store _store;
        private GeneticAlgorithm _geneticAlgorithm;

        public MainWindow()
        {
            InitializeComponent();
            _store = new Store();
            _geneticAlgorithm = new GeneticAlgorithm();
            LoadArticles();
        }

        private void LoadArticles()
        {
            foreach (var article in _store.Articles)
            {
                ArticleListBox.Items.Add($"{article.Name} ({article.Weight}kg)");
            }
        }

        private void OptimizePath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedArticles = ArticleListBox.SelectedItems
                    .Cast<string>()
                    .Select(item => _store.Articles.First(a => item.StartsWith(a.Name)))
                    .ToList();

                if (selectedArticles.Count == 0)
                {
                    MessageBox.Show("Please select at least one article.");
                    return;
                }

                var optimalPath = _geneticAlgorithm.FindOptimalPath(_store, selectedArticles);
                if (optimalPath != null && optimalPath.Count > 0)
                {
                    DrawStore(optimalPath);
                }
                else
                {
                    MessageBox.Show("Failed to find an optimal path.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void DrawStore(List<(int X, int Y)> path)
        {
            StoreCanvas.Children.Clear();

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    var rect = new Rectangle
                    {
                        Width = 50,
                        Height = 50,
                        Stroke = Brushes.Black
                    };

                    if (_store.CashRegister == (x, y))
                    {
                        rect.Stroke = Brushes.Yellow;
                        rect.Fill = Brushes.Yellow;
                    }

                    var step = path.IndexOf((x, y)) + 1;
                    if (step > 0)
                    {
                        rect.Fill = Brushes.Green;
                    }

                    if (_store.Articles.Any(a => a.Location == (x, y)))
                    {
                        var article = _store.Articles.First(a => a.Location == (x, y));
                        var text = new TextBlock
                        {
                            Text = article.Name,
                            Foreground = Brushes.Black,
                            FontSize = 12,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        Canvas.SetLeft(text, x * 50 + 5);
                        Canvas.SetTop(text, y * 50 + 15);
                        StoreCanvas.Children.Add(text);

                        if (path.Contains((x, y)))
                        {
                            rect.Fill = Brushes.Purple;
                        }
                        else
                        {
                            rect.Stroke = Brushes.Red;
                        }
                    }

                    Canvas.SetLeft(rect, x * 50);
                    Canvas.SetTop(rect, y * 50);
                    StoreCanvas.Children.Add(rect);
                }
            }

            // Draw the path steps
            for (int i = 0; i < path.Count; i++)
            {
                var (x, y) = path[i];
                var text = new TextBlock
                {
                    Text = (i + 1).ToString(),
                    Foreground = Brushes.White,
                    FontSize = 20,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Canvas.SetLeft(text, x * 50 + 15);
                Canvas.SetTop(text, y * 50 + 10);
                StoreCanvas.Children.Add(text);
            }
        }
    }
}
