using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BizTransactionalServer
{
	/// <summary>
	/// Clase que representa una colecci�n de objectos ResultSetRowAttribute
	/// </summary>
	internal sealed class ResultSetRowAttributeCollection : ArrayList
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
				if (0 == _cultureAwareCompare(((ResultSetRowAttribute)this[index]).Name, attributeName))
					return index;
			}

			/*foreach(ResultSetRowAttribute item in this) 
			{
				if (0 == _cultureAwareCompare(item.Name, attributeName))
					return index;

				index++;
			}*/

			return -1;
		}

		public void RemoveAt(string attributeName)
		{
			RemoveAt(IndexOf(attributeName));
		}

		public override int Add(object value)
		{
			return Add((ResultSetRowAttribute)value);
		}

		internal int Add(ResultSetRowAttribute value)
		{
			if (((ResultSetRowAttribute)value).Name != null)
				return base.Add(value);
			else
				throw new ArgumentException("El Atributo debe tener un nombre!");
		}

		public int Add(string name, string value)
		{
			return Add(new ResultSetRowAttribute(name, value));
		}

		private int _cultureAwareCompare(string strA, string strB)
		{
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.IgnoreCase);
		}

		~ResultSetRowAttributeCollection()
		{
			base.Clear();
		}
	}

}
