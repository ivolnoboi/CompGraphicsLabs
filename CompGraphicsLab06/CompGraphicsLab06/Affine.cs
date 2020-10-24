using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGraphicsLab06
{
    class Affine
    {
        static private float[,] matrixColumnFromPoint3D(Point3D point)
        {
            return new float[,] { { point.X }, { point.Y }, { point.Z }, { 1 } };

        }
        static private Polyhedron ChangePolyhedron(Polyhedron polyhedron, float[,] matrix)
        {
            List<Point3D> points = new List<Point3D>();
            foreach (var point in polyhedron.Vertexes) // применяем преобразования к каждой точке
            {
                var matrixPoint = Projection.MultMatrix(matrix, matrixColumnFromPoint3D(point));
                Point3D newPoint = new Point3D(matrixPoint[0, 0] / matrixPoint[3, 0], matrixPoint[1, 0] / matrixPoint[3, 0], matrixPoint[2, 0] / matrixPoint[3, 0]);
                points.Add(newPoint);
            }

            Polyhedron result = new Polyhedron(points);

            foreach (var edge in polyhedron.Edges)
            {
                int indexPoint1 = polyhedron.Vertexes.FindIndex(point => point == edge.From);
                int indexPoint2 = polyhedron.Vertexes.FindIndex(point => point == edge.To);
                result.AddEdge(points[indexPoint1], points[indexPoint2]);
            }

            return result;
        }
        /// <summary>
        /// Сдвинуть многогранник
        /// </summary>
        static public Polyhedron translate(Polyhedron polyhedron, float tx, float ty, float tz)
        {
            float[,] translation = { { 1, 0, 0, tx },
                                     { 0, 1, 0, ty },
                                     { 0, 0, 1, tz },
                                     { 0, 0, 0,  1 }};

            return ChangePolyhedron(polyhedron, translation);
        }

        /// <summary>
        /// Масштабирование
        /// </summary>
        static public Polyhedron scale(Polyhedron polyhedron, float mx, float my, float mz)
        {
            float[,] scale = { { mx,  0,  0,  0 },
                               {  0, my,  0,  0 },
                               {  0,  0, mz,  0 },
                               {  0,  0,  0,  1 }};

            return ChangePolyhedron(polyhedron, scale);
        }
    }
}
