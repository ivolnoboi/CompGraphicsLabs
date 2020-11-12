using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointF = System.Drawing.PointF;

namespace CompGraphicsLab06
{
    class Camera
    {
        public Point3D Position { get; set; } = new Point3D(0, 0, 0);
        public Point3D Focus { get; set; } = new Point3D(0, 0, 0);
        public PointF Offset { get; set; } = new PointF(0, 0);
        public double AngleX { get; private set; } = 0;
        public double AngleY { get; private set; } = 0;
 

        public void MoveCamera(int dx, int dy, int dz)
        {
            Position += new Point3D(dx, dy, dz);
            Focus += new Point3D(dx, dy, 0);
        }

        public void RotateCamera(double ax, double ay)
        {
            AngleX += ax; AngleY += ay;
            if (AngleX > 360) AngleX -= 360;
            else if (AngleX < 0) AngleX += 360;
        }
    }

    class Projection
    {
        private static float c = 1000;
        static private float[,] perspective =
        {
            { 1, 0, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 0, -1 / c },
            { 0, 0, 0, 1 }
        };

        static private float[,] isometric =
            {  { (float)Math.Sqrt(0.5), 0, (float)-Math.Sqrt(0.5), 0 },
               { 1 / (float)Math.Sqrt(6), 2 /(float) Math.Sqrt(6), 1 / (float)Math.Sqrt(6), 0 },
               { 1 / (float)Math.Sqrt(3), -1 / (float)Math.Sqrt(3), 1 / (float)Math.Sqrt(3), 0 },
               { 0, 0, 0, 1 }};

        //перемножение матриц
        static public float[,] MultMatrix(float[,] m1, float[,] m2)
        {
            float[,] res = new float[m1.GetLength(0), m2.GetLength(1)];

            for (int i = 0; i < m1.GetLength(0); ++i)
                for (int j = 0; j < m2.GetLength(1); ++j)
                    for (int k = 0; k < m2.GetLength(0); k++)
                    {
                        res[i, j] += m1[i, k] * m2[k, j];
                    }

            return res;
        }

        /// <summary>
        /// Выполняет проекцию
        /// </summary>
        /// <param name="polyhedron">входной многогранник</param>
        /// <returns>Список ребер на плоскости (для рисования на экране)</returns>
        public List<Edge> Project(Polyhedron polyhedron, int mode)
        {
            // TODO: Добавить сюда выбор проекции, сейчас только перспективная одноточечная
            float[,] matr;
            switch (mode)
            {
                case 0:
                    matr = perspective;
                    break;
                case 1:
                    matr = isometric;
                    break;
                default:
                    throw new ArgumentException();
            }
            List<Edge> edges = new List<Edge>();

            int i = 0;
            // Для каждой вершины обрабатываем её и запускаем обработку смежных с ней
            foreach (Point3D p in polyhedron.Vertexes)
            {
                // Все многогранники начинаются в (0, 0, 0). Добавляем смещение, чтобы фигуры были примерно по центру
                Point3D p1 = p;// + new Point3D(250 , 150, 200 );
                float[,] tmp = MultMatrix(new float[,] { { p1.X, p1.Y, p1.Z, 1 } }, matr);
                Point3D from = new Point3D(tmp[0, 0] / tmp[0, 3], tmp[0, 1] / tmp[0, 3]);


                // Обработка смежных с вершиной
                foreach (int index in polyhedron.Adjacency[i])
                {
                    // Все многогранники начинаются в (0, 0, 0). Добавляем смещение, чтобы фигуры были примерно по центру
                    // Оставлю эту жесть для истории, саундтрек "время пострелять, между нами пальба"
                    Point3D t = polyhedron.Vertexes[index];// + new Point3D(250 , 150, 200 ); 

                    float[,] tmp1 = MultMatrix(new float[,] { { t.X, t.Y, t.Z, 1 } }, matr);
                    Point3D to = new Point3D(tmp1[0, 0] / tmp1[0, 3], tmp1[0, 1] / tmp1[0, 3]);
                    edges.Add(new Edge(from, to));
                }
                i++;
            }

            return edges;
        }

        /// <summary>
        /// Выполняет проекцию (возвращает список точек на плоскости)
        /// </summary>
        /// <param name="polyhedron"></param>
        /// <param name="mode"></param>
        public List<Point3D> Project2(Polyhedron polyhedron, int mode)
        {
            // TODO: Добавить сюда выбор проекции, сейчас только перспективная одноточечная
            float[,] matr;
            switch (mode)
            {
                case 0:
                    matr = perspective;
                    break;
                case 1:
                    matr = isometric;
                    break;
                default:
                    throw new ArgumentException();
            }
            List<Point3D> points = new List<Point3D>(polyhedron.Vertexes);

            for (int i = 0; i < points.Count; ++i)
            {
                float[,] tmp1 = MultMatrix(new float[,] { { points[i].X, points[i].Y, points[i].Z, 1 } }, matr);
                points[i] = new Point3D(tmp1[0, 0] / tmp1[0, 3], tmp1[0, 1] / tmp1[0, 3], points[i].illumination);
            }
            return points;
        }

        public List<Point3D> Project3(List<Point3D> fase, int mode = 0)
        {
            // TODO: Добавить сюда выбор проекции, сейчас только перспективная одноточечная
            float[,] matr;
            switch (mode)
            {
                case 0:
                    matr = perspective;
                    break;
                case 1:
                    matr = isometric;
                    break;
                default:
                    throw new ArgumentException();
            }
            List<Point3D> points = new List<Point3D>(fase);

            for (int i = 0; i < points.Count; ++i)
            {
                float[,] tmp1 = MultMatrix(new float[,] { { points[i].X, points[i].Y, points[i].Z, 1 } }, matr);
                points[i] = new Point3D(tmp1[0, 0] / tmp1[0, 3], tmp1[0, 1] / tmp1[0, 3], points[i].illumination, points[i].Z);
            }
            return points;
        }
    }
}
