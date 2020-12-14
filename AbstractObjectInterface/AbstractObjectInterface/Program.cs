using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using GuidanceSystem;

namespace AbstractObjectInterface
{
	static class Program
	{
		const bool DesktopFileOutputHTML = true;
		const bool AudioOutput = true;
		const bool AutoRestartOnUnable = false;
		const int ABound = 1;
		const int VBound = 10;
		const int PBound = 10000;
		const int PamBound = 10;
		const int PvmBound = 100;
		const int PpmBound = 10;
		const int DecimalDigits = 4;
		const int RbtBound = 1000;
		const double RadiusOfTolerance = 250.0;

		static void audio(double dist, double lastDist)
		{
			switch (AudioOutput)
			{
				case (true):
					Console.Beep(Math.Max(Math.Min(1760 - (int)(dist - lastDist) / 5, 3520), 55), 50);
					Thread.Sleep((int)(10 * dist / PBound));
					break;
				case (false):
					Thread.Sleep(100);
					break;
			}
		}

		static void Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Clear();
			Console.Title = "Missile Guidance System 5.1";
			Console.WindowWidth = 85;
			Console.WindowHeight = 25;
			Console.BufferWidth = 85;
			Console.BufferHeight = short.MaxValue - 1;
			Console.CursorVisible = false;
			Console.WriteLine("Press any key to begin.");
			ConsoleKey key = Console.ReadKey(true).Key;
			if (key == ConsoleKey.Escape)
				return;
			Console.Clear();
			Random r = new Random();
			while (true)
			{
				double dec = Math.Pow(10.0, DecimalDigits);
				double tax = r.Next((int)(-ABound * dec), (int)(ABound * dec)) / dec;
				double tay = r.Next((int)(-ABound * dec), (int)(ABound * dec)) / dec;
				double taz = r.Next((int)(-ABound * dec), (int)(ABound * dec)) / dec;
				double tvx = r.Next((int)(-VBound * dec), (int)(VBound * dec)) / dec;
				double tvy = r.Next((int)(-VBound * dec), (int)(VBound * dec)) / dec;
				double tvz = r.Next((int)(-VBound * dec), (int)(VBound * dec)) / dec;
				double tpx = r.Next((int)(-PBound * dec), (int)(PBound * dec)) / dec;
				double tpy = r.Next((int)(-PBound * dec), (int)(PBound * dec)) / dec;
				double tpz = r.Next((int)(-PBound * dec), (int)(PBound * dec)) / dec;
				double sax = r.Next((int)(-ABound * dec), (int)(ABound * dec)) / dec;
				double say = r.Next((int)(-ABound * dec), (int)(ABound * dec)) / dec;
				double saz = r.Next((int)(-ABound * dec), (int)(ABound * dec)) / dec;
				double svx = r.Next((int)(-VBound * dec), (int)(VBound * dec)) / dec;
				double svy = r.Next((int)(-VBound * dec), (int)(VBound * dec)) / dec;
				double svz = r.Next((int)(-VBound * dec), (int)(VBound * dec)) / dec;
				double spx = r.Next((int)(-PBound * dec), (int)(PBound * dec)) / dec;
				double spy = r.Next((int)(-PBound * dec), (int)(PBound * dec)) / dec;
				double spz = r.Next((int)(-PBound * dec), (int)(PBound * dec)) / dec;
				double pam = r.Next((int)(PamBound * dec)) / dec;
				double pvm = r.Next((int)(PvmBound * dec)) / dec;
				double ppm = r.Next((int)(PpmBound * dec)) / dec;
				int rbt = r.Next(RbtBound);
				Console.Title = "Attempting to lock on";
				Console.ForegroundColor = ConsoleColor.Blue;
				Output.Clear();
				Output.WriteLine("INITALIZATION\n");
				Output.WriteLine("TAX:\t\t" + tax);
				Output.WriteLine("TAY:\t\t" + tay);
				Output.WriteLine("TAZ:\t\t" + taz);
				Output.WriteLine("TVX:\t\t" + tvx);
				Output.WriteLine("TVY:\t\t" + tvy);
				Output.WriteLine("TVZ:\t\t" + tvz);
				Output.WriteLine("TPX:\t\t" + tpx);
				Output.WriteLine("TPY:\t\t" + tpy);
				Output.WriteLine("TPZ:\t\t" + tpz);
				Output.WriteLine("SAX:\t\t" + sax);
				Output.WriteLine("SAY:\t\t" + say);
				Output.WriteLine("SAZ:\t\t" + saz);
				Output.WriteLine("SVX:\t\t" + svx);
				Output.WriteLine("SVY:\t\t" + svy);
				Output.WriteLine("SVZ:\t\t" + svz);
				Output.WriteLine("SPX:\t\t" + spx);
				Output.WriteLine("SPY:\t\t" + spy);
				Output.WriteLine("SPZ:\t\t" + spz);
				Output.WriteLine("PAM:\t\t" + pam);
				Output.WriteLine("PVM:\t\t" + pvm);
				Output.WriteLine("PPM:\t\t" + ppm);
				Output.WriteLine("RBT:\t\t" + rbt);
				Console.ForegroundColor = ConsoleColor.Cyan;
				AbstractObject shooter = new AbstractObject(new Point3D(sax, say, saz), new Point3D(svx, svy, svz), new Point3D(spx, spy, spz));
				AbstractObject target = new AbstractObject(new Point3D(tax, tay, taz), new Point3D(tvx, tvy, tvz), new Point3D(tpx, tpy, tpz));
				double dist = shooter.DistanceTo(target);
				Output.WriteLine("\nTRACKING\n");
				Output.WriteLine("Time:\t\t-2");
				Output.WriteLine("Target:\t\t" + target);
				Output.WriteLine("Shooter:\t" + shooter);
				Output.WriteLine("Fuel:\t\t" + rbt);
				Output.WriteLine("Distance:\t" + dist);
				Output.WriteLine("");
				Point3D tp1 = (Point3D)target.P.Clone();
				target.Update();
				shooter.Update();
				double lastDist = dist;
				dist = shooter.DistanceTo(target);
				audio(dist, lastDist);
				Output.WriteLine("TRACKING\n");
				Output.WriteLine("Time:\t\t-1");
				Output.WriteLine("Target:\t\t" + target);
				Output.WriteLine("Shooter:\t" + shooter);
				Output.WriteLine("Fuel:\t\t" + rbt);
				Output.WriteLine("Distance:\t" + dist);
				Output.WriteLine("");
				Point3D tp2 = (Point3D)target.P.Clone();
				target.Update();
				shooter.Update();
				lastDist = dist;
				dist = shooter.DistanceTo(target);
				audio(dist, lastDist);
				Output.WriteLine("TRACKING\n");
				Output.WriteLine("Time:\t\t0");
				Output.WriteLine("Target:\t\t" + target);
				Output.WriteLine("Shooter:\t" + shooter);
				Output.WriteLine("Fuel:\t\t" + rbt);
				Output.WriteLine("Distance:\t" + dist);
				Output.WriteLine("");
				Point3D tp3 = (Point3D)target.P.Clone();
				Point3D tEstA = new Point3D(Targeting.CalculateAcceleration(tp1.X, tp2.X, tp3.X), Targeting.CalculateAcceleration(tp1.Y, tp2.Y, tp3.Y), Targeting.CalculateAcceleration(tp1.Z, tp2.Z, tp3.Z));
				Point3D tEstV = new Point3D(Targeting.CalculateVelocity(tp1.X, tp2.X, tp3.X), Targeting.CalculateVelocity(tp1.Y, tp2.Y, tp3.Y), Targeting.CalculateVelocity(tp1.Z, tp2.Z, tp3.Z));
				lastDist = dist;
				dist = shooter.DistanceTo(target);
				audio(dist, lastDist);
				double[] tui;
				double[,] vectors;
				bool able = Targeting.Intercept(tEstA.X, tEstA.Y, tEstA.Z, tEstV.X, tEstV.Y, tEstV.Z, target.P.X, target.P.Y, target.P.Z, shooter.V.X, shooter.V.Y, shooter.V.Z, shooter.P.X, shooter.P.Y, shooter.P.Z, pam, pvm, ppm, rbt, RadiusOfTolerance, out vectors, out tui);
				if (AutoRestartOnUnable && !able)
					continue;
				AbstractObject missile = (AbstractObject)shooter.Clone();
				Console.ForegroundColor = able ? ConsoleColor.Green : ConsoleColor.DarkGreen;
				int i = 0;
				if (tui.Length == 0)
				{
					Console.Title = "Unable to lock on";
					missile.A = new Point3D(pam * vectors[0, 0], pam * vectors[0, 1], pam * vectors[0, 2]);
					missile.V.Add(new Point3D(pvm * vectors[0, 0], pvm * vectors[0, 1], pvm * vectors[0, 2]));
					missile.P.Add(new Point3D(ppm * vectors[0, 0], ppm * vectors[0, 1], ppm * vectors[0, 2]));
					Output.WriteLine("No intercept exists.\nPress any key to attempt");
					if (AutoRestartOnUnable)
					{
						Output.WriteLine("\nINITAL\n");
						Output.WriteLine("Target:\t\t" + target);
						Output.WriteLine("Missile:\t" + missile);
						Output.WriteLine("Fuel:\t\t" + rbt);
						Output.WriteLine("Distance:\t" + missile.DistanceTo(target));
						Output.WriteLine("T until impact:\tNEVER");
					}
					key = Console.ReadKey(true).Key;
					if (key == ConsoleKey.Escape)
						return;
					if (AutoRestartOnUnable)
						continue;
				}
				else if (tui.Length == 1)
				{
					Console.Title = "Locked onto target";
					missile.A = new Point3D(pam * vectors[0, 0], pam * vectors[0, 1], pam * vectors[0, 2]);
					missile.V.Add(new Point3D(pvm * vectors[0, 0], pvm * vectors[0, 1], pvm * vectors[0, 2]));
					missile.P.Add(new Point3D(ppm * vectors[0, 0], ppm * vectors[0, 1], ppm * vectors[0, 2]));
					Output.WriteLine("Only one intercept exists.\nPress any key to simulate.\n");
					key = Console.ReadKey(true).Key;
					if (key == ConsoleKey.Escape)
						return;
				}
				else
				{
					Console.Title = "Locked onto target";
					string input;
					Output.WriteLine("More than one intercept exists.\nEnter '1' for the fastest intercept, '" + vectors.GetLength(0) + "' for the slowest, or any intiger in between.\nA blank entry will imply a selection of '1'\n\n");
					int loc = Console.CursorTop - 1;
					Console.CursorTop = loc;
					do
					{
						input = Console.ReadLine();
						Console.CursorTop = loc;
						Console.Write(new string(' ', input.Length));
						Console.CursorLeft = 0;
					}
					while (!int.TryParse(input == "" ? "1" : input, out i) || i < 1 || i > tui.Length);
					Console.CursorTop = loc;
					Console.Write(new string(' ', input.Length));
					Console.CursorLeft = 0;
					if (input != "")
						Output.WriteLine(input + "\n");
					i--;
					missile.A = new Point3D(pam * vectors[i, 0], pam * vectors[i, 1], pam * vectors[i, 2]);
					missile.V.Add(new Point3D(pvm * vectors[i, 0], pvm * vectors[i, 1], pvm * vectors[i, 2]));
					missile.P.Add(new Point3D(ppm * vectors[i, 0], ppm * vectors[i, 1], ppm * vectors[i, 2]));
				}
				int index = i;
				Output.WriteLine("INITAL\n");
				Output.WriteLine("Target:\t\t" + target);
				Output.WriteLine("Missile:\t" + missile);
				Output.WriteLine("Fuel:\t\t" + rbt);
				Output.WriteLine("Distance:\t" + missile.DistanceTo(target));
				Output.WriteLine("T until impact:\t" + (tui.Length == 0 ? "NEVER" : tui[i].ToString()) + "\n");
				Console.ForegroundColor = rbt == 0 ? ConsoleColor.White : ConsoleColor.Yellow;
				for (int t = 1; true; t++)
				{
					if (t - 1 == rbt)
					{
						missile.A = new Point3D(0.0, 0.0, 0.0);
						pam = 0.0;
						index = 0;
						Console.ForegroundColor = tui.Length == 0 ? ConsoleColor.Gray : ConsoleColor.White;
					}
					lastDist = missile.DistanceTo(target);
					Console.Title = tui.Length == 0 ? "Impact is not expected" : "Expecting impact in T-" + tui[i].ToString();
					if (Console.KeyAvailable)
					{
						if (Console.ReadKey(true).Key == ConsoleKey.Escape)
							return;
						else
							break;
					}
					Point3D targetLast = (Point3D)target.P.Clone();
					Point3D missileLast = (Point3D)missile.P.Clone();
					target.Update();
					missile.Update();
					if (Math.Sign(targetLast.X - missileLast.X) != Math.Sign(target.P.X - missile.P.X) && Math.Sign(targetLast.Y - missileLast.Y) != Math.Sign(target.P.Y - missile.P.Y) && Math.Sign(targetLast.Z - missileLast.Z) != Math.Sign(target.P.Z - missile.P.Z))
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Output.WriteLine("IMPACT APEX\n");
					}
					dist = missile.DistanceTo(target);
					tp1 = (Point3D)tp2.Clone();
					tp2 = (Point3D)tp3.Clone();
					tp3 = (Point3D)target.P.Clone();
					tEstA = new Point3D(Targeting.CalculateAcceleration(tp1.X, tp2.X, tp3.X), Targeting.CalculateAcceleration(tp1.Y, tp2.Y, tp3.Y), Targeting.CalculateAcceleration(tp1.Z, tp2.Z, tp3.Z));
					tEstV = new Point3D(Targeting.CalculateVelocity(tp1.X, tp2.X, tp3.X), Targeting.CalculateVelocity(tp1.Y, tp2.Y, tp3.Y), Targeting.CalculateVelocity(tp1.Z, tp2.Z, tp3.Z));
					double[,] updates;
					index = i;
					Console.ForegroundColor = Targeting.Intercept(tEstA.X, tEstA.Y, tEstA.Z, tEstV.X, tEstV.Y, tEstV.Z, target.P.X, target.P.Y, target.P.Z, missile.V.X, missile.V.Y, missile.V.Z, missile.P.X, missile.P.Y, missile.P.Z, pam, 0.0, 0.0, rbt - t, RadiusOfTolerance, out updates, out tui) ? rbt < t + 1 ? ConsoleColor.White : ConsoleColor.Yellow : rbt < t + 1 ? ConsoleColor.Gray : ConsoleColor.DarkYellow;
					i = Math.Max(0, Math.Min(i, tui.Length - 1));
					Point3D error;
					if (i == -1)
						error = new Point3D();
					else
					{
						error = new Point3D(Math.Abs(vectors[i, 0] - updates[i, 0]), Math.Abs(vectors[i, 1] - updates[i, 1]), Math.Abs(vectors[i, 2] - updates[i, 2]));
						missile.A = new Point3D(pam * updates[i, 0], pam * updates[i, 1], pam * updates[i, 2]);
					}
					while (dist <= RadiusOfTolerance)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Output.WriteLine("IPACT");
						Output.WriteLine("Time:\t\t" + t);
						Output.WriteLine("Target:\t\t" + target);
						Output.WriteLine("Missile:\t" + missile);
						Output.WriteLine("Fuel:\t\t" + Math.Max(0, rbt - t++));
						Output.WriteLine("Distance:\t" + dist);
						Output.WriteLine("T until impact:\t" + (tui.Length == 0 ? "NEVER" : tui[i].ToString()) + "\n");
						Console.ForegroundColor = tui.Length == 0 ? t > rbt ? ConsoleColor.Gray : ConsoleColor.DarkYellow : t > rbt ? ConsoleColor.White : ConsoleColor.Yellow;
						target.Update();
						missile.Update();
						tp1 = (Point3D)tp2.Clone();
						tp2 = (Point3D)tp3.Clone();
						tp3 = (Point3D)target.P.Clone();
						tEstA = target.A;
						tEstV = target.V;
						tEstA = new Point3D(Targeting.CalculateAcceleration(tp1.X, tp2.X, tp3.X), Targeting.CalculateAcceleration(tp1.Y, tp2.Y, tp3.Y), Targeting.CalculateAcceleration(tp1.Z, tp2.Z, tp3.Z));
						tEstV = new Point3D(Targeting.CalculateVelocity(tp1.X, tp2.X, tp3.X), Targeting.CalculateVelocity(tp1.Y, tp2.Y, tp3.Y), Targeting.CalculateVelocity(tp1.Z, tp2.Z, tp3.Z));
						index = i;
						Console.ForegroundColor = Targeting.Intercept(tEstA.X, tEstA.Y, tEstA.Z, tEstV.X, tEstV.Y, tEstV.Z, target.P.X, target.P.Y, target.P.Z, missile.V.X, missile.V.Y, missile.V.Z, missile.P.X, missile.P.Y, missile.P.Z, pam, 0.0, 0.0, rbt - t, RadiusOfTolerance, out vectors, out tui) ? rbt < t + 1 ? ConsoleColor.White : ConsoleColor.Yellow : rbt < t + 1 ? ConsoleColor.Gray : ConsoleColor.DarkYellow;
						i = Math.Min(index, tui.Length - 1);
						Console.Title = tui.Length == 0 ? "Impact is not expected" : "Expecting impact in T-" + tui[i].ToString();
						if (i != -1)
							missile.A = new Point3D(pam * vectors[i, 0], pam * vectors[i, 1], pam * vectors[i, 2]);
						dist = missile.DistanceTo(target);
						audio(dist, lastDist);
					}
					audio(dist, lastDist);
					Output.WriteLine("Time:\t\t" + t);
					Output.WriteLine("Target:\t\t" + target);
					Output.WriteLine("Missile:\t" + missile);
					Output.WriteLine("Fuel:\t\t" + Math.Max(0, rbt - t));
					Output.WriteLine("Distance:\t" + dist);
					Output.WriteLine("Error:\t\t" + (double.IsNaN(error.X) ? "LAUNCH ERROR" : error.ToString()));
					Output.WriteLine("T until impact:\t" + (tui.Length == 0 ? "NEVER" : tui[i].ToString()) + "\n");
					vectors = updates;
				}
			}
		}

		static class Output
		{
			public static void WriteLine(string data)
			{
				Console.WriteLine(data);
				if (DesktopFileOutputHTML)
					File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/Intercept Trajectory Data.html", "<font color=" + Console.ForegroundColor.ToString() + ">" + data.Replace("\n", "<br>") + "</font><br>");
			}

			public static void Clear()
			{
				Console.Clear();
				if (DesktopFileOutputHTML)
					File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/Intercept Trajectory Data.html", "<html><head><title>Intercept Trajectory Data</title></head><body bgcolor=black><tt><h3>");
			}
		}
	}
}