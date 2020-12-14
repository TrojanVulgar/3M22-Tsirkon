using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractObjectInterface
{
	class Point3D : ICloneable
	{
		private double x;
		private double y;
		private double z;

		public double X
		{
			get { return this.x; }
			set { this.x = cap(value); }
		}

		public double Y
		{
			get { return this.y; }
			set { this.y = cap(value); }
		}

		public double Z
		{
			get { return this.z; }
			set { this.z = cap(value); }
		}

		public Point3D(double X, double Y, double Z)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
		}

		public Point3D()
		{
			this.x = double.NaN;
			this.y = double.NaN;
			this.z = double.NaN;
		}

		public double Magnitude
		{
			get { return Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z); }
		}

		public void Add(Point3D point)
		{
			this.X += point.X;
			this.Y += point.Y;
			this.Z += point.Z;
		}

		public Point3D OverTwoPlus(Point3D point)
		{
			return new Point3D(this.x / 2.0 + point.x, this.y / 2.0 + point.y, this.z / 2.0 + point.z);
		}

		private double cap(double n)
		{
			if (double.IsPositiveInfinity(n))
				return double.MaxValue;
			if (double.IsNegativeInfinity(n))
				return double.MinValue;
			return n;
		}

		public override string ToString()
		{
			return string.Format("({0}, {1}, {2})", Math.Round(this.x, 8), Math.Round(this.y, 8), Math.Round(this.z, 8));
		}

		#region ICloneable Members

		public object Clone()
		{
			return new Point3D(this.x, this.y, this.z);
		}

		#endregion
	}
}
