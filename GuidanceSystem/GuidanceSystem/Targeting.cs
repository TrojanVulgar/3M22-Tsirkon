using System;
using System.Collections.Generic;

namespace GuidanceSystem
{
	public static class Targeting
	{
		public static double CalculateAcceleration(double PAtTMinus2, double PAtTMinus1, double PAtTMinus0)
		{
			return PAtTMinus2 - 2.0 * PAtTMinus1 + PAtTMinus0;
		}

		public static double CalculateVelocity(double PAtTMinus2, double PAtTMinus1, double PAtTMinus0)
		{
			return PAtTMinus2 / 2.0 - 2.0 * PAtTMinus1 + 3.0 * PAtTMinus0 / 2.0;
		}

		public static bool Intercept(double tax, double tay, double taz, double tvx, double tvy, double tvz, double tpx, double tpy, double tpz, double svx, double svy, double svz, double spx, double spy, double spz, double pam, double pvm, double ppm, int rbt, double tolerance, out double[,] vectors)
		{
			double[] garbage;
			return Intercept(tax, tay, taz, tvx, tvy, tvz, tpx, tpy, tpz, svx, svy, svz, spx, spy, spz, pam, pvm, ppm, rbt, tolerance, out vectors, out garbage);
		}

		public static bool Intercept(double tax, double tay, double taz, double tvx, double tvy, double tvz, double tpx, double tpy, double tpz, double svx, double svy, double svz, double spx, double spy, double spz, double pam, double pvm, double ppm, int rbt, double tolerance, out double[,] vectors, out double[] tui)
		{
			tvx -= svx;
			tvy -= svy;
			tvz -= svz;
			tpx -= spx;
			tpy -= spy;
			tpz -= spz;
			if (tolerance < 1.0)
				tolerance = 1.0;
			if (rbt < 0)
				rbt = 0;
			if (rbt == 0)
				pam = 0.0;
			double tA = 0.25 * (tax * tax + tay * tay + taz * taz);
			double tB = tax * tvx + tay * tvy + taz * tvz;
			double tC = tvx * tvx + tax * tpx + tvy * tvy + tay * tpy + tvz * tvz + taz * tpz;
			double tD = 2.0 * (tvx * tpx + tvy * tpy + tvz * tpz);
			double tE = tpx * tpx + tpy * tpy + tpz * tpz;
			double fsA = tA - 0.25 * pam * pam;
			double fsB = tB - pam * pvm;
			double fsC = tC - pam * ppm - pvm * pvm;
			double fsD = tD - 2.0 * pvm * ppm;
			double fsE = tE - ppm * ppm;
			double ssC = tC - pam * pam * rbt * rbt - 2.0 * pam * pvm * rbt - pvm * pvm;
			double ssD = tD + pam * pam * rbt * rbt * rbt + pam * pvm * rbt * rbt - 2.0 * (pam * rbt + pvm) * ppm;
			double ssE = tE - 0.25 * pam * pam * rbt * rbt * rbt * rbt + pam * ppm * rbt * rbt - ppm * ppm;
			List<double> fsroots = rootsOf(fsA, fsB, fsC, fsD, fsE);
			List<double> fsrootsCA = rootsOf(0.0, 4.0 * fsA, 3.0 * fsB, 2.0 * fsC, fsD);
			List<double> ssroots = rootsOf(tA, tB, ssC, ssD, ssE);
			List<double> ssrootsCA = rootsOf(0.0, 4.0 * tA, 3.0 * tB, 2.0 * ssC, ssD);
			List<double> roots = new List<double>();
			foreach (double root in fsroots)
				if (root >= 0.0 && root <= rbt && !roots.Contains(root))
					roots.Add(root);
			foreach (double root in ssroots)
				if (root > rbt && !roots.Contains(root))
					roots.Add(root);
			foreach (double root in fsrootsCA)
				if (tolerance >= Math.Abs(fsA * root * root * root * root + fsB * root * root * root + fsC * root * root + fsD * root + fsE) && root >= 0.0 && root <= rbt && !roots.Contains(root))
					roots.Add(root);
			foreach (double root in ssrootsCA)
				if (tolerance >= Math.Abs(tA * root * root * root * root + tB * root * root * root + ssC * root * root + ssD * root + ssE) && root > rbt && !roots.Contains(root))
					roots.Add(root);
			roots.Sort();
			tui = roots.ToArray();
			bool able = roots.Count != 0;
			if (!able)
			{
				List<double> fsattempts = new List<double>();
				List<double> ssattempts = new List<double>();
				foreach (double root in fsrootsCA)
					if (root >= 0.0 && root <= rbt && !fsattempts.Contains(root))
						fsattempts.Add(root);
				fsattempts.Add(0);
				foreach (double root in ssrootsCA)
					if (root > rbt && !ssattempts.Contains(root))
						ssattempts.Add(root);
				double[] keys = new double[fsattempts.Count + ssattempts.Count];
				double[] items = new double[fsattempts.Count + ssattempts.Count];
				for (int i = 0; i != fsattempts.Count; i++)
				{
					keys[i] = Math.Abs(fsA * fsattempts[i] * fsattempts[i] * fsattempts[i] * fsattempts[i] + fsB * fsattempts[i] * fsattempts[i] * fsattempts[i] + fsC * fsattempts[i] * fsattempts[i] + tD * fsattempts[i] + tE);
					items[i] = fsattempts[i];
				}
				for (int i = 0; i != ssattempts.Count; i++)
				{
					keys[fsattempts.Count + i] = Math.Abs(tA * ssattempts[i] * ssattempts[i] * ssattempts[i] * ssattempts[i] + tB * ssattempts[i] * ssattempts[i] * ssattempts[i] + ssC * ssattempts[i] * ssattempts[i] + ssD * ssattempts[i] + ssE);
					items[fsattempts.Count + i] = ssattempts[i];
				}
				Array.Sort(keys, items);
				roots.AddRange(items);
			}
			vectors = new double[roots.Count, 3];
			for (int i = 0; i != roots.Count; i++)
			{
				double x;
				double y;
				double z;
				if (pam == 0.0 && pvm == 0.0)
				{
					x = svx;
					y = svy;
					z = svz;
				}
				else
				{
					x = 0.5 * tax * roots[i] * roots[i] + tvx * roots[i] + tpx;
					y = 0.5 * tay * roots[i] * roots[i] + tvy * roots[i] + tpy;
					z = 0.5 * taz * roots[i] * roots[i] + tvz * roots[i] + tpz;
				}
				double m = Math.Sqrt((x * x + y * y + z * z));
				if (m < double.Epsilon)
					m = 1.0;
				vectors[i, 0] = x / m;
				vectors[i, 1] = y / m;
				vectors[i, 2] = z / m;
			}
			return able;
		}

