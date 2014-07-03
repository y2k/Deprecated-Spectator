using System;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using Microsoft.Practices.ServiceLocation;

namespace Spectator.Core.Model.Database
{
	internal class ConnectionOpenHelper
	{
		private const string DatabaseName = "net.itwister.spectator.main.db";
		private const int DatabaseVersion = 1;

		private static volatile ISQLiteConnection instance;
		private static object syncRoot = new Object ();

		public static ISQLiteConnection Current {
			get {
				if (instance == null) {
					lock (syncRoot) {
						if (instance == null) {
							var f = ServiceLocator.Current.GetInstance<ISQLiteConnectionFactory> ();
							instance = f.Create (DatabaseName);
							InitializeDatabase (instance);
						}
					}
				}

				return instance;
			}
		}

		protected static void OnCreate (ISQLiteConnection db)
		{
			db.CreateTable<Subscription> ();
			db.CreateTable<Snapshot> ();
		}

		protected static void OnUpdate (int oldVersion, int newVersion)
		{
			// Reserverd for future
		}

		#region Private methods

		private static void InitializeDatabase (ISQLiteConnection db)
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

		private static void SetUserVersion (ISQLiteConnection db, int version)
		{
			db.Execute ("PRAGMA user_version = " + version);
		}

		private static int GetUserVesion (ISQLiteConnection db)
		{
			return db.ExecuteScalar<int> ("PRAGMA user_version");
		}

		#endregion
	}
}