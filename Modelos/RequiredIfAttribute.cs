using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.Odbc;

namespace Modelos
{

    //https://blogs.msdn.microsoft.com/simonince/2010/06/04/conditional-validation-in-mvc/

    public class CotizacionRequiredIfAttribute : ValidationAttribute
    {
        // Note: we don't inherit from RequiredAttribute as some elements of the MVC
        // framework specifically look for it and choose not to add a RequiredValidator
        // for non-nullable fields if one is found. This would be invalid if we inherited
        // from it as obviously our RequiredIf only applies if a condition is satisfied.
        // Therefore we're using a private instance of one just so we can reuse the IsValid
        // logic, and don't need to rewrite it.
        private RequiredAttribute innerAttribute = new RequiredAttribute();
        public string DependentProperty { get; set; }
        public object TargetValue { get; set; }

        //public CotizacionRequiredIfAttribute(string dependentProperty, object targetValue) ==> esto es por si quiero mandar el valor que debe tener el dependentProperty
        public CotizacionRequiredIfAttribute(string dependentProperty)
        {
            this.DependentProperty = dependentProperty;
            this.TargetValue = ObtenerMonedaNacional();
        }

        public override bool IsValid(object value)
        {
            return innerAttribute.IsValid(value);
        }

        private string ObtenerMonedaNacional()
        {

            string l_s_stSql = "";
            string sResultado = "";

            string connectionString = ConfigurationManager.ConnectionStrings["SISCOMPRASWEB"].ConnectionString;

            l_s_stSql = "SELECT moneda_id";
            l_s_stSql += " FROM monedas";
            l_s_stSql += " WHERE flag_activo = 'Si'";
            l_s_stSql += " AND flag_nacional = 'Si'";

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
}