using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SearchAlgorithms; 
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window {

    public Int32 gridSize = 3;
    public ObservableCollection<string> BFSTiles = new ObservableCollection<string>();

    public MainWindow() {
        InitializeComponent();

        SetupGrids();
    }

    private void SetupGrids() {
        List<string> map = new List<string>() {
            "#####",
            "#...#",
            "#####",
        };
        var cols = map.First().Length;
        var rows = map.Count();
        gridSize = cols;

        GridView[] grids = new GridView[4] {
            BFSGrid,
            DFSGrid,
            GreedyGrid,
            AGrid
        };

        //Rectangle wall = new Rectangle();
        //wall.Width = 30;
        //wall.Height = 30;
        var wallColor = new SolidColorBrush(Color.FromArgb(1,255,255,0));
    }
}
