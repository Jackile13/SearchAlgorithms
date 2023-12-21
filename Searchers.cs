using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithms;

public interface Searcher {
    public List<string> GetNextState();
}

public enum State {
    Pending,
    NoSolution,
    Finished
}
public record struct Point(int x, int y);
// Steps is the number of steps it took to get to this node
public record struct Node(Point p, int steps);

public class SearchAlg : Searcher {
    public List<string> state;
    public IFrontier<Node> frontier;
    public HashSet<Point> explored;
    public State algState = State.Pending;

    private Point terminal;

    public SearchAlg(List<string> initialState, IFrontier<Node> frontier) {
        this.state = initialState;
        this.frontier = frontier;
        explored = new HashSet<Point>();

        // Add start point to frontier
        Node? start = null;
        Point? end = null;
        for (int i = 0; i < state.Count; i++) {
            for (int j = 0; j < state[i].Length; j++) {
                if (state[i][j] == 'S') {
                    start = new Node(new Point(j, i), 0);
                }
                if (state[i][j] == 'T') {
                    end = new Point(j, i);
                }
            }
        }

        if (start is null) throw new Exception("No starting point found.");
        if (end is null) throw new Exception("No ending point found.");
        frontier.Add((Node)start);
        terminal = (Point)end;
    }

    public List<string> GetNextState() {
        // If no nodes exist in the frontier, no more steps
        if (frontier.IsEmpty()) {
            algState = State.NoSolution;
            return state;
        }

        var node = frontier.Remove();

        // If node is goal, done
        if (node.p == terminal) {
            algState = State.Finished;
            return state;
        }

        // Add to explored
        explored.Add(node.p);

        // Add adjacent nodes to frontier (if not explored already and if not a wall)
        Point p;
        if (node.p.x > 0) {
            p = new Point(node.p.x - 1, node.p.y);
            if (!explored.Contains(p) && state[p.y][p.x] != '#') frontier.Add(new Node(p, node.steps+1));
        }
        if (node.p.x < state[node.p.y].Length - 1) {
            p = new Point(node.p.x + 1, node.p.y);
            if (!explored.Contains(p) && state[p.y][p.x] != '#') frontier.Add(new Node(p, node.steps+1));
        }
        if (node.p.y > 0) {
            p = new Point(node.p.x, node.p.y - 1);
            if (!explored.Contains(p) && state[p.y][p.x] != '#') frontier.Add(new Node(p, node.steps+1));
        }
        if (node.p.y < state.Count - 1) {
            p = new Point(node.p.x, node.p.y + 1);
            if (!explored.Contains(p) && state[p.y][p.x] != '#') frontier.Add(new Node(p, node.steps+1));
        }

        // Adjust new state
        if (state[node.p.y][node.p.x] == '.') {
            var line = state[node.p.y];
            line = line.Substring(0, node.p.x) + 'E' + line.Substring(node.p.x + 1);
            state[node.p.y] = line;
        }

        return state;
    }
}