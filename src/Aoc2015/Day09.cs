namespace Aoc2015;

public class Day09 : BaseDay
{
    private readonly Graph _graph = new();

    public Day09()
    {
        ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        return new ValueTask<string>(_graph.FindShortestRoute().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new ValueTask<string>(_graph.FindLongestRoute().ToString());
    }

    private void ParseInput()
    {
        foreach (var line in File.ReadAllLines(InputFilePath))
        {
            var tokens = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var from = tokens[0];
            var to = tokens[2];

            var distance = int.Parse(tokens[4]);
            _graph.AddEdge(from, to, distance);
        }
    }

    private class Graph
    {
        public Graph()
        {
            Vertices = new List<Vertex>();
        }

        private List<Vertex> Vertices { get; set; }

        public void AddEdge(string v, string w, int distance)
        {
            var vertexV = Vertices.FirstOrDefault(x => x.Name == v);
            if (vertexV == null)
            {
                vertexV = new Vertex(v);
                Vertices.Add(vertexV);
            }

            var vertexW = Vertices.FirstOrDefault(x => x.Name == w);
            if (vertexW == null)
            {
                vertexW = new Vertex(w);
                Vertices.Add(vertexW);
            }

            vertexV.AddEdge(vertexW, distance);
            vertexW.AddEdge(vertexV, distance);
        }

        private int _shortestRoute;
        private int _longestRoute;

        public int FindShortestRoute()
        {
            ClearVisited();
            _shortestRoute = int.MaxValue;
            foreach (var vertex in Vertices)
            {
                GetVertexShortestRoute(vertex, 0);
            }

            ClearVisited();
            return _shortestRoute;
        }


        private void GetVertexShortestRoute(Vertex v, int route)
        {
            v.SetVisited();

            if (v.Edges.All(x => x.To.Visited))
            {
                if (route < _shortestRoute)
                    _shortestRoute = route;
                v.ClearVisited();
            }

            foreach (var edge in v.Edges.Where(x => !x.To.Visited))
            {
                GetVertexShortestRoute(edge.To, route + edge.Distance);
            }

            v.ClearVisited();
        }

        public int FindLongestRoute()
        {
            ClearVisited();
            _longestRoute = 0;
            foreach (var vertex in Vertices)
            {
                GetVertexLongestRoute(vertex, 0);
            }

            ClearVisited();
            return _longestRoute;
        }

        private void GetVertexLongestRoute(Vertex v, int route)
        {
            v.SetVisited();

            if (v.Edges.All(x => x.To.Visited))
            {
                if (route > _longestRoute)
                    _longestRoute = route;
                v.ClearVisited();
            }

            foreach (var edge in v.Edges.Where(x => !x.To.Visited))
            {
                GetVertexLongestRoute(edge.To, route + edge.Distance);
            }

            v.ClearVisited();
        }


        private void ClearVisited()
        {
            foreach (var v in Vertices)
            {
                v.ClearVisited();
            }
        }
    }

    public class Edge
    {
        public Edge(Vertex to, int distance)
        {
            To = to;
            Distance = distance;
        }

        public Vertex To { get; set; }

        public int Distance { get; set; }
    }

    public record Vertex(string Name)
    {
        public string Name { get; set; } = Name;

        public List<Edge> Edges { get; set; } = new();

        public bool Visited { get; private set; }

        public void AddEdge(Vertex vertexW, int distance)
        {
            Edges.Add(new Edge(vertexW, distance));
        }

        public void SetVisited()
        {
            Visited = true;
        }

        public void ClearVisited()
        {
            Visited = false;
        }
    }
}