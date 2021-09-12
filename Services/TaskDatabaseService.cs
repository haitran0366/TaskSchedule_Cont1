using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSchedule_Cont1.Models;

namespace TaskSchedule_Cont1.Services
{
    public class TaskDatabaseService
    {
        readonly SQLite.SQLiteAsyncConnection _database;

        public TaskDatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<TaskModel>().Wait();
        }

        public Task<List<TaskModel>> GetTaskAsync()
        {
            return _database.Table<TaskModel>().OrderByDescending(x => x.DueDate).ToListAsync();
        }

        public Task<int> SaveTaskAsync(TaskModel taskModel)
        {
            return _database.InsertAsync(taskModel);

        }
        public async Task DeleteTask(TaskModel taskModel)
        {
            await _database.DeleteAsync(taskModel);
        }
        public async Task<int> UpdateTask(TaskModel taskModel)
        {
            return await _database.UpdateAsync(taskModel);
        }
        public async Task<TaskModel> GetaTask(int id)
        {
            return await _database.FindAsync<TaskModel>(id);
        }
    }
}
