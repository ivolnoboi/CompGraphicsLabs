using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGraphicsInd02_Savelev
{
    public class AffineTransformations
    {
        public static Point pRotateXAxis(Point point, double angle)
        {
            double[,] left = { { point.x, point.y, point.z, 1 } };
            double[,] right = { { 1, 0, 0, 0},
                                    { 0, Math.Cos(angle), -Math.Sin(angle), 0 },
                                    { 0, Math.Sin(angle), Math.Cos(angle), 0 },
                                    { 0, 0, 0, 1 }};
            double[,] points = Proections.multilyMatrix(left, right);

            return new Point(points[0, 0], points[0, 1], points[0, 2]);
        }

        public static Point pRotateYAxis(Point point, double angle)
        {
            double[,] left = { { point.x, point.y, point.z, 1 } };
            double[,] right = { { Math.Cos(angle), 0, Math.Sin(angle), 0},
                                    { 0, 1, 0, 0 },
                                    { -Math.Sin(angle), 0, Math.Cos(angle), 0 },
                                    { 0, 0, 0, 1 }};
            double[,] points = Proections.multilyMatrix(left, right);

            return new Point(points[0, 0], points[0, 1], points[0, 2]);
        }

        public static Point pRotateZAxis(Point point, double angle)
        {
            double[,] left = { { point.x, point.y, point.z, 1 } };
            double[,] right = { { Math.Cos(angle), -Math.Sin(angle), 0, 0},
                                    { Math.Sin(angle), Math.Cos(angle), 0, 0 },
                                    { 0, 0, 1, 0 },
                                    { 0, 0, 0, 1 }};
            double[,] points = Proections.multilyMatrix(left, right);

            return new Point(points[0, 0], points[0, 1], points[0, 2]);
        }

        public static Polyhedron rotateXAxis(Polyhedron polyhedron, double angle)
        {
            double rads = angle * Math.PI / 180;
            Polyhedron res = polyhedron.DeepCopy();
            for (int i = 0; i < res._points.Count; i++)
            {
                res._points[i] = pRotateXAxis(res._points[i], rads);
            }
            return res;
        }

        public static Polyhedron rotateYAxis(Polyhedron polyhedron, double angle)
        {
            double rads = angle * Math.PI / 180;
            Polyhedron res = polyhedron.DeepCopy();
            for (int i = 0; i < res._points.Count; i++)
            {
                res._points[i] = pRotateYAxis(res._points[i], rads);
            }
            return res;
        }

        public static Polyhedron rotateZAxis(Polyhedron polyhedron, double angle)
        {
            double rads = angle * Math.PI / 180;
            Polyhedron res = polyhedron.DeepCopy();
            for (int i = 0; i < res._points.Count; i++)
            {
                res._points[i] = pRotateZAxis(res._points[i], rads);
            }
            return res;
        }

        public static Point ptranslation(Point point, double tx, double ty, double tz)
        {
            double[,] left = { { point.x, point.y, point.z, 1 } };
            double[,] right = { { 1, 0, 0, 0},
                                    { 0, 1, 0, 0 },
                                    { 0, 0, 1, 0 },
                                    { tx, ty, tz, 1 }};
            double[,] points = Proections.multilyMatrix(left, right);

            return new Point(points[0, 0], points[0, 1], points[0, 2]);
        }

        public static Polyhedron translation(Polyhedron polyhedron, double tx, double ty, double tz)
        {
            Polyhedron res = polyhedron.DeepCopy();
            for (int i = 0; i < res._points.Count; i++)
            {
                res._points[i] = ptranslation(res._points[i], tx, ty, tz);
            }
            return res;
        }

        private static Point pscale(Point point, double scale)
        {
            double[,] left = { { point.x, point.y, point.z, 1 } };
            double[,] right = { { scale, 0, 0, 0},
                                    { 0, scale, 0, 0 },
                                    { 0, 0, scale, 0 },
                                    { 0, 0, 0, 1 }};
            double[,] points = Proections.multilyMatrix(left, right);

            return new Point(points[0, 0], points[0, 1], points[0, 2]);
        }

        public static Polyhedron scalePoint(Polyhedron polyhedron, double scale, double dx, double dy, double dz)
        {
            Polyhedron res = polyhedron.DeepCopy();
            for (int i = 0; i < res._points.Count; i++)
            {
                res._points[i] = ptranslation(pscale(ptranslation(res._points[i], -dx, -dy, -dz), scale), dx, dy, dz);
            }
            return res;
        }
        public static Polyhedron scaleCenter(Polyhedron polyhedron, double scale)
        {

            Polyhedron res = polyhedron.DeepCopy();
            double dx = polyhedron.center().x;
            double dy = polyhedron.center().y;
            double dz = polyhedron.center().z;
            for (int i = 0; i < res._points.Count; i++)
            {
                res._points[i] = ptranslation(pscale(ptranslation(res._points[i], -dx, -dy, -dz), scale), dx, dy, dz);
            }

            return res;
        }

        public static Point preflectX(Point point)
        {
            double[,] left = { { point.x, point.y, point.z, 1 } };
            double[,] right = { { -1, 0, 0, 0},
                                    { 0, 1, 0, 0 },
                                    { 0, 0, 1, 0 },
                                    { 0, 0, 0, 1 }};
            double[,] points = Proections.multilyMatrix(left, right);

            return new Point(points[0, 0], points[0, 1], points[0, 2]);
        }

        public static Polyhedron reflectX(Polyhedron polyhedron)
        {
            Polyhedron res = polyhedron.DeepCopy();
            for (int i = 0; i < res._points.Count; i++)
            {
                res._points[i] = preflectX(res._points[i]);
            }
            return res;
        }

        public static Point preflectY(Point point)
        {
            double[,] left = { { point.x, point.y, point.z, 1 } };
            double[,] right = { { 1, 0, 0, 0},
                                    { 0, -1, 0, 0 },
                                    { 0, 0, 1, 0 },
                                    { 0, 0, 0, 1 }};
            double[,] points = Proections.multilyMatrix(left, right);

            return new Point(points[0, 0], points[0, 1], points[0, 2]);
        }

        public static Polyhedron reflectY(Polyhedron polyhedron)
        {
            Polyhedron res = polyhedron.DeepCopy();
            for (int i = 0; i < res._points.Count; i++)
            {
                res._points[i] = preflectY(res._points[i]);
            }
            return res;
        }

        public static Point preflectZ(Point point)
        {
            double[,] left = { { point.x, point.y, point.z, 1 } };
            double[,] right = { { 1, 0, 0, 0},
                                    { 0, 1, 0, 0 },
                                    { 0, 0, -1, 0 },
                                    { 0, 0, 0, 1 }};
            double[,] points = Proections.multilyMatrix(left, right);

            return new Point(points[0, 0], points[0, 1], points[0, 2]);
        }

        public static Polyhedron reflectZ(Polyhedron polyhedron)
        {
            Polyhedron res = polyhedron.DeepCopy();
            for (int i = 0; i < res._points.Count; i++)
            {
                res._points[i] = preflectZ(res._points[i]);
            }
            return res;
        }

        public static Polyhedron rotateCenterOX(Polyhedron polyhedron, double angle)
        {
            Polyhedron res = polyhedron.DeepCopy();
            double dx = polyhedron.center().x;
            double dy = polyhedron.center().y;
            double dz = polyhedron.center().z;
            for (int i = 0; i < res._points.Count; i++)
            {
                res._points[i] = ptranslation(pRotateXAxis(ptranslation(res._points[i], -dx, -dy, -dz), angle), dx, dy, dz);
            }
            return res;
        }

        public static Polyhedron rotateCenterOY(Polyhedron polyhedron, double angle)
        {
            Polyhedron res = polyhedron.DeepCopy();
            double dx = polyhedron.center().x;
            double dy = polyhedron.center().y;
            double dz = polyhedron.center().z;
            for (int i = 0; i < res._points.Count; i++)
            {
                res._points[i] = ptranslation(pRotateYAxis(ptranslation(res._points[i], -dx, -dy, -dz), angle), dx, dy, dz);
            }
            return res;
        }

        public static Polyhedron rotateCenterOZ(Polyhedron polyhedron, double angle)
        {
            Polyhedron res = polyhedron.DeepCopy();
            double dx = polyhedron.center().x;
            double dy = polyhedron.center().y;
            double dz = polyhedron.center().z;
            for (int i = 0; i < res._points.Count; i++)
            {
                res._points[i] = ptranslation(pRotateZAxis(ptranslation(res._points[i], -dx, -dy, -dz), angle), dx, dy, dz);
            }
            return res;
        }

        public static Polyhedron rotateLine(Polyhedron polyhedron, Point start, Point end, double rotAngle)
        {
            Polyhedron res = polyhedron.DeepCopy();

            Point lineCenter = new Point((start.x + end.x) / 2, (start.y + end.y) / 2, (start.z + end.z) / 2);

            Point axisStart = new Point(0, 0, 0);
            Point axisEnd = new Point(1, 0, 0);

            Point directLine = new Point(end.x - start.x, end.y - start.y, end.z - start.z);
            Point directAxis = new Point(1, 0, 0);

            double angle = ((directLine.x * directAxis.x) + (directLine.y * directAxis.y) + (directLine.z + directAxis.z)) / 
                (Math.Sqrt(directLine.x * directLine.x + directLine.y * directLine.y + directLine.z * directLine.z) * Math.Sqrt(directAxis.x * directAxis.x + directAxis.y * directAxis.y + directAxis.z * directAxis.z));

            angle = (angle * Math.PI) / 180;
            angle = Math.Acos(angle);

            double dx = polyhedron.center().x;
            double dy = polyhedron.center().y;
            double dz = polyhedron.center().z;
            double radRotAngle = (rotAngle * Math.PI) / 180;
            for (int i = 0; i < res._points.Count; i++)
            {
                res._points[i] = pRotateYAxis(ptranslation(res._points[i], -lineCenter.x, -lineCenter.y, -lineCenter.z), -angle);
                res._points[i] = pRotateXAxis(res._points[i], radRotAngle);
                res._points[i] = ptranslation(pRotateYAxis(res._points[i], angle), lineCenter.x, lineCenter.y, lineCenter.z);
            }
            return res;
        }
    }
}