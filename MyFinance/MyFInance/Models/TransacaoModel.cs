using Microsoft.AspNetCore.Http;
using MyFInance.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyFInance.Models
{
    public class TransacaoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe a Data!")]
        public string Data { get; set; }
        public string DataFinal { get; set; }
        public string Tipo { get; set; }
        public double Valor { get; set; }

        [Required(ErrorMessage = "Informe a Descrição!")]
        public string Descricao { get; set; }
        

        public int Conta_Id { get; set; }
        public string NomeConta { get; set; }

        public int Plano_Contas_ID { get; set; }
        public string DescricaoPlanoConta { get; set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public TransacaoModel()
        {

        }

        //Recebe o contexto para acesso ás variáveis de sesssão.
        public TransacaoModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        //executa um selecte para exibir todas as transações do usuario logado
        public List<TransacaoModel> ListaTransacao()
        {
            List<TransacaoModel> lista = new List<TransacaoModel>();
            TransacaoModel item;

            //utilizado pela View estrato
            string filtro = "";
            if((Data != null) && (DataFinal != null))
            {
                filtro = $" and t.Data >='{DateTime.Parse(Data).ToString("yyyy/MM/dd")}' and t.Data <= '{DateTime.Parse(DataFinal).ToString("yyyy/MM/dd")}'";
            }

            if(Tipo != null)
            {
                if(Tipo != "A")
                {
                    filtro += $" and t.Tipo = '{Tipo}'";
                }
            }

            if(Conta_Id != 0)
            {
                filtro += $" and t.Conta_id = '{Conta_Id}'";
            }
            //Fim

            //identifica o usuario logado atravez do ID pela session
            string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");

            string sql = $" select t.id, t.data, t.tipo, t.valor, t.descricao as Historico, " +
                        " t.conta_id, c.nome as Conta, t.plano_contas_id, p.descricao as Plano_Conta " +
                        " from transacao as t inner join conta c " +
                        " on t.conta_id = c.id inner join plano_contas as p " +
                        " on t.plano_contas_id = p.id " +
                        $" where t.Usuario_id = {id_usuario_logado} {filtro} order by t.data desc limit 10 ";
            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //ceta os valores no objeto item
                item = new TransacaoModel();
                item.Id = int.Parse(dt.Rows[i]["ID"].ToString());
                item.Data = DateTime.Parse(dt.Rows[i]["Data"].ToString()).ToString("dd/MM/yyyy");
                item.Tipo = dt.Rows[i]["Tipo"].ToString();
                item.Descricao = dt.Rows[i]["Historico"].ToString();
                item.Valor = double.Parse(dt.Rows[i]["Valor"].ToString());
                item.Conta_Id = int.Parse(dt.Rows[i]["Conta_Id"].ToString());
                item.NomeConta = dt.Rows[i]["Conta"].ToString();
                item.Plano_Contas_ID = int.Parse(dt.Rows[i]["plano_contas_id"].ToString());
                item.DescricaoPlanoConta = dt.Rows[i]["Plano_Conta"].ToString();
                lista.Add(item);
            }
            return lista;
        }

        public TransacaoModel CarregarRegistro(int? id)
        {
            TransacaoModel item;

            //identifica o usuario logado atravez do ID pela session
            string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            //cria query para consulta no banco
            string sql = $" select t.id, t.data, t.tipo, t.valor, t.Descricao as Historico, " +
                        " t.conta_id, c.nome as Conta, t.plano_contas_id, p.descricao as Plano_Conta " +
                        " from transacao as t inner join conta c " +
                        " on t.conta_id = c.id inner join plano_contas as p " +
                        " on t.plano_contas_id = p.id " +
                        $" where t.Usuario_id = {id_usuario_logado} and t.id='{id}' ";
            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);

            //ceta os valores no objeto item
            item = new TransacaoModel();
            item.Id = int.Parse(dt.Rows[0]["ID"].ToString());
            item.Data = DateTime.Parse(dt.Rows[0]["Data"].ToString()).ToString("dd/MM/yyyy");
            item.Tipo = dt.Rows[0]["Tipo"].ToString();
            item.Descricao = dt.Rows[0]["Historico"].ToString();
            item.Valor = double.Parse(dt.Rows[0]["Valor"].ToString());
            item.Conta_Id = int.Parse(dt.Rows[0]["Conta_Id"].ToString());
            item.NomeConta = dt.Rows[0]["Conta"].ToString();
            item.Plano_Contas_ID = int.Parse(dt.Rows[0]["plano_contas_id"].ToString());
            item.DescricaoPlanoConta = dt.Rows[0]["Plano_Conta"].ToString();

            return item;
                
        }

        //Insere ou atualiza uma transação
        public void Insert()
        {
            string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = "";
            if (Id == 0)
            {
                sql = $"INSERT INTO TRANSACAO (DATA, TIPO, DESCRICAO, VALOR, CONTA_ID, PLANO_CONTAS_ID,  USUARIO_ID)" + 
                    $" VALUES('{DateTime.Parse(Data).ToString("yyyy/MM/dd")}','{Tipo}','{Descricao}','{Valor}','{Conta_Id}','{Plano_Contas_ID}','{id_usuario_logado}')";
            }
            else
            {
                sql = $"UPDATE TRANSACAO SET DATA='{DateTime.Parse(Data).ToString("yyyy/MM/dd")}', " +
                    $"DESCRICAO='{Descricao}', " +
                    $"TIPO='{Tipo}', " +
                    $"VALOR='{Valor}', " +
                    $"CONTA_ID='{Conta_Id}', " +
                    $"PLANO_CONTAS_ID='{Plano_Contas_ID}' " +
                    $"WHERE USUARIO_ID = '{id_usuario_logado}' AND ID = '{Id}'";
            }
            DAL objDAl = new DAL();
            objDAl.ExecutarComandoSql(sql);
        }

        public void Excluir(int id)
        {
            new DAL().ExecutarComandoSql("DELETE FROM TRANSACAO WHERE ID = " + id);
        }
    }

    public class Dashboard
    {
        public double Total { get; set; }
        public string PlanoConta { get; set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public Dashboard()
        {

        }

        //Recebe o contexto para acesso ás variáveis de sesssão.
        public Dashboard(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Busca no banco os dados para o grafico
        /// </summary>
        /// <returns>lista com os dados para o grafico</returns>
        public List<Dashboard> RetornaDadosGraficosPie()
        {
            List<Dashboard> lista = new List<Dashboard>();
            Dashboard item;

            string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");

            string sql = "SELECT Sum(t.valor) as total, p.Descricao FROM transacao as t inner join plano_contas as p " +
                         $"on t.plano_contas_id = p.id where t.tipo = 'D' and t.Usuario_ID={id_usuario_logado} group by p.Descricao";
            DAL objDAL = new DAL();
            DataTable dt = new DataTable();
            dt = objDAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new Dashboard();
                item.Total = double.Parse(dt.Rows[i]["total"].ToString());
                item.PlanoConta = dt.Rows[i]["descricao"].ToString();
                lista.Add(item);
            }
            return lista;
        }
    }
}
