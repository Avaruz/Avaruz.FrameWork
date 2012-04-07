
namespace Avaruz.FrameWork.Collections
{
    public class Parametro
    {
        private string _Nombre;
        private object _Valor;

        public Parametro(string nombre, object valor)
        {
            this._Nombre = nombre;
            this._Valor = valor;
        }

        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }
        public object Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }
    }
}
