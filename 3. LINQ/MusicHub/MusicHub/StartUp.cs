namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            string result = ExportAlbumsInfo(context, 9);
            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {

            StringBuilder output = new StringBuilder();
        

            var albums = context.Albums
                 .ToList()
                 .Where(a => a.ProducerId == producerId)
                 .OrderByDescending(a => a.Price)
                 .Select(a => new
                 {
                     AlbumName = a.Name,
                     ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                     ProducerName = a.Producer.Name,
                     Songs = a.Songs
                     .Select(s => new
                     {
                         SongName = s.Name,
                         Price = s.Price.ToString("f2"),
                         WriterName = s.Writer.Name,
                     })
                    .OrderByDescending(s => s.SongName)
                    .ThenBy(w => w.WriterName)
                    .ToList(),
                     TotalAlbumPrice = a.Price.ToString("f2"),
                 })                               
                 .ToList();

            foreach (var album in albums)
            {
                output.AppendLine($"-AlbumName: {album.AlbumName}")
                     .AppendLine($"-ReleaseDate: {album.ReleaseDate}")
                     .AppendLine($"-ProducerName: {album.ProducerName}")
                     .AppendLine($"-Songs:");

                int counter = 1;

                foreach (var song in album.Songs)
                {
                    output.AppendLine($"---#{counter++}")
                        .AppendLine($"---SongName: {song.SongName}")
                        .AppendLine($"---Price: {song.Price:f2}")
                        .AppendLine($"---Writer: {song.WriterName}");
                }

                output.AppendLine($"-AlbumPrice: {album.TotalAlbumPrice:f2}");
            }
            return output.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            throw new NotImplementedException();
        }
    }
}
