using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSpy
{
    internal class Collision
    {
        public List<Wall> Walls = new List<Wall>();
        public class Wall
        {
            public Point StartWall = Point.Zero;
            public Point EndWall = Point.Zero;
            public Wall(Point StartWall, Point EndWall)
            {
                this.StartWall = StartWall;
                this.EndWall = EndWall;
            }
            public void ChangeScaleWall(float OldScale, float NewScale)
            {
                StartWall.X /= (int)OldScale;
                StartWall.Y /= (int)OldScale;
                EndWall.X /= (int)OldScale;
                EndWall.Y /= (int)OldScale;

                StartWall.X *= (int)NewScale;
                StartWall.Y *= (int)NewScale;
                EndWall.X *= (int)NewScale;
                EndWall.Y *= (int)NewScale;
            }
        }


        public bool CheckCollision(Point diagonalStart, Point diagonalEnd, Rectangle rectangle)
        {
            // Получаем координаты вершин прямоугольника
            int left = rectangle.Left;
            int top = rectangle.Top;
            int right = rectangle.Right;
            int bottom = rectangle.Bottom;

            // Проверяем пересечение отрезков
            bool intersects1 = LineIntersectsLine(diagonalStart, diagonalEnd, new Point(left, top), new Point(right, top));
            bool intersects2 = LineIntersectsLine(diagonalStart, diagonalEnd, new Point(right, top), new Point(right, bottom));
            bool intersects3 = LineIntersectsLine(diagonalStart, diagonalEnd, new Point(right, bottom), new Point(left, bottom));
            bool intersects4 = LineIntersectsLine(diagonalStart, diagonalEnd, new Point(left, bottom), new Point(left, top));

            // Если хотя бы один отрезок пересекается с диагональю, значит произошло столкновение
            return intersects1 || intersects2 || intersects3 || intersects4;
        }

        public bool LineIntersectsLine(Point line1Start, Point line1End, Point line2Start, Point line2End)
        {
            int denominator = ((line2End.Y - line2Start.Y) * (line1End.X - line1Start.X)) - ((line2End.X - line2Start.X) * (line1End.Y - line1Start.Y));
            if (denominator == 0)
            {
                return false;
            }

            int numerator1 = ((line2End.X - line2Start.X) * (line1Start.Y - line2Start.Y)) - ((line2End.Y - line2Start.Y) * (line1Start.X - line2Start.X));
            int numerator2 = ((line1End.X - line1Start.X) * (line1Start.Y - line2Start.Y)) - ((line1End.Y - line1Start.Y) * (line1Start.X - line2Start.X));

            if (numerator1 == 0 || numerator2 == 0)
            {
                return false;
            }

            double r = (double)numerator1 / denominator;
            double s = (double)numerator2 / denominator;

            return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        }
    }
}
