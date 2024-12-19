#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2020, all rigths reserved.*/
#endregion
#region Usings
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools.Geometry.ContourData;

#endregion

namespace Tools.Geometry
{
    public static class Contour
    {
        /// <summary>
        /// Перемещает одну из линий контура на заданное расстояние
        /// </summary>
        /// <param name="loop">Линии первоначального контура</param>
        /// <param name="delta">Положительное значение - смещение наружу, отрицательное - внутрь</param>
        /// <param name="side">Одна из сторон</param>
        /// <returns>Набор линий, в котором одна из линий смещена на расстояние</returns>
        public static List<Curve> MoveLine(List<Curve> lines, double delta, LineSide side)
        {
            List<Line> loop = lines.Cast<Line>().ToList();

            List<Line> sideLines = GetSideLines(loop, side);

            XYZ moveVector = null;
            if (side == LineSide.Top)
                moveVector = new XYZ(0, 0, delta);
            else if (side == LineSide.Bottom)
                moveVector = new XYZ(0, 0, delta);
            else
            {
                List<Line> verticalLines = loop.Where(i => Math.Abs(i.Direction.Z) == 1).ToList();

                List<XYZ> xyPoints = verticalLines.Select(i => new XYZ(i.GetEndPoint(0).X, i.GetEndPoint(1).Y, 0)).ToList();

                XYZ directionPoint0 = xyPoints[0];
                XYZ directionPoint1 = null;

                foreach (XYZ point in xyPoints)
                {
                    bool overlap = Points.CheckPointsIsOverlap(directionPoint0, point);
                    if (overlap) continue;
                    directionPoint1 = point;
                    break;
                }


                XYZ wallDirectionVector = VectorsMath.getVectorFromTwoPoints(directionPoint1, directionPoint0);
                XYZ normalizedDirection = VectorsMath.normalizeVector(wallDirectionVector);

                if (side == LineSide.Left) delta = -delta;

                moveVector = new XYZ(normalizedDirection.X * delta, normalizedDirection.Y * delta, 0);
            }


            foreach (Line curLine in sideLines)
            {
                XYZ p1 = curLine.GetEndPoint(0);
                XYZ p1new = new XYZ(p1.X + moveVector.X, p1.Y + moveVector.Y, p1.Z + moveVector.Z);

                XYZ p2 = curLine.GetEndPoint(1);
                XYZ p2new = new XYZ(p2.X + moveVector.X, p2.Y + moveVector.Y, p2.Z + moveVector.Z);

                Line mainLineNew = Line.CreateBound(p1new, p2new);
                SearchLineResult res0 = GetJointLineAtEnd(curLine, loop, 0);

                Line prevLineNew = MoveJointLine(res0, moveVector);

                SearchLineResult res1 = GetJointLineAtEnd(curLine, loop, 1);
                Line nextLine = res1.Line;

                Line nextLineNew = MoveJointLine(res1, moveVector);

                loop[loop.IndexOf(curLine)] = mainLineNew;
                loop[loop.IndexOf(res0.Line)] = prevLineNew;
                loop[loop.IndexOf(res1.Line)] = nextLineNew;

                /*loop.Remove(curLine);
                loop.Remove(res0.Line);
                loop.Remove(res1.Line);
                loop.Add(mainLineNew);
                loop.Add(prevLineNew);
                loop.Add(nextLineNew);*/
            }

            return loop.Cast<Curve>().ToList();
        }



        /// <summary>
        /// Создает новый контур из верхней линии, скопированной вниз
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static List<List<Curve>> CopyTopOrBottomLines(List<Curve> lines, double delta, LineSide side)
        {
            if (side == LineSide.Left || side == LineSide.Right) throw new Exception("Not supported line side");
            if (side == LineSide.Bottom)
            {
                delta = -delta;
            }

            List<Line> loop = lines.Cast<Line>().ToList();

            List<Line> linesToMove = GetSideLines(loop, side);

            List<List<Curve>> profiles = new List<List<Curve>>();

            foreach (Line topLine in linesToMove)
            {
                XYZ topLine1 = topLine.GetEndPoint(0);
                XYZ topLine2 = topLine.GetEndPoint(1);
                XYZ bottomLine1 = new XYZ(topLine1.X, topLine1.Y, topLine1.Z - delta);
                XYZ bottomLine2 = new XYZ(topLine2.X, topLine2.Y, topLine2.Z - delta);

                Line bottomLine = Line.CreateBound(bottomLine1, bottomLine2);
                Line side1 = Line.CreateBound(topLine1, bottomLine1);
                Line side2 = Line.CreateBound(topLine2, bottomLine2);

                List<Curve> newLoop = new List<Curve> { topLine, side2, bottomLine, side1 };
                profiles.Add(newLoop);
            }
            return profiles;
        }


