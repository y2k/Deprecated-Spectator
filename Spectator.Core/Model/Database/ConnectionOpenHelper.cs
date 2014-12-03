using System;
using Microsoft.Practices.ServiceLocation;
using PCLStorage;
using SQLite.Net;
using SQLite.Net.Interop;

namespace Spectator.Core.Model.Database
{
	public class ConnectionOpenHelper
	{
		const string DatabaseName = "net.itwister.spectator.main.db";
		const int DatabaseVersion = 1;

		static volatile SQLiteConnection instance;
		static object syncRoot = new object ();

		public static SQLiteConnection Current {
			get {
				if (instance == null) {
					lock (syncRoot) {
						if (instance == null) {
							var platform = ServiceLocator.Current.GetInstance<ISQLitePlatform> ();
							var path = PortablePath.Combine (FileSystem.Current.LocalStorage.Path, DatabaseName);
							var connection = new SQLiteConnection (platform, path);

							InitializeDatabase (connection);
							instance = connection;
						}
					}
				}

				return instance;
			}
		}

		protected static void OnCreate (SQLiteConnection db)
		{
			CreateTabled (db);
		}

		public static void CreateTabled (SQLiteConnection db)
		{
			db.CreateTable<Subscription> ();
			db.CreateTable<Snapshot> ();
			db.CreateTable<AccountCookie> ();
			db.CreateTable<Attachment> ();
		}

		protected static void OnUpdate (int oldVersion, int newVersion)
		{
			// Reserverd for future
		}

		#region Private methods

		static void InitializeDatabase (SQLiteConnection db)
		{
			int ver = GetUserVesion (db);
			if (ver == 0)
				db.RunInTransaction (() => {
					OnCreate (db);
					SetUserVersion (db, DatabaseVersion);
				});
			else if (ver < DatabaseVersion)
				db.RunInTransaction (() => {
					OnUpdate (ver, DatabaseVersion);
					SetUserVersion (db, DatabaseVersion);
				});
		}

		static void SetUserVersion (SQLiteConnection db, int version)
		{
			db.Execute ("PRAGMA user_version = " + version);
		}

		static int GetUserVesion (SQLiteConnection db)
		{
			return db.ExecuteScalar<int> ("PRAGMA user_version");
		}

		#endregion
	}
}