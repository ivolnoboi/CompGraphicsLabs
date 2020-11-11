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

        public float DistanceTo(Point3D p2)
        {
            return (float)Math.Sqrt((X - p2.X) * (X - p2.X) + (Y - p2.Y) * (Y - p2.Y) + (Z - p2.Z) * (Z - p2.Z));
        }
    }

    class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Length
        {
            get
            {
                return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        public Vector3(float x, float y, float z)
        {
            X = x; Y = y; Z = z;
        }

        public Vector3 Normalize() => Length > 0 ? new Vector3(X / Length, Y / Length, Z / Length) : new Vector3(0, 0, 0);

        public static float ScalarProduct(Vector3 v1, Vector3 v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        public static Vector3 VectorProduct(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2) => new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        public static Vector3 operator -(Vector3 v1, Vector3 v2) => new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
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
   /* public class Polygon
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
    }*/


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
        public List<Edge> Edges { get; set; } = new List<Edge>();

        /// <summary>
        /// Список граней. Грани заданы списком вершин (вершины заданы индексами в списке вершин)
        /// </summary>
        public List<List<int>> Faces { get; } = new List<List<int>>();

        /// <summary>
        /// Матрица смежности - для каждой точки хранит список смежных с ней
        /// </summary>
        public Dictionary<int, List<int>> Adjacency { get; set; } = new Dictionary<int, List<int>>();

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

        public bool IsEmpty() => Vertexes.Count < 1;

        public void Clear()
        {
            Vertexes.Clear();
            Edges.Clear();
            Faces.Clear();
            Adjacency.Clear();
        }

        /// <summary>
        /// Конструктор многогранника от списка вершин
        /// </summary>
        /// <param name="points"></param>
        public Polyhedron(List<Point3D> points)
        {
            Vertexes = points;
            int i = 0;
            foreach (Point3D point in points)
            {
                i++;
                Adjacency.Add(i, new List<int>());
            }
        }
        public Polyhedron() { }
        public Polyhedron(Polyhedron old)
        {
            Vertexes = new List<Point3D>(old.Vertexes);
            Edges = new List<Edge>(old.Edges);
            Adjacency = new Dictionary<int, List<int>>();
            foreach (var ad in old.Adjacency)
                Adjacency.Add(ad.Key, new List<int>(ad.Value));
            Faces = new List<List<int>>();
            foreach (var f in old.Faces)
                Faces.Add(new List<int>(f));            
        }


        /// <summary>
        /// Добавить ребро
        /// </summary>
        /// <param name="from">Начало ребра</param>
        /// <param name="to">Конец ребра</param>
        public void AddEdge(int from, int to)
        {
            if (!Adjacency.ContainsKey(from))
                Adjacency.Add(from, new List<int> { to });
            else
                Adjacency[from].Add(to);

           /* if (!Adjacency.ContainsKey(index2))
                Adjacency.Add(index2, new List<int> { index1 });
            else
                Adjacency[index2].Add(index1);*/
        }

        /// <summary>
        /// Добавить множество ребер из точки FROM в каждую точку списка LST
        /// </summary>
        /// <param name="from">Индекс начальной точки</param>
        /// <param name="lst">Индексы конечных точек, в которые идут ребра из начальной</param>
        public void AddEdges(int from, List<int> lst)
        {
            foreach (int to in lst)
                AddEdge(from, to);
        }

        /// <summary>
        /// Добавляет грань
        /// </summary>
        /// <param name="lst"></param>
        public void AddFace(List<int> lst)
        {
            Faces.Add(lst);
        }

        public Point3D[] GetPoints(List<int> face)
        {
            Point3D[] point3Ds = new Point3D[face.Count];

            for (int i = 0; i < face.Count; ++i)
                point3Ds[i] = (Vertexes[i]);

            return point3Ds;
        }
    }
}