        private static Line MoveJointLine(SearchLineResult res, XYZ delta)
        {
            Line prevLine = res.Line;
            Line prevLineNew = null;
            if (res.EndpointNumber == 0)
            {
                XYZ p = prevLine.GetEndPoint(0);
                XYZ newp = new XYZ(p.X + delta.X, p.Y + delta.Y, p.Z + delta.Z);
                prevLineNew = Line.CreateBound(newp, prevLine.GetEndPoint(1));
            }
            if (res.EndpointNumber == 1)
            {
                XYZ p = prevLine.GetEndPoint(1);
                XYZ newp = new XYZ(p.X + delta.X, p.Y + delta.Y, p.Z + delta.Z);
                prevLineNew = Line.CreateBound(prevLine.GetEndPoint(0), newp);
            }
            return prevLineNew;
        }



        /// <summary>
        /// Возвращает нижнюю или верхнюю горизонтальную линию
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<Line> GetSideLines(List<Line> lines, LineSide side)
        {
            if (side == LineSide.Bottom || side == LineSide.Top)
            {
                List<Line> horizontalLines = lines
                    .Where(i => CheckDoubleEquals(i.Direction.Z, 0))
                    .ToList();

                Line l = lines.First();
                double heigth = 0;
                if (side == LineSide.Top) heigth = -999999;
                if (side == LineSide.Bottom) heigth = 999999;

                foreach (Line curLine in horizontalLines)
                {
                    double curHeigth = curLine.GetEndPoint(0).Z;
                    if (side == LineSide.Top)
                    {
                        if (curHeigth > heigth)
                        {
                            l = curLine;
                            heigth = curHeigth;
                        }
                    }
                    else if (side == LineSide.Bottom)
                    {
                        if (curHeigth < heigth)
                        {
                            l = curLine;
                            heigth = curHeigth;
                        }
                    }
                }

                //определю все линии на такой же высоте
                List<Line> resultLines = horizontalLines
                    .Where(i => CheckDoubleEquals(i.Origin.Z, l.Origin.Z))
                    .ToList();

                return resultLines;
            }
            else
            {
                List<Line> verticalLines = lines.Where(i => Math.Abs(i.Direction.Z) == 1).ToList();
                Line[] leftAndRightLines = GetLeftAndRightLine(verticalLines);
                Line l = null;
                if (side == LineSide.Left)
                    l = leftAndRightLines[0];
                else
                    l = leftAndRightLines[1];

                List<Line> resultLines = verticalLines
                    .Where(i => CheckDoubleEquals(i.Origin.X, l.Origin.X) && CheckDoubleEquals(i.Origin.Y, l.Origin.Y))
                    .ToList();

                return resultLines;
            }
        }



