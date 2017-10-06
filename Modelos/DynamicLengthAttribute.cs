using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Odbc;

namespace Modelos
{
    //Con algunos cambios en los modelos, pero lo saqué de: https://stackoverflow.com/questions/40218566/mvc-data-annotations-dynamic-string-length
    //y de http://techfunda.com/howto/264/custom-validation-in-asp-net-mvc
    public class CodigoDynamicLengthAttribute : ValidationAttribute
    {
        private string _lengthKey;

        public CodigoDynamicLengthAttribute(string lengthKey)
        {
            _lengthKey = lengthKey;
        }

        //public override bool IsValid(object value)
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {

            if (value != null && value.GetType() == typeof(string))
            {
                //retrive teh max length from the database according to the lengthKey variable, if you will store it in web.config you can do:
                int maxLength = Convert.ToInt32(ObtenerOpcionSistema(_lengthKey));
                //int maxLength = ConfigurationManager.AppSettings[_lengthKey];

                if (((string)value).Length <= maxLength)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("El dato para " + context.DisplayName + " debe ser de hasta " + maxLength.ToString() + " caracteres");
                }
            }

            return ValidationResult.Success;
        }

        private string ObtenerOpcionSistema(string sOpcionCod)
        {

            string l_s_stSql = "";
            string sResultado = "";

            string connectionString = ConfigurationManager.ConnectionStrings["SISCOMPRASWEB"].ConnectionString;

            l_s_stSql = "SELECT po_v_opcionValor";
            l_s_stSql += " FROM sp_sistema_opciones_buscar_valor('" + sOpcionCod + "',NULL,NULL)";

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                connection.Open();

                OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                sResultado = cmd.ExecuteScalar().ToString();
                cmd.Dispose();

            }

            return sResultado;
        }
    }

    public class DBColumnaDynamicLengthAttribute : ValidationAttribute
    {
        private string _sTabla;
        private string _sColumna;

        public DBColumnaDynamicLengthAttribute(string sTabla, string sColumna)
        {
            _sColumna = sColumna;
            _sTabla = sTabla;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {

            if (value != null && value.GetType() == typeof(string))
            {
                //retrive teh max length from the database according to the lengthKey variable, if you will store it in web.config you can do:
                int maxLength = ObtenerLargoColumna(_sTabla, _sColumna);
                //int maxLength = ConfigurationManager.AppSettings[_lengthKey];

                if (((string)value).Length <= maxLength)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("El dato para " + context.DisplayName + " debe ser de hasta " + maxLength.ToString() + " caracteres");
                }
            }

            return ValidationResult.Success;
        }

        private int ObtenerLargoColumna(string sTabla, string sColumna)
        {

            string l_s_stSql = "";
            int l_i_Resultado = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["SISCOMPRASWEB"].ConnectionString;

            l_s_stSql = "SELECT "+ sColumna;
            l_s_stSql += " FROM " + sTabla;
            l_s_stSql += " WHERE 0=1 ";

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                connection.Open();

                //OdbcCommand cmd = new OdbcCommand(l_s_stSql, connection);
                //OdbcDataReader dr = cmd.ExecuteReader();
                //DataTable schemaTable = dr.GetSchemaTable();
                ////foreach (DataRow row in schemaTable.Rows)
                ////{
                //foreach (DataColumn column in schemaTable.Columns)
                //{
                //    l_i_Resultado = column.MaxLength;
                //}
                ////}

                //cmd.Dispose();
                OdbcCommand cmd = new OdbcCommand();
                cmd.Connection = connection;
                cmd.CommandText = l_s_stSql;
                OdbcDataReader dr = cmd.ExecuteReader(CommandBehavior.KeyInfo);
                //Retrieve column schema into a DataTable.
                DataTable schemaTable = dr.GetSchemaTable();
                //For each field in the table...
                foreach (DataRow myField in schemaTable.Rows)
                {
                    //For each property of the field...
                    foreach (DataColumn myProperty in schemaTable.Columns)
                    {
                        //Display the field name and value.
                        //Console.WriteLine(myProperty.ColumnName + " = " + myField[myProperty].ToString());
                        //l_i_Resultado = myProperty.MaxLength;
                        //l_s_stSql = myField[myProperty].ToString();
                        if(myProperty.ToString().ToUpper() == "COLUMNSIZE")
                        {
                            l_i_Resultado = Convert.ToInt32(myField[myProperty].ToString());
                            dr.Close();
                            return l_i_Resultado;
                        }
                    }
                }
                dr.Close();

            }

            return -1;
        }

    }

}
