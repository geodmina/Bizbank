using System;
using System.Collections.Generic;
using System.Text;

namespace BizTransactionalServer
{
	/// <summary>
	/// Clase que representa una fila de el set de resultados
	/// </summary>
	internal struct ResultSetRow
	{
		string _name;
		ResultSetRowAttributeCollection _rsAttributeColl;

		public ResultSetRow(string name)
		{
			_name = name;
			_rsAttributeColl = new ResultSetRowAttributeCollection();
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public ResultSetRowAttributeCollection Attributes
		{
			get { return _rsAttributeColl; }
		}

		public int Count
		{
			get { return _rsAttributeColl.Count; }
		}
	}
}
