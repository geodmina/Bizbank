using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BizTransactionalServer
{
	/// <summary>
	/// Clase que representa una colecci�n de set de resultados
	/// </summary>
	internal sealed class ResultSetCollection : ArrayList
	{
		public object this[string index]
		{
			get { return this[IndexOf(index)]; }
			set { this[IndexOf(index)] = value; }
		}

		public bool Contains(string resultSetName)
		{
			return (-1 != IndexOf(resultSetName));
		}

		public int IndexOf(string resultSetName)
		{
			int index = 0;

			for (index = 0; index < this.Count; index++)
			{
				if (0 == _cultureAwareCompare(((ResultSet)this[index]).Name, resultSetName))
					return index;
			}

			/*foreach(ResultSet item in this) 
			{
				if (0 == _cultureAwareCompare(item.Name, resultSetName))
					return index;

				index++;
			}*/
			return -1;
		}

		public void RemoveAt(string resultSetName)
		{
			RemoveAt(IndexOf(resultSetName));
		}

		public override int Add(object value)
		{
			return Add((ResultSet)value);
		}

		public int Add(ResultSet value)
		{
			if (((ResultSet)value).Name != null)
				return base.Add(value);
			else
				throw new ArgumentException("El Resulset debe tener un nombre!");
		}

		public int Add(string name, TypeResultSet type)
		{
			return Add(new ResultSet(name, type));
		}

		private int _cultureAwareCompare(string strA, string strB)
		{
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.IgnoreCase);
		}

		~ResultSetCollection()
		{
			base.Clear();
		}
	}
}