        /// <summary>
        /// ПОлучает крайнюю правую и левую вертикальную линию из контура
        /// </summary>
        /// <param name="verticalLines"></param>
        /// <returns></returns>
        public static Line[] GetLeftAndRightLine(List<Line> verticalLines)
        {
            double distance = 0;

            Line[] result = new Line[2];
            for (int i = 0; i < verticalLines.Count - 1; i++)
            {
                Line l1 = verticalLines[i];
                XYZ point1 = l1.GetEndPoint(0);
                XYZ p1 = new XYZ(point1.X, point1.Y, 0);
                for (int j = 1; j < verticalLines.Count; j++)
                {
                    Line l2 = verticalLines[j];
                    XYZ point2 = l2.GetEndPoint(0);
                    XYZ p2 = new XYZ(point2.X, point2.Y, 0);
                    double temp = Points.GetLengthBetweenPoints(p1, p2);
                    if (temp > distance)
                    {
                        distance = temp;
                        result[0] = l1;
                        result[1] = l2;
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Возвращает линию, примыкающую к данной линии
        /// </summary>
        /// <param name="l"></param>
        /// <param name="lines"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static SearchLineResult GetJointLineAtEnd(Line l, List<Line> lines, int endpoint)
        {
            Line thisLine = null;
            foreach (Line curLine in lines)
            {
                bool check = CheckLinesIsOverlap(l, curLine);
                if (check)
                {
                    thisLine = curLine;
                    break;
                }
            }

            foreach (Line curLine in lines)
            {
                bool check = CheckLinesIsOverlap(thisLine, curLine);
                if (check) continue;

                int[] jointPoints = CheckLinesHaveJoinPoint(thisLine, curLine, true);
                if (jointPoints == null) continue;
                if (jointPoints[0] == endpoint)
                {
                    SearchLineResult result = new SearchLineResult(curLine, jointPoints[1]);
                    return result;
                }
            }
            return null;
        }



        /// <summary>
        /// Выполняет очистку списка от параллельных совпадающих линий.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static List<Curve> CleanLoop(List<Curve> lines)
        {
        Start:
            for (int i = 0; i < lines.Count; i++)
            {
                Line l1 = lines[i] as Line;
                if (l1 == null) continue;
                for (int j = 0; j < lines.Count; j++)
                {
                    Line l2 = lines[j] as Line;
                    if (l2 == null) continue;

                    bool checkOverlap = CheckLinesIsOverlap(l1, l2);
                    if (checkOverlap) continue;

                    bool checkParallel = CheckLinesIsParallel(l1, l2);
                    if (!checkParallel) continue;

                    int[] checkJointPoint = CheckLinesHaveJoinPoint(l1, l2, false);
                    if (checkJointPoint == null) continue;


                    //если оказался здесь - значит, l1 и l2 можно объединить
                    //получу объединенную линию
                    XYZ p1 = l1.GetEndPoint(checkJointPoint[0]);
                    XYZ p2 = l2.GetEndPoint(checkJointPoint[1]);
                    Line l3 = Line.CreateBound(p1, p2);

                    //удаляю линии из списка и добавляю объединенную
                    lines.Remove(l1);
                    lines.Remove(l2);
                    lines.Add(l3);

                    //и начинаем проход по циклу заново
                    goto Start;

                    //lines = CleanLoop(lines);
                }
            }

            return lines;
        }


        /// <summary>
        /// Проверяет, параллельны ли линии.
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        private static bool CheckLinesIsParallel(Line l1, Line l2)
        {
            XYZ v1 = VectorsMath.GetVectorFromLine(l1);
            XYZ v2 = VectorsMath.GetVectorFromLine(l2);
            XYZ crossProduct = VectorsMath.CrossProduct(v1, v2);
            double length = VectorsMath.GetVectorLength(crossProduct);
            if (length < 0.000001) return true;
            return false;
        }

        /// <summary>
        /// Проверяет, совпадают ли линии.
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        private static bool CheckLinesIsOverlap(Line l1, Line l2)
        {
            XYZ p11 = l1.GetEndPoint(0);
            XYZ p12 = l1.GetEndPoint(1);
            XYZ p21 = l2.GetEndPoint(0);
            XYZ p22 = l2.GetEndPoint(1);

            bool check11 = Points.CheckPointsIsOverlap(p11, p21);
            if (check11)
            {
                bool check22 = Points.CheckPointsIsOverlap(p12, p22);
                if (check22) return true;
            }

            bool check12 = Points.CheckPointsIsOverlap(p11, p22);
            if (check12)
            {
                bool check21 = Points.CheckPointsIsOverlap(p12, p21);
                if (check21) return true;
            }
            return false;
        }


        /// <summary>
		/// Проверка, если ли у линий совпадающая точка.
		/// </summary>
		/// <param name="l1"></param>
		/// <param name="l2"></param>
		/// <returns>null если совпадающей точки нет. иначе указываются номера точек, которые не совпадают.</returns>
		private static int[] CheckLinesHaveJoinPoint(Line l1, Line l2, bool GetCommonPoint)
        {
            XYZ p1 = l1.GetEndPoint(0);
            XYZ p2 = l2.GetEndPoint(0);
            bool check = false;

            check = Points.CheckPointsIsOverlap(p1, p2);
            if (check)
            {
                if (GetCommonPoint) return new int[] { 0, 0 };
                else return new int[] { 1, 1 };
            }


            p1 = l1.GetEndPoint(1);
            check = Points.CheckPointsIsOverlap(p1, p2);
            if (check)
            {
                if (GetCommonPoint) return new int[] { 1, 0 };
                else return new int[] { 0, 1 };
            }

            p2 = l2.GetEndPoint(1);
            check = Points.CheckPointsIsOverlap(p1, p2);
            if (check)
            {
                if (GetCommonPoint) return new int[] { 1, 1 };
                else return new int[] { 0, 0 };
            }

            p1 = l1.GetEndPoint(0);
            check = Points.CheckPointsIsOverlap(p1, p2);
            if (check)
            {
                if (GetCommonPoint) return new int[] { 0, 1 };
                return new int[] { 1, 0 };
            }

            return null;
        }




        public static bool CheckDoubleEquals(double d1, double d2)
        {
            double delta = d2 - d1;
            double abs = Math.Abs(delta);
            if (abs < 0.000001) return true;
            return false;
        }



        public static List<Curve> GetFloorOuterBoundary(Floor floor)
        {
            Reference rf = HostObjectUtils.GetTopFaces(floor).First();
            Face face = floor.GetGeometryObjectFromReference(rf) as Face;

            PlanarFace pface = face as PlanarFace;
            if (pface == null) return null;

            List<Curve> profile = FaceUtils.GetFaceOuterBoundary(pface);


            return profile;
        }
    }
}