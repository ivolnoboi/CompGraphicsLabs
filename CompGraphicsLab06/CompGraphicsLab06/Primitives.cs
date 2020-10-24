using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGraphicsLab06
{
    /// <summary>
    /// Класс точки в пространстве 
    /// </summary>
    public class Point3D
    {
        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;
        public float Z { get; set; } = 0;

        public Point3D(float x, float y, float z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        static public bool operator ==(Point3D point1, Point3D point2)
        {
            return point1.X == point2.X && point1.Y == point2.Y && point1.Z == point2.Z;
        }

        static public bool operator !=(Point3D point1, Point3D point2)
        {
            return !(point1 == point2);
        }

        static public Point3D operator +(Point3D point1, Point3D point2)
        {
            return new Point3D(point1.X + point2.X, point1.Y + point2.Y, point1.Z + point2.Z);
        }

        static public Point3D operator -(Point3D point1, Point3D point2)
        {
            return new Point3D(point1.X - point2.X, point1.Y - point2.Y, point1.Z - point2.Z);
        }

        public Point ConvertToPoint() 
        { 
            return new Point((int)X, (int)Y); 
        }
    }

    /// <summary>
    /// Класс ребра в пространтсве
    /// </summary>
    public class Edge
    {
        public Point3D From { get; set; }
        public Point3D To { get; set; }

        public Edge(Point3D point1, Point3D point2)
        {
            From = point1;
            To = point2;
        }
        public Edge(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            From = new Point3D(x1, y1, z1);
            To = new Point3D(x2, y2, z2);
        }
    }
    /// <summary>
    /// Класс грани, заданной полигоном
    /// </summary>
    public class Polygon
    {
        public List<Point3D> Points { get; set; } = new List<Point3D>();

        public Polygon(List<Point3D> points)
        {
            Points = points;
        }

        public void AddPoint(Point3D point)
        {
            Points.Add(point);
        }

        public void AddPoint(float x, float y, float z)
        {
            Points.Add(new Point3D(x, y, z));
        }
    }


    /// <summary>
    /// Класс многогранника
    /// </summary>
    public class Polyhedron
    {
        /// <summary>
        /// Список вершин
        /// </summary>
        public List<Point3D> Vertexes { get; set; } = new List<Point3D>();

        /// <summary>
        /// Список ребер
        /// </summary>
        public List<Edge> Edges { get; } = new List<Edge>();

        /// <summary>
        /// Матрица смежности - для каждой точки хранит список смежных с ней
        /// </summary>
        public Dictionary<Point3D, List<Point3D>> Adjacency { get; } = new Dictionary<Point3D, List<Point3D>>();

        /// <summary>
        /// Находит центр многогранника
        /// </summary>
        public Point3D Center()
        {
            float x = Vertexes.Average(point => point.X);
            float y = Vertexes.Average(point => point.Y);
            float z = Vertexes.Average(point => point.Z);
            return new Point3D(x, y, z);
        }

        /// <summary>
        /// Конструктор многогранника от списка вершин
        /// </summary>
        /// <param name="points"></param>
        public Polyhedron(List<Point3D> points)
        {
            Vertexes = points;
            foreach (Point3D point in points)
                Adjacency.Add(point, new List<Point3D>());
        }

        /// <summary>
        /// Добавить ребро
        /// </summary>
        /// <param name="from">Начало ребра</param>
        /// <param name="to">Конец ребра</param>
        public void AddEdge(Point3D from, Point3D to)
        {
            Edges.Add(new Edge(from, to));

            Point3D point1 = Vertexes.Find(p => p == from);
            Point3D point2 = Vertexes.Find(p => p == to);

            if (!Adjacency.ContainsKey(point1))
                Adjacency.Add(point1, new List<Point3D> { to });
            else
                Adjacency[point1].Add(to);

            if (!Adjacency.ContainsKey(point2))
                Adjacency.Add(point2, new List<Point3D> { from });
            else
                Adjacency[point2].Add(from);
        }

        /// <summary>
        /// Добавить семейство ребер из точки FROM в каждую точку списка LST
        /// </summary>
        /// <param name="from">Начальная точка</param>
        /// <param name="lst">Конечные точки, в которые идут ребра из начальной</param>
        public void AddEdges(Point3D from, List<Point3D> lst)
        {
            foreach (Point3D to in lst)
                AddEdge(from, to);
        }
    }
}
