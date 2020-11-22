using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGraphicsInd02_Savelev
{
    public static class Proections
    {

        public static double[,] multilyMatrix(double[,] left, double[,] right)
        {
            int r1 = left.GetLength(0);
            int c1 = left.GetLength(1);

            int r2 = right.GetLength(0);
            int c2 = right.GetLength(1);

            if (c1 != r2)
            {
                throw new Exception("Matrix multiplication with non-fitting sizes");
            }

            double[,] res = new double[r1, c2];
            for (int i = 0; i < r1; i++)
            {
                for (int j = 0; j < c2; j++)
                {
                    res[i, j] = 0;
                    for (int k = 0; k < c1; k++)
                    {
                        res[i, j] += left[i, k] * right[k, j];
                    }
                }
            }

            return res;
        }

        public static Point pCentralProj(Point point, double xc, double yc, double zc)
        {
            //double a = yc == 0 ? 0 : -1.0 / yc;
            double b = zc == 0 ? 0 : -1.0 / zc;
            //double c = xc == 0 ? 0 : -1.0 / xc;
            double[,] left = { { point.y, point.z, point.x, 1 } };
            double[,] right = { { 1, 0, 0, 0 },
                                { 0, 1, 0, 0 },
                                { 0, 0, 0, b },
                                { 0, 0, 0, 1 }};
            double[,] points = multilyMatrix(left, right);

            return new Point(points[0, 0] / points[0, 3], points[0, 1] / points[0, 3], points[0, 2] / points[0, 3]);
        }

        public static Point pIsometricProj(Point point, double phi, double psi)
        {
            double degPhi = (phi * Math.PI) / 180;
            double degPsi = (psi * Math.PI) / 180;

            double[,] left = { { point.x, point.y, point.z, 1 } };
            double[,] right = { { Math.Cos(degPsi), Math.Sin(degPhi)*Math.Sin(degPsi), 0, 0},
                                    { 0, Math.Cos(degPhi), 0, 0 },
                                    { Math.Sin(degPsi), -Math.Sin(degPhi)*Math.Cos(degPsi), 0, 0 },
                                    { 0, 0, 0, 1 }};

            double[,] points = multilyMatrix(left, right);

            return new Point(points[0, 0], points[0, 1], points[0, 2]);
        }

        public static Point pOrtProj(Point point, char axis)
        {
            double xVal = 1, yVal = 1, zVal = 1;
            switch (axis)
            {
                case 'x':
                    xVal = 0;
                    break;
                case 'y':
                    yVal = 0;
                    break;
                case 'z':
                    zVal = 0;
                    break;
            }
            double[,] left = { { point.x, point.y, point.z, 1 } };
            double[,] right = { { xVal, 0, 0, 0},
                                    { 0, yVal, 0, 0 },
                                    { 0, 0, zVal, 0 },
                                    { 0, 0, 0, 1 }};
            double[,] points = multilyMatrix(left, right);
            double x = 0, y = 0, z = 0;
            switch (axis)
            {
                case 'x':
                    z = points[0, 0];
                    x = points[0, 1];
                    y = points[0, 2];
                    break;
                case 'y':
                    x = points[0, 0];
                    z = points[0, 1];
                    y = points[0, 2];
                    break;
                case 'z':
                    x = points[0, 0];
                    y = points[0, 1];
                    z = points[0, 2];
                    break;
            }
            return new Point(x, y, z);
        }


        public static Polyhedron centralProection(Point center, Polyhedron shape)
        {
            Polyhedron res = shape.DeepCopy();
            for (int i = 0; i < res._points.Count; i++)
            {
                res._points[i] = pCentralProj(res._points[i], center.x, center.y, center.z);
            }
            return res;
        }

        public static Polyhedron isometricProection(double phi, double psi, in Polyhedron shape)
        {
            Polyhedron res = shape.DeepCopy();
            for (int i = 0; i < res._points.Count; i++)
            {
                res._points[i] = pIsometricProj(res._points[i], phi, psi);
            }
            return res;
        }

        public static Polyhedron ortographicProection(char axis, Polyhedron shape)
        {
            Polyhedron res = shape.DeepCopy();
            for (int i = 0; i < res._points.Count; i++)
            {
                res._points[i] = pOrtProj(res._points[i], axis);
            }
            return res;
        }
    }

    public class Camera
    {
        public Point position;
        public double xN, yN, zN;
        public Point n, u, v;

        public Point Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public Point ViewVector
        {
            get
            {
                return n;
            }

            set
            {
                n = value;
            }
        }




        public Camera(double x, double y, double z, double x0, double y0, double z0)

        {
            position = new Point(x, y, z);
            xN = x0;
            yN = y0;
            zN = z0;
            n = normalize(new Point(xN, yN, zN));
            v = AffineTransformations.pRotateZAxis(n, -90.0 * Math.PI / 180);
            u = AffineTransformations.pRotateYAxis(n, 90.0 * Math.PI / 180);
        }

        public static Double scalar(Point vec1, Point vec2)
        {
            return (vec1.x * vec2.x) + (vec1.y * vec2.y) + (vec1.z * vec2.z);
        }

        public static Point normalize(Point vec)
        {
            double len = Math.Sqrt((vec.x * vec.x) + (vec.y * vec.y) + (vec.z * vec.z));
            return new Point(vec.x / len, vec.y / len, vec.z / len);
        }

        public double[,] viewMatrix()
        {
            return new double[4, 4] { { u.x, v.x, n.x, 0 },
                                      { u.y, v.y, n.y, 0 },
                                      { u.z, v.z, n.z, 0 },
                                      {-scalar(u,position),-scalar(v,position),-scalar(n,position),1 }
            };
        }

        public Polyhedron pCam(Polyhedron polyhedron, double x, double y, double z, double pitch, double yaw, double roll)
        {

            Polyhedron res = polyhedron.DeepCopy();

            var cam = new Camera(x, y, z, pitch, yaw, roll);


            for (int j = 0; j < res._points.Count; ++j)
            {

                double[,] left = { { res._points[j].y, res._points[j].z, res._points[j].x, 1 } };
                //var t0 = Proections.multilyMatrix(left, cam.translateAtPosition());
                var t1 = Proections.multilyMatrix(left, cam.viewMatrix());
                res._points[j] = new Point(t1[0, 0], t1[0, 2], t1[0, 1]);
            }
            return res;
        }
    }
}
    
