using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGraphicsLab06
{
    class GouraudShading
    {
        private static double CalculateLenghtOfVec(Point3D vec)
        {
            return Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
        }

        private static double ModelLambert(Point3D pt, Point3D normal, Point3D light)
        {
            Point3D vectr = new Point3D(light.X - pt.X, light.Y - pt.Y, light.Z - pt.Z);
            double cos = (vectr.X * normal.X + vectr.Y * normal.Y + vectr.Z * normal.Z) / (CalculateLenghtOfVec(normal) * CalculateLenghtOfVec(vectr));
            return cos;
        }

        private static Point3D CalculateNormal(List<List<int>> faces, Polyhedron polyhedron)
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

        private static Point3D CalculateNormalFace(List<int> face, Polyhedron polyhedron)
        {
            Point3D p0 = polyhedron.Vertexes[0];
            Point3D p1 = polyhedron.Vertexes[1];
            Point3D p2 = polyhedron.Vertexes[polyhedron.Vertexes.Count - 1];
            Point3D v1 = new Point3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            Point3D v2 = new Point3D(p2.X - p0.X, p2.Y - p0.Y, p2.Z - p0.Z);
            return CrossProduct(v1, v2);
        }

        // Векторное произведение векторов
        private static Point3D CrossProduct(Point3D vec1, Point3D vec2)
        {
            float x = vec1.Y * vec2.Z - vec1.Z * vec2.Y;
            float y = vec1.Z * vec2.X - vec1.X * vec2.Z;
            float z = vec1.X * vec2.Y - vec1.Y * vec2.X;
            return new Point3D(x, y, z);
        }

        private static double cosToBrightness(double cos)
        {
            return (cos + 1) / 2;
        }
        private static void calculate_shading(Polyhedron polyhedron, Point3D light)
        {
            // Добавление нормали к каждой вершине
            Dictionary<int, Point3D> pointNormal = new Dictionary<int, Point3D>(); // индекс точки - нормаль
            for (int i = 0; i < polyhedron.Vertexes.Count; i++)
            {
                List<List<int>> neededFaces = polyhedron.Faces.Where(x => x.Contains(i)).ToList();
                pointNormal.Add(i, CalculateNormal(neededFaces, polyhedron));
            }

            // Вычисление цвета по модели Ламберта в каждой вершине
            for (int i = 0; i < polyhedron.Vertexes.Count; i++)
            {
                polyhedron.Vertexes[i].illumination = cosToBrightness(ModelLambert(polyhedron.Vertexes[i], pointNormal[i], light));
            }
        }

        private static int ProjMode = 0;
        private static List<Point3D> vertexes;
        public static Bitmap Gouraud(int width, int heigh, Polyhedron pl, Color color, Point3D ligth, int projMode = 0)
        {
            calculate_shading(pl, ligth); // вычисление яркостей
            vertexes = pl.Vertexes;
            ProjMode = projMode;

            Bitmap newImg = new Bitmap(width, heigh);
            for (int i = 0; i < width; i++)
                for (int j = 0; j < heigh; j++)
                    newImg.SetPixel(i, j, Color.White);

            float[,] zbuff = new float[width, heigh];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < heigh; j++)
                    zbuff[i, j] = float.MinValue;

            // Многогранник <-- Грань <-- Точка - освещенность 
            List<List<Tuple<Point3D, double>>> rasterizedPolyhedron = Rasterize(pl);

            var centerX = width / 2;
            var centerY = heigh / 2;

            //Смещение по центру фигуры
            var figureLeftX = rasterizedPolyhedron.Where(face => face.Count != 0).Min(face => face.Min(vertex => vertex.Item1.X));
            var figureLeftY = rasterizedPolyhedron.Where(face => face.Count != 0).Min(face => face.Min(vertex => vertex.Item1.Y));
            var figureRightX = rasterizedPolyhedron.Where(face => face.Count != 0).Max(face => face.Max(vertex => vertex.Item1.X));
            var figureRightY = rasterizedPolyhedron.Where(face => face.Count != 0).Max(face => face.Max(vertex => vertex.Item1.Y));
            var figureCenterX = (figureRightX - figureLeftX) / 2;
            var figureCenterY = (figureRightY - figureLeftY) / 2;

            for (int i = 0; i < rasterizedPolyhedron.Count; i++)
            {
                List<Tuple<Point3D, double>> curr = rasterizedPolyhedron[i]; // текущая грань
                foreach (Tuple<Point3D, double> point in curr)
                {
                    int x = (int)(point.Item1.X + centerX - figureCenterX);
                    int y = (int)(point.Item1.Y + centerY - figureCenterY);
                    if (x < width && y < heigh && x > 0 && y > 0)
                    {
                        if (point.Item1.Z > zbuff[x, y])
                        {
                            zbuff[x, y] = point.Item1.Z;
                            newImg.SetPixel(x, y, Color.FromArgb((int)(color.R * point.Item2), (int)(color.G * point.Item2), (int)(color.B * point.Item2)));
                        }
                    }
                }
            }
            return newImg;
        }

        // Растеризирует многогранник
        private static List<List<Tuple<Point3D, double>>> Rasterize(Polyhedron polyhedron)
        {
            List<List<Tuple<Point3D, double>>> rasterized = new List<List<Tuple<Point3D, double>>>();
            // Каждую грань многогранника триангулируем (бьём на треугольники)
            foreach (var facet in polyhedron.Faces)
            {
                List<Tuple<Point3D, double>> currentFac = new List<Tuple<Point3D, double>>();
                List<Point3D> facetPoint3Ds = new List<Point3D>(); // точки вершин, принадлежащих данной грани
                for (int i = 0; i < facet.Count; i++)
                {
                    facetPoint3Ds.Add(polyhedron.Vertexes[facet[i]]);
                }

                List<List<Point3D>> triangles = Triangulate(facetPoint3Ds); // триангулируем
                foreach (List<Point3D> triangle in triangles)
                {
                    currentFac.AddRange(RasterizeTriangle(MakeProj(triangle)));
                }
                rasterized.Add(currentFac);
            }
            return rasterized;
        }

        // points - вершины треугольника
        // возвращает растеризованный треугольник (список точек, находящихся полностью внутри треугольника и на рёбрах)
        private static List<Tuple<Point3D, double>> RasterizeTriangle(List<Point3D> points)
        {
            List<Tuple<Point3D, double>> res = new List<Tuple<Point3D, double>>();

            points.Sort((point1, point2) => point1.Y.CompareTo(point2.Y)); // сортируем по возрастанию Y
            var rpoints = points.Select(point => (X: (int)Math.Round(point.X), Y: (int)Math.Round(point.Y), Z: (int)Math.Round(point.Z), ilum: point.illumination)).ToList();

            var x01 = Interpolate(rpoints[0].Y, rpoints[0].X, rpoints[1].Y, rpoints[1].X);
            var x12 = Interpolate(rpoints[1].Y, rpoints[1].X, rpoints[2].Y, rpoints[2].X);
            var x02 = Interpolate(rpoints[0].Y, rpoints[0].X, rpoints[2].Y, rpoints[2].X);

            var z01 = Interpolate(rpoints[0].Y, rpoints[0].Z, rpoints[1].Y, rpoints[1].Z);
            var z12 = Interpolate(rpoints[1].Y, rpoints[1].Z, rpoints[2].Y, rpoints[2].Z);
            var z02 = Interpolate(rpoints[0].Y, rpoints[0].Z, rpoints[2].Y, rpoints[2].Z);


            var h01 = Interpolate(rpoints[0].Y, rpoints[0].ilum, rpoints[1].Y, rpoints[1].ilum);
            var h12 = Interpolate(rpoints[1].Y, rpoints[1].ilum, rpoints[2].Y, rpoints[2].ilum);
            var h02 = Interpolate(rpoints[0].Y, rpoints[0].ilum, rpoints[2].Y, rpoints[2].ilum);

            x01.RemoveAt(x01.Count - 1);
            List<int> x012 = x01.Concat(x12).ToList();

            z01.RemoveAt(z01.Count - 1);
            List<int> z012 = z01.Concat(z12).ToList();

            h01.RemoveAt(h01.Count - 1);
            List<double> h012 = h01.Concat(h12).ToList();

            int middle = x012.Count / 2;
            List<int> leftX, rightX, leftZ, rightZ;
            List<double> leftH, rigthH;
            if (x02[middle] < x012[middle])
            {
                leftX = x02;
                leftZ = z02;
                leftH = h02;
                rightX = x012;
                rightZ = z012;
                rigthH = h012;
            }
            else
            {
                leftX = x012;
                leftZ = z012;
                leftH = h012;
                rightX = x02;
                rightZ = z02;
                rigthH = h02;
            }

            int y0 = rpoints[0].Y;
            int y2 = rpoints[2].Y;

            for (int ind = 0; ind <= y2 - y0; ind++) // двигаемся по Y (построчно) и интерполируем Z и H в зависимости от X 
            {                                        // и получаем список точек, которые внутри треугольника
                int XL = leftX[ind];
                int XR = rightX[ind];

                List<int> intCurrZ = Interpolate(XL, leftZ[ind], XR, rightZ[ind]);
                List<double> doubleCurrH = Interpolate(XL, leftH[ind], XR, rigthH[ind]);

                for (int x = XL; x < XR; x++)
                {
                    res.Add(new Tuple<Point3D, double>(new Point3D(x, y0 + ind, intCurrZ[x - XL]), doubleCurrH[x - XL]));
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

        // d = f(i)
        // получает две точки (i0, d0), (i1, d1)
        private static List<int> Interpolate(int i0, int d0, int i1, int d1)
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

        private static List<double> Interpolate(int i0, double h0, int i1, double h1)
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

        public static List<Point3D> MakeProj(List<Point3D> init) => new Projection().Project3(init, ProjMode);
    }
}
