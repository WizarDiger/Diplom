﻿using DiplomBackendASPNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace DiplomBackendASPNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;


        public ProductsController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;

        }

        [HttpGet]
        public JsonResult GetMessages()
        {
            string query = $@"SELECT * FROM ""Products""";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("SocialNetworkCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult SaveMessage(Product product)
        {
            string query = $@"INSERT INTO ""Products"" (title, description, price, amount, senderid, sendername, imagepath)
               VALUES(@title,@description,@price,@amount,@senderid,@sendername,@imagepath)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("SocialNetworkCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@title", product.Title);
                    myCommand.Parameters.AddWithValue("@description", product.Description);
                    myCommand.Parameters.AddWithValue("@price", product.Price);
                    myCommand.Parameters.AddWithValue("@amount", product.Amount);
                    myCommand.Parameters.AddWithValue("@senderid", product.SenderId);
                    myCommand.Parameters.AddWithValue("@sendername", product.SenderName);
                    myCommand.Parameters.AddWithValue("@imagepath", product.ImagePath);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }
            }

            return new JsonResult(table);
        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from ""Products""
	        where id = @id";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("SocialNetworkCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }
            }
            return new JsonResult("Deleted Succesfully");
        }
    }
}
