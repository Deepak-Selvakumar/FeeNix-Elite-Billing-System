using FmsEliteBilling.Data.Oracle;
using System.Data; 
using Newtonsoft.Json;
using FmsEliteBilling.Model.AppNavigationModel;
using Dapper;
using FmsEliteBilling.Model.AppNavigationModel.PostModel;
using FmsEliteBilling.Model.Response;
 
namespace FmsEliteBilling.Data.Repositories
{
    public class AppNavigationRepository<T>
    {

        public GetResponse<T> MenuNavigationIns(IConnectionProvider _config,string randomnumber,int accesslevel)
        {
           try
            {
               GetResponse<T> result=new GetResponse<T>();
                result.response=new ResponseModel();
                var parameters = new DynamicParameters();
                var dtatvalue=1001;
                parameters.Add("@RandNo", randomnumber,DbType.String,ParameterDirection.Input,100);
                parameters.Add("@AccessLevel",accesslevel,DbType.Int32,ParameterDirection.Input);
                parameters.Add("@ConsultantId",dtatvalue,DbType.Int32,ParameterDirection.Input);
                parameters.Add("@rtn", null, DbType.Int32, ParameterDirection.Output);
                parameters.Add("@error_msg",null,DbType.String,ParameterDirection.Output,1000);
               
           
                var query = "MenuSessionIns_Prc";
                 using (var connection = _config.CreateConnection(ConnectionStringConfig.SCDB))
                {
                   
                    var json = JsonConvert.SerializeObject(SqlMapper.Query(connection, query, param: parameters, commandType: CommandType.StoredProcedure).AsList());
                    result.value=JsonConvert.DeserializeObject<List<T>>(json);
                    result.response.ReturnNumber=parameters.Get<int>("rtn");
                    result.response.ErrorMessage=parameters.Get<string>("error_msg");
                  
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
            
        }



        public GetResponse<T> GetPersonaList(IConnectionProvider _config,int LogSysId,int AccessLevel,int siteid, string Type)
        {
            try
            {
               GetResponse<T> result=new GetResponse<T>();
                result.response=new ResponseModel();
                var parameters = new DynamicParameters();

                parameters.Add("@rtn_io", 0,DbType.Int32,ParameterDirection.Output);
                parameters.Add("@errmsg_o",null,DbType.String,ParameterDirection.Output,100);
                parameters.Add("@LogSysId_i", LogSysId,DbType.Int32,ParameterDirection.Input);
                parameters.Add("@AccessLevel",AccessLevel,DbType.Int32,ParameterDirection.Input);
                parameters.Add("@siteid",siteid,DbType.Int32,ParameterDirection.Input);
                parameters.Add("@Type",Type,DbType.String,ParameterDirection.Input,100);


                var query = "PersonaList_prc";
                 using (var connection = _config.CreateConnection(ConnectionStringConfig.SCDB))
                {
                   
                    var json = JsonConvert.SerializeObject(SqlMapper.Query(connection, query, param: parameters, commandType: CommandType.StoredProcedure).AsList());
                    result.value=JsonConvert.DeserializeObject<List<T>>(json);
                 
                    result.response.ErrorMessage=parameters.Get<string>("errmsg_o");
                  
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
            
        }



        public PostResponse PostMenuNavigation(IConnectionProvider _config,PostMenuNavigation menu)
        {
            try
            {
                
               PostResponse result=new PostResponse();
               result.RandomNumber = null;
                var parameters = new DynamicParameters();

                parameters.Add("@UserSysId", menu.UserSysId,DbType.Int32,ParameterDirection.Input);
                parameters.Add("@FromMenuName",menu.FromMenuName,DbType.String,ParameterDirection.Input,15);
                parameters.Add("@ToMenuName",menu.ToMenuName,DbType.String,ParameterDirection.Input,15);
                parameters.Add("@siteid",menu.siteid,DbType.Int32,ParameterDirection.Input);
                parameters.Add("@LoginHistoryID", menu.LoginHistoryID,DbType.String,ParameterDirection.Input,100);
                parameters.Add("@ISAdminCred",menu.ISAdminCred,DbType.String,ParameterDirection.Input,1);
                parameters.Add("@rdnno",null,DbType.String,ParameterDirection.Output,25);
                parameters.Add("@SessionYearStr",menu.SessionYearStr,DbType.String,ParameterDirection.Input,4);


                var query = "MenuNavigationIns_Prc";
                 using (var connection = _config.CreateConnection(ConnectionStringConfig.SCDB))
                {
                   
                    var polices = connection.Query(query,parameters,commandType: CommandType.StoredProcedure).AsList();
                    // result.ReturnNumber=parameters.Get<int>("Rtn_io");
                    // result.ErrorMessage=parameters.Get<string>("Errormsg_o");
                     result.RandomNumber=parameters.Get<string>("rdnno");
                  
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
            
        }



        public PostResponse UserTokenInsertion(IConnectionProvider _config,string type,string token,long usersysid)
        {
            try
            {
                
               PostResponse result=new PostResponse();
               result.RandomNumber = null;
                var parameters = new DynamicParameters();

                parameters.Add("@Token_i", token,DbType.String,ParameterDirection.Input);
                parameters.Add("@UserSysId_i",usersysid,DbType.Int64,ParameterDirection.Input,15);
                parameters.Add("@Type_i",type,DbType.String,ParameterDirection.Input,1);
              
                var query = "UserTokenINsUpd_prc";
                using (var connection = _config.CreateConnection(ConnectionStringConfig.FeeDB))
                {
                   
                    var polices = connection.Query(query,parameters,commandType: CommandType.StoredProcedure).AsList();
                   
                  
                    
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
            
        }


    

        public GetResponse<T> GetMenuList(IConnectionProvider _config, string menuapps,int personaId,UserInformation userdet)
        {
            try
            {
               GetResponse<T> result=new GetResponse<T>();
                result.response=new ResponseModel();
                var parameters = new DynamicParameters();

                parameters.Add("@rtn_io", 0,DbType.Int32,ParameterDirection.Output);
                parameters.Add("@errmsg_o",null,DbType.String,ParameterDirection.Output,100);
                parameters.Add("@MenuApps_i",menuapps,DbType.String,ParameterDirection.Input,50);
                parameters.Add("@LogSysId_i", userdet.UserSysId,DbType.Int32,ParameterDirection.Input);
                parameters.Add("@SiteId_i",0,DbType.Int32,ParameterDirection.Input);
                parameters.Add("@Personaid",personaId,DbType.Int32,ParameterDirection.Input);
                parameters.Add("@AccessLevel",userdet.AccessLevel,DbType.Int32,ParameterDirection.Input,100);


                var query = "MenuList_prc";
                 using (var connection = _config.CreateConnection(ConnectionStringConfig.SCDB))
                {
                   
                    var json = JsonConvert.SerializeObject(SqlMapper.Query(connection, query, param: parameters, commandType: CommandType.StoredProcedure).AsList());
                    result.value=JsonConvert.DeserializeObject<List<T>>(json);
                    // result.response.ReturnNumber=parameters.Get<int>("rtn_io");
                    // result.response.ErrorMessage=parameters.Get<string?>("errmsg_o");
                  
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
            
        }



        
            
        
    

    }
}