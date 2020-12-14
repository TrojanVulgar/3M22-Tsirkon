using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractObjectInterface
{
	class AbstractObject : ICloneable
	{
		public Point3D A;
		public Point3D V;
		public Point3D P;

		public AbstractObject(Point3D A, Point3D V, Point3D P)
		{
			this.A = A;
			this.V = V;
			this.P = P;
		}

		public void Update()
		{
			this.P.Add(this.A.OverTwoPlus(this.V));
			this.V.Add(this.A);
		}

		public double DistanceTo(AbstractObject obj)
		{
			double xOffset = this.P.X - obj.P.X;
			double yOffset = this.P.Y - obj.P.Y;
			double zOffset = this.P.Z - obj.P.Z;
			return Math.Sqrt(xOffset * xOffset + yOffset * yOffset + zOffset * zOffset);
		}

		public override string ToString()
		{
			return P.ToString();
		}

		#region ICloneable Members

		public object Clone()
		{
			return new AbstractObject((Point3D)this.A.Clone(), (Point3D)this.V.Clone(), (Point3D)this.P.Clone());
		}

		#endregion
	}
}
