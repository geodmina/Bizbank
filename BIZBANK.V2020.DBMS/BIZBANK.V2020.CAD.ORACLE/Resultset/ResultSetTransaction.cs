using System;
using System.Collections.Generic;
using System.Text;

namespace BizTransactionalServer
{
	/// <summary>
	/// Clase que representa el conjunto de datos obtenidos de la transacci�n
	/// </summary>
	internal struct ResultSetTransaction
	{
		string _name;
		//int _returnValue;
		ResultSetCollection _rsColl;

		internal ResultSetTransaction(string name)
		{
			_name = name;
			_rsColl = new ResultSetCollection();
			//_returnValue = 0;
		}

		internal string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/*internal int ReturnValue
		{
			get { return _returnValue; }
			set { _returnValue = value; }
		}*/

		internal ResultSetCollection ResultSets
		{
			get { return _rsColl; }
			set { _rsColl = value; }
		}

		internal int Count
		{
			get { return _rsColl.Count; }
		}
	}
}
