using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microservices.Shared;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Microservices.Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;


        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }



        public async Task<ResponseObject<ICollection<Models.Discount>>> GetAll()
        {
            var discounts = await _dbConnection.QueryAsync<Models.Discount>("Select * from discount");
            return ResponseObject<ICollection<Models.Discount>>.CreateSuccessResponse(discounts.ToList(), 200);
        }



        public async Task<ResponseObject<Models.Discount>> GetById(Func<Models.Discount, bool> predicate)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("select * from discount")).FirstOrDefault(predicate);

            if (discount == null)
            {
                return ResponseObject<Models.Discount>.CreateErrorResponse(404,"Discount Not Found.");
            }

            return ResponseObject<Models.Discount>.CreateSuccessResponse(discount);

        }


        public async Task<ResponseObject<Models.Discount>> Insert(Models.Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("INSERT INTO discount(userid,rate,code) VALUES (@UserId,@Rate,@Code)", discount);

            if (status <= 0)
            {
                return ResponseObject<Models.Discount>.CreateErrorResponse(404,"Discount Could not Saved.");
            }

            return ResponseObject<Models.Discount>.CreateSuccessResponse(discount);
        } 



        public async Task<ResponseObject<Models.Discount>> Update(Models.Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("update discount set userid=@UserId, code=@Code, rate=@Rate where id=Id", discount);

            if (status <= 0)
            {
                return ResponseObject<Models.Discount>.CreateErrorResponse(404,"Discount Could not Updated.");
            }

            return ResponseObject<Models.Discount>.CreateSuccessResponse(discount);
        }



        public async Task<ResponseObject<NoContentObject>> Delete(int discountId)
        {
            var status = await _dbConnection.ExecuteAsync("delete from discount where id=@Id", new{Id=discountId});

            if (status <= 0)
            {
                return ResponseObject<NoContentObject>.CreateErrorResponse(404,"Discount Could not Removed.");
            }

            return ResponseObject<NoContentObject>.CreateSuccessResponse();
        }



        public async Task<ResponseObject<Models.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("select * from discount where userid=@UserId and code=@Code", new{userid = userId, code = code})).FirstOrDefault();

            if (discount == null)
            {
                return ResponseObject<Models.Discount>.CreateErrorResponse(404,"Discount Not Found.");
            }

            return ResponseObject<Models.Discount>.CreateSuccessResponse(discount);
        }
    }
}