using System;
using System.Collections.Generic;
using System.Text;

namespace BizTransactionalServer
{
	public enum TypeResultSet : short
	{
		ResultSetRow,
		ResultSetParameters,
		SystemError,
		DbError,
		PackageError
	}

	internal struct ResultSet
	{
		string _name;
		int _resultsetCount;
		int _resultsetFieldCount;
		TypeResultSet _type;
		ResultSetRowCollection _rsRowColl;

		public ResultSet(string name, TypeResultSet type)
		{
			_name = name;
			_type = type;
			_rsRowColl = new ResultSetRowCollection();
			_resultsetCount = 0;
			_resultsetFieldCount = 0;
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public TypeResultSet Type
		{
			get { return _type; }
			set { _type = value; }
		}

		public ResultSetRowCollection ResultSetRows
		{
			get { return _rsRowColl; }
		}

		public int ResultSetCount
		{
			get { return _resultsetCount; }
			set { _resultsetCount = value; }
		}

		public int ResultSetFieldCount
		{
			get { return _resultsetFieldCount; }
			set { _resultsetFieldCount = value; }
		}
	}
}
