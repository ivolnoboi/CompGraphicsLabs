using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGraphicsLab06
{
    public class Vectors
    {
        // Длина вектора
        public static double LenghtOfVec(Point3D vec)
        {
            return Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
        }

        // Векторное произведение векторов
        private static Point3D CrossProduct(Point3D vec1, Point3D vec2)
        {
            float x = vec1.Y * vec2.Z - vec1.Z * vec2.Y;
            float y = vec1.Z * vec2.X - vec1.X * vec2.Z;
            float z = vec1.X * vec2.Y - vec1.Y * vec2.X;
            return new Point3D(x, y, z);
        }
        public static Point3D CalculateNormalFace(List<int> face, Polyhedron polyhedron)
        {
            Point3D p0 = polyhedron.Vertexes[face[0]];
            Point3D p1 = polyhedron.Vertexes[face[1]];
            Point3D p2 = polyhedron.Vertexes[face[face.Count - 1]];
            Point3D v1 = new Point3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            Point3D v2 = new Point3D(p2.X - p0.X, p2.Y - p0.Y, p2.Z - p0.Z);
            return CrossProduct(v1, v2);
        }

        public static Point3D CalculateNormal(List<List<int>> faces, Polyhedron polyhedron)
        {
            Point3D res = new Point3D(0, 0, 0);
            foreach (var face in faces)
            {
                res.X += CalculateNormalFace(face, polyhedron).X;
                res.Y += CalculateNormalFace(face, polyhedron).Y;
                res.Z += CalculateNormalFace(face, polyhedron).Z;
            }
            res.X /= faces.Count;
            res.Y /= faces.Count;
            res.Z /= faces.Count;
            return res;
        }

        public static double cosBetweenVectors(Point3D vec1, Point3D vec2)
        {
            var scalar = vec1.X * vec2.X + vec1.Y * vec2.Y + vec1.Z * vec2.Z;
            var prodLength = LenghtOfVec(vec1) * LenghtOfVec(vec2);
            return scalar / prodLength;
        }
    }
}