		private static List<double> rootsOf(double A, double B, double C, double D, double E)
		{
			List<double> roots = new List<double>();
			if (A == 0.0)
			{
				if (B == 0.0)
				{
					if (C == 0.0)
					{
						if (D == 0.0)
						{
							if (E == 0.0)
								roots.Add(0.0);
						}
						else
							roots.Add(-D / E);
					}
					else
						roots.AddRange(new double[] { (-D + Math.Sqrt(D * D - 4 * C * E)) / (2.0 * C), (-D - Math.Sqrt(D * D - 4 * C * E)) / (2.0 * C) });
				}
				else
				{
					C /= B;
					D /= B;
					E /= B;
					double F = (3.0 * D - C * C) / 3.0;
					double G = (2.0 * C * C * C - 9.0 * C * D + 27.0 * E) / 27.0;
					double H = (G * G) / 4.0 + (F * F * F) / 27.0;
					if (H > 0)
					{
						double intermediate = -G / 2.0 + Math.Sqrt(H);
						double m = intermediate < 0.0 ? -Math.Pow(-intermediate, 1.0 / 3.0) : Math.Pow(intermediate, 1.0 / 3.0);
						intermediate -= 2.0 * Math.Sqrt(H);
						double n = intermediate < 0.0 ? -Math.Pow(-intermediate, 1.0 / 3.0) : Math.Pow(intermediate, 1.0 / 3.0);
						roots.Add(m + n - C / 3.0);
					}
					else
					{
						double intermediate = Math.Sqrt(G * G / 4.0 - H);
						double rc = intermediate < 0.0 ? -Math.Pow(-intermediate, 1.0 / 3.0) : Math.Pow(intermediate, 1.0 / 3.0);
						double theta = Math.Acos(-G / (2.0 * intermediate)) / 3.0;
						roots.AddRange(new double[] { 2.0 * rc * Math.Cos(theta) - C / 3.0, -rc * (Math.Cos(theta) + Math.Sqrt(3.0) * Math.Sin(theta)) - C / 3.0, -rc * (Math.Cos(theta) - Math.Sqrt(3.0) * Math.Sin(theta)) - C / 3.0 });
					}
					if (F + G + H == 0.0)
					{
						double intermediate = E < 0.0 ? Math.Pow(-E, 1.0 / 3.0) : -Math.Pow(E, 1.0 / 3.0);
						roots.Clear();
						roots.AddRange(new double[] { intermediate, intermediate, intermediate });
					}
				}
			}
			else
			{
				B /= A;
				C /= A;
				D /= A;
				E /= A;
				double F = C - (3.0 * B * B) / 8.0;
				double G = D + B * B * B / 8.0 - (B * C) / 2.0;
				double H = E - 3.0 * B * B * B * B / 256.0 + B * B * C / 16.0 - B * D / 4.0;
				double b = F / 2.0;
				double c = (F * F - 4.0 * H) / 16.0;
				double d = (G * G) / -64.0;
				double f = (3.0 * c - b * b) / 3.0;
				double g = (2.0 * b * b * b - 9.0 * b * c + 27.0 * d) / 27.0;
				double h = (g * g) / 4.0 + (f * f * f) / 27.0;
				double y1;
				double y2r;
				double y2i;
				double y3r;
				double y3i;
				if (h > 0.0)
				{
					double intermediate = -g / 2.0 + Math.Sqrt(h);
					double m = intermediate < 0.0 ? -Math.Pow(-intermediate, 1.0 / 3.0) : Math.Pow(intermediate, 1.0 / 3.0);
					intermediate -= 2.0 * Math.Sqrt(h);
					double n = intermediate < 0.0 ? -Math.Pow(-intermediate, 1.0 / 3.0) : Math.Pow(intermediate, 1.0 / 3.0);
					y1 = m + n - b / 3.0;
					y2r = (m + n) / -2.0 - b / 3.0;
					y2i = ((m - n) / 2.0) * Math.Sqrt(3.0);
					y3r = (m + n) / -2.0 - b / 3.0;
					y3i = ((m - n) / 2.0) * Math.Sqrt(3.0);
				}
				else
				{
					double intermediate = Math.Sqrt((g * g / 4.0 - h));
					double rc = intermediate < 0.0 ? -Math.Pow(-intermediate, 1.0 / 3.0) : Math.Pow(intermediate, 1.0 / 3.0);
					double theta = Math.Acos((-g / (2.0 * intermediate))) / 3.0;
					y1 = 2.0 * rc * Math.Cos(theta) - b / 3.0;
					y2r = -rc * (Math.Cos(theta) + Math.Sqrt(3.0) * Math.Sin(theta)) - b / 3.0;
					y2i = 0.0;
					y3r = -rc * (Math.Cos(theta) - Math.Sqrt(3.0) * Math.Sin(theta)) - b / 3.0;
					y3i = 0.0;
				}
				if (f + g + h == 0.0)
				{
					double intermediate = d < 0.0 ? Math.Pow(-d, 1.0 / 3.0) : -Math.Pow(d, 1.0 / 3.0);
					y1 = intermediate;
					y2r = intermediate;
					y2i = 0.0;
					y3r = intermediate;
					y3i = 0.0;
				}
				double p;
				double q;
				if (h <= 0.0)
				{
					int zeroCheck = 0;
					double[] cubicRoots = new double[] { y1, y2r, y3r };
					Array.Sort(cubicRoots);
					p = Math.Sqrt(cubicRoots[1]);
					q = Math.Sqrt(cubicRoots[2]);
					if (Math.Round(y1, 13) == 0.0)
					{
						p = Math.Sqrt(y2r);
						q = Math.Sqrt(y3r);
						zeroCheck = 1;
					}
					if (Math.Round(y2r, 13) == 0.0)
					{
						p = Math.Sqrt(y1);
						q = Math.Sqrt(y3r);
						zeroCheck += 2;
					}
					if (Math.Round(y3r, 13) == 0.0)
					{
						p = Math.Sqrt(y1);
						q = Math.Sqrt(y2r);
						zeroCheck += 4;
					}
					switch (zeroCheck)
					{
						case (3):
							p = Math.Sqrt(y3r);
							break;
						case (5):
							p = Math.Sqrt(y2r);
							break;
						case (6):
							p = Math.Sqrt(y1);
							break;
					}
					if (Math.Round(y1, 13) < 0.0 || Math.Round(y2r, 13) < 0.0 || Math.Round(y3r, 13) < 0.0)
					{
						if (E == 0.0)
							roots.Add(0.0);
					}
					else
					{
						double r;
						if (zeroCheck < 5)
							r = G / (-8.0 * p * q);
						else
							r = 0.0;
						double s = B / 4.0;
						roots.AddRange(new double[] { p + q + r - s, p - q - r - s, -p + q - r - s, -p - q + r - s });
					}
				}
				else
				{
					double r2mod = Math.Sqrt(y2r * y2r + y2i * y2i);
					double y2mod = Math.Sqrt((r2mod - y2r) / 2.0);
					double x2mod = y2i / (2.0 * y2mod);
					p = x2mod + y2mod;
					double r3mod = Math.Sqrt(y3r * y3r + y3i * y3i);
					double y3mod = Math.Sqrt((r3mod - y3r) / 2.0);
					double x3mod = y3i / (2.0 * y3mod);
					q = x3mod + y3mod;
					double r = G / (-8.0 * (x2mod * x3mod + y2mod * y3mod));
					double s = B / 4.0;
					roots.AddRange(new double[] { x2mod + x3mod + r - s, -x2mod - x3mod + r - s });
				}
			}
			for (int i = 0; i != roots.Count; i++)
				if (double.IsInfinity(roots[i]) || double.IsNaN(roots[i]))
					roots.RemoveAt(i--);
			roots.Sort();
			return roots;
		}
	}
}
