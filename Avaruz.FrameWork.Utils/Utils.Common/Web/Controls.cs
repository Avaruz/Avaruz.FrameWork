using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Avaruz.FrameWork.Utils.Common.Web
{
    public static class Controls
    {

        public static void AnexarCombo<T>(ref DropDownList listaDesplegable, string campoTexto, string campoValor, IList<T> fuenteDatos)
        {
            listaDesplegable.DataTextField = campoTexto;
            listaDesplegable.DataValueField = campoValor;
            listaDesplegable.DataSource = fuenteDatos;
        }
        public static void AnexarCheckBoxList<T>(ref CheckBoxList listaDesplegable, string campoTexto, string campoValor, IList<T> fuenteDatos)
        {
            listaDesplegable.DataTextField = campoTexto;
            listaDesplegable.DataValueField = campoValor;
            listaDesplegable.DataSource = fuenteDatos;
        }

        public static void AnexarCheckBoxList(ref CheckBoxList listaDesplegable, string campoTexto, string campoValor, IList fuenteDatos)
        {
            listaDesplegable.DataTextField = campoTexto;
            listaDesplegable.DataValueField = campoValor;
            listaDesplegable.DataSource = fuenteDatos;
        }

    }
}
