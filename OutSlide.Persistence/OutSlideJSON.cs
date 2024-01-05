using Newtonsoft.Json;

namespace OutSlide.Persistence
{
    public class OutSlideJSON
    {
        public async Task SaveGameStateAsync(OutSlidePersistence state)
        {
            try
            {
                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "save.dat");
                string json = JsonConvert.SerializeObject(state);
                await File.WriteAllTextAsync(fileName, json);
            }
            catch { }
        }
        public async Task<OutSlidePersistence?> LoadGameStateAsync()
        {
            try
            {
                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "save.dat");
                OutSlidePersistence? state = null;
                if (File.Exists(fileName))
                {
                    string json = await File.ReadAllTextAsync(fileName);
                    state = JsonConvert.DeserializeObject<OutSlidePersistence>(json);
                }
                return state;
            }
            catch
            {
                return null;
            }
        }
    }
}
