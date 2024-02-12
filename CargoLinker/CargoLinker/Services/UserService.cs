using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CargoLinker
{
    public class UserService
    {

        SQLiteConnection database;

        public UserService(string dbPath)
        {
            database = new SQLiteConnection(dbPath);
            database.CreateTable<User>();
        }

        public int RegisterUser(User user)
        {
            return database.Insert(user);
        }

        public int DeleteUser(User user)
        {
            return database.Delete(user);
        }

        public void UpdateUser(User user)
        {
            database.Update(user);
        }

        public bool AuthenticateUser(string email, string password)
        {
            var existingUser = database.Table<User>().FirstOrDefault(u => u.Email == email && u.Password == password);
            return existingUser != null;
        }

        public bool DoesEmailExist(string email)
        {
            var existingUser = database.Table<User>().FirstOrDefault(u => u.Email == email);
            return existingUser != null;
        }

        public User GetCurrentLoggedInUser(string email, string password)
        {
            return database.Table<User>().FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public User GetUserByUsername(string username)
        {
            return database.Table<User>().FirstOrDefault(u => u.Username == username);
        }

        public string GetUsernameByEmailAndPassword(string email, string password)
        {
            var loggedInUser = GetCurrentLoggedInUser(email, password);
            return loggedInUser?.Username;
        }

    }
}
