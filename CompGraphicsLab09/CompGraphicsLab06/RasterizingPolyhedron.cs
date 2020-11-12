using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGraphicsLab06
{
    public class RasterizingPolyhedron
    {
        public static int ProjMode = 0;

        // Растеризирует многогранник
        public static List<List<Point3D>> Rasterize(Polyhedron polyhedron, bool is_lightning)
        {
            List<List<Point3D>> rasterized = new List<List<Point3D>>();
            // Каждую грань многогранника триангулируем (бьём на треугольники)
            foreach (var facet in polyhedron.Faces)
            {
                List<Point3D> currentFac = new List<Point3D>();
                List<Point3D> facetPoint3Ds = new List<Point3D>(); // точки вершин, принадлежащих данной грани
                for (int i = 0; i < facet.Count; i++)
                {
                    facetPoint3Ds.Add(polyhedron.Vertexes[facet[i]]);
                }

                List<List<Point3D>> triangles = Triangulate(facetPoint3Ds); // триангулируем
                foreach (List<Point3D> triangle in triangles)
                {
                    currentFac.AddRange(RasterizeTriangle(MakeProj(triangle), is_lightning));
                }
                rasterized.Add(currentFac);
            }
            return rasterized;
        }

        // points - вершины треугольника
        // возвращает растеризованный треугольник (список точек, находящихся полностью внутри треугольника и на рёбрах)
        private static List<Point3D> RasterizeTriangle(List<Point3D> points, bool is_lightning)
        {
            List<Point3D> res = new List<Point3D>();

            points.Sort((point1, point2) => point1.Y.CompareTo(point2.Y)); // сортируем по возрастанию Y
            var rpoints = points.Select(point => (X: (int)Math.Round(point.X), Y: (int)Math.Round(point.Y), Z: (int)Math.Round(point.Z), ilum: point.illumination)).ToList();

            var x01 = Interpolate(rpoints[0].Y, rpoints[0].X, rpoints[1].Y, rpoints[1].X);
            var x12 = Interpolate(rpoints[1].Y, rpoints[1].X, rpoints[2].Y, rpoints[2].X);
            var x02 = Interpolate(rpoints[0].Y, rpoints[0].X, rpoints[2].Y, rpoints[2].X);

            var z01 = Interpolate(rpoints[0].Y, rpoints[0].Z, rpoints[1].Y, rpoints[1].Z);
            var z12 = Interpolate(rpoints[1].Y, rpoints[1].Z, rpoints[2].Y, rpoints[2].Z);
            var z02 = Interpolate(rpoints[0].Y, rpoints[0].Z, rpoints[2].Y, rpoints[2].Z);

            x01.RemoveAt(x01.Count - 1);
            List<int> x012 = x01.Concat(x12).ToList();

            z01.RemoveAt(z01.Count - 1);
            List<int> z012 = z01.Concat(z12).ToList();

            int middle = x012.Count / 2;
            List<int> leftX, rightX, leftZ, rightZ;
            List<double> leftH = new List<double>(), rigthH = new List<double>();

            if (x02[middle] < x012[middle])
            {
                leftX = x02;
                leftZ = z02;
                rightX = x012;
                rightZ = z012;
            }
            else
            {
                leftX = x012;
                leftZ = z012;
                rightX = x02;
                rightZ = z02;
            }

            int y0 = rpoints[0].Y;
            int y2 = rpoints[2].Y;

            if (is_lightning)
            {
                var h01 = Interpolate(rpoints[0].Y, rpoints[0].ilum, rpoints[1].Y, rpoints[1].ilum);
                var h12 = Interpolate(rpoints[1].Y, rpoints[1].ilum, rpoints[2].Y, rpoints[2].ilum);
                var h02 = Interpolate(rpoints[0].Y, rpoints[0].ilum, rpoints[2].Y, rpoints[2].ilum);

                h01.RemoveAt(h01.Count - 1);
                List<double> h012 = h01.Concat(h12).ToList();

                if (x02[middle] < x012[middle])
                {
                    leftH = h02;
                    rigthH = h012;
                }
                else
                {
                    leftH = h012;
                    rigthH = h02;
                }
            }

            for (int ind = 0; ind <= y2 - y0; ind++) // двигаемся по Y (построчно) и интерполируем Z и H в зависимости от X 
            {                                        // и получаем список точек, которые внутри треугольника
                int XL = leftX[ind];
                int XR = rightX[ind];

                List<int> intCurrZ = Interpolate(XL, leftZ[ind], XR, rightZ[ind]);
                if (is_lightning)
                {
                    List<double> doubleCurrH = Interpolate(XL, leftH[ind], XR, rigthH[ind]);
                    for (int x = XL; x < XR; x++)
                    {
                        res.Add(new Point3D(x, y0 + ind, doubleCurrH[x - XL], intCurrZ[x - XL]));
                    }
                }
                else
                {
                    for (int x = XL; x < XR; x++)
                    {
                        res.Add(new Point3D(x, y0 + ind, intCurrZ[x - XL]));
                    }
                }
            }

            return res; // список точек всего треугольника
        }


        // Триангуляция (по грани возвращает список треугольников)
        private static List<List<Point3D>> Triangulate(List<Point3D> points)
        {
            if (points.Count == 3)
                return new List<List<Point3D>> { points };

            List<List<Point3D>> res = new List<List<Point3D>>();
            for (int i = 2; i < points.Count; i++)
            {
                res.Add(new List<Point3D> { points[0], points[i - 1], points[i] });
            }

            return res;
        }

        public static List<Point3D> MakeProj(List<Point3D> init) => new Projection().Project3(init, ProjMode);

        // d = f(i)
        // получает две точки (i0, d0), (i1, d1)
        public static List<int> Interpolate(int i0, int d0, int i1, int d1)
        {
            if (i0 == i1)
            {
                return new List<int> { d0 };
            }
            List<int> res = new List<int>();

            float step = (d1 - d0) * 1.0f / (i1 - i0);
            float value = d0;
            for (int i = i0; i <= i1; i++)
            {
                res.Add((int)value);
                value += step;
            }

            return res;
        }

        public static List<double> Interpolate(int i0, double h0, int i1, double h1)
        {
            if (i0 == i1)
            {
                return new List<double> { h0 };
            }
            List<double> res = new List<double>();

            double step = (h1 - h0) / (i1 - i0);
            double value = h0;
            for (int i = i0; i <= i1; i++)
            {
                res.Add(value);
                value += step;
            }

            return res;
        }
    }
}
