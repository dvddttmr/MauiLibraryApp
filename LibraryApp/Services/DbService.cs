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
            await RunWithTimeout(() => db.CreateTableAsync<MediaType>(), 3000);
            await RunWithTimeout(() => db.CreateTableAsync<Genre>(), 3000);
        }

        #region Media
        public async Task<int> CreateMedia(Media media)
        {
            try
            {
                await db.InsertAsync(media);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }

            return media.Id;
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

        public async Task<MediaPerson> GetMediaPerson(int id)
        {
            return await db.Table<MediaPerson>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<MediaPerson>> GetMediaPersonsForPerson(int personId)
        {
            return await db.Table<MediaPerson>().Where(mp => mp.PersonId == personId).ToListAsync();
        }

        public async Task<List<MediaPerson>> GetMediaPersonsForMedia(int mediaId)
        {
            return await db.Table<MediaPerson>().Where(mp => mp.MediaId == mediaId).ToListAsync();
        }

        public async Task UpdateMediaPerson(MediaPerson mp)
        {
            await db.UpdateAsync(mp);
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

        public async Task DeleteMediaPerson(int id)
        {
            try
            {
                var mp = await GetMediaPerson(id);
                if(mp is not null)
                    await db.DeleteAsync(mp);
            }
            catch (Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task DeleteMediaPersonForPerson(int personId)
        {
            try
            {
                var mediaPersons = await GetMediaPersonsForPerson(personId);
                foreach(var mp in mediaPersons)
                {
                    await DeleteMediaPerson(mp);
                }
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task DeleteMediaPersonForMedia(int mediaId)
        {
            try
            {
                var mediaPersons = await GetMediaPersonsForMedia(mediaId);
                foreach(var mp in mediaPersons)
                {
                    await DeleteMediaPerson(mp);
                }
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

        #region MediaType

        public async Task<int> CreateMediaType(MediaType mt)
        {
            try
            {
                await db.InsertAsync(mt);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }

            return mt.Id;
        }

        public async Task<List<MediaType>> GetAllMediaTypes()
        {
            try
            {
                return await db.Table<MediaType>().ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task<MediaType> GetMediaType(int Id)
        {
            try
            {
                return await db.Table<MediaType>().FirstOrDefaultAsync(mt => mt.Id == Id);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task UpdateMediaType(MediaType mt)
        {
            try
            {
                await db.UpdateAsync(mt);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task DeleteMediaType(MediaType mt)
        {
            try
            {
                await db.DeleteAsync(mt);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        #endregion

        #region Genre

        public async Task<int> CreateGenre(Genre genre)
        {
            try
            {
                await db.InsertAsync(genre);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }

            return genre.Id;
        }

        public async Task<List<Genre>> GetAllGenres()
        {
            try
            {
                return await db.Table<Genre>().ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task<List<Genre>> GetGenresForMediaType(int mediaTypeId)
        {
            try
            {
                return await db.Table<Genre>().Where(g => g.MediaTypeId == mediaTypeId).ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task<Genre> GetGenre(int id)
        {
            try
            {
                return await db.Table<Genre>().FirstOrDefaultAsync(g => g.Id == id);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task UpdateGenre(Genre genre)
        {
            try
            {
                await db.UpdateAsync(genre);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task DeleteGenre(Genre genre)
        {
            try
            {
                await db.DeleteAsync(genre);
            }
            catch(Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public async Task DeleteGenreByMediaType(int mediaTypeId)
        {
            try
            {
                var genres = await GetGenresForMediaType(mediaTypeId);
                foreach(var g in genres)
                {
                    await db.DeleteAsync(g);
                }
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
