using MyFInance.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyFInance.Models
{
    public class HomeModel
    {
        /// <summary>
        /// BUsca o nome do usuario no banco
        /// </summary>
        /// <returns>Nome do usuario</returns>
        public String LerNomeUsuario()
        {
            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable("SELECT * FROM USUARIO");
            if(dt != null)
            {
                if(dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Nome"].ToString();
                }
            }
            return "Nome não encontrado!";
        }
    }
}
