using System;
using System.Collections.Generic;
using System.Text;

namespace MusicGlue.Models.Formatters
{
    public class OCCFormatter : IFormatter
    {
        public string Format(List<Consignment> consignments)
        {
            string date = DateTime.Now.ToString("yyMMdd");
            string result = string.Empty;

            List<string> zipcodes = consignments
                .Select(consignment => consignment.ZipCode)
                .Distinct()
                .ToList();

            zipcodes.ForEach(zipcode =>
            {
                int count = 0;
                string zipcodePaddet = zipcode.PadRight(5, ' ');

                // Header formatting
                result += "0" + zipcodePaddet + date + Environment.NewLine;

                // Dictionary key = musicproduct id
                Dictionary<int, List<MusicProduct>> musicProductsByZipcode = GetMusicProductsByZipcode(consignments, zipcode);
                musicProductsByZipcode.Keys.ToList().ForEach(musicProductId =>
                {
                    // Dictionary key = musicproduct price
                    Dictionary<double, List<MusicProduct>> musicProductsByPrice = GetMusicProductsByPrice(musicProductsByZipcode[musicProductId]);
                    musicProductsByPrice.Keys.ToList().ForEach(price =>
                    {
                        List<MusicProduct> musicProducts = musicProductsByPrice[price].ToList();
                        string amount = musicProducts.Count.ToString().PadLeft(6, '0');
                        string barcode = musicProducts[0].Description.Barcode;
                        string priceString = (price * 100).ToString().PadLeft(5, '0');

                        result += "1" + barcode + amount + " " + priceString + Environment.NewLine;
                        count++;
                    });
                });

                // Footer formatting
                string amount = count.ToString().PadLeft(5, '0');
                result += "9" + zipcodePaddet + amount;

                // Checks index of zipcode to determine if newlines should be added
                if (zipcodes.IndexOf(zipcode) != zipcodes.Count - 1)
                {
                    result += Environment.NewLine + Environment.NewLine;
                }
            });

            return result;
        }

        private Dictionary<int, List<MusicProduct>> GetMusicProductsByZipcode(List<Consignment> consignments, string zipcode)
        {
            Dictionary<int, List<MusicProduct>> musicProducts = new Dictionary<int, List<MusicProduct>>();
            List<Consignment> consignmentsByZip = consignments
                .Where(consignment => consignment.ZipCode == zipcode)
                .ToList();

            consignmentsByZip.ForEach(consignment =>
            {
                consignment.MusicProducts.ForEach(musicProduct =>
                {
                    if (!musicProducts.ContainsKey(musicProduct.Id))
                    {
                        musicProducts[musicProduct.Id] = new List<MusicProduct>();
                    }

                    musicProducts[musicProduct.Id].Add(musicProduct);
                });
            });

            return musicProducts;
        }

        private Dictionary<double, List<MusicProduct>> GetMusicProductsByPrice(List<MusicProduct> musicProducts)
        {
            Dictionary<double, List<MusicProduct>> musicProductsByPrice = new Dictionary<double, List<MusicProduct>>();
            musicProducts.ForEach(musicProduct =>
            {
                if (!musicProductsByPrice.ContainsKey(musicProduct.Price))
                {
                    musicProductsByPrice[musicProduct.Price] = new List<MusicProduct>();
                }

                musicProductsByPrice[musicProduct.Price].Add(musicProduct);
            });

            return musicProductsByPrice;
        }
    }
}
