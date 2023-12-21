using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SearchAlgorithms; 
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window {
    public ObservableCollection<string> BFSTiles = new ObservableCollection<string>();
    public ObservableCollection<string> DFSTiles = new ObservableCollection<string>();
    public ObservableCollection<string> GreedyTiles = new ObservableCollection<string>();
    public ObservableCollection<string> ATiles = new ObservableCollection<string>();

    //private List<string> map = new List<string>() {
    //        "#########",
    //        "#S......#",
    //        "#.#####.#",
    //        "#..#.##.#",
    //        "##.#....#",
    //        "#...###.#",
    //        "#.#...#T#",
    //        "#########",
    //    };

    //private List<string> map = new List<string>() {
    //    "###########################",
    //    "#....#....................#",
    //    "#.##S####################.#",
    //    "#.#.##...#......#...#...#.#",
    //    "#......#...####...#...#.#.#",
    //    "#######################.#.#",
    //    "#.........................#",
    //    "#.##################.######",
    //    "###.......................#",
    //    "#T#.#####################.#",
    //    "#.#.#.....................#",
    //    "#.##.##.########.##########",
    //    "#.............#...........#",
    //    "###########################",
    //};

    //private List<string> map = new List<string>() {
    //    "###########################",
    //    "#....#....................#",
    //    "#.##S.....................#",
    //    "#.#.##...#......#...#...#.#",
    //    "#......#...####...#...#.#.#",
    //    "#.........................#",
    //    "#.........................#",
    //    "#.##.########......#.######",
    //    "###.......................#",
    //    "#T#.......................#",
    //    "#.#.#.....................#",
    //    "#.#.......................#",
    //    "#.............#...........#",
    //    "###########################",
    //};

    private List<string> map = new List<string>() {
        "#..........T",
        "#.##########",
        "#.#.......#.",
        "#.#.#####.#.",
        "#...#.....#.",
        "###.#.#####.",
        "S...#.......",
    };

    private Point terminal {
        get {
            var p = new Point(0, 0);
            for (int i=0; i<map.Count; i++) {
                for (int j=0; j<map[i].Length; j++) {
                    if (map[i][j] == 'T') p = new Point(j, i);
                }
            }

            return p;
        }
    }


    public MainWindow() {
        InitializeComponent();

        SetupGrids();
    }

    private void SetupGrids() {
        var cols = map.First().Length;
        var rows = map.Count();

        GridView[] grids = new GridView[4] {
            BFSGrid,
            DFSGrid,
            GreedyGrid,
            AGrid
        };

        foreach (var row in map) {
            foreach (var col in row) {
                switch (col) {
                    case '.':
                        BFSTiles.Add("LightGray");
                        DFSTiles.Add("LightGray");
                        GreedyTiles.Add("LightGray");
                        ATiles.Add("LightGray");
                        break;
                    case 'S':
                        BFSTiles.Add("Green");
                        DFSTiles.Add("Green");
                        GreedyTiles.Add("Green");
                        ATiles.Add("Green");
                        break;
                    case 'T':
                        BFSTiles.Add("Red");
                        DFSTiles.Add("Red");
                        GreedyTiles.Add("Red");
                        ATiles.Add("Red");
                        break;
                    default:
                        BFSTiles.Add("DarkGray");
                        DFSTiles.Add("DarkGray");
                        GreedyTiles.Add("DarkGray");
                        ATiles.Add("DarkGray");
                        break;
                }
            }
        }
    }

    private async void Start(object sender, RoutedEventArgs e) {
        while (true) {
            var dfs = new SearchAlg(CloneMap(), new FrontierStack<Node>());
            var bfs = new SearchAlg(CloneMap(), new FrontierQueue<Node>());
            var greedy = new SearchAlg(CloneMap(), new FrontierPriorityQueue<Node>(n => {
                var xdiff = Math.Abs(n.p.x - terminal.x);
                var ydiff = Math.Abs(n.p.y - terminal.y);

                return xdiff + ydiff;
            }));
            var astar = new SearchAlg(CloneMap(), new FrontierPriorityQueue<Node>(n => {
                var xdiff = Math.Abs(n.p.x - terminal.x);
                var ydiff = Math.Abs(n.p.y - terminal.y);

                return (xdiff + ydiff) + n.steps;
            }));

            var algs = new List<SearchAlg>() { bfs, dfs, greedy };

            while (algs.Any(x => x.algState == State.Pending)) {
                if (bfs.algState == State.Pending)
                    Process(bfs, BFSTiles);

                if (dfs.algState == State.Pending)
                    Process(dfs, DFSTiles);

                if (greedy.algState == State.Pending)
                    Process(greedy, GreedyTiles);

                if (astar.algState == State.Pending)
                    Process(astar, ATiles);

                await Task.Delay(50);
            }
            await Task.Delay(1000);
        }
    }

    private void Process(SearchAlg alg, ObservableCollection<string> ui) {
        var newState = alg.GetNextState();

        // Update UI
        for (int i=0; i<newState.Count; i++) {
            for (int j=0; j<newState[i].Length; j++) {
                var pos = i * newState[i].Length + j;
                switch (newState[i][j]) {
                    case '.':
                        if (ui[pos] != "LightGray") ui[pos] = "LightGray";
                        break;
                    case 'S':
                        if (ui[pos] != "Green") ui[pos] = "Green";
                        break;
                    case 'T':
                        if (ui[pos] != "Red") ui[pos] = "Red";
                        break;
                    case 'E':
                        if (ui[pos] != "Blue") ui[pos] = "Blue";
                        break;
                    default:
                        if (ui[pos] != "DarkGray") ui[pos] = "DarkGray";
                        break;
                }
            }
        }
    }

    private List<string> CloneMap() {
        var res = new List<string>();
        foreach (var line in map) {
            var str = "";
            foreach (char c in line) {
                str += c;
            }
            res.Add(str);
        }

        return res;
    }
}
