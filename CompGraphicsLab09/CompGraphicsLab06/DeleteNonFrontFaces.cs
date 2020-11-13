using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CompGraphicsLab06
{
    class DeleteNonFrontFaces
    {
        public static List<List<int>> DeleteFaces(Polyhedron pl, Point3D viewDirection)
        {
            List<List<int>> res = new List<List<int>>();
            Point3D proec = pl.Center() - viewDirection; // вектор проекции
            foreach (var face in pl.Faces) // для каждой грани ищем вектор нормали
            {
                Point3D norm = Vectors.CalculateNormalFace(face, pl);
                var cos = Vectors.cosBetweenVectors(norm, proec);
                if (cos > 0)
                    res.Add(face);
            }
            return res;
        }
    }
}
