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
        static private void ChangePolyhedron(Polyhedron polyhedron, float[,] matrix)
        {
            List<Point3D> points = new List<Point3D>();
            //foreach (var point in polyhedron.Vertexes) // применяем преобразования к каждой точке
            for (int i = 0; i < polyhedron.Vertexes.Count; ++i)
            {
                var matrixPoint = Projection.MultMatrix(matrix, matrixColumnFromPoint3D(polyhedron.Vertexes[i]));
                Point3D newPoint = new Point3D(matrixPoint[0, 0] / matrixPoint[3, 0], matrixPoint[1, 0] / matrixPoint[3, 0], matrixPoint[2, 0] / matrixPoint[3, 0]);
                //points.Add(newPoint);
                polyhedron.Vertexes[i] = newPoint; //Projection.MultMatrix(matrix, matrixColumnFromPoint3D(polyhedron.Vertexes[i]));
            }

           /*Polyhedron result = new Polyhedron(points);

             foreach (var edge in polyhedron.Edges)
             {
                 int indexPoint1 = polyhedron.Vertexes.FindIndex(point => point == edge.From);
                 int indexPoint2 = polyhedron.Vertexes.FindIndex(point => point == edge.To);
                 result.AddEdge(points[indexPoint1], points[indexPoint2]);
             }

            return result;*/
        }
        /// <summary>
        /// Сдвинуть многогранник
        /// </summary>
        static public void translate(Polyhedron polyhedron, float tx, float ty, float tz)
        {
            float[,] translation = { { 1, 0, 0, tx },
                                     { 0, 1, 0, ty },
                                     { 0, 0, 1, tz },
                                     { 0, 0, 0,  1 }};

             ChangePolyhedron(polyhedron, translation);
        }

        /// <summary>
        /// Масштабирование
        /// </summary>
        static public void scale(Polyhedron polyhedron, float mx, float my, float mz)
        {
            float[,] scale = { { mx,  0,  0,  0 },
                               {  0, my,  0,  0 },
                               {  0,  0, mz,  0 },
                               {  0,  0,  0,  1 }};

             ChangePolyhedron(polyhedron, scale);
        }

        static public Polyhedron rotation(Polyhedron polyhedron, float angleX, float angleY, float angleZ)
        {
            return polyhedron;
        }
    }
}
