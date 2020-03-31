using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BizTransactionalServer
{
	/// <summary>
	/// Clase que representa una colecci�n de filas de el set de resultado
	/// </summary>

	internal sealed class ResultSetRowCollection : ArrayList
	{
		public object this[string index]
		{
			get { return this[IndexOf(index)]; }
			set { this[IndexOf(index)] = value; }
		}

		public bool Contains(string attributeName)
		{
			return (-1 != IndexOf(attributeName));
		}

		public int IndexOf(string attributeName)
		{
			int index = 0;

			for (index = 0; index < this.Count; index++)
			{
				if (0 == _cultureAwareCompare(((ResultSetRow)this[index]).Name, attributeName))
					return index;
			}

			return -1;
		}

		public void RemoveAt(string attributeName)
		{
			RemoveAt(IndexOf(attributeName));
		}

		public override int Add(object value)
		{
			return Add((ResultSetRow)value);
		}

		internal int Add(ResultSetRow value)
		{
			if (value.Name != null)
				return base.Add(value);
			else
				throw new ArgumentException("El registro debe tener un nombre debe tener un nombre!");
		}

		public int Add(string name)
		{
			return Add(new ResultSetRow(name));
		}

		private int _cultureAwareCompare(string strA, string strB)
		{
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.IgnoreCase);
		}
	}

}
