using Microsoft.Data.SqlClient;
using MusicGlue.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MusicGlue.ViewModels
{
    public class MusicProductRepository
    {
        private readonly string connectionString;
        private List<MusicProduct> musicProducts;

        public MusicProductRepository()
        {
            connectionString = Configuration.ConnectionString;

            musicProducts = new List<MusicProduct>();
            InitializeRepository();
        }

        private void InitializeRepository()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string query = "SELECT " +
                    "MUSICPRODUCT.Id AS 'MusicProductId', MUSICPRODUCT.Price, MUSICPRODUCT.ProductDescriptionId AS 'MusicProductDescriptionId', " +
                    "PRODUCTDESCRIPTION.Id AS 'ProductDescriptionId', PRODUCTDESCRIPTION.APN, PRODUCTDESCRIPTION.CatalogNumber, PRODUCTDESCRIPTION.SKU, PRODUCTDESCRIPTION.Barcode" +
                    "FROM MUSICPRODUCT, PRODUCTDESCRIPTION" +
                    "WHERE PRODUCTDESCRIPTION.Id = MUSICPRODUCT.ProductDescriptionId";

                SqlCommand cmd = new SqlCommand(query, con);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ProductDescription productDescription = new ProductDescription
                        {
                            Id = (int)dr["ProductDescriptionId"],
                            APN = (string)dr["APN"],
                            CatalogNumber = (string)dr["CatalogNumber"],
                            SKU = (string)dr["SKU"],
                            Barcode = (string)dr["Barcode"]
                        };

                        MusicProduct musicProduct = new MusicProduct
                        {
                            Id = (int)dr["MusicProductId"],
                            Price = (float)dr["Price"],
                            Description = productDescription
                        };

                        musicProducts.Add(musicProduct);
                    }
                }
            }
        }

        public List<MusicProduct> GetAll()
        {
            return musicProducts;
        }

        public MusicProduct? GetById(int id)
        {
            return musicProducts.Find(x => x.Id == id); 
        }
    }
}
