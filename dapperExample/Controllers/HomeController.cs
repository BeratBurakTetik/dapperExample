using Dapper;
using dapperExample.Models;
using dapperExample.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace dapperExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IDapper _dapper;
        public HomeController(IDapper dapper)
        {
            _dapper = dapper;
        }
        [HttpPost(nameof(Create))]
        public async Task<int> Create(Cocktails data)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("Id", data.id, DbType.Int32);
            var result = await Task.FromResult(_dapper.Insert<int>("[dbo].[SP_Add_Article]"
                , dbparams,
                commandType: CommandType.StoredProcedure));
            return result;
        }
        [HttpGet]
        [Route("getbyid/{Id}")]
        public async Task<Cocktails> GetById(int Id)  
        {  
            var result = await Task.FromResult(_dapper.Get<Cocktails>($"Select * from [tblcocktail] where id = {Id}", null, commandType: CommandType.Text));  
            return result;  
        }
        [HttpGet]
        [Route("getall")]
        public async Task<List<Cocktails>> GetAll()
        {
            var result = await Task.FromResult(_dapper.GetAll<Cocktails>($"Select * from [tblcocktail]", null, commandType: CommandType.Text));
            return result;
        }
        [HttpDelete(nameof(Delete))]  
        public async Task<int> Delete(int Id)  
        {  
            var result = await Task.FromResult(_dapper.Execute($"Delete [tblcocktail] Where Id = {Id}", null, commandType: CommandType.Text));  
            return result;  
        }  
        [HttpGet(nameof(Count))]  
        public Task<int> Count(int num)  
        {  
            var totalcount = Task.FromResult(_dapper.Get<int>($"select COUNT(*) from [tblcocktail] WHERE Age like '%{num}%'", null,  
                    commandType: CommandType.Text));  
            return totalcount;  
        }  
        [HttpPatch(nameof(Update))]  
        public Task<int> Update(Cocktails data)  
        {  
            var dbPara = new DynamicParameters();  
            dbPara.Add("Id", data.id);  
            dbPara.Add("Name", data.name, DbType.String);  
  
            var updateArticle = Task.FromResult(_dapper.Update<int>("[dbo].[SP_Update_Article]",  
                            dbPara,  
                            commandType: CommandType.StoredProcedure));  
            return updateArticle;  
        }
    }
}
