using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace sports
	{

	[DataContract]
	public class Player
		{
		private static Regex regex = new Regex("^[a-zA-Z]*$");

		[DataMember]
		public readonly int Number;

		[DataMember]
		public readonly String FirstName;

		[DataMember]
		public readonly int Age;

		[DataMember]
		public readonly int Experience;

		public Player(int number, String firstName, int age, int experience)
			{
			if (age < experience)
				throw new ArgumentException("age must be grater than experience");
			if (!regex.IsMatch(firstName))
				{
				throw new ArgumentException("names must contain only letters");
				}
			Number = number;
			FirstName = firstName;
			Age = age;
			Experience = experience;
			}
		}

	public static class PlayersStore
		{
		/// <summary>
		/// Gets list of numbers of all players in system.
		/// </summary>
		/// <returns>The player numbers.</returns>
		public static List<int> GetPlayerNumbers()
			{
			return getPlayers().Select(p => p.Number).ToList();
			}

		/// <summary>
		/// Gets info about player with given start number. 
		/// </summary>
		/// <returns>The player. Null if player does not exist.</returns>
		/// <param name="number">Number.</param>
		public static Player GetPlayer(int number)
			{
			return getPlayers().Find(p => p.Number == number);
			}

		/// <summary>
		/// Removes the player.
		/// </summary>
		/// <returns><c>true</c>, if player was removed, <c>false</c> otherwise (player does not exist).</returns>
		/// <param name="number">Number.</param>
		public static bool RemovePlayer(int number)
			{
			var players = getPlayers();
			if (players.Find(p => p.Number == number) != null)
				{
				setPlayers(players.Where(p => p.Number != number).ToList());
				return true;
				}
			return false;
			}

		/// <summary>
		/// Adds the player.
		/// </summary>
		/// <returns><c>true</c>, if player was added, <c>false</c> otherwise (player number already exists).</returns>
		/// <param name="pl">Pl.</param>
		public static bool AddPlayer(Player pl)
			{
			var players = getPlayers();
			if (players.Find(p => p.Number == pl.Number) != null)
				{
				return false;
				}
			players.Add(pl);
			setPlayers(players);
			return true;
			}

		/// <summary>
		/// Performs the competition.
		/// </summary>
		/// <returns>List of numbers of players who received a point in the copetition. </returns>
		public static List<int> PerformCompetition()
			{
			System.Threading.Thread.Sleep((int)(rng.NextDouble()*5000));
			var list = GetPlayerNumbers().ToList();
			list.Shuffle();
			return list.Take(3).ToList();
			}

	        private static String filePath = Path.Combine(Path.GetTempPath(),"players.csv");

		private static Player lineToPlayer(String line)
		{
			String[] elements = line.Split(',');
			return new Player(int.Parse(elements[0]), elements[1], int.Parse(elements[2]), int.Parse(elements[3]));
		}

		private static String playerToLine(Player player)
		{
			return String.Format("{0},{1},{2},{3}", player.Number, player.FirstName, player.Age, player.Experience);
		}

		private static List<Player> getPlayers()
		{
			List<Player> ret = new List<Player>();
			using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
			{
				fs.Lock(0, 10000);
				StreamReader rs = new StreamReader(fs);
				String line;
				while (null != (line = rs.ReadLine()))
				{
					ret.Add(lineToPlayer(line));
				}
				fs.Unlock(0, 10000);
				rs.Close();
			}
			return ret;
		}

		private static void setPlayers(List<Player> players)
		{
			using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate | FileMode.Truncate))
			{
				fs.Lock(0, 10000);
				StreamWriter sw = new StreamWriter(fs);
				players.ForEach(p => sw.WriteLine(playerToLine(p)));
				fs.Unlock(0, 10000);
				sw.Close();
			}
		}

		private static Random rng = new Random();

		public static void Shuffle<T>(this IList<T> list)
			{  
			int n = list.Count;  
			while (n > 1)
				{  
				n--;  
				int k = rng.Next(n + 1);  
				T value = list[k];  
				list[k] = list[n];  
				list[n] = value;  
				}  
			}
		}
	}

