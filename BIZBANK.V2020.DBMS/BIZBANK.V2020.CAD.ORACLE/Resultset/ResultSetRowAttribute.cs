using System;
using System.Collections.Generic;
using System.Text;

namespace BizTransactionalServer
{
	/// <summary>
	/// Clase b�sica para representar un atributo para una determinada
	/// fila del set de resultado generados
	/// </summary>

	internal struct ResultSetRowAttribute
	{
		string _name;   //Variable para guardar el nombre del atributo
		string _value;  //Variable para guardar el valor del atributo

		public ResultSetRowAttribute(string name, string value)
		{
			_name = name;
			_value = value;
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public string Value
		{
			get { return _value; }
			set { _value = value; }
		}
	}

}
