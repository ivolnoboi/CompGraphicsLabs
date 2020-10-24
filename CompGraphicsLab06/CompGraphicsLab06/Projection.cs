using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGraphicsLab06
{
    class Projection
    {
        private static float c = 1000;
        private float[,] perspective =
        {
            { 1, 0, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 0, -1 / c },
            { 0, 0, 0, 1 }
        };

        //перемножение матриц
        private float[,] MultMatrix(float[,] m1, float[,] m2)
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
        /// <returns>Список точек на плоскости (для рисования на экране)</returns>
        public List<Edge> Project(Polyhedron polyhedron)
        {
            // TODO: Добавить сюда выбор проекции, сейчас только перспективная одноточечная
            float[,] matr = perspective;
            List<Edge> edges = new List<Edge>();
            List<Point3D> vertices = new List<Point3D>();

            // Для каждой вершины обрабатываем её и запускаем обработку смежных с ней
            foreach (var p in polyhedron.Vertexes)
            {
                float[,] temp = MultMatrix(new float[,] { { p.X, p.Y, p.Z, 1 } }, matr);
                Point3D temp2d = new Point3D(temp[0, 0] / temp[0, 3], temp[0, 1] / temp[0, 3]);
                vertices.Add(new Point3D(temp[0, 0] / temp[0, 3], temp[0, 1] / temp[0, 3]));

                // Обработка смежных с вершиной
                foreach (var t in polyhedron.Adjacency[p])
                {
                    float[,] t1 = MultMatrix(new float[,] { { t.X, t.Y, t.Z, 1 } }, matr);
                    vertices.Add(new Point3D(t1[0, 0] / t1[0, 3], t1[0, 1] / t1[0, 3]));
                    edges.Add(new Edge(temp2d, vertices.Last()));
                }
            }

            return edges;
        }
    }
}
