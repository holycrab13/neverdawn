using System;
using UnityEngine;

namespace nAlpha
{
    public struct Point : IEquatable<Point>
    {
        public bool Equals(Point other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Point && Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode()*397) ^ Y.GetHashCode();
            }
        }

        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !left.Equals(right);
        }

        public float X;
        public float Y;

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return "X={X}; Y={Y}";
        }

        public float DistanceTo(Point p)
        {
            return Mathf.Sqrt((X - p.X)*(X - p.X) + (Y - p.Y)*(Y - p.Y));
        }

        public Point CenterTo(Point p)
        {
            return new Point((X + p.X)/2, (Y + p.Y)/2);
        }

        public Point VectorTo(Point p)
        {
            float d = DistanceTo(p);
            return new Point((p.X - X)/d, 
                (p.Y - Y)/d);
        }
    }
}