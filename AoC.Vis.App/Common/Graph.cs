using Ujeby.Graphics;
using Ujeby.Graphics.Entities;
using Ujeby.Graphics.Sdl;
using Ujeby.Vectors;

namespace Ujeby.AoC.Vis.App.Common
{
    public class GraphNodeMeta
    {
        public v2i Position { get; set; }
    }

    public class GraphNode
    {
        public string Id { get; set; }
        public string[] Children { get; set; }

        public GraphNodeMeta Meta { get; set; }

        public GraphNode() { }
    }

    public class Graph
    {
        const int _pickZone = 5;
        string _selected = null;
        bool _moving = false;

        public Dictionary<string, GraphNode> Data { get; private set; }

        public void LeftMouseDown()
        {
            if (_selected != null)
            {
                _moving = true;
            }
        }

        public void LeftMouseUp()
        {
            if (_moving)
            {
                _moving = false;
            }
        }

        public void GraphLayout_Random(v2i from, v2i to)
        {
            var rng = new Random((int)DateTime.Now.Ticks);
            foreach (var node in Data)
            {
                node.Value.Meta = new GraphNodeMeta
                {
                    Position = new v2i(rng.Next((int)from.X, (int)to.X), rng.Next((int)from.Y, (int)to.Y))
                };
            }
        }

        public void RenderGraph(WorkspaceGrid grid)
        {
            var lineColor = Colors.DarkGray;
            lineColor.W = .8;

            foreach (var node in Data.Values)
            {
                foreach (var child in node.Children)
                {
                    var from = node.Meta.Position;
                    var to = Data[child].Meta.Position;
                    grid.DrawLine((int)from.X, (int)from.Y, (int)to.X, (int)to.Y, lineColor);
                }
            }

            foreach (var node in Data.Values)
                grid.DrawCell(node.Meta.Position, Colors.White, Colors.LightGray);

            if (_selected != null)
            {
                var node = Data[_selected];
                var p = node.Meta.Position;
                var fill = Colors.Yellow;
                fill.W = .2;
                grid.DrawRect((int)p.X - _pickZone, (int)p.Y - _pickZone, _pickZone * 2 + 1, _pickZone * 2 + 1, Colors.Red, fill);

                Sdl2Wrapper.DrawText(new v2i(32, 32), null,
                    new Text($"{nameof(_selected)}: {_selected?.ToString()}"),
                    new Text($"{nameof(GraphNodeMeta.Position)}: {Data[_selected].Meta.Position}")
                    );
            }
        }

        internal void SetData(Dictionary<string, GraphNode> data)
        {
            Data = data;
        }

        internal void Update(WorkspaceGrid grid)
        {
            var p = grid.MousePositionDiscrete;
            _selected = Data
                .FirstOrDefault(x =>
                {
                    var dist = (x.Value.Meta.Position - p).Abs();
                    return dist.X <= _pickZone && dist.Y <= _pickZone;
                })
                .Key;

            if (_moving)
            {
                Data[_selected].Meta.Position = p;
            }
        }
    }
}
