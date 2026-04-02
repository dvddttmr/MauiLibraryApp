using LibraryApp.Models;
using SQLite;

namespace LibraryApp.Services
{
    public class DbService
    {
        private readonly SQLiteAsyncConnection db;

        public DbService(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
        }

        public async Task Init()
        {
            await RunWithTimeout(() => db.CreateTableAsync<Media>(), 3000);
            await RunWithTimeout(() => db.CreateTableAsync<Person>(), 3000);
            await RunWithTimeout(() => db.CreateTableAsync<MediaPerson>(), 3000);
            await RunWithTimeout(() => db.CreateTableAsync<Publisher>(),3000);
        }

        #region Media
        public async Task CreateMedia(Media media)
        {
            try
            {
                await db.InsertAsync(media);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task<List<Media>>GetAllMedia()
        {
            try
            {
                return await db.Table<Media>().ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task<Media> GetMediaById(int id)
        {
            try
            {
                return await db.Table<Media>().Where(m => m.Id == id).FirstOrDefaultAsync();
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task UpdateMedia(Media media)
        {
            try
            {
                await db.UpdateAsync(media);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task DeleteMedia(Media media)
        {
            try
            {
                await db.DeleteAsync(media);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        #endregion

        #region Person

        public async Task<int> CreatePerson(Person person)
        {
            try
            {
                await db.InsertAsync(person);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
            return person.Id;
        }

        public async Task<List<Person>> GetAllPersons()
        {
            try
            {
                return await db.Table<Person>().ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task<Person> GetPersonById(int id)
        {
            try
            {
                return await db.Table<Person>().Where(p => p.Id == id).FirstOrDefaultAsync();
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task UpdatePerson(Person person)
        {
            try
            {
                await db.UpdateAsync(person);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task DeletePerson(Person person)
        {
            try
            {
                await db.DeleteAsync(person);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        #endregion

        #region MediaPerson

        public async Task CreateMediaPerson(MediaPerson mp)
        {
            try
            {
                await db.InsertAsync(mp);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task<List<MediaPerson>> GetAllMediaPersons()
        {
            try
            {
                return await db.Table<MediaPerson>().ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task DeleteMediaPerson(MediaPerson mp)
        {
            try
            {
                await db.DeleteAsync(mp);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        #endregion

        #region Publisher

        public async Task<int> CreatePublisher(Publisher publisher)
        {
            try
            {
                await db.InsertAsync(publisher);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
            return publisher.Id;
        }

        public async Task<List<Publisher>> GetAllPublishers()
        {
            try
            {
                return await db.Table<Publisher>().ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task<Publisher> GetPublisherById(int id)
        {
            try
            {
                return await db.Table<Publisher>().Where(p => p.Id == id).FirstOrDefaultAsync();
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task UpdatePublisher(Publisher publisher)
        {
            try
            {
                await db.UpdateAsync(publisher);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task DeletePublisher(Publisher publisher)
        {
            try
            {
                await db.DeleteAsync(publisher);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        #endregion

        private async Task RunWithTimeout(Func<Task> action, int timeoutMs)
        {
            var task = action();
            var delay = Task.Delay(timeoutMs);

            var completed = await Task.WhenAny(task, delay);

            if (completed == delay)
                throw new TimeoutException("Database operation timed out.");

            await task;
        }
    }
}
