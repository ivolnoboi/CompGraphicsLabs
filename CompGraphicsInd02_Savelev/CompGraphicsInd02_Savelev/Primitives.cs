using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGraphicsInd02_Savelev
{
    public class Point : Shape
    {
        public double x, y, z;
        public Point(double x, double y, double z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Point()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }

        public override Point center()
        {
            return this;
        }
        public override List<Point> points()
        {
            return new List<Point> { this };
        }
        public static bool operator ==(Point left, Point right)
        {
            if (object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) || !object.ReferenceEquals(left, null) && object.ReferenceEquals(right, null))
                return false;
            return object.ReferenceEquals(left, null) && object.ReferenceEquals(right, null) || left.x == right.x && left.y == right.y && left.z == right.z;
        }
        public static Point operator +(Point left, Point right)
        {
            return new Point(left.x + right.x, left.y + right.y, left.z + right.z);
        }
        public static Point operator +(Point left, double d)
        {
            return new Point(left.x + d, left.y + d, left.z + d);
        }
        public static Point operator -(Point left, double d)
        {
            return left + (-1) * d;
        }
        public static Point operator -(Point left, Point right)
        {
            return left + (-1) * right;
        }
        public static Point operator *(Point left, double d)
        {
            return new Point(left.x * d, left.y * d, left.z * d);
        }
        public static Point operator /(Point left, double d)
        {
            return new Point(left.x / d, left.y / d, left.z / d);
        }
        public static Point operator *(double d, Point left)
        {
            return left * d;
        }
        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }
        public bool between(Point first, Point second)
        {
            return Math.Min(first.x, second.x) <= this.x &&
                this.x <= Math.Max(first.x, second.x) &&
                Math.Min(first.y, second.y) <= this.y &&
                this.y <= Math.Max(first.y, second.y) &&
                Math.Min(first.z, second.z) <= this.z &&
                this.z <= Math.Max(first.z, second.z);
        }
        public double distance(Point to)
        {
            double dx = this.x - to.x, dy = this.y - to.y, dz = this.z - to.z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public static double GetDouble(string value, double defaultValue)
        {
            double result;

            //Try parsing in the current culture
            if (!double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out result) &&
                //Then try in US english
                !double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.GetCultureInfo("en-US"), out result) &&
                //Then in neutral language
                !double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out result))
            {
                result = defaultValue;
            }

            return result;
        }

        public void parse(string to_parse)
        {
            string str = "";
            char ch;
            int i = 0;
            do
            {
                ch = (char)to_parse[i];
                str += ch;
                i++;
            } while (ch != ')');
            str = str.Remove(str.Length - 1, 1);
            str = str.Remove(0, 1);
            char[] delimiters = { ',' };
            string[] delims = str.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);
            x = GetDouble(delims[0], 0);
            y = GetDouble(delims[1], 0);
            z = GetDouble(delims[2], 0);
        }
    }

    public abstract class Shape
    {
        abstract public Point center();

        abstract public List<Point> points();
    }

    public class Edge : Shape
    {
        public Point start, end;
        private double axis_center(double left, double right)
        {
            return (left + right) / 2;
        }

        override public Point center()
        {
            return new Point(axis_center(start.x, end.x),
                             axis_center(start.y, end.y),
                             axis_center(start.z, end.z));
        }

        public Edge(Point start, Point end)
        {
            this.start = start;
            this.end = end;
        }

        public Edge()
        {
            this.start = new Point();
            this.end = new Point();
        }

        public override List<Point> points()
        {
            return new List<Point> { start, end };
        }
    }


    public class Polygon : Shape
    {
        public List<Edge> edges;
        override public Point center()
        {
            double A = 0;
            Point center = new Point(0, 0);
            var points = this.points();
            for (int i = 0; i < points.Count; i++)
            {
                int next = (i + 1) % points.Count;
                double temp = points[i].x * points[next].y - points[next].x * points[i].y;
                A += temp;
                center.x += (points[i].x + points[next].x) * temp;
                center.y += (points[i].y + points[next].y) * temp;
            }
            A /= 2;
            center.x /= 6 * A;
            center.y /= 6 * A;
            return center;
        }

        override public List<Point> points()
        {
            int count = edges.Count;
            var result = new List<Point>(count);
            for (int i = 0; i < count; i++)
                result.Add(edges[i].start);
            return result;
        }
    }

    public class Polyhedron : Shape
    {
        public List<Point> _points;
        public List<List<int>> facets;

        public List<(int, int)> edges(int index)
        {
            var facet = facets[index];
            var result = new List<(int, int)>(facet.Count);
            for (int i = 0; i < facet.Count; i++)
                result.Add((facet[i], facet[(i + 1) % facet.Count]));
            return result;
        }

        public Point facet_center(int index)
        {
            var result = new Point(0, 0, 0);
            foreach (var p in facets[index])
            {
                result += _points[p];
            }
            return result / facets[index].Count;
        }

        public void move(Point center)
        {
            for (int i = 0; i < _points.Count; i++)
                _points[i] -= center;
        }

        public int facet_visibility(int index, Point lookup)
        {
            var normal = facet_center(index);
            return Math.Sign(normal.x * lookup.x + normal.y * lookup.y + normal.z * normal.z);
        }

        public List<Edge> edges_by_visibility(int visibility, Point lookup, Point direction)
        {
            var center = this.center();
            move(center);
            lookup -= center;
            direction -= lookup;
            var raw_result = new HashSet<(int, int)>();
            for (int i = 0; i < facets.Count; i++)
            {
                if (facet_visibility(i, direction) != visibility)
                {
                    foreach (var edge in edges(i))
                    {
                        raw_result.Add(edge);
                    }
                }
            }
            var result = new List<Edge>(raw_result.Count);
            lookup += center;
            move(-1 * center);
            foreach (var edge in raw_result)
                result.Add(new Edge(_points[edge.Item1], _points[edge.Item2]));
            return result;
        }

        public List<Edge> hidden_edges(Point lookup, Point direction)
        {
            return edges_by_visibility(1, lookup, direction);
        }

        public List<Edge> visible_edges(Point lookup, Point direction)
        {
            return edges_by_visibility(-1, lookup, direction);
        }

        public List<Edge> edges()
        {
            var raw_result = new HashSet<(int, int)>();
            for (int i = 0; i < facets.Count; i++)
                foreach (var edge in edges(i))
                {
                    raw_result.Add(edge);
                }
            var result = new List<Edge>(raw_result.Count);
            foreach (var edge in raw_result)
                result.Add(new Edge(_points[edge.Item1], _points[edge.Item2]));
            return result;
        }
        public override List<Point> points()
        {
            return _points;
        }

        public override Point center()
        {
            Point sum = new Point(0, 0, 0);
            foreach (var p in _points)
                sum += p;
            return sum / _points.Count;
        }

        public class MyClassSpecialComparer : IEqualityComparer<Point>
        {
            bool IEqualityComparer<Point>.Equals(Point l, Point r)
            {
                return l == r;
            }

            int IEqualityComparer<Point>.GetHashCode(Point p)
            {
                return p.x.GetHashCode() + p.y.GetHashCode() + p.z.GetHashCode();
            }

        }

        public Polyhedron ShallowCopy()
        {
            return (Polyhedron)this.MemberwiseClone();
        }

        public Polyhedron DeepCopy()
        {
            Polyhedron other = (Polyhedron)this.MemberwiseClone();
            other._points = new List<Point>(this._points.ToArray());
            other.facets = new List<List<int>>(this.facets.ToArray());
            return other;
        }
    }
}
